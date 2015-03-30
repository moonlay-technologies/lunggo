using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Model;
using Lunggo.ApCommon.Mystifly;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.BackendWeb.Interface;
using Lunggo.BackendWeb.Model;
using Lunggo.Flight.Model;
using Lunggo.Framework.Http;
using Lunggo.Repository.TableRecord;
using CabinType = Lunggo.ApCommon.Flight.Constant.CabinType;

namespace Lunggo.BackendWeb.Controllers
{
    public class FlightReservationController : Controller
    {
        // GET: FlightReservation
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(FlightReservationSearch search)
        {
            OnePointInterface.AirLowFareSearch(search);
            return RedirectToAction(search.RsvNo == null ? "List" : "Detail", search);
        }

        public ActionResult List(FlightReservationSearch search)
        {
            ViewData.Add("Name", search.PassengerName);
            var result = FlightReservationIntegrated.GetFromDb(search, QueryType.Overview);
            return View(result);
        }

        public ActionResult Detail(FlightReservationSearch search)
        {
            var result = FlightReservationIntegrated.GetFromDb(search, QueryType.Complete).SingleOrDefault();
            return View(result);
        }
        public ActionResult TestSearchFlight()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestSearchFlight(SearchFlightConditions conditions)
        {
            conditions.CabinType = CabinType.Economy;
            var infos = new List<OriginDestinationInfo>();
            infos.Add(new OriginDestinationInfo
            {
                DepartureDate = new DateTime(2015,4,4),
                OriginAirport = "CGK",
                DestinationAirport = "SIN"
            });
            infos.Add(new OriginDestinationInfo
            {
                DepartureDate = new DateTime(2015, 4, 6),
                OriginAirport = "SIN",
                DestinationAirport = "CGK"
            });
            conditions.OriDestInfos = infos;
            TempData["cond"] = conditions;
            return RedirectToAction("TestSearchedFlights");
        }

        public ActionResult TestSearchedFlights()
        {
           var flightService = FlightService.GetInstance();
            flightService.Init("MCN004085", "GOAXML", "GA2014_xml", Target.Test);
            var x = flightService.SearchFlight((SearchFlightConditions) TempData["cond"]);
            return View(x);
        }
    }

}