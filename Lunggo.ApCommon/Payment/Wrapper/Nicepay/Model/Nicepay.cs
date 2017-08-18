using System.Collections.Generic;

public class NotificationResult
{
    public string tXid { get; set; }
    public string referenceNo { get; set; }
    public string amt { get; set; }
    public string merchantToken { get; set; }
    public string reqTm { get; set; }
    public string goodsNm { get; set; }
    public string resultCd { get; set; }
    public string instmntType { get; set; }
    public string iMid { get; set; }
    public string billingNm { get; set; }
    public string resultMsg { get; set; }
    public string vacctValidDt { get; set; }
    public string payMethod { get; set; }
    public string reqDt { get; set; }
    public string currency { get; set; }
    public string instmntMon { get; set; }
    public string vacctValidTm { get; set; }
    public string status { get; set; }
    public string vacctNo { get; set; }
    public string bankCd { get; set; }
}

public class NicepayResponse
{
    public string resultCd { get; set; }
    public string amount { get; set; }
    public string referenceNo { get; set; }
    public string transTm { get; set; }
    public string payMethod { get; set; }
    public string tXid { get; set; }
    public string description { get; set; }
    public string callbackUrl { get; set; }
    public string bankVacctNo { get; set; }
    public string transDt { get; set; }
    public string resultMsg { get; set; }

    public string apiType { get; set; }
    public string requestDate { get; set; }
    public string responseDate { get; set; }
    public Data data { get; set; }

}

public class Data
{
    public string tXid { get; set; }
    public string resultCd { get; set; }
    public string resultMsg { get; set; }
    public string requestURL { get; set; }

}

public class registerCustomerId
{
    public string customerId { get; set; }
    public string customerNm { get; set; }
    public string resultCd { get; set; }
    public string resultMsg { get; set; }
    public VacctInfoList[] VacctInfoList { get; set; }

}

public class VacctInfoList
{
    public string bankName { get; set; }
    public string bankCd { get; set; }
    public string vacctNo { get; set; }
}

public class CustomerDeposit
{
    public string customerId { get; set; }
    public string resultCd { get; set; }
    public string resultMsg { get; set; }
    public depositCustomerIdInfo[] depositCustomerIdInfo { get; set; }
    public depositInfo[] depositInfo { get; set; }
}

public class depositCustomerIdInfo
{
    public string bankCd { get; set; }
    public string vacctNo { get; set; }
    public string amt { get; set; }
    public string date { get; set; }
    public string time { get; set; }
    public string tXid { get; set; }
    public string status { get; set; }
}

public class depositInfo
{
    public string bankCd { get; set; }
    public string referenceNo { get; set; }
    public string amt { get; set; }
    public string date { get; set; }
    public string time { get; set; }
    public string tXid { get; set; }
    public string status { get; set; }
}

public class NicepayModel
{
    public string customerId { get; set; }
    public string customerNm { get; set; }
    public string startDt { get; set; }
    public string endDt { get; set; }
    public string vacctNo { get; set; }

    public string cancelType;
    private string _tXid;
    private string _Currency;
    private string _BankCd;
    private string _DateNow;
    private string _vaExpDate;
    private string _iMid;
    private string _MerchantKey;
    private string _PayMethod;
    private string _amt;
    private string _referenceNo;
    private string _description;
    private string _billingNm;
    private string _billingPhone;
    private string _billingEmail;
    private string _billingAddr;
    private string _billingCity;
    private string _billingState;
    private string _billingPostCd;
    private string _billingCountry;
    private string _deliveryNm;
    private string _deliveryPhone;
    private string _deliveryEmail;
    private string _deliveryAddr;
    private string _deliveryCity;
    private string _deliveryState;
    private string _deliveryPostCd;
    private string _deliveryCountry;
    private string _vacctValidDt;
    private string _vacctValidTm;
    private string _merchantToken;
    private string _dbProcessUrl;
    private string _callBackUrl;
    private string _instmntMon;
    private string _instmntType;
    private string _userIP;
    private string _goodsNm;
    private string _notaxAmt;
    private string _vat;
    private string _fee;
    private string _cartData;

    //public string customerId
    //{
    //    get { return _customerId; }
    //    set { _customerId = value; }
    //}

    //public string customerNm
    //{
    //    get { return _customerNm; }
    //    set { _customerNm = value; }
    //}

    public string tXid
    {
        get { return _tXid; }
        set { _tXid = value; }
    }
    public string currency
    {
        get { return _Currency; }
        set { _Currency = value; }
    }
        
    public string BankCd
    {
        get { return _BankCd; }
        set { _BankCd = value; }
    }
        
    public string DateNow
    {
        get { return _DateNow; }
        set { _DateNow = value; }
    }
        
    public string vaExpDate
    {
        get { return _vaExpDate; }
        set { _vaExpDate = value; }
    }
    
    public string iMid
    {
        get { return _iMid; }
        set { _iMid = value; }
    }

    public string MerchantKey
    {
        get { return _MerchantKey; }
        set { _MerchantKey = value; }
    }

    public string PayMethod
    {
        get { return _PayMethod; }
        set { _PayMethod = value; }
    }

    public string amt
    {
        get { return _amt; }
        set { _amt = value; }
    }
    
    public string referenceNo
    {
        get { return _referenceNo; }
        set { _referenceNo = value; }
    }

    public string description
    {
        get { return _description; }
        set { _description = value; }
    }

    public string billingNm
    {
        get { return _billingNm; }
        set { _billingNm = value; }
    }

    public string billingPhone
    {
        get { return _billingPhone; }
        set { _billingPhone = value; }
    }

    public string billingEmail
    {
        get { return _billingEmail; }
        set { _billingEmail = value; }
    }

    public string billingAddr
    {
        get { return _billingAddr; }
        set { _billingAddr = value; }
    }

    public string billingCity
    {
        get { return _billingCity; }
        set { _billingCity = value; }
    }
    
    public string billingState
    {
        get { return _billingState; }
        set { _billingState = value; }
    }

    public string billingPostCd
    {
        get { return _billingPostCd; }
        set { _billingPostCd = value; }
    }

    public string billingCountry
    {
        get { return _billingCountry; }
        set { _billingCountry = value; }
    }

    public string deliveryNm
    {
        get { return _deliveryNm; }
        set { _deliveryNm = value; }
    }

    public string deliveryPhone
    {
        get { return _deliveryPhone; }
        set { _deliveryPhone = value; }
    }

    public string deliveryEmail
    {
        get { return _deliveryEmail; }
        set { _deliveryEmail = value; }
    }
    
    public string deliveryAddr
    {
        get { return _deliveryAddr; }
        set { _deliveryAddr = value; }
    }
    
    public string deliveryCity
    {
        get { return _deliveryCity; }
        set { _deliveryCity = value; }
    }
    
    public string deliveryState
    {
        get { return _deliveryState; }
        set { _deliveryState = value; }
    }

    public string deliveryPostCd
    {
        get { return _deliveryPostCd; }
        set { _deliveryPostCd = value; }
    }
    
    public string deliveryCountry
    {
        get { return _deliveryCountry; }
        set { _deliveryCountry = value; }
    }
    
    public string vacctValidDt
    {
        get { return _vacctValidDt; }
        set { _vacctValidDt = value; }
    }

    public string vacctValidTm
    {
        get { return _vacctValidTm; }
        set { _vacctValidTm = value; }
    }

    public string merchantToken
    {
        get { return _merchantToken; }
        set { _merchantToken = value; }
    }
    
    public string dbProcessUrl
    {
        get { return _dbProcessUrl; }
        set { _dbProcessUrl = value; }
    }

    public string callBackUrl
    {
        get { return _callBackUrl; }
        set { _callBackUrl = value; }
    }
    
    public string instmntMon
    {
        get { return _instmntMon; }
        set { _instmntMon = value; }
    }
    
    public string instmntType
    {
        get { return _instmntType; }
        set { _instmntType = value; }
    }
    
    public string userIP
    {
        get { return _userIP; }
        set { _userIP = value; }
    }

    public string goodsNm
    {
        get { return _goodsNm; }
        set { _goodsNm = value; }
    }

    public string vat
    {
        get { return _vat; }
        set { _vat = value; }
    }

    public string fee
    {
        get { return _fee; }
        set { _fee = value; }
    }

    public string notaxAmt
    {
        get { return _notaxAmt; }
        set { _notaxAmt = value; }
    }

    public string cartData
    {
        get { return _cartData; }
        set { _cartData = value; }
    }
    
}