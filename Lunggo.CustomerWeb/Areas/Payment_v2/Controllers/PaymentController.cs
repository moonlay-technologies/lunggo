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
        public ActionResult Payment(string cartId)
        {
            var cartPayment = PaymentService.GetInstance().GetCartPaymentDetails(cartId);
            if (cartPayment == null)
                return View("invalid ID");
            if (!cartPayment.RsvPaymentDetails.Any())
                return View("Cart Empty");

            if (cartPayment.Status == PaymentStatus.Expired)
                return View("Expired");
            if (cartPayment.RedirectionUrl != null)
                return RedirectToAction("3rd party");
            if (cartPayment.Status != PaymentStatus.Undefined && cartPayment.Status != PaymentStatus.Failed)
                //alreadt paid
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