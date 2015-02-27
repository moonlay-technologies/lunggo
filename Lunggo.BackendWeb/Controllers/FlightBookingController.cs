using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.Flight.Model;

namespace Lunggo.BackendWeb.Controllers
{
    public class FlightBookingController : Controller
    {
        // GET: FlightBooking
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchFlightBooking(FlightBookingDetail record)
        {
            
        }

    }
}