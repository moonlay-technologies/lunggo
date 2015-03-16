using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using Lunggo.ApCommon.Model;
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
            if (search.RsvNo == null)
            {
                return RedirectToAction("List", search);
            }
            else
            {
                return RedirectToAction("Detail", search);
            }
        }
        public ActionResult List(FlightReservationSearch search)
        {
            ViewData.Add("Name", search.PassengerName);
            return View(FlightReservationIntegrated.GetFromDb(search, FlightReservationIntegrated.QueryType.Overview).ToList());
        }

        public ActionResult Detail(FlightReservationSearch search)
        {
            return View(FlightReservationIntegrated.GetFromDb(search, FlightReservationIntegrated.QueryType.Complete).SingleOrDefault());
        }

        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insert(FlightReservationIntegrated integrated)
        {
            integrated.TripDetail.Add(0, new List<FlightTripDetailTableRecord>());
            integrated.Passenger.Add(0, new List<FlightPassengerTableRecord>());
            var x = Request.Form.Get(5);
            var y = Request.Form.Get("Reservation.ContactName");
            return RedirectToAction("Search");
        }

        public ActionResult Edit(FlightReservationSearch search)
        {
            var integrated = FlightReservationIntegrated.GetFromDb(search, FlightReservationIntegrated.QueryType.Complete).SingleOrDefault();
            return View(integrated);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditConfirm(FlightReservationIntegrated integrated)
        {
            integrated.TripDetail.Add(long.Parse(Request.Form["Trip[0].TripId"]), new List<FlightTripDetailTableRecord>());
           // TryUpdateModel(integrated);
            TryUpdateModel(integrated.TripDetail[long.Parse(Request.Form["Trip[0].TripId"])],
                "TripDetail[" + Request.Form["Trip[0].TripId"] + "]");
            integrated.UpdateToDb();
            return RedirectToAction("Detail", integrated);
        }
        public ActionResult Delete(FlightReservationSearch search)
        {
            var integrated = FlightReservationIntegrated.GetFromDb(search, FlightReservationIntegrated.QueryType.PrimKeys).SingleOrDefault();
            if (integrated != null)
                integrated.DeleteFromDb();
            return RedirectToAction("Search");
        }

        public ActionResult RenderTripDetailInput(dynamic input)
        {
            ViewData.Add("int", 5);
            return PartialView();
        }
    }
}