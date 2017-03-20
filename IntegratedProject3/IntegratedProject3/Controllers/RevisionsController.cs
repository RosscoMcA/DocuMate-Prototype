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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Revisions
        [Authorize]
        public ActionResult Index()
        {
            int docID = 1;
            var revisions = db.Revisions.Where(r => r.document.ID == docID);
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
            return View();
        }

        // POST: Revisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RevisionNum,DocumentTitle,DocCreationDate,State,ActivationDate")] Revision revision)
        {

                if (ModelState.IsValid)
                {
                    revision.id = Guid.NewGuid().ToString();
                    revision.ActivationDate = DateTime.Now;
                    revision.State = DocumentState.Draft;
                    db.Revisions.Add(revision);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
         
            return View(revision);
        }

        // GET: Revisions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var revision = db.Revisions.Where(r => r.id == id).SingleOrDefault();

            if (revision == null)
            {
                return RedirectToAction("Index", "Revisions");
            }

            if (!(VerifyAuthor(revision)))
            {
                new Exception("current user is not author of the document");
                return RedirectToAction("index");
            }

            return View(revision);
        }

        // POST: Revisions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,RevisionNum,DocumentTitle,DocCreationDate,State,ActivationDate")] Revision revision)
        {
                
                if(revision.State == DocumentState.Draft)
                {
                    throw new Exception("Only draft documents can be edited. Please create a new revision.");
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
