using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntegratedProject3.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace IntegratedProject3.Controllers
{
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// List all the documents in the system. Only accessible to the admin.
        /// </summary>
        /// <returns>View of all documents</returns>
        public ActionResult Index()
        {
            var doc = db.Documents.ToList();
            var docView = new HashSet<DocumentViewModel>();
            foreach (var singleDoc in doc)
            {
                if (singleDoc.Revisions != null)
                {
                    var revision = db.Revisions.Where(r => r.document.id == singleDoc.id && r.State == DocumentState.Active).SingleOrDefault();
                    if (revision != null)
                    {
                        var newDoc = new DocumentViewModel()
                        {
                            id = singleDoc.id,
                            ActivationDate = revision.ActivationDate.Value,
                            Author = singleDoc.Author,
                            DocCreationDate = revision.DocCreationDate,
                            DocumentTitle = revision.DocumentTitle,
                            RevisionNum = revision.RevisionNum
                        };
                        docView.Add(newDoc);
                    }
                }
            }
            return View(docView.ToList());
        }

        /// <summary>
        /// List of all a document's revisions. Only accessible by an author.
        /// </summary>
        /// <param name="id">document id</param>
        /// <returns>View of a specific document's revisions</returns>
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Finds the specified document using the ID.
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        
        /// <summary>
        /// Saves the document to database. Only accessible by authors.
        /// </summary>
        /// <param name="document"></param>
        /// <returns>Redirect to Revision/Create</returns>
        
        public ActionResult Create([Bind(Include = "id")] Document document)
        {
            if (ModelState.IsValid)
            {
                //Auto generated ID
                document.id = Guid.NewGuid().ToString();
                //Sets the document's author to the current user.
                document.Author = db.Accounts.Find(User.Identity.GetUserId());
                //Saves the document to the database.
                db.Documents.Add(document);
                db.SaveChanges();
                //Redirects the Revision/Create so the author can create the first revision for the document.
                return RedirectToAction("Create", "Revisions", new { id = document.id });
            }

            return View(document);
        }

        /// <summary>
        /// Retrieves Document/Edit. Only accessible by the author.
        /// </summary>
        /// <param name="id">document ID</param>
        /// <returns>View of Document/Edit</returns>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Finds the specficied document.
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        /// <summary>
        /// Saves the updated document to the database. Only accessible by the author.
        /// </summary>
        /// <param name="document">Updated document</param>
        /// <returns>Redirect to Document/Index</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(document);
        }

        /// <summary>
        /// Deletes the document from the database.
        /// </summary>
        /// <param name="id">document ID</param>
        /// <returns>View of Document Delete</returns>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        /// <summary>
        /// Confirms the deletion of a document
        /// </summary>
        /// <param name="id">document ID</param>
        /// <returns>Redirect to Document/Index</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //New instance of email service.
            EmailService emailService = new EmailService();
            //Find the specified document.
            Document document = db.Documents.Find(id);
            //Removes the document from the database.
            db.Documents.Remove(document);
            db.SaveChanges();
            //Redirects to Document/Index
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
