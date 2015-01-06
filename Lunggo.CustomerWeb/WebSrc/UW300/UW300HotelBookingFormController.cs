using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lunggo.CustomerWeb.WebSrc.UW300
{
    public class Uw300HotelBookingFormController : Controller
    {
        // GET: UW300HotelBookingForm
        public ActionResult DisplayBookingForm()
        {
            return View();
        }
    }
}