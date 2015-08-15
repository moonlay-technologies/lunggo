using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Microsoft.AspNet.Identity;

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

        public ActionResult FlightSearchListReturn()
        {
            return View();
        }

        public ActionResult FlightSearchListOneway()
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

        public ActionResult Static()
        {
            return View();
        }

        public ActionResult UserLogin()
        {
            return View();
        }

        public ActionResult UserRegister()
        {
            return View();
        }

        public ActionResult UserAccount()
        {
            return View();
        }

        public ActionResult FlightSearchListNew()
        {
            return View();
        }

        public ActionResult LandingPage()
        {
            return View();
        }

        public ActionResult Activity()
        {
            return View();
        }

        public ActionResult eticket()
        {
            return View();
        }

        public ActionResult eticket2(string rsvNo)
        {
            var rsv = FlightService.GetInstance().GetReservation(rsvNo);
            return View(rsv);
        }

        public ActionResult OrderFlightHistory()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult HowToBook()
        {
            return View();
        }

        public ActionResult EmailSpecialRelease()
        {
            return View();
        }

        public ActionResult HomePageCampaign()
        {
            return View();
        }
    }
}