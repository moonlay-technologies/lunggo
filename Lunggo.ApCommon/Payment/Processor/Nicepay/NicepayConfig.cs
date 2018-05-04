using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Lunggo.Framework.Environment;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        internal class NicepayConfig
        {
            internal const string NICEPAY_CALLBACK_URL = ""; // Sett by merchant

            internal static string NICEPAY_DBPROCESS_URL =
                EnvVariables.Get("api", "apiUrl") + "/v1/payment/nicepay/paymentnotification"; // Sett by merchant

            internal const sbyte NICEPAY_TIMEOUT_CONNECT = 15;

            internal const sbyte NICEPAY_TIMEOUT_READ = 25;
            internal const string NICEPAY_PROGRAM = "NicepayLite";
            internal const string NICEPAY_VERSION = "1.0";

            internal const string NICEPAY_BUILDDATE = "20160805";

            internal const string NICEPAY_REQ_CC_URL = "/nicepay/api/orderRegist.do"; // Credit Card API URL
            internal const string NICEPAY_REQ_VA_URL = "/nicepay/api/onePass.do"; // Request Virtual Account API URL

            internal const string
                NICEPAY_CANCEL_VA_URL = "/nicepay/api/onePassAllCancel.do"; // Cancel Virtual Account API URL

            internal const string NICEPAY_ORDER_STATUS_UR = "/nicepay/api/onePassStatus.do"; // Check payment status URL

            internal const string
                NICEPAY_FIX_REG_CUSTOMER_ID =
                    "/nicepay/api/vacctCustomerRegist.do"; // Register customer ID (Fix Account)

            internal const string
                NICEPAY_FIX_RETRIEVE_VA_INFO = "/nicepay/api/vacctCustomerInquiry.do"; // List customer ID (Fix Account)

            internal const string
                NICEPAY_FIX_LIST_DEPOSIT_CUSTOMERID =
                    "/nicepay/api/vacctCustomerIdInquiry.do"; // List Status VA by Customer ID (Fix Account)

            internal const string
                NICEPAY_FIX_LIST_DEPOSIT_VA = "/nicepay/api/vacctInquiry.do"; // List Status VA (Fix Account)

            internal const string NICEPAY_ORDER_STATUS_URL = "/nicepay/api/onePassStatus.do";

            internal const string NICEPAY_READ_TIMEOUT_ERR = "10200";
            internal const sbyte NICEPAY_LOG_CRITICAL = 1;
            internal const sbyte NICEPAY_LOG_ERROR = 2;
            internal const sbyte NICEPAY_LOG_NOTICE = 3;
            internal const sbyte NICEPAY_LOG_INFO = 5;

            internal const sbyte NICEPAY_LOG_DEBUG = 7;


            internal static string GetDBProcessUrl()
            {
                return NICEPAY_DBPROCESS_URL;
            }

        }
    }
}