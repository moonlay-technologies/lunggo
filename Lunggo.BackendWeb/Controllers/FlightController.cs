using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.BackendWeb.Models;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRepository;
using Lunggo.Repository.TableRecord;
using Lunggo.BackendWeb.Model;

namespace Lunggo.BackendWeb.Controllers
{
    [Authorize]
    public class FlightController : Controller
    {
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult List(FlightReservationSearch search)
        {
            if (search.RsvDateSelection == FlightReservationSearch.DateSelectionType.MonthYear)
                return RedirectToAction("List", new {month = search.RsvDateMonth, year = search.RsvDateYear});
            var flight = FlightService.GetInstance();
            var reservations = flight.SearchReservations(search);
            return View(reservations);
        }

        public ActionResult List(int month, int year, bool? hideExpired, bool? completedOnly)
        {
            var rsv = FlightService.GetInstance().SearchReservations(new FlightReservationSearch
            {
                RsvDateSelection = FlightReservationSearch.DateSelectionType.MonthYear,
                RsvDateMonth = month,
                RsvDateYear = year
            });

            ViewBag.MonthTotal = rsv.Sum(r => r.Payment.Status == PaymentStatus.Settled ? r.Payment.FinalPrice : 0);

            if (hideExpired.GetValueOrDefault())
                rsv = rsv.Where(r => r.Payment.Status != PaymentStatus.Expired).ToList();
            if (completedOnly.GetValueOrDefault())
                rsv = rsv.Where(r => r.Payment.Status == PaymentStatus.Settled).ToList();
            rsv = rsv.OrderBy(r => r.RsvTime).ToList();
            
            ViewBag.Month = month;
            ViewBag.Year = year;
            return View(rsv);
        }

        public ActionResult Detail(string rsvNo)
        {
            var flight = FlightService.GetInstance();
            var reservation = flight.GetReservation(rsvNo);
            return View(reservation);
        }
    }
}