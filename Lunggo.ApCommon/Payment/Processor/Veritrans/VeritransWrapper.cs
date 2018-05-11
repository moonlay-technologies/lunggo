using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Context;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json.Schema;
using Lunggo.ApCommon.Log;
using Lunggo.Framework.Environment;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        internal class VeritransWrapper
        {

            private static string _chargeEndPoint;
            private static string _serverKey;
            private static string _rootUrl;
            private static string _cancelEndPoint;
            private static string _approveEndPoint;
            private static string _finishedRedirectUrl;
            private static string _unfinishedRedirectUrl;
            private static string _errorRedirectUrl;
            private static string _temp = "";

            private const string FinishRedirectPath = @"/id/Veritrans/PaymentFinish";
            private const string UnfinishRedirectPath = @"/id/Veritrans/PaymentUnfinish";
            private const string ErrorRedirectPath = @"/id/Veritrans/PaymentError";

            internal VeritransWrapper()
            {
                _chargeEndPoint = EnvVariables.Get("veritrans", "chargeEndPoint");
                _serverKey = EnvVariables.Get("veritrans", "serverKey") + ":";
                _cancelEndPoint = EnvVariables.Get("veritrans", "cancelEndPoint");
                _approveEndPoint = EnvVariables.Get("veritrans", "approveEndPoint");
                _rootUrl = EnvVariables.Get("general", "rootUrl");
            }

            internal PaymentDetails ProcessPayment(PaymentDetails payment, TransactionDetails transactionDetail)
            {
                var TableLog = new GlobalLog();

                TableLog.PartitionKey = "PAYMENT LOG";


                var authorizationKey = ProcessAuthorizationKey(_serverKey);
                WebRequest request;
                WebResponse response;
                VeritransResponse content;
                switch (payment.Method)
                {
                    case PaymentMethod.CreditCard:
                        payment.Data.CreditCard.Bank = "mandiri";
                        request = CreateVtDirectRequest(authorizationKey, payment.Data, transactionDetail,
                            payment.Method);
                        response = SubmitRequest(request);
                        content = GetResponseContent(response);
                        if (content != null && content.StatusCode.StartsWith("2"))
                        {
                            payment.Status = PaymentResult(content);
                            if (payment.Status == PaymentStatus.Challenged)
                            {
                                CancelTransaction(content.OrderId, content.TransactionId);
                                payment.Status = PaymentStatus.Denied;

                            }

                            payment.ExternalId = content.TransactionId;
                        }
                        else
                        {
                            payment.Status = PaymentStatus.Failed;
                            payment.FailureReason = FailureReason.PaymentFailure;

                            var log = LogService.GetInstance();
                            var env = EnvVariables.Get("general", "environment");
                            TableLog.Log = "```Payment Log```"
                                           + "\n`*Environment :* " + env.ToUpper()
                                           + "\n*PAYMENT DETAILS :*\n"
                                           + payment.Serialize()
                                           + "\n*TRANSAC DETAILS :*\n"
                                           + transactionDetail.Serialize()
                                           //+ "\n*ITEM DETAILS :*\n"
                                           //+ itemDetails.Serialize()
                                           + "\n*REQUEST :*\n"
                                           + _temp
                                           + "\n*RESPONSE :*\n"
                                           + content.Serialize()
                                           + "\n*Platform :* "
                                           + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId());
                            log.Post(TableLog.Log
                                ,
                                env == "production" ? "#logging-prod" : "#logging-dev");
                            TableLog.Logging();
                        }

                        return payment;
                    case PaymentMethod.VirtualAccount:
                        request = CreateVtDirectRequest(authorizationKey, payment.Data, transactionDetail,
                            payment.Method);
                        response = SubmitRequest(request);
                        content = GetResponseContent(response);
                        if (content != null && content.StatusCode.StartsWith("2"))
                        {
                            payment.TransferAccount = content.PermataVANumber;
                            payment.Status = PaymentResult(content);
                            payment.ExternalId = content.TransactionId;
                        }
                        else
                        {
                            payment.Status = PaymentStatus.Failed;
                            payment.FailureReason = FailureReason.PaymentFailure;

                            var log = LogService.GetInstance();
                            var env = EnvVariables.Get("general", "environment");
                            TableLog.Log = "```Payment Log```"
                                           + "\n`*Environment :* " + env.ToUpper()
                                           + "\n*PAYMENT DETAILS :*\n"
                                           + payment.Serialize()
                                           + "\n*TRANSAC DETAILS :*\n"
                                           + transactionDetail.Serialize()
                                           //+ "\n*ITEM DETAILS :*\n"
                                           //+ itemDetails.Serialize()
                                           + "\n*REQUEST :*\n"
                                           + _temp
                                           + "\n*RESPONSE :*\n"
                                           + content.Serialize()
                                           + "\n*Platform :* "
                                           + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId());
                            log.Post(TableLog.Log
                                ,
                                env == "production" ? "#logging-prod" : "#logging-dev");
                            TableLog.Logging();
                        }

                        return payment;

                    case PaymentMethod.MandiriClickPay:
                        request = CreateVtDirectRequest(authorizationKey, payment.Data, transactionDetail,
                            payment.Method);
                        response = SubmitRequest(request);
                        content = GetResponseContent(response);
                        if (content != null && content.StatusCode.StartsWith("2"))
                        {
                            payment.Status = PaymentResult(content);
                            payment.ExternalId = content.TransactionId;
                        }
                        else
                        {
                            payment.Status = PaymentStatus.Failed;
                            payment.FailureReason = FailureReason.PaymentFailure;

                            var log = LogService.GetInstance();
                            var env = EnvVariables.Get("general", "environment");
                            TableLog.Log = "```Payment Log```"
                                           + "\n`*Environment :* " + env.ToUpper()
                                           + "\n*PAYMENT DETAILS :*\n"
                                           + payment.Serialize()
                                           + "\n*TRANSAC DETAILS :*\n"
                                           + transactionDetail.Serialize()
                                           //+ "\n*ITEM DETAILS :*\n"
                                           //+ itemDetails.Serialize()
                                           + "\n*REQUEST :*\n"
                                           + _temp
                                           + "\n*RESPONSE :*\n"
                                           + content.Serialize()
                                           + "\n*Platform :* "
                                           + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId());
                            log.Post(TableLog.Log
                                ,
                                env == "production" ? "#logging-prod" : "#logging-dev");
                            TableLog.Logging();
                        }

                        return payment;

                    case PaymentMethod.CimbClicks:
                        request = CreateVtDirectRequest(authorizationKey, payment.Data, transactionDetail,
                            payment.Method);
                        response = SubmitRequest(request);
                        content = GetResponseContent(response);
                        if (content != null && content.StatusCode.StartsWith("2"))
                        {
                            payment.RedirectionUrl = content.RedirectUrl;
                            payment.ExternalId = content.TransactionId;
                            payment.Status = PaymentResult(content);
                        }
                        else
                        {
                            payment.Status = PaymentStatus.Failed;
                            payment.FailureReason = FailureReason.PaymentFailure;

                            var log = LogService.GetInstance();
                            var env = EnvVariables.Get("general", "environment");
                            TableLog.Log = "```Payment Log```"
                                           + "\n`*Environment :* " + env.ToUpper()
                                           + "\n*PAYMENT DETAILS :*\n"
                                           + payment.Serialize()
                                           + "\n*TRANSAC DETAILS :*\n"
                                           + transactionDetail.Serialize()
                                           //+ "\n*ITEM DETAILS :*\n"
                                           //+ itemDetails.Serialize()
                                           + "\n*REQUEST :*\n"
                                           + _temp
                                           + "\n*RESPONSE :*\n"
                                           + content.Serialize()
                                           + "\n*Platform :* "
                                           + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId());
                            log.Post(TableLog.Log
                                ,
                                env == "production" ? "#logging-prod" : "#logging-dev");
                            TableLog.Logging();
                        }

                        return payment;

                    case PaymentMethod.MandiriBillPayment:
                        request = CreateVtDirectRequest(authorizationKey, payment.Data, transactionDetail,
                            payment.Method);
                        response = SubmitRequest(request);
                        content = GetResponseContent(response);
                        if (content != null && content.StatusCode.StartsWith("2"))
                        {
                            payment.RedirectionUrl = content.RedirectUrl;
                            payment.ExternalId = content.TransactionId;
                            payment.Status = PaymentResult(content);
                        }
                        else
                        {
                            payment.Status = PaymentStatus.Failed;
                            payment.FailureReason = FailureReason.PaymentFailure;

                            var log = LogService.GetInstance();
                            var env = EnvVariables.Get("general", "environment");
                            TableLog.Log = "```Payment Log```"
                                           + "\n`*Environment :* " + env.ToUpper()
                                           + "\n*PAYMENT DETAILS :*\n"
                                           + payment.Serialize()
                                           + "\n*TRANSAC DETAILS :*\n"
                                           + transactionDetail.Serialize()
                                           //+ "\n*ITEM DETAILS :*\n"
                                           //+ itemDetails.Serialize()
                                           + "\n*REQUEST :*\n"
                                           + _temp
                                           + "\n*RESPONSE :*\n"
                                           + content.Serialize()
                                           + "\n*Platform :* "
                                           + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId());
                            log.Post(TableLog.Log,
                                env == "production" ? "#logging-prod" : "#logging-dev");
                            TableLog.Logging();
                        }

                        return payment;

                    default:
                        payment.Status = PaymentStatus.Failed;
                        payment.FailureReason = FailureReason.PaymentFailure;
                        return payment;

                }
            }


            internal VeritransResponse ApproveTransaction(string orderId, string transactionId)
            {
                var TableLog = new GlobalLog();

                TableLog.PartitionKey = "PAYMENT LOG";

                var authorizationKey = ProcessAuthorizationKey(_serverKey);
                WebRequest request;
                WebResponse response;
                VeritransResponse content;
                _approveEndPoint = _approveEndPoint.Replace("order_id", orderId);
                request = CreateCancelTransactionRequest(authorizationKey, _approveEndPoint);
                response = SubmitRequest(request);
                content = GetResponseContent(response);
                if (content != null && content.StatusCode.StartsWith("2"))
                {
                    return content;
                }
                else
                {
                    var log = LogService.GetInstance();
                    var env = EnvVariables.Get("general", "environment");
                    TableLog.Log = "CANCEL TRANSACTION" +
                                   "```Payment Log```"
                                   + "\n`*Environment :* " + env.ToUpper()
                                   + "\n*ORDER ID  :*\n"
                                   + orderId
                                   + "\n*TRANSAC ID :*\n"
                                   + transactionId
                                   //+ "\n*ITEM DETAILS :*\n"
                                   //+ itemDetails.Serialize()
                                   + "\n*REQUEST :*\n"
                                   + _temp
                                   + "\n*RESPONSE :*\n"
                                   + content.Serialize()
                                   + "\n*Platform :* "
                                   + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId());
                    log.Post(TableLog.Log,
                        env == "production" ? "#logging-prod" : "#logging-dev");
                    TableLog.Logging();
                    return null;
                }
            }

            internal VeritransResponse CancelTransaction(string orderId, string transactionId)
            {
                var TableLog = new GlobalLog();

                TableLog.PartitionKey = "PAYMENT LOG";

                var authorizationKey = ProcessAuthorizationKey(_serverKey);
                WebRequest request;
                WebResponse response;
                VeritransResponse content;
                _cancelEndPoint = _cancelEndPoint.Replace("order_id", orderId);
                request = CreateCancelTransactionRequest(authorizationKey, _cancelEndPoint);
                response = SubmitRequest(request);
                content = GetResponseContent(response);
                if (content != null && content.StatusCode.StartsWith("2"))
                {
                    return content;
                }
                else
                {
                    var log = LogService.GetInstance();
                    var env = EnvVariables.Get("general", "environment");
                    TableLog.Log = "CANCEL TRANSACTION" +
                                   "```Payment Log```"
                                   + "\n`*Environment :* " + env.ToUpper()
                                   + "\n*ORDER ID  :*\n"
                                   + orderId
                                   + "\n*TRANSAC ID :*\n"
                                   + transactionId
                                   //+ "\n*ITEM DETAILS :*\n"
                                   //+ itemDetails.Serialize()
                                   + "\n*REQUEST :*\n"
                                   + _temp
                                   + "\n*RESPONSE :*\n"
                                   + content.Serialize()
                                   + "\n*Platform :* "
                                   + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId());
                    log.Post(TableLog.Log,
                        env == "production" ? "#logging-prod" : "#logging-dev");
                    TableLog.Logging();
                    return null;
                }
            }

            internal string GetPaymentUrl(TransactionDetails transactionDetail, List<ItemDetails> itemDetails,
                PaymentMethod method)
            {
                var authorizationKey = ProcessAuthorizationKey(_serverKey);
                var request = CreateVtWebRequest(authorizationKey, transactionDetail, itemDetails, method);
                var response = SubmitRequest(request);
                var url = GetUrlFromResponse(response);
                return url;
            }

            private static string ProcessAuthorizationKey(string serverKey)
            {
                var plainAuthorizationKey = Encoding.UTF8.GetBytes(serverKey);
                var hashedAuthorizationKey = Convert.ToBase64String(plainAuthorizationKey);
                return hashedAuthorizationKey;
            }

            private static WebRequest CreateVtDirectRequest(string authorizationKey, PaymentData data,
                TransactionDetails transactionDetail, PaymentMethod method)
            {
                var request = (HttpWebRequest)WebRequest.Create(_chargeEndPoint);
                request.Method = "POST";
                request.Headers.Add("Authorization", "Basic " + authorizationKey);
                request.ContentType = "application/json";
                request.Accept = "application/json";

                ProcessVtDirectRequestParams(request, data, transactionDetail, method);
                return request;
            }

            private static WebRequest CreateCancelTransactionRequest(string authorizationKey, string endPoint)
            {
                var request = (HttpWebRequest)WebRequest.Create(endPoint);
                request.Method = "POST";
                request.Headers.Add("Authorization", "Basic " + authorizationKey);
                request.ContentType = "application/json";
                request.Accept = "application/json";
                return request;
            }

            private static WebRequest CreateVtWebRequest(string authorizationKey, TransactionDetails transactionDetail,
                List<ItemDetails> itemDetails, PaymentMethod method)
            {
                var request = (HttpWebRequest)WebRequest.Create(_chargeEndPoint);
                request.Method = "POST";
                request.Headers.Add("Authorization", "Basic " + authorizationKey);
                request.ContentType = "application/json";
                request.Accept = "application/json";
                ProcessVtWebRequestParams(request, transactionDetail, itemDetails, method);
                return request;
            }

            private static void ProcessVtDirectRequestParams(WebRequest request, PaymentData data,
                TransactionDetails transactionDetail, PaymentMethod method)
            {
                data = data ?? new PaymentData();
                var timeout = int.Parse(EnvVariables.Get("flight", "paymentTimeout"));
                var requestParams = new VeritransRequest
                {
                    PaymentType = MapPaymentMethod(method),
                    TransactionDetail = transactionDetail,
                    PaymentExpiry = new PaymentExpiry
                    {
                        OrderTime = transactionDetail.OrderTime.ToString("yyyy-MM-dd HH:mm:ss' +0000'",
                            CultureInfo.InvariantCulture),
                        Duration = timeout,
                        Unit = "minute"
                    }

                };
                if (method == PaymentMethod.CreditCard)
                {
                    var contact = transactionDetail.Contact;

                    requestParams.CreditCard = new CreditCard
                    {
                        TokenId = data.CreditCard.TokenId,
                        Bank = "mandiri",
                        AllowedBins = new List<string> { data.CreditCard.TokenId.Substring(0, 6) },
                        TokenIdSaveEnabled = data.CreditCard.TokenIdSaveEnabled
                    };
                    requestParams.CustomerDetail = new CustomerDetails
                    {
                        FirstName = contact.Name,
                        Phone = contact.CountryCallingCode + contact.Phone,
                        Email = contact.Email
                    };
                }

                else if (method == PaymentMethod.VirtualAccount)
                {
                    requestParams.BankTransfer = new BankTransfer
                    {
                        Bank = "permata"
                    };
                }
                //Mandiri CLick Pay
                else if (method == PaymentMethod.MandiriClickPay)
                {
                    requestParams.MandiriClickPay = new MandiriClickPay
                    {
                        CardNumber = data.MandiriClickPay.CardNumber,
                        CardNumberLast10 = data.MandiriClickPay.CardNumberLast10,
                        GivenRandomNumber = data.MandiriClickPay.GivenRandomNumber,
                        Amount = (long)data.MandiriClickPay.Amount,
                        Token = data.MandiriClickPay.Token
                    };
                }

                //CimbClicks
                else if (method == PaymentMethod.CimbClicks)
                {
                    requestParams.CimbClicks = new CimbClicks
                    {
                        Description = data.CimbClicks.Description,
                        FinishRedirectUrl = FinishRedirectPath,
                        UnfinishRedirectUrl = UnfinishRedirectPath,
                        ErrorRedirectUrl = ErrorRedirectPath
                    };

                }

                //Mandiri Bill Payment
                else if (method == PaymentMethod.MandiriBillPayment)
                {
                    requestParams.MandiriBillPayment = new MandiriBillPayment
                    {
                        Label1 = data.MandiriBillPayment.Label1,
                        Value1 = data.MandiriBillPayment.Value1
                    };
                }

                var jsonRequestParams = JsonConvert.SerializeObject(requestParams);
                var dataStream = request.GetRequestStream();
                using (var streamWriter = new StreamWriter(dataStream))
                {
                    streamWriter.Write(jsonRequestParams);
                }

                _temp = jsonRequestParams;
            }

            private static void ProcessVtWebRequestParams(WebRequest request, TransactionDetails transactionDetail,
                List<ItemDetails> itemDetails, PaymentMethod method)
            {
                var langCode = @"/" + OnlineContext.GetActiveLanguageCode();
                _finishedRedirectUrl = _rootUrl + langCode + FinishRedirectPath;
                _unfinishedRedirectUrl = _rootUrl + langCode + UnfinishRedirectPath;
                _errorRedirectUrl = _rootUrl + langCode + ErrorRedirectPath;

                var timeout = int.Parse(EnvVariables.Get("flight", "paymentTimeout"));
                var requestParams = new VeritransRequest
                {
                    PaymentType = "vtweb",
                    TransactionDetail = transactionDetail,
                    ItemDetail = itemDetails,
                    PaymentExpiry = new PaymentExpiry
                    {
                        OrderTime = transactionDetail.OrderTime.ToString("yyyy-MM-dd HH:mm:ss' +0000'",
                            CultureInfo.InvariantCulture),
                        Duration = timeout,
                        Unit = "minute"
                    },
                    VtWeb = new VtWeb
                    {
                        EnabledPayments = new List<string> { MapPaymentMethod(method) },
                        CreditCard3DSecure = true,
                        FinishRedirectUrl = _finishedRedirectUrl,
                        UnfinishRedirectUrl = _unfinishedRedirectUrl,
                        ErrorRedirectUrl = _errorRedirectUrl
                    }
                };
                var jsonRequestParams = JsonConvert.SerializeObject(requestParams);
                var dataStream = request.GetRequestStream();
                using (var streamWriter = new StreamWriter(dataStream))
                {
                    streamWriter.Write(jsonRequestParams);
                }
            }

            private static string MapPaymentMethod(PaymentMethod method)
            {
                switch (method)
                {
                    case PaymentMethod.CreditCard:
                        return "credit_card";
                    case PaymentMethod.MandiriClickPay:
                        return "mandiri_clickpay";
                    case PaymentMethod.CimbClicks:
                        return "cimb_clicks";
                    case PaymentMethod.VirtualAccount:
                        return "bank_transfer";
                    case PaymentMethod.MandiriBillPayment:
                        return "echannel";
                    case PaymentMethod.EpayBri:
                        return "bri_epay";
                    case PaymentMethod.BcaKlikpay:
                        return "bca_klikpay";
                    case PaymentMethod.TelkomselTcash:
                        return "telkomsel_cash";
                    case PaymentMethod.XlTunai:
                        return "xl_tunai";
                    case PaymentMethod.BbmMoney:
                        return "bbm_money";
                    case PaymentMethod.IndosatDompetku:
                        return "indosat_dompetku";
                    case PaymentMethod.MandiriEcash:
                        return "mandiri_ecash";
                    case PaymentMethod.Indomaret:
                        return "cstore";
                    default:
                        return null;
                }
            }

            private static WebResponse SubmitRequest(WebRequest request)
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var response = (HttpWebResponse)request.GetResponse();
                return response;
            }

            private static VeritransResponse GetResponseContent(WebResponse response)
            {
                var streamContent = response.GetResponseStream();
                if (streamContent != null)
                {
                    string jsonContent;
                    using (var reader = new StreamReader(streamContent))
                    {
                        jsonContent = reader.ReadToEnd();
                    }

                    var content = JsonConvert.DeserializeObject<VeritransResponse>(jsonContent);
                    return content;
                }
                else
                {
                    return null;
                }
            }

            private static string GetUrlFromResponse(WebResponse response)
            {
                var content = GetResponseContent(response);
                if (content != null)
                    return content.StatusCode.StartsWith("2") ? content.RedirectUrl : null;
                else
                    return null;
            }

            private static PaymentStatus PaymentResult(VeritransResponse response)
            {
                if (response.StatusCode.StartsWith("2"))
                {
                    switch (response.TransactionStatus.ToLower())
                    {
                        case "capture":
                            switch (response.FraudStatus.ToLower())
                            {
                                case "accept":
                                    return PaymentStatus.Settled;
                                case "challenge":
                                    return PaymentStatus.Challenged;
                                case "deny":
                                    return PaymentStatus.Denied;
                                default:
                                    return PaymentStatus.Failed;
                            }
                        case "settlement":
                            return PaymentStatus.Settled;
                        case "expire":
                            return PaymentStatus.Expired;
                        case "deny":
                        case "authorize":
                            return PaymentStatus.Denied;
                        case "pending":
                            return PaymentStatus.Pending;
                        default:
                            return PaymentStatus.Failed;
                    }
                }
                else
                    return PaymentStatus.Failed;
            }
        }
    }
}