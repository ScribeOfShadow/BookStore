using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BookStore.Controllers
{
    public class WishListsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WishLists
        public ActionResult Index()
        {
            return View(db.WishLists.ToList());
        }
        public ActionResult Wlist(WishLists wishLists)
        {

            wishLists.UserId = User.Identity.GetUserId();
            var wlist = db.WishLists.Include(b => b.ApplicationUser).Where(x => x.UserId == wishLists.UserId).ToList();
            return View(wlist);
        }

        // GET: WishLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishLists wishLists = db.WishLists.Find(id);
            if (wishLists == null)
            {
                return HttpNotFound();
            }
            return View(wishLists);
        }

        // GET: WishLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WishLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ListId,ListName,UserId")] WishLists wishLists)
        {

            var userId = User.Identity.GetUserId();
            wishLists.UserId = userId;

            if (ModelState.IsValid)
            {
                db.WishLists.Add(wishLists);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wishLists);
        }

        // GET: WishLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishLists wishLists = db.WishLists.Find(id);
            if (wishLists == null)
            {
                return HttpNotFound();
            }
            return View(wishLists);
        }

        // POST: WishLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ListId,ListName")] WishLists wishLists)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wishLists).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(wishLists);
        }

        // GET: WishLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishLists wishLists = db.WishLists.Find(id);
            if (wishLists == null)
            {
                return HttpNotFound();
            }
            return View(wishLists);
        }

        // POST: WishLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WishLists wishLists = db.WishLists.Find(id);
            db.WishLists.Remove(wishLists);
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