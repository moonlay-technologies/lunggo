using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Core;
using Lunggo.Framework.Error;

namespace Lunggo.CustomerWeb.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            throw new Exception();
            try
            {
                //LunggoLogger.Info("test aja");
                //LunggoLogger.Error("error boong");
                return View();
            }
            catch (Exception exception)
            {
                LunggoLogger.Error("error aja");
            }

            return new EmptyResult();
        }
        //public ActionResult Index()
        //{
        //    var coba = Session["test"];
        //    Trace.TraceWarning("Trace.TraceWarning");
        //    string idflight = FlightReservationSequence.GetInstance().GetFlightReservationId(EnumReservationType.ReservationType.Member);
        //    string idhotel = HotelReservationSequence.GetInstance().GetHotelReservationId(EnumReservationType.ReservationType.Member);
        //    Dictionary<string,int> testDic = new Dictionary<string, int>();
        //    testDic.Add("asd",1);
        //    testDic.Add("asde", 2);

        //    Session["test"] = testDic;
        //    return View();
        //}

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

        

        public ActionResult CekPemesanan()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CekPemesanan(string rsvNo, string lastName)
        {
            if (rsvNo.IsEmpty() || lastName.IsEmpty())
                return Redirect("/");
            var flightService = ApCommon.Flight.Service.FlightService.GetInstance();
            var displayReservation = flightService.GetReservationForDisplay(rsvNo);
            //Check lastName

            if (displayReservation != null)
            {
                var passengerLastName = displayReservation.Passengers.Where(x => x.LastName == lastName);
                if (passengerLastName.Any())
                {
                    var rsvNoSet = new
                    {
                        RsvNo = displayReservation.RsvNo
                    };
                    return RedirectToAction("OrderFlightHistoryDetail", "Uw620OrderHistory", rsvNoSet);
                }
            }

            ViewBag.ErrorInfo = new Error
            {
                Code = "ER01",
                Message = "Pemesanan tidak dapat ditemukan"
            };
                
            return View();
        }
    }
}