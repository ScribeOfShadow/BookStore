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
using System.Net.Mail;
using BookStore.ViewModels;
using BookStore.Helpers;
using System.EnterpriseServices;

namespace BookStore.Controllers
{
    public class SalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: 
        //[Authorize(Roles = "Driver")]
        public ViewResult Index(string sortOrder, string searchString, ApplicationUser applicationUser, Sale sale)
        {


            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var students = from s in db.Sales
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Name.Contains(searchString)
                                       || s.ApplicationUser.UserName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.SaleDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.SaleDate);
                    break;
                default:
                    students = students.OrderBy(s => s.SaleDate);
                    break;
            }


            return View(students.ToList());
        }

        public async Task<ActionResult> Delivered(int id)
        {
            int DelId = 0;
            Sale sale = await db.Sales.FindAsync(id);

            var deliverys = from db in db.Deliveries
                            where db.SaleId == id
                            select db;

            foreach (var item in deliverys)
            {
                DelId = item.DeliveryId;
            }

            Delivery delivery = await db.Deliveries.FindAsync(DelId);

            delivery.DeliveryDate = DateTime.Now.ToString();
            delivery.CurrentLocation = sale.Address;
            delivery.isDelivered = true;


            //Successful Delivery...
            MailMessage mail = new MailMessage();
            string emailTo = sale.Email;
            MailAddress from = new MailAddress("africanmagicsystem@gmail.com");
            mail.From = from;
            mail.Subject = "Successful Delivery For Order Number #" + sale.SaleId;
            mail.Body = "Dear Customer " + sale.Name + ", your order has been delivered successfully.";
            mail.To.Add(emailTo);

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
            sale.ConfirmDelivery = true;
            mail.Dispose();

            //Set Sale State to Complete once delivered
            var completedSaleId = delivery.SaleId;
            if (Convert.ToString(completedSaleId) != null || completedSaleId > 0)
            {
                Sale completedSale = db.Sales.Find(completedSaleId);
                completedSale.Complete = true;
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Details" + "/" + id);
        }

        public async Task<ActionResult> ApprovedDispatch(int aid)
        {

            Sale sale = await db.Sales.FindAsync(aid);

            var deliverys = from db in db.Deliveries
                            where db.SaleId == aid
                            select db;

            // CO = sale.ConfirmOrder;
            // CO = true;
            //  ViewBag.ConfirmOrder = CO;

            //Successful Delivery...
            MailMessage mail = new MailMessage();
            string emailTo = sale.Email;
            MailAddress from = new MailAddress("africanmagicsystem@gmail.com");
            mail.From = from;
            mail.Subject = "Approved Delivery For Order Number #" + sale.SaleId;
            mail.Body = "Dear Customer" + sale.Name + " your order has been Dispatched." + " https://2022grp32.azurewebsites.net/Manage/PurchaseHistory  " + "Use this link to confirm you have received your order";
            mail.To.Add(emailTo);

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
            sale.ConfirmOrder = false;
            sale.Dispatched = true;
            sale.ConfirmDelivery = null;

            await db.SaveChangesAsync();
            return RedirectToAction("Details" + "/" + aid);
        }

        public async Task<ActionResult> ConfirmOrder(int hiv)
        {
            Sale sale = await db.Sales.FindAsync(hiv);

            var deliverys = from db in db.Deliveries
                            where db.SaleId == hiv
                            select db;

            if (sale.ConfirmOrder == false)
            {
                sale.ConfirmOrder = true;
                sale.ConfirmDelivery = false;
            }
            else
            { ViewBag.ConfirmOrder = "Sceduled"; }

            // sale.ConfirmOrder = false;
            // sale.Dispatched = true;
            await db.SaveChangesAsync();
            return RedirectToAction("Details" + "/" + hiv);
        }
        // GET: Sales/Details/5

        public async Task<ActionResult> Details([Bind(Include = "ConfirmOrder,Dispatched,ConfirmDelivery")] int? id , string message)
        {

            ViewBag.message = !string.IsNullOrEmpty(message) ? message : "";

            int DelId = 0;
            int SaleId = 0;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = await db.Sales.FindAsync(id);


            if (sale == null)
            {
                return HttpNotFound();
            }



            var productList = from db in db.SaleDetails
                              where db.SaleId == id
                              select db.Products;

            var deliverys = from db in db.Deliveries
                            where db.SaleId == id
                            select db;

            var confirmation = from db in db.Sales
                               where db.SaleId == id
                               select db;



            ViewBag.SaleDetails = productList;
            ViewBag.DeliveryDetails = confirmation;

            foreach (var item in deliverys)
            {
                DelId = item.DeliveryId;
            }

            Delivery delivery = await db.Deliveries.FindAsync(DelId);
            ViewBag.Date = delivery.DeliveryDate;
            ViewBag.Delivery = delivery.CurrentLocation;
            ViewBag.SalesId = id;


            return View(sale);
        }


        public async Task<ActionResult> PickUpOrder(string message)
        {
            var FindAllDeliver = await db.Deliveries.Include(x => x.sale).Where(x =>x.IsForReturn == true).ToListAsync();
            ViewBag.message = message;
            return View(FindAllDeliver);
        }


        public ActionResult PickUpCom(int? id)
        {
            string message = message = string.Format($" Delivery Pick Up is Failed to update  at {DateTime.Now.ToString("dd-MMMM-yyyy")}");
            using (ApplicationDbContext core = new ApplicationDbContext())
            {
                try
                {
                    var FindDelivery = core.Deliveries.FirstOrDefault(x => x.DeliveryId == id) ?? null;
                    if (FindDelivery != null)
                    {
                        FindDelivery.IsPickedUpForReturn = true;
                        FindDelivery.PickUpReturnDate = DateTime.Now;
                        core.Entry(FindDelivery).State = EntityState.Modified;
                        core.SaveChanges();
                        var FindEval = core.Evaluations.FirstOrDefault(x => x.DeliveryId == FindDelivery.DeliveryId) ?? null;
                        if (FindEval != null)
                        {
                            FindEval.DeliveryPickUpForReturn = true;
                            core.Entry(FindEval).State = EntityState.Modified;
                            core.SaveChanges();
                        }
                        message = string.Format($" Delivery Pick Up is successful  at {DateTime.Now.ToString("dd-MMMM-yyyy")}");
                    }
                }
                catch (Exception)
                {

                    // throw;
                }
                return RedirectToAction("PickUpOrder", new { message = message });
            }
        }
        // GET: Sales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SaleId,SaleDate,CustomerName,Email,Address,City,State,PostalCode,Country,PhoneNumber,SaleTotal")] Sale sale, ApplicationUser applicationUser)
        {
            sale.Email = User.Identity.Name;
            sale.Name = applicationUser.UserName;
            sale.Address = User.Identity.GetAddress();
            sale.City = User.Identity.GetCity();
            sale.State = User.Identity.GetState();
            sale.PostalCode = User.Identity.GetPostalCode();
            sale.Country = User.Identity.GetCountry();
            sale.PhoneNumber = User.Identity.GetPhoneNo();

            //Tracking statues
            sale.Dispatched = false;
            sale.ConfirmOrder = null;
            sale.ConfirmDelivery = false;

            if (ModelState.IsValid)
            {
                db.Sales.Add(sale);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sale);
        }

        // GET: Sales/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = await db.Sales.FindAsync(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SaleId,SaleDate,CustomerName,Email,Address,City,State,PostalCode,Country,PhoneNumber,SaleTotal")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = await db.Sales.FindAsync(id);
            if (sale == null)
            {
                return HttpNotFound();
            }
            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Sale sale = await db.Sales.FindAsync(id);
            db.Sales.Remove(sale);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public ActionResult Returns(int? id, int? ProductionId)
        {
            StatusHelper.CheckStatus();
            int DelId = 0;
            int SaleId = 0;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sales.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }

            var productList = from db in db.SaleDetails
                              where db.SaleId == id && db.ProductId == ProductionId
                              select db.Products;


            var deliverys = from db in db.Deliveries
                            where db.SaleId == id
                            select db;

            ViewBag.SaleDetails = productList;

            foreach (var item in deliverys)
            {
                DelId = item.DeliveryId;
            }

            Delivery delivery = db.Deliveries.Find(DelId);
            ViewBag.Date = delivery.DeliveryDate;
            ViewBag.Delivery = delivery.CurrentLocation;


            ReturnRefundVM returnRefundVM = new ReturnRefundVM()
            {
                delivery = delivery,
                sale = sale,
                ProducId = ProductionId
            };
            ViewBag.ReasonId = new SelectList(db.ReasonDrops, "Id", "Name");
            ViewBag.RNRId = new SelectList(db.RNRDatas.Where(x =>x.Name == "Refund" || x.Name == "Return").ToList(), "Id", "Name");
            // ViewBag.OrderId = new SelectList(db.Sales, "SaleId", "SaleDate");
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
            return View(returnRefundVM);
        }

        [HttpPost]
        public ActionResult Returns(ReturnRefundVM returnRefundVM)
        {
            string message = string.Empty;
            using (ApplicationDbContext core = new ApplicationDbContext())
            {
                
                var CheckIfAlreadyPending = core.ReturnAndRefunds.FirstOrDefault(x => x.OrderId == returnRefundVM.sale.SaleId && x.CustomerName == User.Identity.Name && x.ProductionId == returnRefundVM.ProducId) ?? null;
                if (CheckIfAlreadyPending == null)
                {
                    returnRefundVM.ReturnAndRefund.OrderId = returnRefundVM.sale.SaleId;
                    returnRefundVM.ReturnAndRefund.RNRId = returnRefundVM.RNRId;
                    returnRefundVM.ReturnAndRefund.ReasonId = returnRefundVM.ReasonId;
                    returnRefundVM.ReturnAndRefund.ProductionId = returnRefundVM.ProducId;
                    returnRefundVM.ReturnAndRefund.CustomerName = User.Identity.Name;
                    returnRefundVM.ReturnAndRefund.ReferenceNumber = ReturnAndRefundsController.GenerateReferenceNumber();


                    core.ReturnAndRefunds.Add(returnRefundVM.ReturnAndRefund);
                    core.SaveChanges();
                    message = string.Format($"Request for return saved sucessfully, {DateTime.Now.ToString("yyyy-MM-dd")}");
                }
                else
                {
                    message = string.Format($"Request for return already created , unable to create another, {DateTime.Now.ToString("yyyy-MM-dd")}");
                }
              



            }
            return RedirectToAction("Details", "Sales", new
            {
                id = returnRefundVM.sale.SaleId,
                message = message
            });
        }

        public JsonResult Addfeedback(int id, string comment, int rating, string Bookname)
        {
            using (ApplicationDbContext core = new ApplicationDbContext())
            {
                var bookid = (from f in core.Feedbacks
                            join s in core.Sales on f.SaleId equals s.SaleId
                            join sd in core.SaleDetails on s.SaleId equals sd.SaleId
                            join p in core.Products on sd.ProductId equals p.ProductId
                            select p).FirstOrDefault();

                Feedback feedback = new Feedback();
                feedback.Created = DateTime.Now;
                feedback.BookId = bookid.ProductId;
                feedback.SaleId = id;
                feedback.Comment = comment;
                feedback.Rating = rating;
                feedback.Bookname = Bookname;
                core.Feedbacks.Add(feedback);

                Analytic analytic = new Analytic();
                



                core.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Feedback(int? id)
        {
            using (ApplicationDbContext core = new ApplicationDbContext())
            {
                var list = (from feedback in core.Feedbacks
                            join s in core.Sales on feedback.SaleId equals s.SaleId
                            join sd in core.SaleDetails on s.SaleId equals sd.SaleId
                            join p in core.Products on sd.ProductId equals p.ProductId
                            select new FeedbackVM
                            {
                                BookId = id,
                                Bookname = p.ProductName,
                                Comment = feedback.Comment,
                                Rating = feedback.Rating
                            }).ToList();
                
                
                return View(list);
            }


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