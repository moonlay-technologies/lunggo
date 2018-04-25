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
            // if already paid, goto respective page (3rd party, instruction, thank you)

            return View(cartPayment);
        }
    }
}