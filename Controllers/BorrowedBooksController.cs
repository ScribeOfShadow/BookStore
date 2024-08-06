using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;

namespace BookStore.Controllers
{
    public class BorrowedBooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BorrowedBooks
        public ActionResult Index()
        {

            var borrowedBooks = db.BorrowedBooks.Include(b => b.ApplicationUser).Include(b => b.Library);
            return View(borrowedBooks.ToList());
        }

        public ActionResult IndexBorrowed(BorrowViewModel borrow, BookQrCodes BookQrCodes)
        {
            db.BookQrCodes.Add(BookQrCodes);
            var userId = User.Identity.GetUserId();
            var borrowedBooks = db.BorrowedBooks.Include(b => b.ApplicationUser).Include(b => b.Library).Include(b => BookQrCodes).Where(x => x.UserId == userId).ToList();
            return View(borrowedBooks);
        }

        // GET: BorrowedBooks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BorrowedBooks borrowedBooks = db.BorrowedBooks.Find(id);
            if (borrowedBooks == null)
            {
                return HttpNotFound();
            }
            return View(borrowedBooks);
        }

        // GET: BorrowedBooks/Create
        public ActionResult Create()
        {
            //ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "Name");
            ViewBag.BookId = new SelectList(db.Libraries, "LibraryId", "LibraryProductName");
            return View();
        }

        // POST: BorrowedBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,BookId,DateBorrowed,DateReturned,")] BorrowedBooks borrowedBooks)
        {

            if (ModelState.IsValid)
            {
                db.BorrowedBooks.Add(borrowedBooks);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "Name", borrowedBooks.UserId);
            ViewBag.BookId = new SelectList(db.Libraries, "LibraryId", "LibraryProductName", borrowedBooks.BookId);
            return View(borrowedBooks);
        }

        // GET: BorrowedBooks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BorrowedBooks borrowedBooks = db.BorrowedBooks.Find(id);
            if (borrowedBooks == null)
            {
                return HttpNotFound();
            }
            //ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "Name", borrowedBooks.UserId);
            ViewBag.BookId = new SelectList(db.Libraries, "LibraryId", "LibraryProductName", borrowedBooks.BookId);
            return View(borrowedBooks);
        }

        // POST: BorrowedBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,BookId,DateBorrowed,DateReturned,IsAssigned")] BorrowedBooks borrowedBooks )
        {
           
            if (ModelState.IsValid)
            {
                db.Entry(borrowedBooks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "Name", borrowedBooks.UserId);
            ViewBag.BookId = new SelectList(db.Libraries, "LibraryId", "LibraryProductName", borrowedBooks.BookId);
            return View(borrowedBooks);
        }

        // GET: BorrowedBooks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BorrowedBooks borrowedBooks = db.BorrowedBooks.Find(id);
            if (borrowedBooks == null)
            {
                return HttpNotFound();
            }
            return View(borrowedBooks);
        }

        // POST: BorrowedBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BorrowedBooks borrowedBooks = db.BorrowedBooks.Find(id);
            db.BorrowedBooks.Remove(borrowedBooks);
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
