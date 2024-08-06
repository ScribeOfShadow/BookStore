using BookStore.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class UsedCouponListsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UsedCouponLists
        public async Task<ActionResult> Index()
        {
            return View(await db.UsedCouponLists.ToListAsync());
        }

        // GET: UsedCouponLists/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsedCouponList usedCouponList = await db.UsedCouponLists.FindAsync(id);
            if (usedCouponList == null)
            {
                return HttpNotFound();
            }
            return View(usedCouponList);
        }

        // GET: UsedCouponLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsedCouponLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CouponEntry,userId,CouponUsed,CouponUserIsValid,CouponDiscountAmount")] UsedCouponList usedCouponList, Coupon coupon, Cart cart)
        {
            //Queries
            var ValidCoupon = db.Coupons.Where(x => x.CouponCode != null && x.CouponIsActive == true).ToList();
            var getUsersInTable = db.UsedCouponLists.Where(x => x.userId != null).ToList();
            var getUsers = db.Users.Where(x => x.Id != null).ToList();

            //ViewBag

            if (usedCouponList.CouponEntry != null)
            {
                foreach (var item in ValidCoupon)
                {
                    foreach (var user in getUsers)
                    {
                        usedCouponList.userId = User.Identity.Name;

                        if (getUsersInTable.ToList().Count <= 0)
                        {
                            if (usedCouponList.userId != null && usedCouponList.CouponEntry.Equals(item.CouponCode))
                            {

                                usedCouponList.CouponUsed = DateTime.Now;
                                usedCouponList.CouponUserIsValid = true;
                                usedCouponList.CouponDiscountAmount = item.Discount;
                               
                                

                                db.UsedCouponLists.Add(usedCouponList);
                                db.SaveChanges();
                                return RedirectToAction("Index", "ShoppingCart");

                            }
                            else if (usedCouponList.userId != null && usedCouponList.CouponEntry != item.CouponCode)
                            {
                                TempData["Result"] = "Code Is Invalid";
                                return RedirectToAction("Index", "ShoppingCart");
                            }
                        }
                        else if (getUsersInTable.ToList().Count > 0)
                        {
                            foreach (var codeUser in getUsersInTable)
                            {

                                if (usedCouponList.userId != codeUser.userId.ToString() && usedCouponList.CouponEntry.Equals(item.CouponCode))    
                                {
                                    usedCouponList.CouponUsed = DateTime.Now;
                                    usedCouponList.CouponUserIsValid = true;
                                    usedCouponList.CouponDiscountAmount = item.Discount;

                                    db.UsedCouponLists.Add(usedCouponList);
                                    db.SaveChanges();
                                    return RedirectToAction("Index", "ShoppingCart");

                                }
                                else if (usedCouponList.userId != null && usedCouponList.CouponEntry.Equals(item.CouponCode) && usedCouponList.CouponEntry != codeUser.CouponEntry)
                                {

                                    usedCouponList.CouponUsed = DateTime.Now;
                                    usedCouponList.CouponUserIsValid = true;
                                    usedCouponList.CouponDiscountAmount = item.Discount;

                                    db.UsedCouponLists.Add(usedCouponList);
                                    db.SaveChanges();
                                    return RedirectToAction("Index", "ShoppingCart");

                                }
                                else if (usedCouponList.userId != null && usedCouponList.CouponEntry != item.CouponCode) 
                                {

                                    TempData["Result"] = "Code Is Invalid";
                                    return RedirectToAction("Create");

                                }
                                else if (usedCouponList.CouponEntry.Equals(item.CouponCode) && usedCouponList.userId.Equals(codeUser.userId))
                                {
                                    //Put Message here
                                    TempData["Result"] = "Code Is Already Used";
                                    return RedirectToAction("Index", "ShoppingCart");
                                }

                            }
                        }
                    }
                }
            }
            return View(usedCouponList);
        }

        // GET: UsedCouponLists/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsedCouponList usedCouponList = await db.UsedCouponLists.FindAsync(id);
            if (usedCouponList == null)
            {
                return HttpNotFound();
            }
            return View(usedCouponList);
        }

        // POST: UsedCouponLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CouponEntry,userId,CouponUsed")] UsedCouponList usedCouponList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usedCouponList).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(usedCouponList);
        }

        // GET: UsedCouponLists/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsedCouponList usedCouponList = await db.UsedCouponLists.FindAsync(id);
            if (usedCouponList == null)
            {
                return HttpNotFound();
            }
            return View(usedCouponList);
        }

        // POST: UsedCouponLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UsedCouponList usedCouponList = await db.UsedCouponLists.FindAsync(id);
            db.UsedCouponLists.Remove(usedCouponList);
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
