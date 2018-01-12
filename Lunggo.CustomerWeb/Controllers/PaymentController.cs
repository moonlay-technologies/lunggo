using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.CustomerWeb.Helper;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Extension;
using PaymentData = Lunggo.CustomerWeb.Models.PaymentData;

namespace Lunggo.CustomerWeb.Controllers
{
    public partial class PaymentController : Controller
    {
        public ActionResult Payment(string rsvNo = null, string regId = null, string trxId = null, string cartId = null)
        {
            if (!string.IsNullOrEmpty(cartId))
            {
                var payment = PaymentService.GetInstance().GetPayment(cartId);
                if (payment == null)
                    return RedirectToAction("Index", "Index");

                return HandlePaymentState(cartId, regId, payment);
            }

            trxId = trxId ?? rsvNo;
            try
            {
                if (string.IsNullOrEmpty(regId))
                    return RedirectToAction("Index", "Index");

                var signature = Generator.GenerateTrxIdRegId(trxId);
                if (regId != signature)
                    return RedirectToAction("Index", "Index");

                var payment = PaymentService.GetInstance().GetPayment(trxId);
                if (payment == null)
                    return RedirectToAction("Index", "Index");

                return HandlePaymentState(trxId, regId, payment);
            }
            catch
            {
                ViewBag.Message = "Failed";
                return View(new PaymentData
                {
                    TrxId = trxId
                });
            }

        }

        private ActionResult HandlePaymentState(string trxId, string regId, PaymentDetails payment)
        {
            if (payment.RedirectionUrl != null && payment.Status != PaymentStatus.Settled)
            {
                return Redirect(payment.RedirectionUrl);
            }
            else if (payment.Status == PaymentStatus.Pending &&
                     (payment.Method == PaymentMethod.BankTransfer ||
                      payment.Method == PaymentMethod.VirtualAccount))
            {
                return RedirectToAction("Instruction", "Payment", new { trxId, regId });
            }
            else if ((payment.Method == PaymentMethod.Undefined && payment.Status == PaymentStatus.Pending) ||
                     payment.Status == PaymentStatus.Failed)
            {
                ViewBag.SurchargeList = PaymentService.GetInstance().GetSurchargeList().Serialize();
                return View(new PaymentData
                {
                    //RsvNo = rsvNo,
                    //Reservation = rsv,
                    //PaymentDetails = payment,
                    TrxId = trxId,
                    TimeLimit = payment.TimeLimit,
                    OriginalPrice = payment.OriginalPriceIdr
                });
            }
            else
            {
                return RedirectToAction("Thankyou", "Payment", new { trxId, regId });
            }
        }

        [HttpPost]
        [ActionName("Payment")]
        public ActionResult PaymentPost(string rsvNo = null, string trxId = null, string cartId = null, string paymentUrl = null)
        {
            trxId = cartId ?? trxId ?? rsvNo;
            var payment = PaymentService.GetInstance().GetPayment(trxId);
            var regId = Generator.GenerateTrxIdRegId(trxId);

            if (payment.Method == PaymentMethod.BankTransfer ||
                payment.Method == PaymentMethod.VirtualAccount)
            {
                return RedirectToAction("Instruction", "Payment", new { trxId, regId });
            }
            else if (!string.IsNullOrEmpty(paymentUrl))
            {
                return Redirect(paymentUrl);
            }
            else
            {
                TempData["AllowThisThankyouPage"] = trxId;
                return RedirectToAction("Thankyou", "Payment", new { trxId, regId });
            }
        }

        public ActionResult Instruction(string trxId, string regId)
        {
            if (string.IsNullOrEmpty(regId))
                return RedirectToAction("Index", "Index");

            var generatedRegId = Generator.GenerateTrxIdRegId(trxId);
            if (generatedRegId != regId)
                return RedirectToAction("Index", "Index");

            var payment = PaymentService.GetInstance().GetPayment(trxId);

            if ((payment.Method == PaymentMethod.BankTransfer || payment.Method == PaymentMethod.VirtualAccount)
                && payment.Status == PaymentStatus.Pending)
            {
                ViewBag.Instructions = PaymentService.GetInstance().GetInstruction(payment);
                ViewBag.BankName = PaymentService.GetInstance().GetBankName(payment.Medium, payment.Method, payment.Submethod);
                ViewBag.BankBranch = PaymentService.GetInstance().GetBankBranch(payment.TransferAccount);
                ViewBag.BankImageName = GetBankImageName(payment.Submethod);
                return View(payment);
            }
            else
                TempData["AllowThisThankyouPage"] = trxId;
            return RedirectToAction("Thankyou", "Payment", new { trxId, regId });
        }

        public ActionResult InstructionPost(string trxId)
        {
            var regId = Generator.GenerateTrxIdRegId(trxId);
            return RedirectToAction("Thankyou", "Payment", new { trxId, regId });
        }

        public ActionResult Thankyou(string trxId, string regId)
        {
            if (string.IsNullOrEmpty(regId))
                return RedirectToAction("Index", "Index");

            var signature = Generator.GenerateTrxIdRegId(trxId);
            if (regId != signature)
                return RedirectToAction("Index", "Index");

            var payment = PaymentService.GetInstance().GetPayment(trxId);
            return View(payment);
        }

        [HttpPost]
        [ActionName("Thankyou")]
        public ActionResult ThankyouPost(string trxId)
        {
            TempData["AllowThisReservationCheck"] = trxId;
            return RedirectToAction("OrderFlightHistoryDetail", "Account", new { rsvNo = trxId });
        }

        public ActionResult Eticket(string trxId)
        {
            ReservationForDisplayBase rsvData;
            if (trxId.StartsWith("1"))
                rsvData = FlightService.GetInstance().GetReservationForDisplay(trxId);
            else if (trxId.StartsWith("2"))
                rsvData = HotelService.GetInstance().GetReservationForDisplay(trxId);
            else
                rsvData = ActivityService.GetInstance().GetReservationForDisplay(trxId);
            try
            {

                if (rsvData.RsvDisplayStatus == RsvDisplayStatus.Issued)
                {
                    if (rsvData.Type == ProductType.Flight)
                    {
                        var eticketData = FlightService.GetInstance().GetEticket(trxId);
                        return File(eticketData, "application/pdf");
                    }
                    else
                    {
                        var eticketData = HotelService.GetInstance().GetEticket(trxId);
                        return File(eticketData, "application/pdf");
                    }
                }
                else
                {
                    return View(rsvData);
                }

            }
            catch
            {
                return View(rsvData);
            }

        }

        #region Helpers

        public string GetBankImageName(PaymentSubmethod submethod)
        {
            switch (submethod)
            {
                case PaymentSubmethod.Mandiri:
                    return "mandiri.png";
                case PaymentSubmethod.BCA:
                    return "bca.png";
                case PaymentSubmethod.BNI:
                    return "bni.png";
                case PaymentSubmethod.BRI:
                    return "bri.png";
                case PaymentSubmethod.CIMB:
                    return "cimb-niaga.png";
                case PaymentSubmethod.Danamon:
                    return "danamon.png";
                case PaymentSubmethod.KEBHana:
                    return "keb-hana.png";
                case PaymentSubmethod.Permata:
                    return "permata.png";
                case PaymentSubmethod.Maybank:
                    return "maybank.png";
                case PaymentSubmethod.Other:
                    return "permata.png";
                default:
                    return null;
            }
        }

        #endregion
    }
}