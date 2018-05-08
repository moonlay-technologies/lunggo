using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Service;

namespace Lunggo.CustomerWeb.Areas.Payment_v2.Controllers
{
    public class PaymentController : Controller
    {
        private PaymentService _paymentService;

        public PaymentController(PaymentService paymentService = null)
        {
            _paymentService = paymentService ?? new PaymentService();
        }

        public ActionResult Payment(string cartId)
        {
            var cartPayment = _paymentService.GetCartPaymentDetails(cartId);

            if (cartPayment == null)
                return View("Error");
            if (cartPayment.RsvPaymentDetails == null || !cartPayment.RsvPaymentDetails.Any())
                return View("EmptyCart");

            switch (cartPayment.Status)
            {
                case PaymentStatus.Pending:
                    if (cartPayment.HasThirdPartyPage)
                        return RedirectToAction("ThirdParty", new { cartId });
                    if (cartPayment.HasInstruction)
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
                    return View("Payment", cartPayment);
                default:
                    return View("Error");
            }

        }

        public ActionResult Instruction(string cartId)
        {
            var cartPayment = _paymentService.GetCartPaymentDetails(cartId);

            if (cartPayment == null)
                return View("Error");
            if (!cartPayment.HasInstruction)
                return View("Error");
            return View("Instruction");
        }
    }
}