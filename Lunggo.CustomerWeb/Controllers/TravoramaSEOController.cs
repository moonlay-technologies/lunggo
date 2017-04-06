using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.Controllers
{
    public class TravoramaSEOController : Controller
    {
        // GET: TravoramaSEO
        public ActionResult HotelSEO()
        {
            return View();
        }
        public ActionResult AirlineSEO()
        {
            return View();
        } 
        public ActionResult FlightTo()
        {
            return View();
        }
    }
}