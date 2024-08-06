using BookStore.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class CouponsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Coupons
        public ActionResult Index(Coupon coupon)
        {
            var getUsersCount = db.Users.Where(s => s.Email != null).Count().ToString();
            ViewBag.ActiveUsers = getUsersCount;
            var CouponCount = coupon.CouponCounter.ToString();
            if (CouponCount == getUsersCount)
            { 
            coupon.CouponIsActive = false;
            }
            
            return View(db.Coupons.ToList());

        }

        // GET: Coupons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // GET: Coupons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Coupons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodeId,CouponCode,Discount,CouponCounter,CouponIsActive")] Coupon coupon)
        {
            
            coupon.CouponCounter = 0;
            if (ModelState.IsValid)
            {
                db.Coupons.Add(coupon);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(coupon);
        }

        // GET: Coupons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // POST: Coupons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodeId,CouponCode,Discount,CouponCounter,CouponIsActive")] Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coupon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(coupon);
        }

        // GET: Coupons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound();
            }
            return View(coupon);
        }

        // POST: Coupons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Coupon coupon = db.Coupons.Find(id);
            db.Coupons.Remove(coupon);
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

        public ActionResult SendCouponButton(int couponId)
        {

            var coupon = db.Coupons.FirstOrDefault(x => x.CouponIsActive == true && x.CodeId == couponId);
            var getUsers = db.Users.Where(x => x.Email != null).ToList();
            


            foreach (var item in getUsers)
            {
                SendCouponEmail(item.Email, item.Name, coupon.CouponCode);

            }

            return RedirectToAction("Index");
        }



        public void SendCouponEmail(string Email, string Name, string CouponCode)
        {
            MailMessage mail = new MailMessage();
            MailAddress from = new MailAddress("africanmagicsystem@gmail.com");
            mail.From = from;
            mail.Subject = "Get a Dicount";
            mail.Body = "Hi  " + Name + ".Use our Coupon to get discount on your purchase -  " + CouponCode;
            mail.To.Add(Email);

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential networkCredential = new NetworkCredential("africanmagicsystem@gmail.com", "zbpabilmryequenp");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.Send(mail);
            //Clean-up.           
            //Dispose of email.
            mail.Dispose();

        }






    }
}
