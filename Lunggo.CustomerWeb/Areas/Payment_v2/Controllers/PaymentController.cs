using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Service;

namespace Lunggo.CustomerWeb.Areas.Payment_v2.Controllers
{
    public class PaymentController : Controller
    {
        private PaymentService _paymentService;

        public PaymentController() : this(null)
        {
        }

        public PaymentController(PaymentService paymentService = null)
        {
            _paymentService = paymentService ?? new PaymentService();
        }

        public ActionResult Payment(string cartId, string trxId)
        {
            PaymentDetails payment;

            if (!string.IsNullOrWhiteSpace(cartId))
            {
                payment = _paymentService.GetTrxPaymentDetails(cartId);
                if (payment == null)
                    return View("Error");

                var rsvList = (payment as TrxPaymentDetails).RsvPaymentDetails
                    .Select(r => ActivityService.GetInstance().GetReservationForDisplay(r.RsvNo)).ToList();
                ViewBag.RsvList = rsvList;

            }
            else if (!string.IsNullOrWhiteSpace(trxId))
            {
                payment = _paymentService.GetTrxPaymentDetails(trxId);
                if (payment == null)
                    return View("Error");

                var rsvList = (payment as TrxPaymentDetails).RsvPaymentDetails
                    .Select(r => ActivityService.GetInstance().GetReservationForDisplay(r.RsvNo)).ToList();
                ViewBag.RsvList = rsvList;
            }
            else
            {
                return View("Error");

            }

            switch (payment.Status)
            {
                case PaymentStatus.Pending:
                    if (payment.HasThirdPartyPage)
                        return RedirectToAction("ThirdParty", new { cartId });
                    if (payment.HasInstruction)
                        return RedirectToAction("Instruction", new { cartId });
                    return View("Error");
                case PaymentStatus.Settled:
                    return RedirectToAction("ThankYou", new { cartId });
                case PaymentStatus.Verifying:
                    return RedirectToAction("Verifying", new { cartId });
                case PaymentStatus.MethodNotSet:
                case PaymentStatus.Failed:
                case PaymentStatus.Expired:
                case PaymentStatus.Denied:
                    return View("Payment", payment);
                default:
                    return View("Error");
            }

        }

        public ActionResult Instruction(string cartId)
        {
            var cartPayment = _paymentService.GetTrxPaymentDetails(cartId);

            if (cartPayment == null)
                return View("Error");
            if (!cartPayment.HasInstruction)
                return View("Error");
            return View("Instruction");
        }
    }
}