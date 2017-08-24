using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Lunggo.Framework.Config;

public class NicepayConfig
{
    public const string NICEPAY_IMID = "IONPAYTEST"; // Merchant ID   // use VACCTCLOSE TO test cancel VA //IONPAYTEST for normal VA /// FIXOPEN001 for Fixclose register
    public const string NICEPAY_MERCHANT_KEY = "33F49GnCMS1mFYlGXisbUDzVf2ATWCl9k3R++d5hDd3Frmuos/XLx8XhXpe+LDYAbpGKZYSwtlyyLOtS/8aD7A==";
    public const string NICEPAY_CALLBACK_URL = "http://localhost/nicepay-sdk/result.html"; // Sett by merchant
    public static string NICEPAY_DBPROCESS_URL = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl") + "/v1/payment/nicepay/paymentnotification"; //"http://api.local.travorama.com/v1/payment/nicepay/paymentnotification" // Sett by merchant
    public const sbyte NICEPAY_TIMEOUT_CONNECT = 15;

    public const sbyte NICEPAY_TIMEOUT_READ = 25;
    public const string NICEPAY_PROGRAM = "NicepayLite";
    public const string NICEPAY_VERSION = "1.0";

    public const string NICEPAY_BUILDDATE = "20160805";
    

     public const string NICEPAY_REQ_CC_URL = "https://www.nicepay.co.id/nicepay/api/orderRegist.do";            // Credit Card API URL
     public const string NICEPAY_REQ_VA_URL = "https://www.nicepay.co.id/nicepay/api/onePass.do";                // Request Virtual Account API URL
     public const string NICEPAY_CANCEL_VA_URL = "https://www.nicepay.co.id/nicepay/api/onePassAllCancel.do";       // Cancel Virtual Account API URL
     public const string NICEPAY_ORDER_STATUS_UR = "https://www.nicepay.co.id/nicepay/api/onePassStatus.do";          // Check payment status URL
     public const string NICEPAY_FIX_REG_CUSTOMER_ID = "https://www.nicepay.co.id/nicepay/api/vacctCustomerRegist.do";    // Register customer ID (Fix Account)
     public const string NICEPAY_FIX_RETRIEVE_VA_INFO = "https://www.nicepay.co.id/nicepay/api/vacctCustomerInquiry.do";   // List customer ID (Fix Account)
     public const string NICEPAY_FIX_LIST_DEPOSIT_CUSTOMERID = "https://www.nicepay.co.id/nicepay/api/vacctCustomerIdInquiry.do"; // List Status VA by Customer ID (Fix Account)
     public const string NICEPAY_FIX_LIST_DEPOSIT_VA = "https://www.nicepay.co.id/nicepay/api/vacctInquiry.do"; // List Status VA (Fix Account)
     public const string NICEPAY_ORDER_STATUS_URL = "https://www.nicepay.co.id/nicepay/api/onePassStatus.do";

    public const string NICEPAY_READ_TIMEOUT_ERR = "10200";
    public const sbyte NICEPAY_LOG_CRITICAL = 1;
    public const sbyte NICEPAY_LOG_ERROR = 2;
    public const sbyte NICEPAY_LOG_NOTICE = 3;
    public const sbyte NICEPAY_LOG_INFO = 5;

    public const sbyte NICEPAY_LOG_DEBUG = 7;


    public static string  GetDBProcessUrl()
    {
        return NICEPAY_DBPROCESS_URL;
    }

}