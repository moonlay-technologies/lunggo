using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.BackendWeb.Models;
using Lunggo.BackendWeb.Query;
using Lunggo.Framework.Database;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRepository;
using Lunggo.Repository.TableRecord;
using Lunggo.BackendWeb.Model;
using Microsoft.WindowsAzure.Storage.Queue;

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
                return RedirectToAction("List", new { month = search.RsvDateMonth, year = search.RsvDateYear });
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

            ViewBag.MonthTotal = rsv.Sum(r => r.Payment.Status == PaymentStatus.Settled ? r.Payment.FinalPriceIdr : 0);

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
            ViewBag.Issue = TempData["Issue"];
            ViewBag.SendEmail = TempData["SendEmail"];
            ViewBag.OverridePayment = TempData["OverridePayment"];
            ViewBag.OverrideIssue = TempData["OverrideIssue"];
            return View(reservation);
        }

        public ActionResult Issue(string rsvNo)
        {
            var queue = QueueService.GetInstance().GetQueueByReference("FlightIssueTicket");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
            TempData["Issue"] = true;
            Thread.Sleep(3000);
            return RedirectToAction("Detail", null, new { rsvNo });
        }

        public ActionResult SendEmail(string rsvNo)
        {
            var queue = QueueService.GetInstance().GetQueueByReference("FlightEticket");
            queue.AddMessage(new CloudQueueMessage(rsvNo));
            TempData["SendEmail"] = true;
            return RedirectToAction("Detail", null, new { rsvNo });
        }

        public ActionResult ConfirmOverridePayment(string rsvNo)
        {
            var flight = FlightService.GetInstance();
            var reservation = flight.GetReservation(rsvNo);
            return View("ConfirmOverridePayment", reservation);
        }

        [HttpPost]
        [ActionName("ConfirmOverridePayment")]
        public ActionResult ConfirmOverridePaymentPost(string rsvNo)
        {
            PaymentService.GetInstance().UpdatePayment(rsvNo, new PaymentDetails { Status = PaymentStatus.Settled });
            TempData["OverridePayment"] = true;
            Thread.Sleep(3000);
            return RedirectToAction("Detail", null, new { rsvNo });
        }

        public ActionResult ConfirmOverrideIssue(string rsvNo)
        {
            var flight = FlightService.GetInstance();
            var reservation = flight.GetReservation(rsvNo);
            return View("ConfirmOverrideIssue", reservation);
        }

        [HttpPost]
        [ActionName("ConfirmOverrideIssue")]
        public ActionResult ConfirmOverrideIssuePost(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var itins = GetItineraryIdsQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).ToList();
                foreach (var itin in itins)
                {
                    FlightItineraryTableRepo.GetInstance()
                        .Update(conn, new FlightItineraryTableRecord { Id = itin.Id, BookingStatusCd = "TKTD" });
                    var segIds = GetSegmentIdsQuery.GetInstance().Execute(conn, new { ItineraryId = itin.Id }).ToList();
                    foreach (var segId in segIds)
                    {
                        FlightSegmentTableRepo.GetInstance()
                            .Update(conn, new FlightSegmentTableRecord { Id = segId, Pnr = itin.BookingId });
                    }
                }
                var queue = QueueService.GetInstance().GetQueueByReference("FlightEticket");
                queue.AddMessage(new CloudQueueMessage(rsvNo));
                TempData["OverrideIssue"] = true;
                return RedirectToAction("Detail", null, new { rsvNo });
            }
        }
    }
}