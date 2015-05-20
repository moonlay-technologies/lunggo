using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.CustomerWeb.Models;

namespace Lunggo.CustomerWeb.Controllers
{
    public class VeritransController : Controller
    {
        public ActionResult PaymentFinish(VeritransResponse response)
        {
            if (response.status_code == "200" && response.transaction_status == "capture")
                return RedirectToAction("Thankyou", "Flight", new FlightThankyouData {
                    RsvNo = response.order_id,
                    Status = PaymentStatus.Accepted
                });
            else if (response.status_code == "201" && response.transaction_status == "capture")
                return RedirectToAction("Thankyou", "Flight", new FlightThankyouData {
                    RsvNo = response.order_id,
                    Status = PaymentStatus.BeingAuthorized
                });
            else if (response.status_code == "201" && response.transaction_status == "pending")
                return RedirectToAction("Thankyou", "Flight", new FlightThankyouData {
                    RsvNo = response.order_id,
                    Status = PaymentStatus.Pending
                    });
            else if (response.status_code == "202" && response.transaction_status == "deny")
                return RedirectToAction("Thankyou", "Flight", new FlightThankyouData {
                    RsvNo = response.order_id,
                    Status = PaymentStatus.Denied
                    });
            else
                return RedirectToAction("Thankyou", "Flight", new FlightThankyouData {
                    RsvNo = response.order_id,
                    Status = PaymentStatus.Error
                    });
        }

        public ActionResult PaymentUnfinish(VeritransResponse response)
        {
            return RedirectToAction("Thankyou", "Flight", new FlightThankyouData {
                    RsvNo = response.order_id,
                    Status = PaymentStatus.Cancelled
                    });
        }

        public ActionResult PaymentError(VeritransResponse response)
        {
            return RedirectToAction("Thankyou", "Flight", new FlightThankyouData {
                    RsvNo = response.order_id,
                    Status = PaymentStatus.Error
                    });
        }
    }

    public class VeritransResponse
    {
        public string merchant_id { get; set; }
        public string order_id { get; set; }
        public string status_code { get; set; }
        public string transaction_status { get; set; }
    }
}