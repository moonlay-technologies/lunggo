using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Lunggo.CustomerWeb.Controllers
{
    public class MobileTemplateController : Controller
    {
        // GET: Template
        public ActionResult HomePage()
        {
            return View();
        }

        public ActionResult HotelSearchPage()
        {
            return View();
        }

        public ActionResult HotelDetailPage()
        {
            return View();
        }

        public ActionResult FlightSearchPage()
        {
            return View();
        }

        public ActionResult LoginPage()
        {
            return View();
        }

        public ActionResult RegisterPage()
        {
            return View();
        }

        public ActionResult UserAccountPage()
        {
            return View();
        }

        public ActionResult FlightCheckout1()
        {
            return View();
        }

        public ActionResult FlightCheckout2()
        {
            return View();
        }

        public ActionResult FlightCheckout3()
        {
            return View();
        }
    }
}