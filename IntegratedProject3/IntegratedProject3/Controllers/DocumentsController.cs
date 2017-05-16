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
using IntegratedProject3.Extensions;

namespace IntegratedProject3.Controllers
{
    public class DocumentsController : RootController
    {

        /// <summary>
        /// List all the documents in the system. Only accessible to the admin.
        /// </summary>
        /// <returns>View of all documents</returns>
        public ActionResult Index()
        {
            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();

            if (isAuthor())
            {
                var userId = User.Identity.GetUserId();
                var documents = db.Documents.Where(d => d.Author.Id == userId).ToList();
                return ViewDocuments(documents);
            }

            if (isAdmin())
            {
                var documents = db.Documents.ToList();
                return ViewDocuments(documents);
            }

                this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
                return RedirectToAction("Index", "Home");
            
        }

        public ActionResult ViewDocuments(List<Document> doc)
        {
            
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
                            ActivationDate = revision.ActivationDate,
                            Author = singleDoc.Author,
                            DocCreationDate = revision.DocCreationDate,
                            DocumentTitle = revision.DocumentTitle,
                            RevisionNum = revision.RevisionNum
                        };
                        docView.Add(newDoc);
                    }
                    else
                    {
                        var draftRevision = db.Revisions.Where(r => r.document.id == singleDoc.id && r.State == DocumentState.Draft).SingleOrDefault();
                        if (draftRevision != null)
                        {
                            var newDoc = new DocumentViewModel()
                            {
                                id = singleDoc.id,
                                ActivationDate = draftRevision.ActivationDate,
                                Author = singleDoc.Author,
                                DocCreationDate = draftRevision.DocCreationDate,
                                DocumentTitle = draftRevision.DocumentTitle,
                                RevisionNum = draftRevision.RevisionNum
                            };
                            docView.Add(newDoc);
                        }
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

            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();

            var document = db.Documents.Find(id);
            if (VerifyAuthor(document) || isAdmin())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (document == null)
                {
                    return HttpNotFound();
                }
                return View(document);

            }

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        
        /// <summary>
        /// Saves the document to database. Only accessible by authors.
        /// </summary>
        /// <param name="document"></param>
        /// <returns>Redirect to Revision/Create</returns>
        
        public ActionResult Create([Bind(Include = "id")] Document document)
        {
            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();

            if (isAuthor())
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

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Retrieves Document/Edit. Only accessible by the author.
        /// </summary>
        /// <param name="id">document ID</param>
        /// <returns>View of Document/Edit</returns>
        public ActionResult Edit(string id)
        {

            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();

            var document = db.Documents.Find(id);
            if (VerifyAuthor(document))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                if (document == null)
                {
                    return HttpNotFound();
                }
                return View(document);
            }

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
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
            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();

            if (VerifyAuthor(document))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(document).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(document);
            }

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Deletes the document from the database.
        /// </summary>
        /// <param name="id">document ID</param>
        /// <returns>View of Document Delete</returns>
        public ActionResult Delete(string id)
        {
            ViewBag.isAuthor = isAuthor();
            ViewBag.isAdmin = isAdmin();

            var document = db.Documents.Find(id);
            if (VerifyAuthor(document) || isAdmin())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
               
                if (document == null)
                {
                    return HttpNotFound();
                }
                return View(document);
            }

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
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
            var document = db.Documents.Find(id);
            if (VerifyAuthor(document) || isAdmin())
            {
                //New instance of email service.
                EmailService emailService = new EmailService();
                //Removes the document from the database.
                var revisions = document.Revisions.ToList();
               

                foreach (var rev in revisions)
                {
                    rev.Distributees =null;
                    rev.document = null;
                    db.Revisions.Remove(rev);
                    
                }
                
                db.SaveChanges();
                //Redirects to Document/Index
                return RedirectToAction("Index");
            }

            this.AddNotification("Sorry! You do not have permisson to access this page!", NotificationType.ERROR);
            return RedirectToAction("Index", "Home");
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
