using BookStore.Helpers;
using BookStore.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class ReturnAndRefundsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: ReturnAndRefunds
        public async Task<ActionResult> Index()
        {
            var returnAndRefunds = db.ReturnAndRefunds.Include(r => r.ReasonDrop).Include(r => r.RNRData).Include(r => r.Sale).Include(r => r.Status).ToList();
            return View(returnAndRefunds);
        }


        public async Task<ActionResult> UserReturnsAndRefunds(string Name)
        {
            Name = User.Identity.Name;
            var returnAndRefunds = db.ReturnAndRefunds.Include(r => r.ReasonDrop).Include(r => r.RNRData).Include(x => x.Products).Include(r => r.Sale).Include(r => r.Status).Where(x => x.CustomerName == Name).ToList();
            return View(returnAndRefunds);
        }

        // GET: ReturnAndRefunds/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReturnAndRefund returnAndRefund = await db.ReturnAndRefunds.FindAsync(id);
            if (returnAndRefund == null)
            {
                return HttpNotFound();
            }
            return View(returnAndRefund);
        }

        // GET: ReturnAndRefunds/Create
        public ActionResult Create()
        {
            ViewBag.ReasonId = new SelectList(db.ReasonDrops, "Id", "Name");
            ViewBag.RNRId = new SelectList(db.RNRDatas, "Id", "Name");
            ViewBag.OrderId = new SelectList(db.Sales, "SaleId", "SaleDate");
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
            return View();
        }

        // POST: ReturnAndRefunds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,OrderId,RNRId,ReasonId,AdditionalComments,CustomerName,StatusId,AdminComments")] ReturnAndRefund returnAndRefund)
        {
            if (ModelState.IsValid)
            {
                db.ReturnAndRefunds.Add(returnAndRefund);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ReasonId = new SelectList(db.ReasonDrops, "Id", "Name", returnAndRefund.ReasonId);
            ViewBag.RNRId = new SelectList(db.RNRDatas, "Id", "Name", returnAndRefund.RNRId);
            ViewBag.OrderId = new SelectList(db.Sales, "SaleId", "SaleDate", returnAndRefund.OrderId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", returnAndRefund.StatusId);
            return View(returnAndRefund);
        }

        // GET: ReturnAndRefunds/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReturnAndRefund returnAndRefund = await db.ReturnAndRefunds.FindAsync(id);
            if (returnAndRefund == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReasonId = new SelectList(db.ReasonDrops, "Id", "Name", returnAndRefund.ReasonId);
            ViewBag.RNRId = new SelectList(db.RNRDatas.Where(x => x.CodeKey == null || x.CodeKey == ""), "Id", "Name", returnAndRefund.RNRId);
            ViewBag.OrderId = new SelectList(db.Sales, "SaleId", "TranscationId", returnAndRefund.OrderId);
            ViewBag.StatusId = new SelectList(db.Statuses.Where(x => x.Name == "Approved" || x.Name == "Declined"), "Id", "Name", returnAndRefund.StatusId);
            return View(returnAndRefund);
        }

        // POST: ReturnAndRefunds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OrderId,RNRId,ReasonId,AdditionalComments,CustomerName,StatusId,AdminComments")] ReturnAndRefund returnAndRefund)
        {
            StatusHelper.CheckStatus();
            if (ModelState.IsValid)
            {
                var FindRetu = _db.ReturnAndRefunds.Include(x => x.Sale).Include(x => x.Status).Include(x => x.ApplicationUser).Include(x => x.RNRData).FirstOrDefault(x => x.Id == returnAndRefund.Id) ?? null;
                returnAndRefund.ReferenceNumber = FindRetu.ReferenceNumber;
                returnAndRefund.IsCompleted = true;
                db.Entry(returnAndRefund).State = EntityState.Modified;
                await db.SaveChangesAsync();


                string Subject = "Request Reference Number :" + returnAndRefund.ReferenceNumber;
                string body = string.Format("Dear valued customer <br>" + "You request for a {1} has been {0}, Thank you ", db.Statuses.FirstOrDefault(x => x.Id == returnAndRefund.StatusId).Name ?? "",
                    db.RNRDatas.FirstOrDefault(x => x.Id == returnAndRefund.RNRId).Name);
                string StatusMade = db.Statuses.FirstOrDefault(x => x.Id == returnAndRefund.StatusId).Name;
                var Refund = db.RNRDatas.FirstOrDefault(x => x.Name.ToLower().Contains("refund")) ?? null;
                if (Refund.Id == returnAndRefund.RNRId && !StatusMade.ToLower().Contains("declin"))
                {
                    body += "   - A driver will be sent to pick up the goods ";
                }
                SendMail(returnAndRefund.CustomerName, body, Subject);


                try
                {

                    if (!StatusMade.ToLower().Contains("declin"))
                    {
                        Delivery delivery = new Delivery
                        {
                            SaleId = FindRetu.Sale.SaleId,
                            DeliveryFee = 0,
                            CurrentLocation = FindRetu.Sale.Address,
                            isDelivered = false,
                            IsPickedUpForReturn = false,
                            IsForReturn = true,
                            DeliveryDate = DateTime.Now.AddDays(5).ToString(),
                        };
                        db.Deliveries.Add(delivery);
                        db.SaveChanges();



                        #region Push To Evaluation
                        string refno = String.Format($"EVL-0" + (db.Evaluations.Where(x => x.IsActive == true).Count() + 1).ToString());
                        Status Idstatus = StatusHelper.GetStatus(Keys.STS_Pending);
                        Evaluation evaluation = new Evaluation()
                        {
                            CustomerName = FindRetu.CustomerName,
                            IsActive = true,
                            OrderId = FindRetu.OrderId,
                            ReferenceNumber = refno,
                            StatusId = Idstatus.Id,
                            RRIdValue = FindRetu.Id,
                            PickedUpAndReadyForEval = false,
                            IsCompleted = false,
                            DeliveryPickUpForReturn = false,
                            DeliveryId = delivery.DeliveryId
                            //ApplicationUser = FindRetu.ApplicationUser,
                            //ProductionId = FindRetu.ProductionId ?? null
                        };
                        db.Evaluations.Add(evaluation);
                        db.SaveChanges();
                        #endregion
                    }


                }
                catch (Exception e)
                {

                    //  throw;
                }
                return RedirectToAction("Index");
            }
            ViewBag.ReasonId = new SelectList(db.ReasonDrops, "Id", "Name", returnAndRefund.ReasonId);
            ViewBag.RNRId = new SelectList(db.RNRDatas, "Id", "Name", returnAndRefund.RNRId);
            ViewBag.OrderId = new SelectList(db.Sales, "SaleId", "SaleDate", returnAndRefund.OrderId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", returnAndRefund.StatusId);
            return View(returnAndRefund);
        }


        // GET: ReturnAndRefunds/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReturnAndRefund returnAndRefund = await db.ReturnAndRefunds.FindAsync(id);
            if (returnAndRefund == null)
            {
                return HttpNotFound();
            }
            return View(returnAndRefund);
        }

        // POST: ReturnAndRefunds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ReturnAndRefund returnAndRefund = await db.ReturnAndRefunds.FindAsync(id);
            db.ReturnAndRefunds.Remove(returnAndRefund);
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

        public static void SendMail(string email, string body, string subject)
        {
            try
            {

                MailMessage mail = new MailMessage();
                string emailTo = email;
                MailAddress from = new MailAddress("africanmagicsystem@gmail.com");
                mail.From = from;
                mail.Subject = subject;
                mail.Body = body;
                mail.To.Add(emailTo);

                //mail.Attachments.Add(invoicePdf);

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
                //Close the document.

                //Dispose of email.
                mail.Dispose();
            }
            catch (Exception)
            {

                // throw;
            }
        }


        public static string GenerateReferenceNumber()
        {
            using (ApplicationDbContext core = new ApplicationDbContext())
            {
                var findList = core.ReturnAndRefunds.ToList() ?? null;

                string refno = ("RNR" + DateTime.Now.ToString("yyyyMMdd") + (findList.Count() + 1)).ToString();


                _ = core.ReturnAndRefunds.Where(x => x.ReferenceNumber == refno).ToList().Count() > 0 ? GenerateReferenceNumber() : refno;
                return refno;



            }
        }
    }
}
