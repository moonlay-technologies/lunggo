using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lunggo.ApCommon.Currency.Service;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.BackendWeb.Models;

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
            var flight = FlightService.GetInstance();
            var reservations = flight.SearchReservations(search);
            return View(reservations);
        }

        public ActionResult Detail(string rsvNo)
        {
            var flight = FlightService.GetInstance();
            var reservation = flight.GetReservationForDisplay(rsvNo);
            return View(reservation);
        }

        public ActionResult CheckPayment()
        {
            var reports = PaymentService.GetInstance().GetUncheckedTransferConfirmationReports();
            var reservations = FlightService.GetInstance().GetUnpaidReservations();
            var reportedReservations = reservations.Where(rsv => reports.Exists(rep => rep.RsvNo == rsv.RsvNo)).ToList();
            var unreportedReservations = reservations.Except(reportedReservations);
            var temporary = new CheckPaymentViewModel
            {
                Reports = reports,
                ReportedReservations = reportedReservations,
                UnreportedReservations = unreportedReservations.ToList()
            };
            return View(temporary);
        }

        [HttpPost]
        public ActionResult CheckPayment(List<CheckPaymentControllerModel> payments)
        {
            var payment = PaymentService.GetInstance();
            var flight = FlightService.GetInstance();
            foreach (var listPayment in payments)
            {
                if (listPayment.Status != TransferConfirmationReportStatus.Unchecked)
                {
                    payment.UpdateTransferConfirmationReportStatus(listPayment.RsvNo, listPayment.Status);
                    if (listPayment.Status == TransferConfirmationReportStatus.Confirmed)
                    {
                        var updatedInfo = new PaymentInfo {PaidAmount = listPayment.Amount};
                        var reservation = flight.GetReservationForDisplay(listPayment.RsvNo);
                        if (listPayment.Amount >= reservation.Payment.FinalPrice)
                            updatedInfo.Status = PaymentStatus.Settled;
                        FlightService.GetInstance().UpdateFlightPayment(listPayment.RsvNo, updatedInfo);
                    }   
                }
            }
            return RedirectToAction("CheckPayment", "Flight");
        }

        public ActionResult ExchangeRate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExchangeRate(string currencyCode)
        {
            var currencyService = CurrencyService.GetInstance();
            var modelRate = currencyService.GetCurrencyExchangeRate(currencyCode);
            if (modelRate == 0)
            {
                var temporary = new ExchangeRateViewModel
                {
                    CurrencyCode = currencyCode,
                    Rate = modelRate,
                    Hidden = 0
                };
                return RedirectToAction("InsertExchangeRate", "Flight", temporary);

            }
            else
            {
                var temporary = new ExchangeRateViewModel
                {
                    CurrencyCode = currencyCode,
                    Rate = modelRate,
                    Hidden = 1
                };
                return View(temporary);
            }  
        }

        public ActionResult InsertExchangeRate(ExchangeRateViewModel checkRate)
        {
            return View(checkRate);
        }

        [HttpPost]
        [ActionName("InsertExchangeRate")]
        public ActionResult InsertExchangeRatePost(ExchangeRateViewModel checkRate)
        {
            var currencyService = CurrencyService.GetInstance();
            currencyService.SetCurrencyExchangeRate(checkRate.CurrencyCode, checkRate.Rate);
            return RedirectToAction("ExchangeRate", "Flight");
        }

        public ActionResult PriceMarginList()
        {
            var flight = FlightService.GetInstance();
            var rules = flight.GetAllPriceMarginRules();
            return View(rules);
        }
    }
}