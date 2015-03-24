using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.CustomerWeb.WebSrc.UW100.Logic;
using Lunggo.CustomerWeb.WebSrc.UW100.Object;

namespace Lunggo.CustomerWeb.WebSrc.UW100
{
    public class Uw100HotelSearchController : Controller
    {
        public ActionResult Search(Uw100HotelSearchRequest request)
        {
            var response = HotelSearchLogic.GetHotels(request);
            return View(response);
        }
    }
}