using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.Framework.Filter;

namespace Lunggo.CustomerWeb.WebSrc.UW000
{
    public class UW000TopPageController : Controller
    {
        // GET: UW000TopPage
        [DeviceDetectionFilter]
        public ActionResult Index(string destination)
        {
            if (destination == null)
                return View();
            else
            {
                ViewBag.Destination = destination;
                return View();
            }
        }
    }
}