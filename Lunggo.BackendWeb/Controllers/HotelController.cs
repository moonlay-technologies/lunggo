using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.BackendWeb.Controllers
{
    [Authorize]
    public class HotelController : Controller
    {
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult List(HotelReservationSearch search)
        {
            if (User.IsInRole("Admin"))
            {
                var test = "OK";
            }
            if (search.RsvDateSelection == HotelReservationSearch.DateSelectionType.MonthYear)
                return RedirectToAction("List", new { month = search.RsvDateMonth, year = search.RsvDateYear });
            var hotel = HotelService.GetInstance();
            var reservations = hotel.SearchReservations(search);
            return View(reservations);
        }

        public ActionResult List(int month, int year, bool? hideExpired, bool? completedOnly)
        {
            var rsv = HotelService.GetInstance().SearchReservations(new HotelReservationSearch
            {
                RsvDateSelection = HotelReservationSearch.DateSelectionType.MonthYear,
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
            var hotel = HotelService.GetInstance();
            var reservation = hotel.GetReservation(rsvNo);
            return View(reservation);
        }
    }
}