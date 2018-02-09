using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Filter;
using Lunggo.Framework.SharedModel;

namespace Lunggo.CustomerWeb.Controllers
{
    public class WebViewController : Controller
    {
        public ActionResult Contact(string title = null, string description = null)
        {
            title = title ?? "Hubungi Kami";
            description = description ??
                          "Jika Anda ingin mengubah deskripsi aktivitas Anda, silakan hubungi kami. Kami akan membantu Anda merangkai kata agar lebih menarik bagi pengunjung.";
            ViewBag.Title = title;
            ViewBag.Description = description;
            return View();
        }
    }
}