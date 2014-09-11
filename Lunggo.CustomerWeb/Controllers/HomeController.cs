using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Diagnostics;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Sequence;

namespace Lunggo.CustomerWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var coba = Session["test"];
            Trace.TraceWarning("Trace.TraceWarning");
            string idflight = FlightReservationSequence.GetInstance().GetFlightReservationId(EnumReservationType.ReservationType.Member);
            string idhotel = HotelReservationSequence.GetInstance().GetHotelReservationId(EnumReservationType.ReservationType.Member);
            Dictionary<string,int> testDic = new Dictionary<string, int>();
            testDic.Add("asd",1);
            testDic.Add("asde", 2);

            Session["test"] = testDic;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}