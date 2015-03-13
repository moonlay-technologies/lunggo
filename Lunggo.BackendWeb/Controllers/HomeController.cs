using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Model;
using Lunggo.Flight.Dictionary;
using Lunggo.Flight.Model;
using Lunggo.Flight.Query;
using Lunggo.Framework.Database;
using Lunggo.Framework.Http;
using Lunggo.Repository.TableRecord;

namespace Lunggo.BackendWeb.Controllers
{
    public class HomeController : Controller
    {

        //public HotelTableRepo hotelTable = HotelTableRepo.GetInstance();
        //public IDbConnection connHotel = DbService.GetInstance().GetOpenConnection();

        public ActionResult Index()
        {
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

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Hotel()
        {
            //var result = hotelTable.FindAll(connHotel);
            //return View(result);
            return View();
        }

        public ActionResult HotelDetail(int id)
        {
            //return View(hotelTable.FindAll(connHotel).Single((x=> x.Id == id)));
            return View();
        }

        public ActionResult FlightReservationSearch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FlightReservationSearch(FlightReservationSearch search)
        {
            if (search.RsvNo == null)
            {
                return RedirectToAction("FlightReservationList", search);
            }
            else
            {
                return RedirectToAction("FlightReservationDetail", search);
            }
        }
        public ActionResult FlightReservationList(FlightReservationSearch search)
        {
            ViewData.Add("Name", search.PassengerName);
            return View(FlightReservationIntegrated.GetFromDb(search, FlightReservationIntegrated.QueryType.Overview).ToList());
        }

        public ActionResult FlightReservationDetail(FlightReservationSearch search)
        {
            return View(FlightReservationIntegrated.GetFromDb(search, FlightReservationIntegrated.QueryType.Complete).SingleOrDefault());
        }

        public ActionResult GetAirline(string prefix)
        {
            return Json(TrieIndex.AirlineName.GetAllIdContainingSuggestedWords(prefix).Select(id => FlightCode.Airline[id]).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAirport(string prefix)
        {
            return Json(TrieIndex.AirportName.GetAllIdContainingSuggestedWords(prefix).Select(id => FlightCode.Airport[id]).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult FlightReservationDelete(FlightReservationSearch search)
        {
            var integrated = FlightReservationIntegrated.GetFromDb(search, FlightReservationIntegrated.QueryType.PrimKeys).SingleOrDefault();
            if (integrated != null)
                integrated.DeleteFromDb();
            return RedirectToAction("FlightReservationSearch");
        }
    }
}