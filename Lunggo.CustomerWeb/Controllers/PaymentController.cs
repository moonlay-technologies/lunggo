using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Constant;
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
using Lunggo.Framework.Redis;
using RestSharp;
using PaymentData = Lunggo.CustomerWeb.Models.PaymentData;
using Lunggo.ApCommon.Account.Service;

namespace Lunggo.CustomerWeb.Controllers
{
    public partial class PaymentController : Controller
    {
        private PaymentService _paymentService;

        public PaymentController(PaymentService paymentService = null)
        {
            _paymentService = paymentService ?? new PaymentService();
        }

        private decimal GetAvailableCredits(string trxId, out string voucherCode)
        {
            voucherCode = "REFERRALCREDIT";
            var activityService = ActivityService.GetInstance();
            var rsvList = _paymentService.GetCart(trxId);
            var userId = activityService.GetUserIdByRsvNo(rsvList.RsvNoList[0]);
            var voucherRs = new PaymentService().ValidateVoucherRequest(trxId, voucherCode);
            var voucherRsLimit = AccountService.GetInstance().GetReferral(userId).ReferralCredit;
            if(voucherRsLimit < voucherRs.TotalDiscount)
            {
                return voucherRsLimit;
            }
            return voucherRs.TotalDiscount;
        }

        public ActionResult Payment(string rsvNo = null, string regId = null, string trxId = null, string cartId = null)
        {
            if (!string.IsNullOrEmpty(cartId))
            {
                var payment = _paymentService.GetPaymentDetails(cartId);
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

                var payment = _paymentService.GetPaymentDetails(trxId);
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
                ViewBag.SurchargeList = _paymentService.GetSurchargeList().Serialize();

                string creditsVoucherCode;
                var creditsAvailable = GetAvailableCredits(trxId, out creditsVoucherCode);
                ViewBag.CreditsAvailable = creditsAvailable;
                ViewBag.CreditsVoucherCode = creditsVoucherCode;

                return View(new PaymentData
                {
                    //RsvNo = rsvNo,
                    //Reservation = rsv,
                    //PaymentDetails = payment,
                    TrxId = trxId,
                    TimeLimit = payment.TimeLimit.GetValueOrDefault(),
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
        public ActionResult PaymentPost(string rsvNo = null, string trxId = null, string paymentUrl = null)
        {
            trxId = trxId ?? rsvNo;
            var payment = _paymentService.GetPaymentDetails(trxId);
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

            var payment = _paymentService.GetPaymentDetails(trxId);

            if ((payment.Method == PaymentMethod.BankTransfer || payment.Method == PaymentMethod.VirtualAccount)
                && payment.Status == PaymentStatus.Pending)
            {
                ViewBag.Instructions = _paymentService.GetInstruction(payment);
                ViewBag.BankName = _paymentService.GetBankName(payment.Medium, payment.Method, payment.Submethod);
                ViewBag.BankBranch = _paymentService.GetBankBranch(payment.TransferAccount);
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

            var payment = _paymentService.GetPaymentDetails(trxId);
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

        public ActionResult OnlineDebit(string rsvNo, string id, int medium)
        {
            if (medium != (int)PaymentMedium.E2Pay)
            {
                return RedirectToAction("Payment", "Payment", new { rsvNo = rsvNo, regId = Generator.GenerateTrxIdRegId(rsvNo) });
            }

            var html = GetE2PayPaymentHtmlFromCache(id);
            if (html == null)
                return RedirectToAction("Payment", "Payment", new { rsvNo = rsvNo, regId = Generator.GenerateTrxIdRegId(rsvNo) });
            return Content(html);
        }

        #region Helpers

        private string GetE2PayPaymentHtmlFromCache(string guid)
        {
            var redisService = RedisService.GetInstance();
            var redisKey = "e2pay:paymentPage:" + guid;
            var redisDb = redisService.GetDatabase(ApConstant.MasterDataCacheName);
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                try
                {
                    var html = redisDb.StringGet(redisKey);
                    return html;
                }
                catch
                {
                    if (i >= ApConstant.RedisMaxRetry)
                        throw;
                }
            }
            return null;
        }

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