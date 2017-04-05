using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IntegratedProject3.Models;

namespace IntegratedProject3.Controllers
{
    public class RevisionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Revisions
        public ActionResult Index()
        {
            return View(db.Revisions.ToList());
        }

        // GET: Revisions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revisions revisions = db.Revisions.Find(id);
            if (revisions == null)
            {
                return HttpNotFound();
            }
            return View(revisions);
        }

        // GET: Revisions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Revisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,RevisionNum,DocumentTitle,DocCreationDate,State,ActivationDate")] Revisions revisions, string id)
        {
            ViewBag.id = id;
        
            if (ModelState.IsValid)
            {
                revisions.document.id = id;
                revisions.id = Guid.NewGuid().ToString();
                db.Revisions.Add(revisions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(revisions);
        }

        // GET: Revisions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revisions revisions = db.Revisions.Find(id);
            if (revisions == null)
            {
                return HttpNotFound();
            }
            return View(revisions);
        }

        // POST: Revisions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,RevisionNum,DocumentTitle,DocCreationDate,State,ActivationDate")] Revisions revisions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(revisions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(revisions);
        }

        // GET: Revisions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revisions revisions = db.Revisions.Find(id);
            if (revisions == null)
            {
                return HttpNotFound();
            }
            return View(revisions);
        }

        // POST: Revisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Revisions revisions = db.Revisions.Find(id);
            db.Revisions.Remove(revisions);
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
