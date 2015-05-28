using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.CustomerWeb.Models;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRepository;

namespace Lunggo.CustomerWeb.Controllers
{
    public class VeritransController : Controller
    {
        public ActionResult PaymentNotification(VeritransNotification notif)
        {
            if (notif.status_code == "200")
            {
                var paymentMethod = MapPaymentMethod(notif);
                var paymentStatus = MapPaymentStatus(notif);

                if (notif.order_id.First() == 'F')
                {
                    return RedirectToAction("PaymentConfirmation", "FlightController");

                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public ActionResult PaymentFinish(VeritransResponse response)
        {
            return RedirectToAction("Thankyou", "Flight", new FlightThankyouData
            {
                RsvNo = response.order_id,
                Status = PaymentStatus.Cancelled
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
                    Status = PaymentStatus.Cancelled
                    });
        }

        private PaymentMethod MapPaymentMethod(VeritransNotification notif)
        {
            // TODO flight add this
            switch (notif.payment_type.ToLower())
            {
                case "credit_card":
                    return PaymentMethod.CreditCard;
                case "bank_transfer":
                    return PaymentMethod.BankTransfer;
                case "mandiri_clickpay":
                    return PaymentMethod.MandiriClickPay;
                case "cimb_clicks":
                    return PaymentMethod.CimbClicks;
                default:
                    return PaymentMethod.Undefined;
            }
        }

        private PaymentStatus MapPaymentStatus(VeritransNotification notif)
        {
            // TODO flight fix this
            switch (notif.fraud_status.ToLower())
            {
                case "capture":
                    switch (notif.transaction_status.ToLower())
                    {
                        case "settlement":
                        case "pending":
                            return PaymentStatus.Pending;
                        case "cancel":
                        case "expire":
                            return PaymentStatus.Cancelled;
                        case "deny":
                            return PaymentStatus.Denied;
                        case "authorize":
                        case "capture":
                        default:
                            return PaymentStatus.Undefined;
                    }
                case "challenge":
                case "deny":
                    return PaymentStatus.Denied;
                default:
                    return PaymentStatus.Undefined;
            }
        }
    }

    public class VeritransNotification
    {
        public string status_code { get; set; }
        public string order_id { get; set; }
        public string payment_type { get; set; }
        public string transaction_time { get; set; }
        public string transaction_status { get; set; }
        public string fraud_status { get; set; }
        public string bank { get; set; }
        public string permata_va_number { get; set; }
    }

    public class VeritransResponse
    {
        public string merchant_id { get; set; }
        public string order_id { get; set; }
        public string status_code { get; set; }
        public string transaction_status { get; set; }
    }
}