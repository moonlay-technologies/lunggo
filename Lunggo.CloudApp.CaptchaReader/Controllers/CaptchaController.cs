using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using AForge.Imaging.Filters;
using Tesseract;

namespace Lunggo.CloudApp.CaptchaReader.Controllers
{
    public class CaptchaController : ApiController
    {
        [HttpPost]
        public string LionAirBreak()
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
            if (file == null || file.ContentLength == 0)
                return null;

            var captchaImage = (Bitmap)Image.FromStream(file.InputStream);

            ////////////////////// Series of Filters ///////////////////////

            // 1. create blur filter
            var filter3 = new ConservativeSmoothing();
            var img2 = filter3.Apply(captchaImage);

            // 2. create Crop filter

            var w = img2.Width;
            var h = img2.Height;
            var filter4 = new Crop(new Rectangle(10, 10, w - 20, h - 20));
            var img3 = filter4.Apply(img2);

            // 3. create Resize filter
            var newWidth = Convert.ToInt32(img3.Width * 1.982);
            var newHeight = Convert.ToInt32(img3.Height * 1.982);
            var filter5 = new ResizeBilinear(newWidth, newHeight);
            var img4 = filter5.Apply(img3);

            //////////////////// end of series of filters ////////////////////

            /// OCR /// 

            var ocr = new TesseractEngine(HttpContext.Current.Server.MapPath("~/Config/"), "eng");
            ocr.SetVariable("tessedit_char_whitelist", "0123456789");
            var result = ocr.Process(img4, null).GetText().Trim();
            var readcaptcha = result.Replace(" ", "");

            return readcaptcha;
        }
    }
}
