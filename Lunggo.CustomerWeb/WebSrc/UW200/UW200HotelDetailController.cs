using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.CustomerWeb.WebSrc.UW200.Logic;
using Lunggo.CustomerWeb.WebSrc.UW200.Object;

namespace Lunggo.CustomerWeb.WebSrc.UW200
{
    public class Uw200HotelDetailController : Controller
    {
        public ActionResult GetHotelDetail(Uw200HotelDetailRequest request)
        {
            var response = HotelDetailLogic.GetHotelDetail(request);
            return View(response);
        }
    }
}