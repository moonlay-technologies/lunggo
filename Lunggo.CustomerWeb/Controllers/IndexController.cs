using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.Framework.Filter;

namespace Lunggo.CustomerWeb.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
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