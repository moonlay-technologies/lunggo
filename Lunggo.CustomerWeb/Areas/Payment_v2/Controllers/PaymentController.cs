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

        public PaymentController() : this(null)
        {
        }

        public PaymentController(PaymentService paymentService = null)
        {
            _paymentService = paymentService ?? new PaymentService();
        }

        public ActionResult Payment(string cartId)
        {
            var cartPayment = _paymentService.GetCartPaymentDetails(cartId);

            if (cartPayment == null)
                return View("Error");

            if (cartPayment.Status == PaymentStatus.Expired)
                return View("Expired");
            if (cartPayment.HasThirdPartyPage)
                return RedirectToAction("ThirdParty", new { cartId });
            if (cartPayment.Status != PaymentStatus.Undefined && cartPayment.Status != PaymentStatus.Failed)
            {
                if (cartPayment.HasThirdPartyPage)
                    return RedirectToAction(cartPayment.RedirectionUrl);
                if (cartPayment.HasInstruction)
                    return RedirectToAction("Instruction", new { cartId });
                if (cartPayment.Status == PaymentStatus.Settled)
                    return RedirectToAction("ThankYou", new { cartId });
            }
            if (cartPayment.RsvPaymentDetails == null || !cartPayment.RsvPaymentDetails.Any())
                return View("EmptyCart");

            return View("Payment", cartPayment);
        }
    }
}