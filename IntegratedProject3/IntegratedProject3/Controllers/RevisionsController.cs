﻿using System;
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
        /// Retrieves the Revisions Index. Displaying all revisions. Accessible to admins.
        /// </summary>
        /// <returns>ViewResult containing a list of Revisions.</returns>
        [Authorize]
        public ActionResult Index()
        {
            var revisions = db.Revisions.ToList();
            return View(revisions);
        }

        //Index shows all the active revisions accessible by the user. Distributees and Authors should have access.
        public ActionResult ActiveRevisions()
        {
            return View();
        }


        //DocumentRevisions will show all the revisions associated with a given document. Accessible by the Author of the document.

        /// <summary>
        /// Retrieves revisions associated with a specific document.
        /// </summary>
        /// <param name="id">document id</param>
        /// <returns>ViewResult containing list of Revisions</returns>
        [Authorize]
        public ActionResult DocumentRevisions(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Retrieves revisions of the selected document.
            var revisions = db.Revisions.Where(r => r.document.id == id);
            if (revisions == null)
            {
                return HttpNotFound();
            }

            return View(revisions.ToList());
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

        // Only accessible to the author of the document

        /// <summary>
        /// Retrives the Revision/Create page.
        /// </summary>
        /// <param name="id">document id</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(string id)
      {
            //Retrieves enumerable of all accounts.
            IEnumerable<Account> accounts = db.Accounts.ToList();

            //New revision viewmodel
            var createRevisionModel = new RevisionViewModel();
            // Retrives the latest active revision of the selected document.
            var revision = db.Revisions.Where(r => r.document.id == id && r.State == DocumentState.Active).SingleOrDefault(); 

            // If there is a revision.
            if(revision != null)
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
            
            return View();
        }

        // Only accessible to the author of the specific document.

        /// <summary>
        /// Saves new revision added through the Revision/Create page.
        /// </summary>
        /// <param name="revision">revision viewmodel</param>
        /// <returns></returns>
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create([Bind(Include = "DocID, RevisionNum, DocumentTitle, Distributees, File")] RevisionViewModel revision)
        {

                if (ModelState.IsValid)
                {
                
                //Retrieves the document from the revision being created.
                var doc = db.Documents.Where(d => d.id == revision.DocID).SingleOrDefault();
                var latestRevision = db.Revisions.Where(r => r.document.id == doc.id && r.State == DocumentState.Active).SingleOrDefault();
                var distributees = new HashSet<Account>();

                if(latestRevision != null)
                {
                    distributees = (HashSet<Account>)latestRevision.Distributees;
                }

                // Files is looking for the corresponding ID in the view
                HttpPostedFileBase file = Request.Files["document"];

                if(file != null)
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
                    ActivationDate = DateTime.Now,
                    //Revision's document set to the document queryed from database.
                    document = doc,
                    //New empty hash of Accounts.
                    Distributees = distributees,
                    fileStoreKey = revision.FileStoreKey

               };

                //Adds the new revision to database.
                db.Revisions.Add(newRevision);
                db.SaveChanges();
                //Redirects to the list of distributees.
                return RedirectToAction("SelectUsers", "Revisions", new { newRevision.id });

            }
         
            return View(revision);
        }

        // Only accessible to the authors

        /// <summary>
        /// Displays a list of distributees to be selected
        /// </summary>
        /// <param name="id">The ID of the revision</param>
        /// <returns></returns>
        public ActionResult SelectUsers(string id)
        {

            var DistributeeList = new DistributeeSelectModel();
            DistributeeList.RevID = id;
            // List of all distributes.
            DistributeeList.Accounts = db.Accounts.Where(u => u.AccountType == AccountType.Distributee);
                
            return View(DistributeeList);
        }

        // Only accessible to authors

        /// <summary>
        /// Adds a new distributee to the revision
        /// </summary>
        /// <param name="userKey">The User to be added </param>
        /// <param name="rev">The Revision key</param>
        /// <returns></returns>
        public ActionResult AddNewDistributee(string userKey, string revID)
        {
            //Finds selected revision
            var revision = db.Revisions.Where(r=> r.id == revID).SingleOrDefault();

            //Finds the distributee to be added to the revision's distributee list.
            var user = db.Accounts.Find(userKey);
            if(user !=null && revision != null)
            {
                //Flag for if the user is already in the distributee list
                var alreadyContained = revision.Distributees.Contains(user);
                if(alreadyContained == false)
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

        // Only accessible to authors

        /// <summary>
        /// Removes a distributee from a revisions distributee list.
        /// </summary>
        /// <param name="userKey">distributee id</param>
        /// <param name="revID">revivion id</param>
        /// <returns></returns>
        public ActionResult RemoveDistributee(string userKey, string revID)
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

        // Only accessible to the author of the specific document.

        /// <summary>
        /// Allow the user to edit detailed of the specified revision.
        /// </summary>
        /// <param name="id">revision id</param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Retrieves the selected revision.
            var revision = db.Revisions.Where(r => r.id == id).SingleOrDefault();

            //If the revision is "Active"
            if (revision.State == DocumentState.Active)
            {
                this.AddNotification("Cannot edit Active documents, please create a new revision.", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
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

        // Only accessible to the author of the specific document.

        /// <summary>
        /// SAves edits made by the user, to the revision, to the database.
        /// </summary>
        /// <param name="revision"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,RevisionNum,DocumentTitle,State,ActivationDate")] Revision revision)
        {

            //Fuck C#, consistently reset the date to 01/01/0001 when the minimum value it accepts
            //is 01/01/1753. When the revision is passed in it still resets to 01/01/0001 however,
            //now it takes the creation date from the current version of the revision and sets it to
            //the creation date value of the updated revision.
            var currentR = db.Revisions.Where(r => r.id == revision.id).SingleOrDefault();
            revision.DocCreationDate = currentR.DocCreationDate;

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
        /// Deletes the selected revision
        /// </summary>
        /// <param name="id">revision id</param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Retrieves the selected revision
            Revision revision = db.Revisions.Where(r => r.id == id).SingleOrDefault();
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

        //Only accessible by the author of the specfic document.

        /// <summary>
        /// Confirms the removal of the selected revision.
        /// </summary>
        /// <param name="id">revision id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            // Retrieves the selected revision.
            Revision revision = db.Revisions.Find(id);
            // Removes the selected revision from the database.
            db.Revisions.Remove(revision);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Archives the revisions of the selected author
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns>True if success</returns>
        public bool ArchiveUserRevisions(string authorId)
        {

            //Retrieves revisions of author
            var authorRevisions = db.Revisions.Where(r => r.document.Author.Id == authorId);
            
            if(authorRevisions != null)
            {
                // Sets each revisions created by author to "Archived"
                foreach (var revision in authorRevisions)
                {
                    //Emailing updated status to distributees
                    EmailService emailService = new EmailService();
                    emailService.massMessageDistribution(revision.Distributees, 2);

                    //Texting updated status to distributees
                    SMSService smsService = new SMSService();
                    smsService.DetermineSMSMessage(revision.Distributees, 2);

                    revision.State = DocumentState.Archived;
                    db.Revisions.AddOrUpdate(revision);
                    db.SaveChanges();
                   
                }
                return true;
            }
            return false;
        }

        // Only accessible to the distributees and author of the specified document.

        /// <summary>
        /// Allows the user to dowload the file given
        /// </summary>
        /// <param name="docKey">They key of the file to be downloaded</param>
        /// <returns>The file to download if successful, otherwise nothing</returns>
        public FileResult Download(string docKey)
        {
            FileStoreService fss = new FileStoreService();

            //gets the file based on the file store key
            var file = fss.GetFile(docKey);

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
                    return File(fileBytes, "application/unknown", document.DocumentTitle);
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
