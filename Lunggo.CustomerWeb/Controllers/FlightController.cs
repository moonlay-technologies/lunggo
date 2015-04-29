using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.CustomerWeb.Models;

namespace Lunggo.CustomerWeb.Controllers
{
    public class FlightController : Controller
    {
        public ActionResult SearchResultList(FlightSearchData search)
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchResultList(string searchId, int itinIndex)
        {
            var service = FlightService.GetInstance();
            var token = service.SaveItineraryToCache(searchId, itinIndex);
            return RedirectToAction("Checkout", new FlightSelectData { token = token });
        }

        public ActionResult Checkout(FlightSelectData select)
        {
            var service = FlightService.GetInstance();
            var itinerary = service.GetItineraryFromCache(select.token);
            return View(itinerary);
        }
    }
}