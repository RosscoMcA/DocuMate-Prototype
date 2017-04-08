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

namespace IntegratedProject3.Controllers
{
    public class RevisionsController : RootController
    {
        
        /// <summary>
        /// Retrieves the Revisions Index. Displaying all revisions.
        /// </summary>
        /// <returns>ViewResult containing a list of Revisions.</returns>
        [Authorize]
        public ActionResult Index()
        {
            var revisions = db.Revisions.ToList();
            return View(revisions);
        }

        /// <summary>
        /// Retrieves revisions associated with a specific document.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ViewResult containing list of Revisions</returns>
        [Authorize]
        public ActionResult DocumentRevisions(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var revisions = db.Revisions.Where(r => r.document.id == id);
            if (revisions == null)
            {
                return HttpNotFound();
            }

            return View(revisions.ToList());
        }

        // GET: Revisions/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revision revision = db.Revisions.Find(id);
            if (revision == null)
            {
                return HttpNotFound();
            }
            return View(revision);
        }

        // GET: Revisions/Create
        [Authorize]
        public ActionResult Create()
        {
            string id = "f4d80ac9-1358-459f-b3af-b6cdbe34da82";
            var document = db.Documents.Where(d => d.id == id).SingleOrDefault();
            if (document != null)
            {

                IEnumerable<Account> accounts = db.Accounts.ToList();
                var createRevisionModel = new RevisonViewModel();
                createRevisionModel.DocID = id;
                return View(createRevisionModel);

            }
            return View();
        }

        // POST: Revisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create([Bind(Include = "id, RevisionNum,DocumentTitle")] RevisonViewModel revision)
        {

                if (ModelState.IsValid)
                {
                string id = "f4d80ac9-1358-459f-b3af-b6cdbe34da82";
                var doc = db.Documents.Where(d => d.id == id).SingleOrDefault();
                Revision newRevision = new Revision()
                {
                    id = Guid.NewGuid().ToString(),
                    DocumentTitle = revision.DocumentTitle,
                    RevisionNum = revision.RevisionNum,
                    DocCreationDate = DateTime.Now,
                    State = DocumentState.Draft,
                    ActivationDate = DateTime.Now,
                    document = doc,
                    Distributees = new HashSet<Account>()
                    
                    

            };

                    
                    db.Revisions.Add(newRevision);
                    db.SaveChanges();
                return RedirectToAction("SelectUsers", "Revisions", new { newRevision.id });
                }
         
            return View(revision);
        }

        /// <summary>
        /// Displays a list of distributees to be selected
        /// </summary>
        /// <param name="id">The ID of the revision</param>
        /// <returns></returns>
        public ActionResult SelectUsers(string id)
        {
            var DistributeeList = new DistributeeSelectModel();
            DistributeeList.RevID = id;
            DistributeeList.Accounts = db.Accounts.Where(u => u.AccountType == AccountType.Distributee);
                
            return View(DistributeeList);
        }
        /// <summary>
        /// Adds a new distributee to the revision
        /// </summary>
        /// <param name="userKey">The User to be added </param>
        /// <param name="rev">The Revision key</param>
        /// <returns></returns>
       
        public ActionResult AddNewDistributee(string userKey, string revID)
        {
            var revision = db.Revisions.Where(r=> r.id == revID).SingleOrDefault();

            var user = db.Accounts.Find(userKey);
            if(user !=null && revision != null)
            {

                var alreadyContained = revision.Distributees.Contains(user);
                if(alreadyContained == false)
                {
                    revision.Distributees.Add(user);
                    db.Revisions.AddOrUpdate(revision);
                    db.SaveChanges();
                }
                else
                {
                    //Insert Notification here
                }
            }

            return RedirectToAction("SelectUsers", "Revisions", new { revID } );

        }

        // GET: Revisions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var revision = db.Revisions.Where(r => r.id == id).SingleOrDefault();

            if (revision.State == DocumentState.Active)
            {
                throw new Exception("Only draft documents can be edited. Please create a new revision.");
            }

            if (revision == null)
            {
                return RedirectToAction("Index", "Revisions");
            }

            // This block is temporarily removed for testing purposes.
            // Due to this branch not containing the document systems implemented.

            //if (!(VerifyAuthor(revision)))
            //{
            //    new Exception("current user is not author of the document");
            //    return RedirectToAction("index");
            //}

            return View(revision);
        }

        // POST: Revisions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

            if (revision.ActivationDate == null && revision.State == DocumentState.Active)
            {
                revision.ActivationDate = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                db.Revisions.AddOrUpdate(revision);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(revision);
        }

        // GET: Revisions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revision revision = db.Revisions.Where(r => r.id == id).SingleOrDefault();
            if (revision == null)
            {
                return HttpNotFound();
            }

            if (!(VerifyAuthor(revision)))
            {
                throw new Exception("Only the author can delete a revision.");
            }

            return View(revision);
        }

        // POST: Revisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Revision revision = db.Revisions.Find(id);
            db.Revisions.Remove(revision);
            db.SaveChanges();
            return RedirectToAction("Index");
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
