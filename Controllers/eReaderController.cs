using BookStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Net;
using System.Data;
using System.Data.Entity;
using System.Net.Http;

namespace BookStore.Controllers
{
    public class eReaderController : Controller
    {
        private ApplicationDbContext applicationDbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            var books = applicationDbContext.eBooks.ToList();
            return View(books);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(eBook book, HttpPostedFileBase file, HttpPostedFileBase fileImage)
        { 
            if (book != null)
            {
                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the filename
                    //var fileName = Path.GetFileName(file.FileName);
                    // store the file inside ~/App_Data/uploads folder
                    //var path = Path.Combine(Server.MapPath("~/eBooks"), fileName);
                    //file.SaveAs(path);
                    //BinaryReader rdr = new BinaryReader(file.InputStream);
                    //var fileByte = rdr.ReadBytes((int)file.ContentLength);

                    StreamReader rdr = new StreamReader(file.InputStream);
                    var imageByte = rdr.ReadToEnd();

                    book.FileContents = imageByte;
                    book.mimetype = Path.GetExtension(file.FileName);
                    book.BookName = file.FileName.Substring(0, file.FileName.IndexOf('.'));
                    //book.FilePath = Path.Combine(Server.MapPath("~/eBooks"), fileName);
                    book.FileName = file.FileName.Substring(0, file.FileName.IndexOf('.'));

                }
                if (fileImage != null && file.ContentLength > 0)
                {
                    BinaryReader rdr = new BinaryReader(fileImage.InputStream);
                    var imageByte = rdr.ReadBytes((int)fileImage.ContentLength);
                    book.BookImageContent = imageByte;
                    book.BookImageName = fileImage.FileName.Substring(0, fileImage.FileName.IndexOf('.'));
                    book.BookImageExtension = Path.GetExtension(fileImage.FileName);
                }

                Products products = new Products();
                products.ProductName = book.BookName;
                products.ProductDescription = book.BookDescription;
                products.ProductPrice = book.BookPrice; 
                products.ProductImage = book.BookImageContent;
                products.ProductStock = 100;
                products.ProductCatergoryId = 11;



                book.LastReadPosition = 0;
                applicationDbContext.eBooks.Add(book);
                applicationDbContext.Products.Add(products);
                applicationDbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public string ReadTextFile(int id, bool? lastRead)
        {
            var book = applicationDbContext.eBooks.FirstOrDefault(x => x.Id == id);
            //string content = string.Empty;
            //var filepath = HttpContext.Server.MapPath($"~{book.FilePath}");
            //using (var stream = new StreamReader(filepath))
            //{
            string content = book.FileContents;
            //}
            //if (lastRead == true && content.Length > book?.LastReadPosition)
            //{
            //    return (content.Substring(book.LastReadPosition), book.BookName);
            //}
            //else
            //{
                return content;
            //}
        }

        //public ActionResult Test()
        //{
        //    return ReadText(1, false);
        //}

        public async Task<ActionResult> ReadText(int id, bool? lastRead)
        {
            var book = ReadTextFile(id, lastRead);
            Task<FileContentResult> task = Task.Run(() =>
            {
                using (var synth = new SpeechSynthesizer())
                {
                    using (var stream = new MemoryStream())
                    {
                        synth.SetOutputToWaveStream(stream);
                        synth.Speak(book);
                        byte[] bytes = stream.GetBuffer();
                        return File(bytes, "audio/wav"/*, book.Item2*/);
                    }
                }
            });
            return await task;
        }

        public ActionResult PlayeBook(int id, bool? lastRead)
        {
            eBook getDetails = applicationDbContext.eBooks.FirstOrDefault(x => x.Id == id);
            //dynamic result = await ReadText(id, lastRead);

            //getDetails.Content = result;
            //ViewBag.Data = "data:audio/wav;base64," + Convert.ToBase64String(result.FileContents);

            return View(getDetails);
        }

       

    }
}