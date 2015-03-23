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
            return
                View(
                    FlightReservationIntegrated.GetFromDb(search, FlightReservationIntegrated.QueryType.Overview)
                        .ToList());
        }

        public ActionResult Detail(FlightReservationSearch search)
        {
            return
                View(
                    FlightReservationIntegrated.GetFromDb(search, FlightReservationIntegrated.QueryType.Complete)
                        .SingleOrDefault());
        }
    }

}