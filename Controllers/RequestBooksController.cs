using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class RequestBooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RequestBooks
        public ActionResult Index()
        {
            ViewBag.Approved = "Approved";
            ViewBag.Pending = "Pending";

            return View(db.RequestBooks.ToList());
        }

        // GET: RequestBooks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestBook requestBook = db.RequestBooks.Find(id);
            if (requestBook == null)
            {
                return HttpNotFound();
            }
            return View(requestBook);
        }

        // GET: RequestBooks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RequestBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Book_RequestId,Book_Request,Date_Requested,Book_Author,Admindecision")] RequestBook requestBook)
        {
            requestBook.Date_Requested = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.RequestBooks.Add(requestBook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(requestBook);
        }

        // GET: RequestBooks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestBook requestBook = db.RequestBooks.Find(id);
            if (requestBook == null)
            {
                return HttpNotFound();
            }
            return View(requestBook);
        }

        // POST: RequestBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Book_RequestId,Book_Request,Date_Requested,Book_Author,Admindecision")] RequestBook requestBook)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestBook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requestBook);
        }

        // GET: RequestBooks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestBook requestBook = db.RequestBooks.Find(id);
            if (requestBook == null)
            {
                return HttpNotFound();
            }
            return View(requestBook);
        }

        // POST: RequestBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestBook requestBook = db.RequestBooks.Find(id);
            db.RequestBooks.Remove(requestBook);
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

