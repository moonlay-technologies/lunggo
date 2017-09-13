using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Extension;
using PaymentData = Lunggo.CustomerWeb.Models.PaymentData;

namespace Lunggo.CustomerWeb.Controllers
{
    public partial class PaymentController : Controller
    {
        public ActionResult Payment(string rsvNo, string regId)
        {
            try
            {
                if (string.IsNullOrEmpty(regId))
                {
                    return RedirectToAction("Index", "Index");
                }
                var signature = GenerateId(rsvNo);
                if (regId.Equals(signature))
                {
                    ReservationForDisplayBase rsv;
                    if (rsvNo[0] == '1')
                        rsv = FlightService.GetInstance().GetReservationForDisplay(rsvNo);
                    else
                        rsv = HotelService.GetInstance().GetReservationForDisplay(rsvNo);


                    if (rsv.Payment.RedirectionUrl != null && rsv.Payment.Status != PaymentStatus.Settled)
                    {
                        return Redirect(rsv.Payment.RedirectionUrl);
                    }
                    else if (rsv.Payment.Status == PaymentStatus.Pending &&
                         (rsv.Payment.Method == PaymentMethod.BankTransfer ||
                          rsv.Payment.Method == PaymentMethod.VirtualAccount))
                    {
                        return RedirectToAction("Instruction", "Payment", new { rsvNo, regId });
                    }
                    else if ((rsv.Payment.Method == PaymentMethod.Undefined && rsv.Payment.Status == PaymentStatus.Pending) ||
                        rsv.Payment.Status == PaymentStatus.Failed)
                    {
                        ViewBag.SurchargeList = PaymentService.GetInstance().GetSurchargeList().Serialize();
                        return View(new PaymentData
                        {
                            RsvNo = rsvNo,
                            Reservation = rsv,
                            TimeLimit = rsv.Payment.TimeLimit.GetValueOrDefault(),
                        });
                    }
                    else
                    {
                        return RedirectToAction("Thankyou", "Payment", new { rsvNo, regId });
                    }
                }
                return RedirectToAction("Index", "Index");

            }
            catch
            {
                ViewBag.Message = "Failed";
                return View(new PaymentData
                {
                    RsvNo = rsvNo
                });
            }

        }

        [HttpPost]
        [ActionName("Payment")]
        public ActionResult PaymentPost(string rsvNo, string paymentUrl)
        {
            var payment = PaymentService.GetInstance().GetPayment(rsvNo);
            var regId = GenerateId(rsvNo);
            if (payment.Method == PaymentMethod.BankTransfer ||
                payment.Method == PaymentMethod.VirtualAccount)
            {
                return RedirectToAction("Instruction", "Payment", new { rsvNo, regId });
            }
            else if (!string.IsNullOrEmpty(paymentUrl))
            {
                return Redirect(paymentUrl);
            }
            else
            {
                TempData["AllowThisThankyouPage"] = rsvNo;
                return RedirectToAction("Thankyou", "Payment", new { rsvNo, regId });
            }
        }

        public ActionResult Instruction(string rsvNo, string regId)
        {
            if (string.IsNullOrEmpty(regId))
            {
                return RedirectToAction("Index", "Index");
            }
            var signature = GenerateId(rsvNo);
            if (regId.Equals(signature))
            {
                ReservationForDisplayBase rsv;
                if (rsvNo[0] == '1')
                    rsv = FlightService.GetInstance().GetReservationForDisplay(rsvNo);
                else
                    rsv = HotelService.GetInstance().GetReservationForDisplay(rsvNo);
                if ((rsv.Payment.Method == PaymentMethod.BankTransfer || rsv.Payment.Method == PaymentMethod.VirtualAccount)
                    && rsv.Payment.Status == PaymentStatus.Pending)
                {
                    ViewBag.Instructions = PaymentService.GetInstance().GetInstruction(rsv.Payment.Medium, rsv.Payment.Method, rsv.Payment.Submethod);
                    ViewBag.BankName = PaymentService.GetInstance().GetBankName(rsv.Payment.Medium, rsv.Payment.Method, rsv.Payment.Submethod);
                    ViewBag.BankBranch = PaymentService.GetInstance().GetBankBranch(rsv.Payment.TransferAccount);
                    ViewBag.BankImageName = GetBankImageName(rsv.Payment.Submethod);
                    return View(rsv);
                }
                else
                    TempData["AllowThisThankyouPage"] = rsvNo;
                return RedirectToAction("Thankyou", "Payment", new { rsvNo, regId = signature });
            }
            return RedirectToAction("Index", "Index");
        }

        public ActionResult InstructionPost(string rsvNo)
        {
            var regId = GenerateId(rsvNo);
            return RedirectToAction("Thankyou", "Payment", new { rsvNo, regId });
        }

        public ActionResult Thankyou(string rsvNo, string regId)
        {
            if (string.IsNullOrEmpty(regId))
            {
                return RedirectToAction("Index", "Index");
            }
            var signature = GenerateId(rsvNo);
            if (regId.Equals(signature))
            {
                ReservationForDisplayBase rsv;
                if (rsvNo[0] == '1')
                    rsv = FlightService.GetInstance().GetReservationForDisplay(rsvNo);
                else
                    rsv = HotelService.GetInstance().GetReservationForDisplay(rsvNo);
                return View(rsv);
            }
            return RedirectToAction("Index", "Index");
        }

        [HttpPost]
        [ActionName("Thankyou")]
        public ActionResult ThankyouPost(string rsvNo)
        {
            TempData["AllowThisReservationCheck"] = rsvNo;
            return RedirectToAction("OrderFlightHistoryDetail", "Account", new { rsvNo });
        }

        public ActionResult Eticket(string rsvNo)
        {
            ReservationForDisplayBase rsvData;
            if (rsvNo.StartsWith("1"))
                rsvData = FlightService.GetInstance().GetReservationForDisplay(rsvNo);
            else
                rsvData = HotelService.GetInstance().GetReservationForDisplay(rsvNo);
            try
            {

                if (rsvData.RsvDisplayStatus == RsvDisplayStatus.Issued)
                {
                    if (rsvData.Type == ProductType.Flight)
                    {
                        var eticketData = FlightService.GetInstance().GetEticket(rsvNo);
                        return File(eticketData, "application/pdf");
                    }
                    else
                    {
                        var eticketData = HotelService.GetInstance().GetEticket(rsvNo);
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

        public string GenerateId(string key)
        {
            string result = "";
            if (key.Length > 7)
            {
                key = key.Substring(key.Length - 7);
            }
            int generatedNumber = (int)double.Parse(key);
            for (int i = 1; i < 4; i++)
            {
                generatedNumber = new Random(generatedNumber).Next();
                result = result + "" + generatedNumber;
            }
            return result;
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
                    return "bersama-prima-alto.jpg";
                default:
                    return null;
            }
        }

        #endregion
    }
}