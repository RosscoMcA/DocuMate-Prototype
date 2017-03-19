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
    public class DocumentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Documents
        public ActionResult Index()
        {
            var documents = db.Documents;
            var revisions = db.Revisions;
            DocumentViewModel DocViewModel = new DocumentViewModel();
            DocViewModel.Revisions = db.Revisions.ToList();
            DocViewModel.Documents = db.Documents.ToList();
            return View(DocViewModel);
        }

        // GET: Documents/Details/5
        public ActionResult Details(int? id)
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
            int marker = 0;
            int c = 0;
            var Rev = db.Revisions.ToList();
            foreach (var item in Rev)
            {
                if ((item.document == document) && (item.DocCreationDate > Rev.ElementAt(marker).DocCreationDate))
                {
                    marker = c;
                    c++;
                }
                else
                {
                    c++;
                }
            }
            string Title = Rev.ElementAt(marker).DocumentTitle;
            double RevisionNum = Rev.ElementAt(marker).RevisionNum;
            SDVMDetails SDVM = new SDVMDetails();
            SDVM.Document = document;
            SDVM.DocTitle = Title;
            SDVM.RevisionNum = RevisionNum;
            return View(SDVM);
        }

        // GET: Documents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Documents.Add(document);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(document);
        }

        // GET: Documents/Edit/5
        public ActionResult Edit(int? id)
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
            int marker = 0;
            int c = 0;
            var Rev = db.Revisions.ToList();
            foreach (var item in Rev)
            {
                if ((item.document == document) && (item.DocCreationDate > Rev.ElementAt(marker).DocCreationDate))
                {
                    marker = c;
                    c++;
                }
                else
                {
                    c++;
                }
            }
            string Title = Rev.ElementAt(marker).DocumentTitle;
            SingleDocViewModel SDVM = new SingleDocViewModel();
            SDVM.Document = document;
            SDVM.DocTitle = Title; 
            return View(SDVM);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            int marker = 0;
            int c = 0;
            var Rev = db.Revisions.ToList();
            foreach (var item in Rev)
            {
                if ((item.document == document) && (item.DocCreationDate > Rev.ElementAt(marker).DocCreationDate))
                {
                    marker = c;
                    c++;
                }
                else
                {
                    c++;
                }
            }
            string Title = Rev.ElementAt(marker).DocumentTitle;
            SingleDocViewModel SDVM = new SingleDocViewModel();
            SDVM.Document = document;
            SDVM.DocTitle = Title;
            return View(SDVM);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(int? id)
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
            int marker = 0;
            int c = 0;
            var Rev = db.Revisions.ToList();
            foreach (var item in Rev)
            {
                if ((item.document == document) && (item.DocCreationDate > Rev.ElementAt(marker).DocCreationDate))
                {
                    marker = c;
                    c++;
                }
                else
                {
                    c++;
                }
            }
            string Title = Rev.ElementAt(marker).DocumentTitle;
            SingleDocViewModel SDVM = new SingleDocViewModel();
            SDVM.Document = document;
            SDVM.DocTitle = Title;
            return View(SDVM);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Document document = db.Documents.Find(id);
            db.Documents.Remove(document);
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

        public ActionResult DetailsOfDoc(double? id)
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
    }
}
