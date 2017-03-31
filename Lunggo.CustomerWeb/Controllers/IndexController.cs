using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.Framework.Config;
using Lunggo.Framework.Filter;

namespace Lunggo.CustomerWeb.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        [DeviceDetectionFilter]
        public ActionResult Index(string destination)
        {
            var b2bHost = ConfigManager.GetInstance().GetConfigValue("general", "b2bRootUrl");
            if (Request.Url.Host == b2bHost)
                return RedirectToAction("Login", "B2BTemplate");
            if (destination == null)
                return View();
            else
            {
                ViewBag.Destination = destination;
                return View();
            }
        }

        [DeviceDetectionFilter]
        [Route("tiket-pesawat")]
        public ActionResult IndexFlight(string destination)
        {
            ViewBag.IndexType = "flight";
            if (destination == null)
                return View("Index");
            else
            {
                ViewBag.Destination = destination;
                return View("Index");
            }
        }

        [DeviceDetectionFilter]
        [Route("hotel")]
        public ActionResult IndexHotel(string destination)
        {
            ViewBag.IndexType = "hotel";
            if (destination == null)
                return View("Index");
            else
            {
                ViewBag.Destination = destination;
                return View("Index");
            }
        }
    }
}