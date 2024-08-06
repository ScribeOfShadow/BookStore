using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ZXing;


namespace BookStore
{
    public partial class QRCodeGenerater : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateCode(txtCode.Text);
        }

        protected void btnRead_Click(object sender, EventArgs e)
        {
            ReadQRCode();
        }
        //Generate QRCode
        private void GenerateCode(string name)
        {
            var writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            var result = writer.Write(name);
            string path = Server.MapPath("~/Images/QRImage.jpg");
            var barcodeBitmap = new Bitmap(result);


            using(MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            imgQRCode.Visible = true;
            imgQRCode.ImageUrl = "~/Images/QRImage.jpg";
        }

        //Read Code from QR Image
        private void ReadQRCode()
        {
            var reader = new BarcodeReader();
            string filename = Path.Combine(Request.MapPath("~/Images"), "QRImage.jpg");
            //Detech and decode the barcode inside the bitmap
            var result = reader.Decode(new Bitmap(filename));
            if(result != null)
            {
                lblQRCode.Text = "QR code :   " + result.Text;
            }
        }
    }
}