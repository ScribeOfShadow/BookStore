using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using System.IO;
using System.Drawing;
using ZXing.Common;
using ZXing.Rendering;
using ZXing;

namespace BookStore.Controllers
{
    public class BookQrCodesController : Controller
    {
        private static readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: BookQrCodes
        public async Task<ActionResult> Index()
        {
            var bookQrCodes = db.BookQrCodes.Include(b => b.Library);
            return View(await bookQrCodes.ToListAsync());
        }

        // GET: BookQrCodes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookQrCodes bookQrCodes = await db.BookQrCodes.FindAsync(id);
            if (bookQrCodes == null)
            {
                return HttpNotFound();
            }
            return View(bookQrCodes);
        }

        // GET: BookQrCodes/Create

     
        // POST: BookQrCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,LibraryBook,PlainText,RenderedImagePath,RenderedBytes,ScannedCount")] BookQrCodes bookQrCodes)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.BookQrCodes.Add(bookQrCodes);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.LibraryBook = new SelectList(db.Libraries, "LibraryId", "LibraryProductName", bookQrCodes.LibraryBook);
        //    return View(bookQrCodes);
        //}

        // GET: BookQrCodes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookQrCodes bookQrCodes = await db.BookQrCodes.FindAsync(id);
            if (bookQrCodes == null)
            {
                return HttpNotFound();
            }
            ViewBag.LibraryBook = new SelectList(db.Libraries, "LibraryId", "LibraryProductName", bookQrCodes.LibraryBookId);
            return View(bookQrCodes);
        }

        // POST: BookQrCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,LibraryBook,PlainText,RenderedImagePath,RenderedBytes,ScannedCount")] BookQrCodes bookQrCodes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookQrCodes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.LibraryBook = new SelectList(db.Libraries, "LibraryId", "LibraryProductName", bookQrCodes.LibraryBookId);
            return View(bookQrCodes);
        }

        // GET: BookQrCodes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookQrCodes bookQrCodes = await db.BookQrCodes.FindAsync(id);
            if (bookQrCodes == null)
            {
                return HttpNotFound();
            }
            return View(bookQrCodes);
        }

        // POST: BookQrCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BookQrCodes bookQrCodes = await db.BookQrCodes.FindAsync(id);
            db.BookQrCodes.Remove(bookQrCodes);
            await db.SaveChangesAsync();
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
