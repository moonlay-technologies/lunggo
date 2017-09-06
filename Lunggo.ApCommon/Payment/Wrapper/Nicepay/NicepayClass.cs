using System;
using System.Net;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Text;
using System.Web;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Wrapper.Nicepay;
using WebRequestor.System.Net;

internal class NicepayClass : System.Web.UI.Page
{
    internal NicepayResponse CreateVA(NicepayModel Nicepay)
    {
        string RequestType = "RequestVA";

        Nicepay.iMid = NicepayWrapper._merchantId;
        Nicepay.merchantToken = merchantToken(Nicepay, RequestType);
        Nicepay.dbProcessUrl = NicepayConfig.GetDBProcessUrl();
        Nicepay.callBackUrl = NicepayConfig.NICEPAY_CALLBACK_URL;
        Nicepay.instmntMon = "1";
        Nicepay.instmntType = "1";
        Nicepay.userIP = GetUserIP();
        Nicepay.goodsNm = Nicepay.description;
        Nicepay.vat = "0";
        Nicepay.fee = "0";
        Nicepay.notaxAmt = "0";
        if (Nicepay.cartData == null)
        {
            Nicepay.cartData = "{}";
        }

        CheckParam(Nicepay.iMid, "01");
        CheckParam(Nicepay.PayMethod, "02");
        CheckParam(Nicepay.currency, "03");
        CheckParam(Nicepay.amt, "04");
        CheckParam(Nicepay.instmntMon, "05");
        CheckParam(Nicepay.referenceNo, "06");
        CheckParam(Nicepay.goodsNm, "07");
        CheckParam(Nicepay.billingNm, "08");
        CheckParam(Nicepay.billingPhone, "09");
        CheckParam(Nicepay.billingEmail, "10");
        CheckParam(Nicepay.billingAddr, "11");
        CheckParam(Nicepay.billingCity, "12");
        CheckParam(Nicepay.billingState, "13");
        CheckParam(Nicepay.billingCountry, "14");
        CheckParam(Nicepay.deliveryNm, "15");
        CheckParam(Nicepay.deliveryPhone, "16");
        CheckParam(Nicepay.deliveryAddr, "17");
        CheckParam(Nicepay.deliveryCity, "18");
        CheckParam(Nicepay.deliveryState, "19");
        CheckParam(Nicepay.deliveryPostCd, "20");
        CheckParam(Nicepay.deliveryCountry, "21");
        CheckParam(Nicepay.callBackUrl, "22");
        CheckParam(Nicepay.dbProcessUrl, "23");
        CheckParam(Nicepay.vat, "24");
        CheckParam(Nicepay.fee, "25");
        CheckParam(Nicepay.notaxAmt, "26");
        CheckParam(Nicepay.description, "27");
        CheckParam(Nicepay.merchantToken, "28");
        CheckParam(Nicepay.BankCd, "29");

        string API_Url = GetApiRequest(RequestType);

        string SingleString = BuildString(Nicepay);

        string Data = WebRequestPostHttp.Post_Http(SingleString, API_Url);
        JavaScriptSerializer JsonSerializer = new JavaScriptSerializer();

        return JsonSerializer.Deserialize<NicepayResponse>(Data);
        
    }

    internal NicepayResponse ChargeCard(NicepayModel Nicepay)
    {
        string RequestType = "CredirCard";
        Nicepay.iMid = NicepayWrapper._merchantId;
        Nicepay.merchantToken = merchantToken(Nicepay, RequestType);
        Nicepay.dbProcessUrl = NicepayConfig.NICEPAY_DBPROCESS_URL;
        Nicepay.callBackUrl = NicepayConfig.NICEPAY_CALLBACK_URL;
        Nicepay.instmntMon = "1";
        Nicepay.instmntType = "1";
        Nicepay.userIP = GetUserIP();
        Nicepay.goodsNm = Nicepay.description;
        Nicepay.vat = "0";
        Nicepay.fee = "0";
        Nicepay.notaxAmt = "0";
        if (Nicepay.cartData == null)
        {
            Nicepay.cartData = "{}";
        }

        CheckParam(Nicepay.iMid, "01");
        CheckParam(Nicepay.PayMethod, "02");
        CheckParam(Nicepay.currency, "03");
        CheckParam(Nicepay.amt, "04");
        CheckParam(Nicepay.instmntMon, "05");
        CheckParam(Nicepay.referenceNo, "06");
        CheckParam(Nicepay.goodsNm, "07");
        CheckParam(Nicepay.billingNm, "08");
        CheckParam(Nicepay.billingPhone, "09");
        CheckParam(Nicepay.billingEmail, "10");
        CheckParam(Nicepay.billingAddr, "11");
        CheckParam(Nicepay.billingCity, "12");
        CheckParam(Nicepay.billingState, "13");
        CheckParam(Nicepay.billingCountry, "14");
        CheckParam(Nicepay.deliveryNm, "15");
        CheckParam(Nicepay.deliveryPhone, "16");
        CheckParam(Nicepay.deliveryAddr, "17");
        CheckParam(Nicepay.deliveryCity, "18");
        CheckParam(Nicepay.deliveryState, "19");
        CheckParam(Nicepay.deliveryPostCd, "20");
        CheckParam(Nicepay.deliveryCountry, "21");
        CheckParam(Nicepay.callBackUrl, "22");
        CheckParam(Nicepay.dbProcessUrl, "23");
        CheckParam(Nicepay.vat, "24");
        CheckParam(Nicepay.fee, "25");
        CheckParam(Nicepay.notaxAmt, "26");
        CheckParam(Nicepay.description, "27");
        CheckParam(Nicepay.merchantToken, "28");

        string API_Url = GetApiRequest(RequestType);

        string SingleString = BuildString(Nicepay);

        string ResultString = WebRequestPostHttp.Post_Http(SingleString, API_Url);
        ResultString = ResultString.Remove(0, 4);
        JavaScriptSerializer JsonSerializer = new JavaScriptSerializer();

        return JsonSerializer.Deserialize<NicepayResponse>(ResultString);
    }

    internal NotificationResult ChargcheckPaymentStatuseCard(string tXid, string referenceNo, string amt)
    {
        string RequestType = "checkPaymentStatus";
        NicepayModel Nicepay = new NicepayModel();

        Nicepay.iMid = NicepayWrapper._merchantId;
        Nicepay.tXid = tXid;
        Nicepay.referenceNo = referenceNo;
        Nicepay.amt = amt;
        Nicepay.merchantToken = merchantToken(Nicepay, RequestType);

        CheckParam(Nicepay.iMid, "01");
        CheckParam(Nicepay.amt, "04");
        CheckParam(Nicepay.referenceNo, "06");
        CheckParam(Nicepay.merchantToken, "28");
        CheckParam(Nicepay.tXid, "30");

        string API_Url = GetApiRequest(RequestType);
        string SingleString = BuildString(Nicepay);

        string ResultString = WebRequestPostHttp.Post_Http(SingleString, API_Url);
        JavaScriptSerializer JsonSerializer = new JavaScriptSerializer();

        return JsonSerializer.Deserialize<NotificationResult>(ResultString);

    }

    internal NotificationResult CancelVA(string tXid, string amt)
    {
        string RequestType = "cancelVA";
        NicepayModel Nicepay = new NicepayModel();

        Nicepay.iMid = NicepayWrapper._merchantId;
        Nicepay.tXid = tXid;
        Nicepay.amt = amt;
        Nicepay.merchantToken = merchantToken(Nicepay, RequestType);
        Nicepay.PayMethod = "02";
        Nicepay.cancelType = "1";

        CheckParam(Nicepay.iMid, "01");
        CheckParam(Nicepay.amt, "04");
        CheckParam(Nicepay.merchantToken, "28");
        CheckParam(Nicepay.tXid, "30");

        string API_Url = GetApiRequest(RequestType);
        string SingleString = BuildString(Nicepay);

        string ResultString = WebRequestPostHttp.Post_Http(SingleString, API_Url);
        JavaScriptSerializer JsonSerializer = new JavaScriptSerializer();

        return JsonSerializer.Deserialize<NotificationResult>(ResultString);

    }
    static string merchantToken(NicepayModel Nicepay, string RequestType)
    {
        byte[] MerchantStr;
        if (RequestType == "cancelVA")
        {
            MerchantStr = Encoding.ASCII.GetBytes(NicepayWrapper._merchantId + Nicepay.tXid + Nicepay.amt + NicepayWrapper._merchantKey);
        }
        else if (RequestType == "registerCustomer" || RequestType == "retrieveVaInfo") {
            MerchantStr = Encoding.ASCII.GetBytes(NicepayWrapper._merchantId + Nicepay.customerId + NicepayWrapper._merchantKey);
        }
        else if (RequestType == "listDepositCustomerId")
        {
            MerchantStr = Encoding.ASCII.GetBytes(NicepayWrapper._merchantId + Nicepay.customerId + Nicepay.startDt + NicepayWrapper._merchantKey);
        }
        else if (RequestType == "listDepositVa")
        {
            MerchantStr = Encoding.ASCII.GetBytes(NicepayWrapper._merchantId + Nicepay.vacctNo + Nicepay.startDt + NicepayWrapper._merchantKey);
        }
        else
        {
            MerchantStr = Encoding.ASCII.GetBytes(NicepayWrapper._merchantId + Nicepay.referenceNo + Nicepay.amt + NicepayWrapper._merchantKey);
        }
        
       
        SHA256Managed hashString = new SHA256Managed();
        string hex = "";

        var hashValue = hashString.ComputeHash(MerchantStr);
        foreach (byte x in hashValue)
        {
            hex += String.Format("{0:x2}", x);
        }
        return hex.ToLower();

    }

    internal registerCustomerId registerCustomerId(NicepayModel Nicepay)
    {
        string RequestType = "registerCustomer";

        Nicepay.iMid = NicepayWrapper._merchantId;
        Nicepay.merchantToken = merchantToken(Nicepay, RequestType);

        CheckParam(Nicepay.iMid, "01");
        CheckParam(Nicepay.customerId, "31");
        CheckParam(Nicepay.customerNm, "32");
        CheckParam(Nicepay.merchantToken, "28");

        string API_Url = GetApiRequest(RequestType);
        string SingleString = BuildString(Nicepay);

        string ResultString = WebRequestPostHttp.Post_Http(SingleString, API_Url);
        JavaScriptSerializer JsonSerializer = new JavaScriptSerializer();

        return JsonSerializer.Deserialize<registerCustomerId>(ResultString);

    }

    internal registerCustomerId retrieveVaInfo(NicepayModel Nicepay)
    {
        string RequestType = "retrieveVaInfo";

        Nicepay.iMid = NicepayWrapper._merchantId;
        Nicepay.merchantToken = merchantToken(Nicepay, RequestType);

        CheckParam(Nicepay.iMid, "01");
        CheckParam(Nicepay.customerId, "31");
        CheckParam(Nicepay.merchantToken, "28");

        string API_Url = GetApiRequest(RequestType);
        string SingleString = BuildString(Nicepay);

        string ResultString = WebRequestPostHttp.Post_Http(SingleString, API_Url);
        JavaScriptSerializer JsonSerializer = new JavaScriptSerializer();

        return JsonSerializer.Deserialize<registerCustomerId>(ResultString);

    }

    internal CustomerDeposit listDepositCustomerId(NicepayModel Nicepay)
    {
        string RequestType = "listDepositCustomerId";

        Nicepay.iMid = NicepayWrapper._merchantId;
        Nicepay.merchantToken = merchantToken(Nicepay, RequestType);

        CheckParam(Nicepay.iMid, "01");
        CheckParam(Nicepay.customerId, "31");
        CheckParam(Nicepay.startDt, "34");
        CheckParam(Nicepay.endDt, "35");
        CheckParam(Nicepay.merchantToken, "28");

        string API_Url = GetApiRequest(RequestType);
        string SingleString = BuildString(Nicepay);

        string ResultString = WebRequestPostHttp.Post_Http(SingleString, API_Url);
        JavaScriptSerializer JsonSerializer = new JavaScriptSerializer();

        return JsonSerializer.Deserialize<CustomerDeposit>(ResultString);
    }

    internal CustomerDeposit listDepositVa(NicepayModel Nicepay)
    {
        string RequestType = "listDepositVa";

        Nicepay.iMid = NicepayWrapper._merchantId;
        Nicepay.merchantToken = merchantToken(Nicepay, RequestType);

        CheckParam(Nicepay.iMid, "01");
        CheckParam(Nicepay.vacctNo, "33");
        CheckParam(Nicepay.startDt, "34");
        CheckParam(Nicepay.endDt, "35");
        CheckParam(Nicepay.merchantToken, "28");

        string API_Url = GetApiRequest(RequestType);
        string SingleString = BuildString(Nicepay);

        string ResultString = WebRequestPostHttp.Post_Http(SingleString, API_Url);
        JavaScriptSerializer JsonSerializer = new JavaScriptSerializer();

        return JsonSerializer.Deserialize<CustomerDeposit>(ResultString);
    }

    internal string GetUserIP()
    {
        string SearchName = string.Empty;
        String strHostName = HttpContext.Current.Request.UserHostAddress.ToString();
        return System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
    }

    internal string Remarks(string BankCD)
    {
        string BankName;
        switch (BankCD)
        {
            case "CENA":
                BankName = "BCA";
                break;
            case "IBBK":
                BankName = "Maybank";
                break;
            case "BNIN":
                BankName = "Bank BNI";
                break;
            case "BMRI":
                BankName = "Bank Mandiri";
                break;
            case "BBBA":
                BankName = "Bank Permata";
                break;
            case "BNIA":
                BankName = "Bank CIMB";
                break;
            case "BRIN":
                BankName = "Bank BRI";
                break;
            case "BDIN":
                BankName = "Bank Danamon";
                break;
            default:
                BankName = "KEB Hana Bank";
                break;
        }
        return BankName;
    }

    internal string GetBankCode(PaymentSubmethod submethod)
    {
        switch (submethod)
        {
            case PaymentSubmethod.BCA:
                return "CENA";
            case PaymentSubmethod.Maybank:
                return "IBBK";
            case PaymentSubmethod.BNI:
                return "BNIN";
            case PaymentSubmethod.Mandiri:
                return "BMRI";
            case PaymentSubmethod.Permata:
                return "BBBA";
            case PaymentSubmethod.CIMB:
                return "BNIA";
            case PaymentSubmethod.BRI:
                return "BRIN";
            case PaymentSubmethod.Danamon:
                return "BDIN";
            case PaymentSubmethod.KEBHana:
                return "HNBN";
            default:
                return "BBBA";
        }
    }

    internal string GetApiRequest(string Request)
    {
        string API_req = NicepayWrapper._endpoint;
        if (Request == "RequestVA")
        {
            API_req += NicepayConfig.NICEPAY_REQ_VA_URL;
        }
        else if (Request == "CredirCard")
        {
            API_req +=  NicepayConfig.NICEPAY_REQ_CC_URL;
        }
        else if (Request == "checkPaymentStatus")
        {
            API_req += NicepayConfig.NICEPAY_ORDER_STATUS_URL;
        }
        else if (Request == "cancelVA")
        {
            API_req += NicepayConfig.NICEPAY_CANCEL_VA_URL;
        }
        else if (Request == "registerCustomer")
        {
            API_req += NicepayConfig.NICEPAY_FIX_REG_CUSTOMER_ID;
        }
        else if (Request == "retrieveVaInfo")
        {
            API_req += NicepayConfig.NICEPAY_FIX_RETRIEVE_VA_INFO;
        }
        else if (Request == "listDepositCustomerId")
        {
            API_req += NicepayConfig.NICEPAY_FIX_LIST_DEPOSIT_CUSTOMERID;
        }
        else if (Request == "listDepositVa")
        {
            API_req += NicepayConfig.NICEPAY_FIX_LIST_DEPOSIT_VA;
        }
        return API_req;
    }

    internal string BuildString(NicepayModel Nicepay)
    {
        StringBuilder stringBuilder = new StringBuilder();

        Type t = Nicepay.GetType();
        string fieldName = null;
        object propertyValue = null;

        string resultStr = null;
        foreach (PropertyInfo pi in t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            fieldName = pi.Name;
            propertyValue = pi.GetValue(Nicepay, null);
            if (propertyValue != null)
            {
                stringBuilder.Append(fieldName + "=" + propertyValue + "&");
            }
        }

        string last = stringBuilder.ToString().Substring(stringBuilder.ToString().Length - 1);
        if (last == "&")
        {
            resultStr = stringBuilder.ToString().Remove(stringBuilder.ToString().Length - 1);
        }

        return resultStr;
    }

    internal string CheckParam(string RequestData, string ErrorNo)
    {
        if (RequestData == null)
        {
            PrintError(ErrorNo);
        }
        return RequestData;
    }

    internal string  PrintError(string ErrorNo)
    {
        string[,] getError = new string[1, 2];

        switch (ErrorNo)
        {
            case "00":
                getError = new string[1, 2] { { "00000", "Unknown error. Contact it.support@ionpay.net." } };
                break;
            case "01":
                getError = new string[1, 2] { { "10001", "(iMid) is not set. Please set (iMid)." } };
                break;
            case "02":
                getError = new string[1, 2] { { "10002", "(payMethod) is not set. Please set (payMethod)." } };
                break;
            case "03":
                getError = new string[1, 2] { { "10003", "(currency) is not set. Please set (currency)." } };
                break;
            case "04":
                getError = new string[1, 2] { { "10004", "(amt) is not set. Please set (amt)." } };
                break;
            case "05":
                getError = new string[1, 2] { { "10005", "(instmntMon) is not set. Please set (instmntMon)." } };
                break;
            case "06":
                getError = new string[1, 2] { { "10006", "(referenceNo) is not set. Please set (referenceNo)." } };
                break;
            case "07":
                getError = new string[1, 2] { { "10007", "(goodsNm) is not set. Please set (goodsNm)." } };
                break;
            case "08":
                getError = new string[1, 2] { { "10008", "(billingNm) is not set. Please set (billingNm)." } };
                break;
            case "09":
                getError = new string[1, 2] { { "10009", "(billingPhone) is not set. Please set (billingPhone)." } };
                break;
            case "10":
                getError = new string[1, 2] { { "10010", "(billingEmail) is not set. Please set (billingEmail)." } };
                break;
            case "11":
                getError = new string[1, 2] { { "10011", "(billingAddr) is not set. Please set (billingAddr)." } };
                break;
            case "12":
                getError = new string[1, 2] { { "10012", "(billingCity) is not set. Please set (billingCity)." } };
                break;
            case "13":
                getError = new string[1, 2] { { "10013", "(billingState) is not set. Please set (billingState)." } };
                break;
            case "14":
                getError = new string[1, 2] { { "10014", "(billingCountry) is not set. Please set (billingCountry)." } };
                break;
            case "15":
                getError = new string[1, 2] { { "10015", "(deliveryNm) is not set. Please set (deliveryNm)." } };
                break;
            case "16":
                getError = new string[1, 2] { { "10016", "(deliveryPhone) is not set. Please set (deliveryPhone)." } };
                break;
            case "17":
                getError = new string[1, 2] { { "10017", "(deliveryAddr) is not set. Please set (deliveryAddr)." } };
                break;
            case "18":
                getError = new string[1, 2] { { "10018", "(deliveryCity) is not set. Please set (deliveryCity)." } };
                break;
            case "19":
                getError = new string[1, 2] { { "10019", "(deliveryState) is not set. Please set (deliveryState)." } };
                break;
            case "20":
                getError = new string[1, 2] { { "10020", "(deliveryPostCd) is not set. Please set (deliveryPostCd)." } };
                break;
            case "21":
                getError = new string[1, 2] { { "10021", "(deliveryCountry) is not set. Please set (deliveryCountry)." } };
                break;
            case "22":
                getError = new string[1, 2] { { "10022", "(callBackUrl) is not set. Please set (callBackUrl)." } };
                break;
            case "23":
                getError = new string[1, 2] { { "10023", "(dbProcessUrl) is not set. Please set (dbProcessUrl)." } };
                break;
            case "24":
                getError = new string[1, 2] { { "10024", "(vat) is not set. Please set (vat)." } };
                break;
            case "25":
                getError = new string[1, 2] { { "10025", "(fee) is not set. Please set (fee)." } };
                break;
            case "26":
                getError = new string[1, 2] { { "10026", "(notaxAmt) is not set. Please set (notaxAmt)." } };
                break;
            case "27":
                getError = new string[1, 2] { { "10027", "(description) is not set. Please set (description)." } };
                break;
            case "28":
                getError = new string[1, 2] { { "10028", "(merchantToken) is not set. Please set (merchantToken)." } };
                break;
            case "29":
                getError = new string[1, 2] { { "10029", "(bankCd) is not set. Please set (bankCd)." } };
                break;
            case "30":
                getError = new string[1, 2] { { "10030", "(tXid) is not set. Please set (tXid)." } };
                break;
            case "31":
                getError = new string[1, 2] { { "10031", "(customerId) is not set. Please set (customerId)." } };
                break;
            case "32":
                getError = new string[1, 2] { { "10032", "(customerNm) is not set. Please set (customerNm)." } };
                break;
            case "33":
                getError = new string[1, 2] { { "10033", "(vacctNo) is not set. Please set (vacctNo)." } };
                break;
            case "34":
                getError = new string[1, 2] { { "10034", "(startDt) is not set. Please set (startDt)." } };
                break;
            case "35":
                getError = new string[1, 2] { { "10035", "(endDt) is not set. Please set (endDt)." } };
                break;

        }

        string s1 = null;
        string s2 = null;
        for (int i = 0; i <= getError.GetUpperBound(0); i++)
        {
             s1 = getError[i, 0];
             s2 = getError[i, 1];
        }

        return s1 + ", " + s2;
        //HttpContext.Current.Response.Redirect("~/ErrorCs.aspx?ErrorNo=" + s1 + "&ErrorMsg=" + s2);

    }

}
