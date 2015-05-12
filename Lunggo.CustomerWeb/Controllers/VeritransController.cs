using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.CustomerWeb.Controllers
{
    public class VeritransController : Controller
    {
        public ActionResult PaymentFinish(VeritransResponse response)
        {
            if (response.StatusCode == "200" && response.TransactionStatus == "capture")
                return RedirectToAction("Thankyou", "Flight", PaymentStatus.Accepted);
            else if (response.StatusCode == "201" && response.TransactionStatus == "capture")
                return RedirectToAction("Thankyou", "Flight", PaymentStatus.BeingAuthorized);
            else if (response.StatusCode == "201" && response.TransactionStatus == "pending")
                return RedirectToAction("Thankyou", "Flight", PaymentStatus.Pending);
            else if (response.StatusCode == "202" && response.TransactionStatus == "deny")
                return RedirectToAction("Thankyou", "Flight", PaymentStatus.Denied);
            else
                return RedirectToAction("Thankyou", "Flight", PaymentStatus.Error);
        }

        public ActionResult PaymentUnfinish(VeritransResponse response)
        {
            return RedirectToAction("Thankyou", "Flight", PaymentStatus.Cancelled);
        }

        public ActionResult PaymentError(VeritransResponse response)
        {
            return RedirectToAction("Thankyou", "Flight", PaymentStatus.Error);
        }
    }

    public class VeritransResponse
    {
        public string MerchantId { get; set; }
        public string OrderId { get; set; }
        public string StatusCode { get; set; }
        public string TransactionStatus { get; set; }
    }
}