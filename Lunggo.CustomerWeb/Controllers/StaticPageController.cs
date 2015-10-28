using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Controllers
{
    public class StaticPageController : Controller
    {
        // GET: StaticPage
        public ActionResult Question()
        {
            return View();
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult HowToOrder()
        {
            return View();
        }

        public ActionResult HowToPay()
        {
            return View();
        }
    }
}