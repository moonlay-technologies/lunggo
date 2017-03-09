using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Util;

namespace Lunggo.CustomerWeb.Controllers
{
    public class B2BTemplateController : Controller
    {
        // GET: B2BTemplate
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        public ActionResult ConfirmPassword()
        {
            return View();
        }
        public ActionResult SearchFlight()
        {
            return View();
        }
        public ActionResult SearchHotel()
        {
            return View();
        }
        public ActionResult Payment()
        {
            return View();
        }
        public ActionResult OrderListFlight()
        {
            return View();
        }
        public ActionResult OrderListHotel()
        {
            return View();
        }
        public ActionResult OrderDetailFlight()
        {
            return View();
        }
        public ActionResult OrderDetailHotel()
        {
            return View();
        }
        public ActionResult UserManagement()
        {
            return View();
        }

        public ActionResult TestEmail()
        {
            var hotelService = HotelService.GetInstance();
            var reservation = hotelService.GetReservationForDisplay("217306558579");
            var mailData = new HotelBookingNotif
            {
                Token = GenerateTokenUtil.GenerateTokenByRsvNo("217306558579"),
                Reservation = reservation
            };
            return View(mailData);
        }

        public ActionResult TestEmailFlight()
        {
            var flightService = FlightService.GetInstance();
            var reservation = flightService.GetReservationForDisplay("116496559679");
            var mailData = new FlightBookingNotif
            {
                Token = GenerateTokenUtil.GenerateTokenByRsvNo("116496559679"),
                Reservation = reservation
            };
            return View(mailData);
        }
    }
}