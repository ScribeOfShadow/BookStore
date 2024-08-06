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

namespace BookStore.Controllers
{
    public class UserLibrariesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserLibraries
        public async Task<ActionResult> Index()
        {
            return View(await db.UserLibraries.ToListAsync());
        }

        // GET: UserLibraries/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLibrary userLibrary = await db.UserLibraries.FindAsync(id);
            if (userLibrary == null)
            {
                return HttpNotFound();
            }
            return View(userLibrary);
        }

        // GET: UserLibraries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserLibraries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EntryId")] UserLibrary userLibrary)
        {
            if (ModelState.IsValid)
            {
                db.UserLibraries.Add(userLibrary);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(userLibrary);
        }

        // GET: UserLibraries/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLibrary userLibrary = await db.UserLibraries.FindAsync(id);
            if (userLibrary == null)
            {
                return HttpNotFound();
            }
            return View(userLibrary);
        }

        // POST: UserLibraries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EntryId")] UserLibrary userLibrary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userLibrary).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(userLibrary);
        }

        // GET: UserLibraries/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserLibrary userLibrary = await db.UserLibraries.FindAsync(id);
            if (userLibrary == null)
            {
                return HttpNotFound();
            }
            return View(userLibrary);
        }

        // POST: UserLibraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UserLibrary userLibrary = await db.UserLibraries.FindAsync(id);
            db.UserLibraries.Remove(userLibrary);
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
