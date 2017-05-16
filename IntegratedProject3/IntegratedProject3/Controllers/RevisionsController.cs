using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntegratedProject3.Models;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using System.Data.Entity.Migrations;
using IntegratedProject3.Extensions;
using System.IO;

namespace IntegratedProject3.Controllers
{
    public class RevisionsController : RootController
    {

        /// <summary>
        /// Index shows all the active revisions accessible by the user. 
        /// For all users.
        /// </summary>
        /// <returns>ViewResult containing a list of Revisions.</returns>
        [Authorize]
        public ActionResult Index()
        {

            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();

            var id = User.Identity.GetUserId();
            var revisions = db.Revisions;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if(isAuthor())
            {
                var authorRevisions = db.Revisions.Where(r => r.document.Author.Id == id).ToList();
                if (authorRevisions != null)
                {
                    return View(authorRevisions);
                }
            }
            
            if(isDistributee())
            {
                var distributeeRevisions = revisions.Where(r => r.Distributees.Where(d => d.Id == id).Any()).ToList();
                if (distributeeRevisions != null)
                {
                    return View(distributeeRevisions);
                }
            }

            if(isAdmin())
            { 
                return View(revisions);
            }

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return View("Index", "Home");
        }

        //Accessible by distributees and author associated with the revision.

        /// <summary>
        /// Displays the details of the selected revision.
        /// </summary>
        /// <param name="id">revision id</param>
        /// <returns>ViewResult containing the selected revision</returns>
        [Authorize]
        public ActionResult Details(string id)
        {

            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Retrieves the selected revision
            Revision revision = db.Revisions.Find(id);
            if (revision == null)
            {
                return HttpNotFound();
            }
            return View(revision);

        }

        

        /// <summary>
        /// Retrives the Revision/Create page.
        /// Only Authors.
        /// </summary>
        /// <param name="id">document id</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(string id)
      {
            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();
            if (isAuthor())
            {

                if(id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //Retrieves enumerable of all accounts.
                IEnumerable<Account> accounts = db.Accounts.ToList();

                //New revision viewmodel
                var createRevisionModel = new RevisionViewModel();
                // Retrives the latest active revision of the selected document.
                var revision = db.Revisions.Where(r => r.document.id == id && r.State == DocumentState.Active).SingleOrDefault();

                // If there is a revision.
                if (revision != null)
                {
                    // Sets viewmodel to latest active revision.
                    createRevisionModel.DocID = revision.document.id;
                    createRevisionModel.DocumentTitle = revision.DocumentTitle;
                    createRevisionModel.RevisionNum = revision.RevisionNum;

                    return View(createRevisionModel);
                }

                //Retrieves the document of the revision.
                var document = db.Documents.Where(d => d.id == id).SingleOrDefault();
                if (document != null)
                {
                    //Sets the document id for the new viewmodel.
                    createRevisionModel.DocID = id;
                    return View(createRevisionModel);
                }
            }
            
            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Saves new revision added through the Revision/Create page.
        /// Only Authors.
        /// </summary>
        /// <param name="revision">revision viewmodel</param>
        /// <returns></returns>
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create([Bind(Include = "DocID, RevisionNum, DocumentTitle, Distributees, File")] RevisionViewModel revision)
        {
            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();
            if (isAuthor())
            {

                if (ModelState.IsValid)
                {

                    //Retrieves the document from the revision being created.
                    var doc = db.Documents.Where(d => d.id == revision.DocID).SingleOrDefault();
                    var latestRevision = db.Revisions.Where(r => r.document.id == doc.id && r.State == DocumentState.Active).SingleOrDefault();
                    var distributees = new HashSet<Account>();

                    if (distributees.Count > 0)
                    {
                        distributees = (HashSet<Account>)latestRevision.Distributees;
                        
                    }

                    // Files is looking for the corresponding ID in the view
                    HttpPostedFileBase file = Request.Files["document"];

                    if (file != null)
                    {
                        FileStoreService fss = new FileStoreService();

                        revision.FileStoreKey = fss.UploadFile(file);
                    }

                    //New revision to be added to the database
                    Revision newRevision = new Revision()
                    {
                        //Autogenerated unique identifer
                        id = Guid.NewGuid().ToString(),
                        DocumentTitle = revision.DocumentTitle,
                        RevisionNum = revision.RevisionNum,
                        //Revision creation date/time set to the current date/time.
                        DocCreationDate = DateTime.Now,
                        State = DocumentState.Draft,
                        //Revision activation date/time set the the current date/time.
                        ActivationDate = null,
                        //Revision's document set to the document queryed from database.
                        document = doc,
                        //New empty hash of Accounts.
                        Distributees = distributees.ToList(),
                        fileStoreKey = revision.FileStoreKey

                    };

                    //Adds the new revision to database.
                    db.Revisions.Add(newRevision);
                    db.SaveChanges();
                    //Redirects to the list of distributees.
                    return RedirectToAction("SelectUsers", "Revisions", new { newRevision.id });

                }
            }

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Displays a list of distributees to be selected.
        /// Only Authors.
        /// </summary>
        /// <param name="id">The ID of the revision</param>
        /// <returns></returns>
        public ActionResult SelectUsers(string id)
        {
            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();
            if (isAuthor())
            {
                var DistributeeList = new DistributeeSelectModel();
                DistributeeList.RevID = id;
                // List of all distributes.
                DistributeeList.Accounts = db.Accounts.Where(u => u.AccountType == AccountType.Distributee);

                return View(DistributeeList);
            }

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Adds a new distributee to the revision.
        /// Only Authors.
        /// </summary>
        /// <param name="userKey">The User to be added </param>
        /// <param name="rev">The Revision key</param>
        /// <returns></returns>
        public ActionResult AddNewDistributee(string userKey, string revID)
        {

            if (isAuthor())
            {
                //Finds selected revision
                var revision = db.Revisions.Where(r => r.id == revID).SingleOrDefault();

                //Finds the distributee to be added to the revision's distributee list.
                var user = db.Accounts.Find(userKey);
                if (user != null && revision != null)
                {
                    //Flag for if the user is already in the distributee list
                    var alreadyContained = revision.Distributees.Contains(user);
                    if (alreadyContained == false)
                    {
                        // Adds the user to the distributee list.
                        revision.Distributees.Add(user);
                        db.Revisions.AddOrUpdate(revision);
                        db.SaveChanges();
                        // User notification stating "Distrbutee added" to the revision.
                        this.AddNotification("Distributee added", NotificationType.SUCCESS);

                        //Emailing updated status to distributees
                        EmailService emailService = new EmailService();
                        emailService.massMessageDistribution(revision.Distributees, 1);

                        //Texting updated status to distributees
                        SMSService smsService = new SMSService();
                        smsService.DetermineSMSMessage(revision.Distributees, 1);
                    }
                    else
                    {
                        // User notification stating "Distributee already added" to the revision.
                        this.AddNotification("Distributee already added", NotificationType.ERROR);
                    }
                }

                //Redirects to a new instance of distributee list so user can add other distributees.
                return RedirectToAction("SelectUsers", "Revisions", new { id = revID });
            }

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Removes a distributee from a revisions distributee list.
        /// Authors only.
        /// </summary>
        /// <param name="userKey">distributee id</param>
        /// <param name="revID">revivion id</param>
        /// <returns></returns>
        public ActionResult RemoveDistributee(string userKey, string revID)
        {

            if (isAuthor())
            {
                // Retrieves selected revision
                var revision = db.Revisions.Where(r => r.id == revID).SingleOrDefault();

                //Retrieves selected user
                var user = db.Accounts.Find(userKey);

                //If the user and revision are not NULL.
                if (user != null && revision != null)
                {
                    //Flag for if user is in distributee list.
                    var contained = revision.Distributees.Contains(user);
                    if (contained == true)
                    {
                        //Removes the distributee from the distributee list.
                        revision.Distributees.Remove(user);
                        db.Revisions.AddOrUpdate(revision);
                        db.SaveChanges();
                        // User notification stating "Distributee removed" from the revision.
                        this.AddNotification("Distributee removed", NotificationType.SUCCESS);
                    }
                    else
                    {
                        // User notification stating "Distributee not assigned to this revision".
                        this.AddNotification("Distributee not assigned to this revision", NotificationType.ERROR);
                    }

                }

                //Redirect to distributee list so user can manage other distributees.

                return RedirectToAction("SelectUsers", "Revisions", new { id = revID });
            }

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Allow the user to edit detailed of the specified revision.
        /// Only author of document.
        /// </summary>
        /// <param name="id">revision id</param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();
            //Retrieves the selected revision.
            var revision = db.Revisions.Find(id);

            //Verifies the current user is author of document.
            if (VerifyAuthor(revision.document))
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //If the revision is "Active"
                if (revision.State == DocumentState.Active)
                {
                    this.AddNotification("This is an active revision. You may only manage the distributees.", NotificationType.INFO);
                    return RedirectToAction("SelectUsers", new { id = revision.id });
                }

                //If no revision is found, redirect to Revisions/Index
                if (revision == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                // If the current user is not the author of the revision.
                if (!(VerifyAuthor(revision)))
                {
                    new Exception("current user is not author of the document");
                    return RedirectToAction("Index", "Home");
                }

                return View(revision);
            }

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// SAves edits made by the user, to the revision, to the database.
        /// Only author of document.
        /// </summary>
        /// <param name="revision"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,RevisionNum,DocumentTitle,State,ActivationDate")] Revision revision)
        {

            var currentRevision = db.Revisions.Find(revision.id);

            //Fuck C#, consistently reset the date to 01/01/0001 when the minimum value it accepts
            //is 01/01/1753. When the revision is passed in it still resets to 01/01/0001 however,
            //now it takes the creation date from the current version of the revision and sets it to
            //the creation date value of the updated revision.
            revision.DocCreationDate = currentRevision.DocCreationDate;
            revision.fileStoreKey = currentRevision.fileStoreKey;
            
            var revisions = db.Revisions.Where(r => r.State == DocumentState.Active && r.document.id == currentRevision.document.id).ToList();
            
            foreach (var item in revisions)
            {
                if (item.State == DocumentState.Active)
                {
                    item.State = DocumentState.Archived;
                }
            }
            
            // IF the revision has no activation date and revision has been made "Active".
            if (revision.ActivationDate == null && revision.State == DocumentState.Active)
            {
                //Activation date is set to current date/time.
                revision.ActivationDate = DateTime.Now;
            }
            
            // Save changes to revision and redirect to the Revision/Index.
            if (ModelState.IsValid)
            {
                db.Revisions.AddOrUpdate(revision);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(revision);

        }

        // Only accessible by the author of the specific document.

        /// <summary>
        /// Deletes the selected revision.
        /// Only author of document.
        /// </summary>
        /// <param name="id">revision id</param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();
            //Retrieves the selected revision.
            var revision = db.Revisions.Where(r => r.id == id).SingleOrDefault();

            //Verifies the current user is author of document.
            if (VerifyAuthor(revision) || isAdmin())
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (revision == null)
                {
                    return HttpNotFound();
                }
                // Verifies the current user is the author of the revision.
                if (!(VerifyAuthor(revision)))
                {
                    this.AddNotification("Only the author can delete a revision.", NotificationType.ERROR);
                    return RedirectToAction("Index", "Home");
                }

                return View(revision);
            }

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Confirms the removal of the selected revision.
        /// Only author of document.
        /// </summary>
        /// <param name="id">revision id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            // Retrieves the selected revision.
            Revision revision = db.Revisions.Find(id);

            if (VerifyAuthor(revision) || isAdmin())
            {
                // Removes the selected revision from the database.
                db.Revisions.Remove(revision);
                db.SaveChanges();
                return RedirectToAction("Index", "Documents");
            }

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home"); ;
        }

        /// <summary>
        /// Archives the revisions of the selected author.
        /// Author only.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if success</returns>
        public bool ArchiveUserRevisions(string id)
        {
            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();
            //Retrieves revisions of author
            var authorRevisions = db.Revisions.Where(r => r.document.Author.Id == id);
            
            if(authorRevisions != null)
            {
                // Sets each revisions created by author to "Archived"
                foreach (var revision in authorRevisions)
                {
                    //Emailing updated status to distributees
                    EmailService emailService = new EmailService();
                    emailService.massMessageDistribution(revision.Distributees, 2);

                    //Texting updated status to distributees
                    //SMSService smsService = new SMSService();
                    //smsService.DetermineSMSMessage(revision.Distributees, 2);

                    revision.State = DocumentState.Archived;
                    db.Revisions.AddOrUpdate(revision);
                    db.SaveChanges();
                   
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Allows the user to dowload the file given.
        /// All users.
        /// </summary>
        /// <param name="docKey">They key of the file to be downloaded</param>
        /// <returns>The file to download if successful, otherwise nothing</returns>
        public FileResult Download(string docKey)
        {
            FileStoreService fss = new FileStoreService();

            //gets the file based on the file store key
            var file = fss.GetFile(docKey).ResponseStream;

            var fileType = fss.GetFile(docKey).Headers.ContentType;

            //finds the document title for the download
            var document = db.Revisions.Where(r => r.fileStoreKey == docKey).SingleOrDefault();

            //Service that allows for the use of dowloading 
            using (var memoryStream = new MemoryStream())
            {
                //if no a file exists then proceed
                if (file != null)
                {
                    //Begin the dowloading procees 
                    file.CopyTo(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();

                    
                    
                    // Promts the user to download the file. File types are unknown 
                    // due to a variety of formats that are supported
                    return File(fileBytes,fileType, document.DocumentTitle);
                }
                else
                {
                    return null;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
