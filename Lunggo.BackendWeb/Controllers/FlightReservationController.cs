using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using Lunggo.ApCommon.Model;
using Lunggo.BackendWeb.Model;
using Lunggo.Flight.Model;
using Lunggo.Framework.Http;
using Lunggo.Repository.TableRecord;

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
    }

}