using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment.Service;

namespace Lunggo.CustomerWeb.Areas.Payment_v2.Controllers
{
    public class PaymentController : Controller
    {
        public ActionResult Payment(string cartId)
        {
            var cart = PaymentService.GetInstance().GetCart(cartId);
            // if cartID invalid, return error
            // if cart empty, return empty cart
            // if other error, return error
            return View();
        }
    }
}