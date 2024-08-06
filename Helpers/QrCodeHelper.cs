using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace BookStore.Helpers
{
    public static class QrCodeHelper
    {
        public static Byte[] GenerateQRCode(int id, string text, HttpServerUtilityBase server, string urlDomain, bool? returnBook = false)
        {
            try
            {
                BarcodeWriter barcodeWriter = new BarcodeWriter();
                var generatedString = text;
                string folder = $"/QRVerificationCodeImages";
                string url = returnBook == true ? $"{urlDomain}/Libraries/ReturnBook/{id}" : $"{urlDomain}/Libraries/SubtractFromLibary/{id}";

                //string prefixPath = $"{HttpContext.Current.Server.MapPath(folderPath)}";
                //prefixPath = prefixPath.Substring(0, prefixPath.LastIndexOf('\\'));
                //prefixPath = prefixPath.Substring(0, prefixPath.LastIndexOf("\\"));
                //string imagePath = HttpContext.Current.Server.MapPath($"\\Content\\Images\\Logo.jpg");
                //string imagePath = HttpContext.Current.Server.MapPath($"\\Content\\Images\\ekurhuleni_logo.png");

                // If the directory doesn't exist then create it.
                string folderPath = server.MapPath(folder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                EncodingOptions encodingOptions = new EncodingOptions() { Width = 200, Height = 200, Margin = 0, PureBarcode = false };
                encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
                barcodeWriter.Renderer = new BitmapRenderer();
                barcodeWriter.Options = encodingOptions;
                barcodeWriter.Format = BarcodeFormat.QR_CODE;



                var result = barcodeWriter.Write($"{url}");
                ////test
                //var result = barcodeWriter.Write($"http://localhost:3456/QRCodeVerification/DecryptQRCode?q= {EncryptedString} | {generatedString.Item2}");

                string barcodePath = $"{folderPath}\\QRVerificationCode{DateTime.Now:yyyyMMdd}.png";
                //Bitmap barcodeBitmap = new Bitmap(result);

                //// custom image
                //Bitmap overlay = new Bitmap(imagePath);
                //int deltaHeigth = barcodeBitmap.Height - overlay.Height;
                //int deltaWidth = barcodeBitmap.Width - overlay.Width;

                //Graphics g = Graphics.FromImage(barcodeBitmap);
                //g.DrawImage(overlay, new Point(deltaWidth / 2, deltaHeigth / 2));


                var barcodeBitmap = new Bitmap(result);
                byte[] bytes = null;



                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        result.Save(memory, ImageFormat.Png);
                        bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }

                if (returnBook == false)
                {
                    _ = CreateQrCode(id, generatedString, folderPath, bytes);
                }

                return bytes;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool SubtractBook(int id)
        {
            using (ApplicationDbContext _core = new ApplicationDbContext())
            {
                var book = _core.Libraries.FirstOrDefault(x => x.LibraryId == id);
                if (book != null)
                {
                    book.Borrowed = DateTime.Now;
                    book.TotalStock--;
                    _core.Entry(book).State = System.Data.Entity.EntityState.Modified;
                    return _core.SaveChanges() > 0;
                }
                return false;
            } 
        }

        public static bool ReturnBook(int id)
        {
            using (ApplicationDbContext _core = new ApplicationDbContext())
            {
                var book = _core.Libraries.FirstOrDefault(x => x.LibraryId == id);
                if (book != null)
                {
                    book.ReturnedDate = DateTime.Now;
                    book.TotalStock++;
                    _core.Entry(book).State = System.Data.Entity.EntityState.Modified;
                    return _core.SaveChanges() > 0;
                }
                return false;
            }
        }
        public static bool CreateQrCode(int libraryBookId, string plainText, string ImagePath, byte[] imageBytes)
        {
            using (ApplicationDbContext _core = new ApplicationDbContext())
            {
                BookQrCodes qrCodes = new BookQrCodes()
                {
                    LibraryBookId = libraryBookId,
                    PlainText = plainText,
                    RenderedImagePath = ImagePath,
                    RenderedBytes = imageBytes,
                    ScannedCount = 0,
                    CreatedDateTime = DateTime.Now,
                };
                _core.BookQrCodes.Add(qrCodes);
                return _core.SaveChanges() > 0;
            }
        }

        public static string QrCodeTextVerifier(string text)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                var values = text.Split('|');
                int id = int.Parse(values[0].ToString());
                BookQrCodes bookQr = _context.BookQrCodes.FirstOrDefault(x => x.Id == id);
                if (bookQr == null) throw new NullReferenceException("Book Qr Code Not Found!");


                bookQr.ScannedCount += 1;
                bookQr.ModifiedDateTime = DateTime.Now;
                _context.Entry(bookQr).State = System.Data.Entity.EntityState.Modified;
                return _context.SaveChanges() > 0 ? "Updated" : "Failed";

                //TODO IM : check validity of image using metadata fields.

            }
        }

    }
}