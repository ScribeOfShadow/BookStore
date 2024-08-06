using BookStore.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Collections.Generic;

namespace BookStore.Controllers
{
    //[Authorize]


    public class ProductsController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ViewResult Index(string sortOrder, string searchString, ProductCatergory productCatergory)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var students = (from s in db.Products
                            select s);
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.ProductName.Contains(searchString)
                                       || s.ProductPrice.ToString().Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.ProductName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.ProductPrice);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.ProductPrice);
                    break;
                default:
                    students = students.OrderBy(s => s.ProductName);
                    break;
            }
            var list = students.Include(x => x.ProductCatergory).ToList();
            return View(list);
        }

        // GET: Products 
        public ActionResult OurProducts(string sortOrder, string currentFilter, string searchString, int? page)
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

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products 
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

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

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

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

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

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

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

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

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

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

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

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

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

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

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

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

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

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

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

            var items = from i in db.Products
                        select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper())
                                       || s.ProductCatergory.CatergoryName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ProductName);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.ProductPrice);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.ProductPrice);
                    break;
                default:  // Sort By Name ASC
                    items = items.OrderBy(s => s.ProductName);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(items.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        // GET: Products/Create
        public ActionResult Create(Products product, ProductCatergory productCatergory)
        {
            ViewBag.ListId = new SelectList(db.WishLists, "ListId", "ListName");
            ViewBag.ProductCatergories = new SelectList(db.ProductCategories, "ProductCatergoryId", "CatergoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductCatergoryId,CatergoryName,ProductCatergory,ProductPrice,ProductImage,ProductDescription,ProductStock,ProductAuthor,ProductName,ProductCatergory")] Products product, HttpPostedFileBase img_upload, ProductCatergory productCatergory)
        {

            byte[] data;
            int changeCount = 0;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            product.ProductImage = data;
           // product.ProductCreatedAt = DateTime.Now;
          //  product.ProductUpdatedAt = DateTime.Now;
            try
            {
                using (ApplicationDbContext _context = new ApplicationDbContext())
                {
                    _context.Database.CommandTimeout = 200;
                    _context.Products.Add(product);
                    changeCount = _context.SaveChanges();
                    if (changeCount > 0) return RedirectToAction("Index");
                    else
                    {
                        ViewBag.ProductCatergories = new SelectList(_context.ProductCategories, "ProductCatergoryId", "CatergoryName", product);
                        return View(product);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductId,ProductCategoryId,ProductName,ProductStock,ProductPrice,ProductImage,isActive")] Products product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Products product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
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
