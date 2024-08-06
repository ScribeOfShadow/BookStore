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
using BookStore.Helpers;
using System.Net.Mail;

namespace BookStore.Controllers
{
    public class EvaluationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Evaluations
        public async Task<ActionResult> Index(string message, string CasesToshow)
        {
            ViewBag.message = message;

            //CasesToshow = String.Empty;
            if (CasesToshow == "none")
            {
                var evaluations = db.Evaluations.Include(e => e.Products).Include(x => x.Severity).Include(e => e.returnAndRefund).Include(e => e.rNRData).Include(e => e.Sale).Include(e => e.Severity).Include(e => e.Status).Where(x => x.IsCompleted == false && x.DeliveryPickUpForReturn == false);
                return View(await evaluations.ToListAsync());
            }
            else if (CasesToshow == "eval")
            {
                var evaluations = db.Evaluations.Include(e => e.Products).Include(x => x.Severity).Include(e => e.returnAndRefund).Include(e => e.rNRData).Include(e => e.Sale).Include(e => e.Severity).Include(e => e.Status).Where(x => x.IsCompleted == true);
                return View(await evaluations.ToListAsync());
            }
            else
            {
                var evaluations = db.Evaluations.Include(e => e.Products).Include(x => x.Severity).Include(e => e.returnAndRefund).Include(e => e.rNRData).Include(e => e.Sale).Include(e => e.Severity).Include(e => e.Status).Where(x => x.IsActive == true);
                return View(await evaluations.ToListAsync());
            }

        }

        // GET: Evaluations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluation evaluation = await db.Evaluations.FindAsync(id);
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            return View(evaluation);
        }

        // GET: Evaluations/Create
        public ActionResult Create()
        {
            ViewBag.ProductionId = new SelectList(db.Products, "ProductId", "ProductName");
            ViewBag.RRIdValue = new SelectList(db.ReturnAndRefunds, "Id", "ReferenceNumber");
            ViewBag.RNRIdE = new SelectList(db.RNRDatas, "Id", "Name");
            ViewBag.OrderId = new SelectList(db.Sales, "SaleId", "SaleDate");
            ViewBag.SeverityId = new SelectList(db.RNRDatas, "Id", "Name");
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
            return View();
        }

        // POST: Evaluations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Comments,StatusId,CustomerName,OrderId,ReferenceNumber,IsActive,IsCompleted,RRIdValue,PickedUpAndReadyForEval,RNRIdE,SeverityId,ProductionId")] Evaluation evaluation)
        {
            if (ModelState.IsValid)
            {
                db.Evaluations.Add(evaluation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProductionId = new SelectList(db.Products, "ProductId", "ProductName", evaluation.ProductionId);
            ViewBag.RRIdValue = new SelectList(db.ReturnAndRefunds, "Id", "ReferenceNumber", evaluation.RRIdValue);
            ViewBag.RNRIdE = new SelectList(db.RNRDatas, "Id", "Name", evaluation.RNRIdE);
            ViewBag.OrderId = new SelectList(db.Sales, "SaleId", "SaleDate", evaluation.OrderId);
            ViewBag.SeverityId = new SelectList(db.RNRDatas, "Id", "Name", evaluation.SeverityId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", evaluation.StatusId);
            return View(evaluation);
        }

        // GET: Evaluations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluation evaluation = await db.Evaluations.FindAsync(id);
            ViewBag.disableSave = false;
            if (evaluation.IsCompleted == true)
            {
                ViewBag.disableSave = true;
            }
     
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductionId = new SelectList(db.Products, "ProductId", "ProductName", evaluation.ProductionId);
            ViewBag.RRIdValue = new SelectList(db.ReturnAndRefunds, "Id", "ReferenceNumber", evaluation.RRIdValue);
            ViewBag.FinalRdValue = new SelectList(db.RNRDatas.Where(x =>x.CodeKey == Keys.Codekey_Evaluation), "Id", "Name", evaluation.RNRIdE);
            ViewBag.OrderId = new SelectList(db.Sales, "SaleId", "SaleDate", evaluation.OrderId);
            ViewBag.SeverityId = new SelectList(db.RNRDatas.Where(x =>x.CodeKey == Keys.Codekey_EvaluationSer), "Id", "Name", evaluation.SeverityId);
            ViewBag.FinalStatusId = new SelectList(db.Statuses, "Id", "Name", evaluation.StatusId);
            return View(evaluation);
        }

        // POST: Evaluations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HttpPostedFileBase img_upload ,Evaluation evaluation)
        {
            StatusHelper.CheckStatus();
            try
            {
              //  , 
                if (ModelState.IsValid)
                {
                    //var files = Request.Files.Get(0);
                    if (Request.Files.Count > 0)
                    {
                        byte[] data;
                        int changeCount = 0;
                        data = new byte[img_upload.ContentLength];
                        img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
                        evaluation.ProductImage = data;
                    }
               

                    var Status = StatusHelper.GetStatus(Keys.STS_Declined);
                    var FindEvaluyation = db.Evaluations.Include(x => x.Sale).Include(x => x.Products).Include(x => x.ApplicationUser).Include(x => x.Severity).FirstOrDefault(x => x.Id == evaluation.Id) ?? null;
                    var SaleItems = db.SaleDetails.FirstOrDefault(x => x.SaleId == FindEvaluyation.OrderId) ?? null;
                    evaluation.StatusId = evaluation.FinalStatusId;
                    // Push For Replacement
                    if (evaluation.FinalRdValue == StatusHelper.GetRNRDate(Keys.RNRE_ReplacementPending).Id && evaluation.FinalStatusId != Status.Id)
                    {
                        Products chck = db.Products.FirstOrDefault(x => x.ProductId == SaleItems.ProductId) ?? null;
                        chck.ProductStock = chck.ProductStock - 1;
                        db.Entry(chck).State = EntityState.Modified;
                        db.SaveChanges();



                        //Save Order to Delivery and Sale Table    
                        Delivery delivery = new Delivery
                        {
                            SaleId = (int)FindEvaluyation.OrderId,
                            DeliveryFee = 0,
                            CurrentLocation = FindEvaluyation.Sale.Address,
                            isDelivered = false,
                            IsPickedUpForReturn = false
                        };
                        db.Deliveries.Add(delivery);
                        db.SaveChanges();
                    }
                    else if (evaluation.FinalRdValue == StatusHelper.GetRNRDate(Keys.RNRE_Refunded).Id && evaluation.FinalStatusId != Status.Id)
                    {
                        string message = string.Format($"A refund Has been authorised, It will be transfer to you account with 2 - 3 business days ,  {DateTime.Now.ToString("yyyy-MMM-dd")}");
                        SendMail(FindEvaluyation.Sale.Email, message, string.Format("Refund authorised"));
                    }
                    else if (evaluation.FinalRdValue == StatusHelper.GetRNRDate(Keys.RNRE_ReturnGoodsNoChange).Id)
                    {
                        Delivery delivery = new Delivery
                        {
                            SaleId = (int)FindEvaluyation.OrderId,
                            DeliveryFee = 0,
                            CurrentLocation = FindEvaluyation.Sale.Address,
                            isDelivered = false,
                            IsPickedUpForReturn = false
                        };
                        db.Deliveries.Add(delivery);
                        db.SaveChanges();
                        string message = string.Format($"Good day Most valued customer , Please note that your request has been {db.Statuses.FirstOrDefault(x => x.Id == evaluation.StatusId).Name} and " +
                            $"A refund Has been authorised, It will be transfer to you account with 2 to 3 business ,     be return back to you,  Thank you Regards Admin");
                        SendMail(FindEvaluyation.Sale.Email, message, string.Format("Refund authorised"));
                    }
                    else
                    {
                        Delivery delivery = new Delivery
                        {
                            SaleId = (int)FindEvaluyation.OrderId,
                            DeliveryFee = 0,
                            CurrentLocation = FindEvaluyation.Sale.Address,
                            isDelivered = false,
                            IsPickedUpForReturn = false
                        };
                        db.Deliveries.Add(delivery);
                        db.SaveChanges();
                        string message = string.Format($"Good day Most valued customer , Please note that your request has been {db.Statuses.FirstOrDefault(x => x.Id == evaluation.StatusId).Name} and " +
                            $"A refund Has been authorised, It will be transfer to you account with 2 to 3 business ,     be return back to you,  Thank you Regards Admin");
                        SendMail(FindEvaluyation.Sale.Email, message, string.Format("Refund authorised"));
                    }

                    FindEvaluyation.Comments = evaluation.Comments ?? "";
                    FindEvaluyation.RNRIdE = evaluation.RNRIdE ?? 0;
                    FindEvaluyation.ProductImage = evaluation.ProductImage ?? null;
                    FindEvaluyation.IsCompleted = true;
                    FindEvaluyation.StatusId = evaluation.FinalStatusId;
                    FindEvaluyation.SeverityId = evaluation.SeverityId;
                    FindEvaluyation.RNRIdE = evaluation.FinalRdValue;
                    db.Entry(FindEvaluyation).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", new { message = string.Format($"Evaluation Update , {DateTime.Now.ToString("yyyy-MM-dd")}") });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { message = string.Format($"Evaluation failed , {DateTime.Now.ToString("yyyy-MM-dd")}") });
                // throw;
            }

            return RedirectToAction("Index", new { message = string.Format($"Evaluation failed , {DateTime.Now.ToString("yyyy-MM-dd")}") });
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

        // GET: Evaluations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evaluation evaluation = await db.Evaluations.FindAsync(id);
            if (evaluation == null)
            {
                return HttpNotFound();
            }
            return View(evaluation);
        }

        // POST: Evaluations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Evaluation evaluation = await db.Evaluations.FindAsync(id);
            db.Evaluations.Remove(evaluation);
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
