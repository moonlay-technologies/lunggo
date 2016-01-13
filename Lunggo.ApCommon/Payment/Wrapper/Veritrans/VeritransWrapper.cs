﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Payment.Wrapper.Veritrans.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Context;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Wrapper.Veritrans
{
    internal class VeritransWrapper
    {
        private static readonly VeritransWrapper Instance = new VeritransWrapper();
        private bool _isInitialized;

        private static string _endPoint;
        private static string _serverKey;
        private static string _rootUrl;
        private static string _finishedRedirectUrl;
        private static string _unfinishedRedirectUrl;
        private static string _errorRedirectUrl;

        private const string FinishRedirectPath = @"/Veritrans/PaymentFinish";
        private const string UnfinishRedirectPath = @"/Veritrans/PaymentUnfinish";
        private const string ErrorRedirectPath = @"/Veritrans/PaymentError";

        private VeritransWrapper()
        {

        }

        internal static VeritransWrapper GetInstance()
        {
            return Instance;
        }

        internal void Init()
        {
            if (!_isInitialized)
            {
                _endPoint = ConfigManager.GetInstance().GetConfigValue("veritrans", "endPoint");
                _serverKey = ConfigManager.GetInstance().GetConfigValue("veritrans", "serverKey") + ":";
                _rootUrl = ConfigManager.GetInstance().GetConfigValue("general", "rootUrl");
                _isInitialized = true;
            }
        }

        internal PaymentStatus ProcessPayment(Data data, TransactionDetails transactionDetail, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            switch (method)
            {
                case PaymentMethod.CreditCard:
                    var authorizationKey = ProcessAuthorizationKey(_serverKey);
                    var request = CreateVtDirectRequest(authorizationKey, data, transactionDetail, itemDetails, method);
                    var response = SubmitRequest(request);
                    var content = GetResponseContent(response);
                    if (content != null)
                    {
                        return PaymentResult(content);
                    }
                    else
                        return PaymentStatus.Denied;
                default:
                    return PaymentStatus.Denied;
            }
        }

        internal string GetPaymentUrl(TransactionDetails transactionDetail, List<ItemDetails> itemDetails, PaymentMethod method)
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

        private static WebRequest CreateVtDirectRequest(string authorizationKey, Data data, TransactionDetails transactionDetail, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var request = (HttpWebRequest)WebRequest.Create(_endPoint);
            request.Method = "POST";
            request.Headers.Add("Authorization", "Basic " + authorizationKey);
            request.ContentType = "application/json";
            request.Accept = "application/json";
            ProcessVtDirectRequestParams(request, data, transactionDetail, itemDetails, method);
            return request;
        }

        private static WebRequest CreateVtWebRequest(string authorizationKey, TransactionDetails transactionDetail, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var request = (HttpWebRequest)WebRequest.Create(_endPoint);
            request.Method = "POST";
            request.Headers.Add("Authorization", "Basic " + authorizationKey);
            request.ContentType = "application/json";
            request.Accept = "application/json";
            ProcessVtWebRequestParams(request, transactionDetail, itemDetails, method);
            return request;
        }

        private static void ProcessVtDirectRequestParams(WebRequest request, Data data, TransactionDetails transactionDetail, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "paymentTimeout"));
            var requestParams = new VeritransRequest
            {
                PaymentType = MapPaymentMethod(method),
                TransactionDetail = transactionDetail,
                ItemDetail = itemDetails,
                PaymentExpiry = new PaymentExpiry
                {
                    OrderTime = transactionDetail.OrderTime.ToString("yyyy-MM-dd HH:mm:ss Z"),
                    Duration = timeout,
                    Unit = "minute"
                }
            };
            if (method == PaymentMethod.CreditCard)
            {
                requestParams.CreditCard = new CreditCard
                {
                    TokenId = data.Data0,
                    Bank = "mandiri"
                };
            }
            var jsonRequestParams = JsonConvert.SerializeObject(requestParams);
            var dataStream = request.GetRequestStream();
            using (var streamWriter = new StreamWriter(dataStream))
            {
                streamWriter.Write(jsonRequestParams);
            }
        }

        private static void ProcessVtWebRequestParams(WebRequest request, TransactionDetails transactionDetail, List<ItemDetails> itemDetails, PaymentMethod method)
        {
            var langCode = @"/" + OnlineContext.GetActiveLanguageCode();
            _finishedRedirectUrl = _rootUrl + langCode + FinishRedirectPath;
            _unfinishedRedirectUrl = _rootUrl + langCode + UnfinishRedirectPath;
            _errorRedirectUrl = _rootUrl + langCode + ErrorRedirectPath;

            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "paymentTimeout"));
            var requestParams = new VeritransRequest
            {
                PaymentType = "vtweb",
                TransactionDetail = transactionDetail,
                ItemDetail = itemDetails,
                PaymentExpiry = new PaymentExpiry
                {
                    OrderTime = transactionDetail.OrderTime.ToString("yyyy-MM-dd HH:mm:ss Z"),
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
                                return PaymentStatus.Verifying;
                            case "challenge":
                            case "deny":
                                return PaymentStatus.Denied;
                            default:
                                return PaymentStatus.Verifying;
                        }
                    case "deny":
                        return PaymentStatus.Denied;
                    case "authorize":
                        return PaymentStatus.Pending;
                    default:
                        return PaymentStatus.Undefined;
                }
            }
            else
                return PaymentStatus.Denied;
        }
    }
}
