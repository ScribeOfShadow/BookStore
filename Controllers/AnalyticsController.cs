using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class AnalyticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public JsonResult WorkFlowGraphData()
        {
            var results = db.Analytics
                .GroupBy(x => new
                {
                    x.BookName,
                    //x.Rating
                })
                .Select(x => new
                {
                    Name = x.Key.BookName,
                    FiveStars = x.Count(r => r.Rating == 5),
                    FourStars = x.Count(r => r.Rating == 4),
                    ThreeStar = x.Count(r => r.Rating == 3),
                    TwoStars = x.Count(r => r.Rating == 2),
                    OneStars = x.Count(r => r.Rating == 1)
                })
                .OrderBy(x => x.Name).ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult WorkFlowGraphDataMostSold()
        {
            List<object> vals = new List<object>();
            var list = db.Analytics.GroupBy(x => new { x.BookName }).Select(x => new
            {
                x.Key,
                count = x.Count()
            }).OrderBy(x => x.count).ToList();
            var max = list.OrderByDescending(x => x.count).FirstOrDefault();
            var min = list.OrderBy(x => x.count).FirstOrDefault();

            vals.Add(list.FirstOrDefault());
            vals.Add(list.OrderByDescending(x => x.count).FirstOrDefault());

            //var list2 = db.Analytics.GroupBy(x => new { x.BookName }).Select(x => new
            //{
            //    bookname = x.Key,
            //    MostSold = x.OrderByDescending(y=>y.BookName).Count(),
            //    LeastSold = x.OrderByDescending(y=>y.BookName).Count()
            //}).ToList();
            return Json(vals, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult WorkFlowGraphDataMostCommented()
        {
            List<object> vals = new List<object>();
            var list = db.Analytics.Where(x => x.Comment != null && x.Comment != "").GroupBy(x => new { x.BookName }).Select(x => new
            {
                x.Key,
                count = x.Count()
            }).OrderBy(x => x.count).ToList();
            var max = list.OrderByDescending(x => x.count).FirstOrDefault();
            var min = list.OrderBy(x => x.count).FirstOrDefault();

            vals.Add(list.FirstOrDefault());
            vals.Add(list.OrderByDescending(x => x.count).FirstOrDefault());

            //var list2 = db.Analytics.GroupBy(x => new { x.BookName }).Select(x => new
            //{
            //    bookname = x.Key,
            //    MostSold = x.OrderByDescending(y=>y.BookName).Count(),
            //    LeastSold = x.OrderByDescending(y=>y.BookName).Count()
            //}).ToList();
            return Json(vals, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Analytics()
        {
            return View();
        }

        // GET: Analytics
        public ActionResult Index()
        {
            return View(db.Analytics.ToList());
        }

        // GET: Analytics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Analytic analytic = db.Analytics.Find(id);
            if (analytic == null)
            {
                return HttpNotFound();
            }
            return View(analytic);
        }

        // GET: Analytics/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Analytics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BookId,BookName,BookDescription,SoldDateTime,Comment,Rating")] Analytic analytic)
        {
            if (ModelState.IsValid)
            {
                db.Analytics.Add(analytic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(analytic);
        }

        // GET: Analytics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Analytic analytic = db.Analytics.Find(id);
            if (analytic == null)
            {
                return HttpNotFound();
            }
            return View(analytic);
        }

        // POST: Analytics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BookId,BookName,BookDescription,SoldDateTime,Comment,Rating")] Analytic analytic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(analytic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(analytic);
        }

        // GET: Analytics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Analytic analytic = db.Analytics.Find(id);
            if (analytic == null)
            {
                return HttpNotFound();
            }
            return View(analytic);
        }

        // POST: Analytics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Analytic analytic = db.Analytics.Find(id);
            db.Analytics.Remove(analytic);
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
