﻿using System.Collections.Generic;
using System.Web.Mvc;
using Lunggo.ApCommon.Currency.Service;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment;
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
            var reports = PaymentService.GetInstance().GetAllTransferConfirmationReports();
            var reservations = FlightService.GetInstance().GetUnpaidReservations();
            var temporary = new CheckPaymentViewModel
            {
                Reports = reports,
                Reservation = reservations
            };
            return View(temporary);
        }

        [HttpPost]
        public ActionResult CheckReservation(List<CheckReservationControllerModel> reservationPayments)
        {
            var useReservation = FlightService.GetInstance();
            foreach (var Payment in reservationPayments)
            {
                Payment.PaymentInfo.Status = Payment.Status;
                useReservation.UpdateFlightPayment(Payment.NoRsv, Payment.PaymentInfo);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult CheckPayment(List<CheckPaymentControllerModel> payments)
        {
            var usePayment = PaymentService.GetInstance();
            foreach (var listPayment in payments)
            {
                usePayment.UpdateTransferConfirmationReportStatus(listPayment.RsvNo, listPayment.Status);
            }
            return RedirectToAction("Index", "Home");
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
    }
}