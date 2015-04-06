using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.BackendWeb.Model;
using Microsoft.Ajax.Utilities;

namespace Lunggo.BackendWeb.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(SearchFlightInput input)
        {
            var result = FlightService.GetInstance().SearchFlight(input);
            TempData["result"] = result;
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            return View(TempData["result"]);
        }

        public ActionResult InputData(TestInputData input)
        {
            var revalidate = new RevalidateFlightInput
            {
                FareId = input.FareId,
                TotalFare = input.TotalFare
            };
            var revalidateResult = FlightService.GetInstance().RevalidateFlight(revalidate);
            FlightFareItinerary itin = null;
            if (revalidateResult.IsValid || revalidateResult.Itinerary != null)
            {
                itin = revalidateResult.Itinerary;
            }
            return View(itin);
        }
    }
}