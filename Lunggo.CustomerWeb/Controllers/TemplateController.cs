using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Controllers
{
    public class TemplateController : Controller
    {
        // GET: Template
        public ActionResult HotelSearchList()
        {
            return View();
        }

        public ActionResult HotelDetail()
        {
            return View();
        }

        public ActionResult HotelCheckout()
        {
            return View();
        }

        public ActionResult HotelCheckoutPayment()
        {
            return View();
        }

        public ActionResult HotelCheckoutThankyou()
        {
            return View();
        }

        public ActionResult FlightSearchList()
        {
            return View();
        }

        public ActionResult FlightCheckout()
        {
            return View();
        }

        public ActionResult FlightCheckoutPayment()
        {
            return View();
        }

        public ActionResult FlightCheckoutThankyou()
        {
            return View();
        }
    }
}