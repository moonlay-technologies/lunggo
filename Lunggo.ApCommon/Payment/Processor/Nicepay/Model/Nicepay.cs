using System.Collections.Generic;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {

        internal class NotificationResult
        {
            internal string tXid { get; set; }
            internal string referenceNo { get; set; }
            internal string amt { get; set; }
            internal string merchantToken { get; set; }
            internal string reqTm { get; set; }
            internal string goodsNm { get; set; }
            internal string resultCd { get; set; }
            internal string instmntType { get; set; }
            internal string iMid { get; set; }
            internal string billingNm { get; set; }
            internal string resultMsg { get; set; }
            internal string vacctValidDt { get; set; }
            internal string payMethod { get; set; }
            internal string reqDt { get; set; }
            internal string currency { get; set; }
            internal string instmntMon { get; set; }
            internal string vacctValidTm { get; set; }
            internal string status { get; set; }
            internal string vacctNo { get; set; }
            internal string bankCd { get; set; }
        }

        internal class NicepayResponse
        {
            internal string resultCd { get; set; }
            internal string amount { get; set; }
            internal string referenceNo { get; set; }
            internal string transTm { get; set; }
            internal string payMethod { get; set; }
            internal string tXid { get; set; }
            internal string description { get; set; }
            internal string callbackUrl { get; set; }
            internal string bankVacctNo { get; set; }
            internal string transDt { get; set; }
            internal string resultMsg { get; set; }

            internal string apiType { get; set; }
            internal string requestDate { get; set; }
            internal string responseDate { get; set; }
            internal Data data { get; set; }

        }

        internal class Data
        {
            internal string tXid { get; set; }
            internal string resultCd { get; set; }
            internal string resultMsg { get; set; }
            internal string requestURL { get; set; }

        }

        internal class registerCustomerId
        {
            internal string customerId { get; set; }
            internal string customerNm { get; set; }
            internal string resultCd { get; set; }
            internal string resultMsg { get; set; }
            internal VacctInfoList[] VacctInfoList { get; set; }

        }

        internal class VacctInfoList
        {
            internal string bankName { get; set; }
            internal string bankCd { get; set; }
            internal string vacctNo { get; set; }
        }

        internal class CustomerDeposit
        {
            internal string customerId { get; set; }
            internal string resultCd { get; set; }
            internal string resultMsg { get; set; }
            internal depositCustomerIdInfo[] depositCustomerIdInfo { get; set; }
            internal depositInfo[] depositInfo { get; set; }
        }

        internal class depositCustomerIdInfo
        {
            internal string bankCd { get; set; }
            internal string vacctNo { get; set; }
            internal string amt { get; set; }
            internal string date { get; set; }
            internal string time { get; set; }
            internal string tXid { get; set; }
            internal string status { get; set; }
        }

        internal class depositInfo
        {
            internal string bankCd { get; set; }
            internal string referenceNo { get; set; }
            internal string amt { get; set; }
            internal string date { get; set; }
            internal string time { get; set; }
            internal string tXid { get; set; }
            internal string status { get; set; }
        }

        internal class NicepayModel
        {
            internal string customerId { get; set; }
            internal string customerNm { get; set; }
            internal string startDt { get; set; }
            internal string endDt { get; set; }
            internal string vacctNo { get; set; }

            internal string cancelType;
            internal string _tXid;
            internal string _Currency;
            internal string _BankCd;
            internal string _DateNow;
            internal string _vaExpDate;
            internal string _iMid;
            internal string _MerchantKey;
            internal string _PayMethod;
            internal string _amt;
            internal string _referenceNo;
            internal string _description;
            internal string _billingNm;
            internal string _billingPhone;
            internal string _billingEmail;
            internal string _billingAddr;
            internal string _billingCity;
            internal string _billingState;
            internal string _billingPostCd;
            internal string _billingCountry;
            internal string _deliveryNm;
            internal string _deliveryPhone;
            internal string _deliveryEmail;
            internal string _deliveryAddr;
            internal string _deliveryCity;
            internal string _deliveryState;
            internal string _deliveryPostCd;
            internal string _deliveryCountry;
            internal string _vacctValidDt;
            internal string _vacctValidTm;
            internal string _merchantToken;
            internal string _dbProcessUrl;
            internal string _callBackUrl;
            internal string _instmntMon;
            internal string _instmntType;
            internal string _userIP;
            internal string _goodsNm;
            internal string _notaxAmt;
            internal string _vat;
            internal string _fee;
            internal string _cartData;

            //internal string customerId
            //{
            //    get { return _customerId; }
            //    set { _customerId = value; }
            //}

            //internal string customerNm
            //{
            //    get { return _customerNm; }
            //    set { _customerNm = value; }
            //}

            internal string tXid
            {
                get { return _tXid; }
                set { _tXid = value; }
            }

            internal string currency
            {
                get { return _Currency; }
                set { _Currency = value; }
            }

            internal string BankCd
            {
                get { return _BankCd; }
                set { _BankCd = value; }
            }

            internal string DateNow
            {
                get { return _DateNow; }
                set { _DateNow = value; }
            }

            internal string vaExpDate
            {
                get { return _vaExpDate; }
                set { _vaExpDate = value; }
            }

            internal string iMid
            {
                get { return _iMid; }
                set { _iMid = value; }
            }

            internal string MerchantKey
            {
                get { return _MerchantKey; }
                set { _MerchantKey = value; }
            }

            internal string PayMethod
            {
                get { return _PayMethod; }
                set { _PayMethod = value; }
            }

            internal string amt
            {
                get { return _amt; }
                set { _amt = value; }
            }

            internal string referenceNo
            {
                get { return _referenceNo; }
                set { _referenceNo = value; }
            }

            internal string description
            {
                get { return _description; }
                set { _description = value; }
            }

            internal string billingNm
            {
                get { return _billingNm; }
                set { _billingNm = value; }
            }

            internal string billingPhone
            {
                get { return _billingPhone; }
                set { _billingPhone = value; }
            }

            internal string billingEmail
            {
                get { return _billingEmail; }
                set { _billingEmail = value; }
            }

            internal string billingAddr
            {
                get { return _billingAddr; }
                set { _billingAddr = value; }
            }

            internal string billingCity
            {
                get { return _billingCity; }
                set { _billingCity = value; }
            }

            internal string billingState
            {
                get { return _billingState; }
                set { _billingState = value; }
            }

            internal string billingPostCd
            {
                get { return _billingPostCd; }
                set { _billingPostCd = value; }
            }

            internal string billingCountry
            {
                get { return _billingCountry; }
                set { _billingCountry = value; }
            }

            internal string deliveryNm
            {
                get { return _deliveryNm; }
                set { _deliveryNm = value; }
            }

            internal string deliveryPhone
            {
                get { return _deliveryPhone; }
                set { _deliveryPhone = value; }
            }

            internal string deliveryEmail
            {
                get { return _deliveryEmail; }
                set { _deliveryEmail = value; }
            }

            internal string deliveryAddr
            {
                get { return _deliveryAddr; }
                set { _deliveryAddr = value; }
            }

            internal string deliveryCity
            {
                get { return _deliveryCity; }
                set { _deliveryCity = value; }
            }

            internal string deliveryState
            {
                get { return _deliveryState; }
                set { _deliveryState = value; }
            }

            internal string deliveryPostCd
            {
                get { return _deliveryPostCd; }
                set { _deliveryPostCd = value; }
            }

            internal string deliveryCountry
            {
                get { return _deliveryCountry; }
                set { _deliveryCountry = value; }
            }

            internal string vacctValidDt
            {
                get { return _vacctValidDt; }
                set { _vacctValidDt = value; }
            }

            internal string vacctValidTm
            {
                get { return _vacctValidTm; }
                set { _vacctValidTm = value; }
            }

            internal string merchantToken
            {
                get { return _merchantToken; }
                set { _merchantToken = value; }
            }

            internal string dbProcessUrl
            {
                get { return _dbProcessUrl; }
                set { _dbProcessUrl = value; }
            }

            internal string callBackUrl
            {
                get { return _callBackUrl; }
                set { _callBackUrl = value; }
            }

            internal string instmntMon
            {
                get { return _instmntMon; }
                set { _instmntMon = value; }
            }

            internal string instmntType
            {
                get { return _instmntType; }
                set { _instmntType = value; }
            }

            internal string userIP
            {
                get { return _userIP; }
                set { _userIP = value; }
            }

            internal string goodsNm
            {
                get { return _goodsNm; }
                set { _goodsNm = value; }
            }

            internal string vat
            {
                get { return _vat; }
                set { _vat = value; }
            }

            internal string fee
            {
                get { return _fee; }
                set { _fee = value; }
            }

            internal string notaxAmt
            {
                get { return _notaxAmt; }
                set { _notaxAmt = value; }
            }

            internal string cartData
            {
                get { return _cartData; }
                set { _cartData = value; }
            }

        }
    }
}