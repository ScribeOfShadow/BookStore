using BookStore.Helpers;
using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;


namespace BookStore.Controllers
{

    public class LibrariesController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult SubtractFromLibary(int id)
        {
            bool result = QrCodeHelper.SubtractBook(id);
            return View(result);
        }
        public ActionResult ReturnToLibary(int id)
        {
            string urlDomain = Request.Url.GetLeftPart(UriPartial.Authority);
            byte[] result = QrCodeHelper.GenerateQRCode(id, "", Server, urlDomain, true);

            return View(result);
        }

        public ActionResult ReturnBook(int id)
        {
            bool result = QrCodeHelper.ReturnBook(id);
            return View(result);
        }

        // GET: Libraries
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var students = from s in db.Libraries
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LibraryProductName.Contains(searchString)
                                       || s.LibraryProductName.ToString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LibraryProductName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.ReturnedDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.ReturnedDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LibraryProductName);
                    break;
            }
            var list = students.Include(x => x.LibraryCatergory).ToList();
            return View(students.ToList());
        }

        // GET: Libraries/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Library library = await db.Libraries.FindAsync(id);
            if (library == null)
            {
                return HttpNotFound();
            }
            return View(library);
        }

        // GET: Libraries/Create
        public ActionResult Create(Library library)
        {
            ViewBag.LibraryCatergories = new SelectList(db.LibraryCatergories.ToList(), "LibraryCatergoryId", "LibraryCatergoryName");
            return View();
        }

        // POST: Libraries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Create(Library library, HttpPostedFileBase img_upload)
        {
            byte[] data;
            int changeCount = 0;
            library.CreatedAt = DateTime.Now;
            library.IsAssigned = false;
            if (ModelState.IsValid)
            {

                db.Libraries.Add(library);
                await db.SaveChangesAsync();
                string urlDomain = Request.Url.GetLeftPart(UriPartial.Authority);

                QrCodeHelper.GenerateQRCode(library.LibraryId, library.LibraryProductName, Server, urlDomain);
                return RedirectToAction("Index");
            }
            try
            {
                using (ApplicationDbContext _context = new ApplicationDbContext())
                {
                    _context.Database.CommandTimeout = 200;
                    _context.Libraries.Add(library);
                    changeCount = _context.SaveChanges();
                    if (changeCount > 0) return RedirectToAction("Index");
                    else
                    {
                        ViewBag.LibraryCatergories = new SelectList(db.LibraryCatergories.ToList(), "LibraryCatergoryId", "LibraryCatergoryName");
                        return View(library);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // GET: Libraries/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Library library = db.Libraries.Find(id);
            BookQrCodes bookQrCodes = db.BookQrCodes.FirstOrDefault(x => x.LibraryBookId == id);
            BorrowViewModel borrow = new BorrowViewModel()
            {
                Library = library,
                BookQrCodes = bookQrCodes
            };
            if (library == null)
            {
                return HttpNotFound();
            }
            return View(borrow);
        }

        // POST: Libraries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BorrowViewModel borrow, Library library)
        {
            //if (ModelState.IsValid)
            //{
            borrow.Library = library;
            library.TotalStock = borrow.Library.TotalStock;
            ViewBag.IndexBorrowed = "~/View/BorrowedBooks/IndexBorrowed.cshtml";
            borrow.Library.Borrowed = DateTime.Now;
            ViewBag.ReturnedDate = DateTime.Now;
            var userId = User.Identity.GetUserId();
            db.Entry(borrow.Library).State = EntityState.Modified;
            if (borrow.Library.IsAssigned == false)
            {
                borrow.Library.ReturnedDate = DateTime.Now;
            }
            BorrowedBooks borrowedBooks = new BorrowedBooks()
            {
                BookId = borrow.Library.LibraryId,
                UserId = userId,
                DateBorrowed = DateTime.Now,
                DateReturned = DateTime.Now.AddDays(10)
            };
            db.BorrowedBooks.Add(borrowedBooks);

            var count = await db.SaveChangesAsync();
            if (count > 0) return RedirectToAction("Index");
            else return View(borrow.Library);

            //}
        }

        // GET: Libraries/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Library library = await db.Libraries.FindAsync(id);
            if (library == null)
            {
                return HttpNotFound();
            }
            return View(library);
        }

        // POST: Libraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(BorrowViewModel borrow)
        {
            //if (ModelState.IsValid)
            //{
            borrow.Library.Borrowed = DateTime.Now;
            ViewBag.ReturnedDate = DateTime.Now;
            var userId = User.Identity.GetUserId();
            db.Entry(borrow.Library).State = EntityState.Modified;

            BorrowedBooks borrowedBooks = new BorrowedBooks()
            {
                BookId = borrow.Library.LibraryId,
                UserId = userId,
                DateBorrowed = DateTime.Now

            };
            db.BorrowedBooks.Add(borrowedBooks);

            var count = await db.SaveChangesAsync();
            if (count > 0) return RedirectToAction("Index");
            else return View(borrow.Library);

            //}
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult BorrowBook(string sortOrder, string currentFilter, string searchString, int? page, Library library)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Libraries
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.LibraryProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LibraryCatergory.LibraryCatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.LibraryProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.LibraryId);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.LibraryId);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.LibraryProductName);
                    break;
            }
            var lib = db.Libraries.Where(x => x.IsAssigned == false).ToList();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));





        }


        public ActionResult ViewBooksBorrowed(string sortOrder, string currentFilter, string searchString, int? page, Library library)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 2;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Libraries
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.LibraryProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LibraryCatergory.LibraryCatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.LibraryProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ReturnedDate);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ReturnedDate);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.LibraryProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        //Overdue Books Action
        public ActionResult SendOverdueEmail()
        {
            var UserId = User.Identity.GetUserId();
            BorrowedBooks borrowedBooks = db.BorrowedBooks.Where(x => x.UserId == UserId).FirstOrDefault();

            if (borrowedBooks != null)
            {
                var BorrowedDate = Convert.ToDateTime(borrowedBooks.DateBorrowed);
                var ExReturnDate = BorrowedDate.AddDays(10);
                var UserEmail = User.Identity.GetUserName();
                var UserName = User.Identity.GetName();

                SendLateBookEmail(UserEmail, UserName, ExReturnDate);
            }

            //Redirect to the action, you can change the action name
            return RedirectToAction("Index");
        }

        //Late Book Return Method
        public void SendLateBookEmail(string StudentEmail, string StudentName, DateTime? ExpectedReturn)
        {
            DateTime CurrentDate = DateTime.Now;
            StudentEmail = User.Identity.Name;
            //Overdue Book...

            //Send email only if the current is more than the expected return date
            if (CurrentDate > ExpectedReturn)
            {
                MailMessage mail = new MailMessage();
                string emailTo = StudentEmail;
                MailAddress from = new MailAddress("africanmagicsystem@gmail.com");
                mail.From = from;
                mail.Subject = "You Have A Overdue Book";
                mail.Body = "Please Return Your Outstanding Book";
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
            }
        }

        public ActionResult Education(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Libraries
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.LibraryProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LibraryCatergory.LibraryCatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.LibraryProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.LibraryId);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.LibraryId);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.LibraryProductName);
                    break;
            }
            var lib = db.Libraries.Where(x => x.IsAssigned == false).ToList();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products 
        public ActionResult Drama(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Libraries
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.LibraryProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LibraryCatergory.LibraryCatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.LibraryProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.LibraryId);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.LibraryId);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.LibraryProductName);
                    break;
            }
            var lib = db.Libraries.Where(x => x.IsAssigned == false).ToList();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products 
        public ActionResult Romance(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Libraries
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.LibraryProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LibraryCatergory.LibraryCatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.LibraryProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.LibraryId);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.LibraryId);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.LibraryProductName);
                    break;
            }
            var lib = db.Libraries.Where(x => x.IsAssigned == false).ToList();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products 
        public ActionResult Sci_fi(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Libraries
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.LibraryProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LibraryCatergory.LibraryCatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.LibraryProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.LibraryId);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.LibraryId);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.LibraryProductName);
                    break;
            }
            var lib = db.Libraries.Where(x => x.IsAssigned == false).ToList();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Action(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Libraries
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.LibraryProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LibraryCatergory.LibraryCatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.LibraryProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.LibraryId);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.LibraryId);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.LibraryProductName);
                    break;
            }
            var lib = db.Libraries.Where(x => x.IsAssigned == false).ToList();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Fantasy(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Libraries
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.LibraryProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LibraryCatergory.LibraryCatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.LibraryProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.LibraryId);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.LibraryId);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.LibraryProductName);
                    break;
            }
            var lib = db.Libraries.Where(x => x.IsAssigned == false).ToList();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Comedy(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Libraries
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.LibraryProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LibraryCatergory.LibraryCatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.LibraryProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.LibraryId);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.LibraryId);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.LibraryProductName);
                    break;
            }
            var lib = db.Libraries.Where(x => x.IsAssigned == false).ToList();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Religion(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Libraries
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.LibraryProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LibraryCatergory.LibraryCatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.LibraryProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.LibraryId);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.LibraryId);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.LibraryProductName);
                    break;
            }
            var lib = db.Libraries.Where(x => x.IsAssigned == false).ToList();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Cooking(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Libraries
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.LibraryProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LibraryCatergory.LibraryCatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.LibraryProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.LibraryId);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.LibraryId);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.LibraryProductName);
                    break;
            }
            var lib = db.Libraries.Where(x => x.IsAssigned == false).ToList();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult History(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in db.Libraries
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.LibraryProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LibraryCatergory.LibraryCatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.LibraryProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.LibraryId);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.LibraryId);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.LibraryProductName);
                    break;
            }
            var lib = db.Libraries.Where(x => x.IsAssigned == false).ToList();
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }


    }
}
