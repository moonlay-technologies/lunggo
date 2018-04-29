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
            if (!cartPayment.RsvPaymentDetails.Any())
                return View("CartEmpty");

            if (cartPayment.Status == PaymentStatus.Expired)
                return View("Expired");
            if (cartPayment.HasThirdPartyPage)
                return RedirectToAction("ThirdParty");
            if (cartPayment.Status != PaymentStatus.Undefined && cartPayment.Status != PaymentStatus.Failed)
            {
                if (cartPayment.RedirectionUrl != null)
                    return RedirectToAction(cartPayment.RedirectionUrl);
                if (cartPayment.TransferAccount != null)
                    return RedirectToAction("Instruction");
            }

            return View(cartPayment);
        }
    }
}