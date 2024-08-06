using System.Linq;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.Controllers;



namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        { 
            var product = db.Products.ToList();
            return View(product);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}