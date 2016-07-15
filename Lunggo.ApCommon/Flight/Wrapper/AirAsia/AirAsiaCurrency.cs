using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using CsQuery;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using RestSharp;
using HttpUtility = RestSharp.Extensions.MonoHttp.HttpUtility;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia

{
    internal partial class AirAsiaWrapper
    {
        internal override Currency CurrencyGetter(string currency)
        {
            return Client.CurrencyGetter(currency);
        }

        private partial class AirAsiaClientHandler
        {
            internal Currency CurrencyGetter(string currencyName)
            {

                var depdate = DateTime.Now.AddMonths(6);
                //var depdate = new DateTime(2016, 12, 21);
                var client = CreateAgentClient();
                string origin;
                const string dest = "CGK";
                Currency curr;
                
                var currencyList = Currency.GetAllCurrencies(Supplier.AirAsia);
                if (!currencyList.TryGetValue(currencyName, out curr))
                {
                    return new Currency(currencyName, Supplier.AirAsia);
                }

                switch (currencyName)
                {
                    case "SGD":
                        origin = "SIN";
                        break;
                    case "MYR":
                        origin = "KUL";
                        break;
                    case "AUD":
                        origin = "SYD";
                        break;
                    case "CNY":
                        origin = "PVG";
                        break;
                    case "HKD":
                        origin = "HKG";
                        break;
                    case "JPY":
                        origin = "HND";
                        break;
                    case "NZD":
                        origin = "AKL";
                        break;
                    case "THB":
                        origin = "DMK";
                        break;
                    case "PHP":
                        origin = "MNL";
                        break;
                    case "USD":
                        origin = "HAN";
                        break;
                    case "BND":
                        origin = "BWN";
                        break;
                    case "SAR":
                        origin = "JED";
                        break;
                    case "INR":
                        origin = "MAA";
                        break;
                    case "KRW":
                        origin = "ICN";
                        break;
                    case "MOP":
                        origin = "MFM";
                        break;
                    case "NPR":
                        origin = "KTM";
                        break;
                    case "LKR":
                        origin = "CMB";
                        break;
                    case "TWD":
                        origin = "TPE";
                        break;
                    default:
                        origin = "CGK";
                        break;
                }
                
                if (!Login(client))
                    return new Currency(currencyName, Supplier.AirAsia);
                try
                {
                    var scheduledNotFound = true;
                    var loopday = 0;
                    var gettable = new CQ();
                    RestRequest searchRequest;
                    IRestResponse searchResponse;
                    string postData;

                    while (scheduledNotFound && loopday < 7)
                    {
                        postData =
                        @"__EVENTTARGET=" +
                        @"&__EVENTARGUMENT=" +
                        @"&__VIEWSTATE=%2FwEPDwUBMGRktapVDbdzjtpmxtfJuRZPDMU9XYk%3D" +
                        @"&pageToken=" +
                        @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24RadioButtonMarketStructure=OneWay" +
                        @"&oneWayOnly=1" +
                        @"&ControlGroupSearchView_AvailabilitySearchInputSearchVieworiginStation1=" + origin +
                        @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24TextBoxMarketOrigin1=" + origin +
                        @"&ControlGroupSearchView_AvailabilitySearchInputSearchViewdestinationStation1=" + dest +
                        @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24TextBoxMarketDestination1=" +
                        dest +
                        @"&ControlGroupSearchView%24MultiCurrencyConversionViewSearchView%24DropDownListCurrency=default" +
                        @"&date_picker=" + depdate.Month + "%2F" + depdate.Day + "%2F" + depdate.Year +
                        @"&date_picker=" +
                        @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListMarketDay1=" +
                        depdate.Day +
                        @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListMarketMonth1=" +
                        depdate.Year + "-" + depdate.Month +
                        @"&date_picker=" + depdate.Month + "%2F" + depdate.Day + "%2F" + depdate.Year +
                        @"&date_picker=" +
                        @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListMarketDay2=" +
                        depdate.Day +
                        @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListMarketMonth2=" +
                        depdate.Year + "-" + depdate.Month +
                        @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListPassengerType_ADT=" +
                        1 +
                        @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListPassengerType_CHD=" +
                        0 +
                        @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListPassengerType_INFANT=" +
                        0 +
                        @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListSearchBy=columnView" +
                        @"&ControlGroupSearchView%24ButtonSubmit=Search" +
                        @"&__VIEWSTATEGENERATOR=05F9A2B0";

                        searchRequest = new RestRequest("Search.aspx", Method.POST);
                        searchRequest.AddHeader("Referer", "https://booking2.airasia.com/Search.aspx");
                        searchRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                        searchResponse = client.Execute(searchRequest);
                        var html0 = (CQ)searchResponse.Content;

                        if (searchResponse.ResponseUri.AbsolutePath != "/Select.aspx" ||
                            (searchResponse.StatusCode != HttpStatusCode.OK &&
                             searchResponse.StatusCode != HttpStatusCode.Redirect))
                            return new Currency(currencyName, Supplier.AirAsia);

                        Thread.Sleep(1000);

                        gettable = html0["#fareTable1_4"];
                        if (gettable.ToString().Length == 0)
                        {
                            depdate = depdate.AddDays(8);
                            loopday++;
                        }
                        else
                        {
                            scheduledNotFound = false;
                        }
                        
                    }

                    if (loopday == 7)
                    {
                        return new Currency(currencyName, Supplier.AirAsia);
                    }

                    var table = gettable[0].ChildElements.ToList()[0];
                    var firstrow = table.ChildElements.ToList()[1];
                    var childcountofrow = firstrow.ChildElements.ToList().Count;
                    var lastcolumn = firstrow.ChildElements.ToList()[childcountofrow - 1];
                    var coreFareId =
                        lastcolumn.ChildElements.ToList()[0].ChildElements.ToList()[0].ChildElements.ToList()[0]
                            .GetAttribute("value");

                    var usedFareId = (coreFareId.Split('@')[0]).Replace(":", "%3A");
                    searchRequest =
                        new RestRequest("TaxAndFeeInclusiveDisplayAjax-resource.aspx?flightKeys=" + usedFareId
                                        + "&numberOfMarkets=1&keyDelimeter=%2C", Method.GET);
                    searchRequest.AddHeader("Referer", "https://booking2.airasia.com/Select.aspx");
                    searchRequest.AddHeader("Accept", "*/*");
                    searchRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
                    searchResponse = client.Execute(searchRequest);
                    var html1 = (CQ) searchResponse.Content;

                    var dataflight = html1[".row1"].ToList();
                    var dataCity = html1[".row2.mtop-row"].ToList();
                    var listflightno = new List<string>();
                    var listcities = new List<string>();
                    foreach (var row in dataflight)
                    {
                        if (row.ChildElements.ToList()[0].GetAttribute("class") == "right-text bold grey1")
                        {
                            listflightno.Add(row.ChildElements.ToList()[0].InnerText);
                        }
                    }

                    foreach (var segment in dataCity)
                    {
                        var children = segment.ChildElements.ToList();
                        var cities = "";
                        foreach (var child in children)
                        {
                            if (child.GetAttribute("class") == "left text")
                            {
                                cities += child.InnerText;

                            }
                        }
                        listcities.Add(cities);
                    }

                    var currency = html1[".black1.total-currency"].ToList()[0].InnerText;

                    postData =
                        @"__EVENTTARGET=" +
                        @"&__EVENTARGUMENT=" +
                        @"&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFP0NvbnRyb2xHcm91cFNlbGVjdFZpZXckU3BlY2lhbE5lZWRzSW5wdXRTZWxlY3RWaWV3JENoZWNrQm94U1NSc2KF%2B3FBQndP4mQD4nrPT4PNXNaR" +
                        @"&pageToken=" +
                        @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24RadioButtonMarketStructure=OneWay" +
                        @"&oneWayOnly=1" +
                        @"&ControlGroupAvailabilitySearchInputSelectView_AvailabilitySearchInputSelectVieworiginStation1=" +
                        origin +
                        @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24TextBoxMarketOrigin1=" +
                        origin +
                        @"&ControlGroupAvailabilitySearchInputSelectView_AvailabilitySearchInputSelectViewdestinationStation1=" +
                        dest +
                        @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24TextBoxMarketDestination1=" +
                        dest +
                        @"&date_picker=" + depdate.Month + "%2F" + depdate.Day + "%2F" + depdate.Year +
                        @"&date_picker=" +
                        @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListMarketDay1=" +
                        depdate.Day +
                        @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListMarketMonth1=" +
                        depdate.Year + "-" + depdate.Month +
                        @"&date_picker=" + depdate.Month + "%2F" + depdate.Day + "%2F" + depdate.Year +
                        @"&date_picker=" +
                        @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListMarketDay2=" +
                        depdate.Day +
                        @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListMarketMonth2=" +
                        depdate.Year + "-" + depdate.Month +
                        @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListPassengerType_ADT=" +
                        1 +
                        @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListPassengerType_CHD=" +
                        0 +
                        @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListPassengerType_INFANT=" +
                        0 +
                        @"&ControlGroupAvailabilitySearchInputSelectView%24MultiCurrencyConversionViewSelectView%24DropDownListCurrency=default" +
                        @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListSearchBy=columnView" +
                        @"&ControlGroupSelectView%24AvailabilityInputSelectView%24HiddenFieldTabIndex1=4" +
                        @"&ControlGroupSelectView%24AvailabilityInputSelectView%24market1=" +
                        HttpUtility.UrlEncode(coreFareId) +
                        @"&ControlGroupSelectView%24SpecialNeedsInputSelectView%24RadioButtonWCHYESNO=RadioButtonWCHNO" +
                        @"&ControlGroupSelectView%24ButtonSubmit=Continue" +
                        @"&__VIEWSTATEGENERATOR=C8F924D9";

                    var selectRequest = new RestRequest("Select.aspx", Method.POST);
                    selectRequest.AddHeader("Referer", "https://booking2.airasia.com/Select.aspx");
                    selectRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var selectResponse = client.Execute(selectRequest);

                    if (selectResponse.ResponseUri.AbsolutePath != "/Traveler.aspx" ||
                        (selectResponse.StatusCode != HttpStatusCode.OK &&
                         selectResponse.StatusCode != HttpStatusCode.Redirect))
                        return new Currency(currencyName, Supplier.AirAsia);

                    html1 = selectResponse.Content;

                    var token =
                        html1[
                            "#CONTROLGROUP_OUTERTRAVELER_CONTROLGROUPTRAVELER_ContactInputTravelerView_CONTROLGROUP_OUTERTRAVELER_CONTROLGROUPTRAVELER_ContactInputTravelerViewHtmlInputHiddenAntiForgeryTokenField"
                            ];
                    var isitoken = token[0].GetAttribute("value");
                    Thread.Sleep(1000);

                    var getVS = selectResponse.Content;
                    var vs = (CQ) getVS;
                    var dataaneh = HttpUtility.UrlEncode(vs["[name='HiFlyerFare']"].Attr("value"));
                    var vs4 = HttpUtility.UrlEncode(vs["#viewState"].Attr("value"));
                    // [POST] Input Data
                    var hidden = depdate.ToString("yyyyMMdd");

                    for (var x = 0; x < listcities.Count; x++)
                    {
                        if (x != listcities.Count - 1)
                        {
                            hidden += listflightno.ElementAt(x) + listcities.ElementAt(x) + "/";
                        }
                        else
                        {
                            hidden += listflightno.ElementAt(x) + listcities.ElementAt(x) + currency;
                        }
                    }

                    //var hidden = string.Join("+", depdate.ToString("yyyyMMdd"), airlineCode, flightNumber,
                    //origin + dest + "IDR");
                    Thread.Sleep(1000);

                    postData =
                        @"__EVENTTARGET=CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24LinkButtonSkipToSeatMap" +
                        @"&__EVENTARGUMENT=" +
                        @"&__VIEWSTATE=" + vs4 +
                        @"&pageToken=" +
                        @"&MemberLoginTravelerView2%24TextBoxUserID=IDTDEZYCGK_ADMIN" +
                        @"&hdRememberMeEmail=IDTDEZYCGK_ADMIN" +
                        @"&MemberLoginTravelerView2%24PasswordFieldPassword=" +
                        @"&memberLogin_chk_RememberMe=on" +
                        @"&HiFlyerFare=" + dataaneh +
                        @"&isAutoSeats=false" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24CONTROLGROUP_OUTERTRAVELER_CONTROLGROUPTRAVELER_ContactInputTravelerViewHtmlInputHiddenAntiForgeryTokenField=" +
                        isitoken +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24HiddenSelectedCurrencyCode=" +
                        currency +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24DropDownListTitle=MS" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxFirstName=DWI" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxLastName=AGUSTINA" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxWorkPhone=62217248040" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxFax=" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxEmailAddress=dwi.agustina%40travelmadezy.com" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24DropDownListHomePhoneIDC=93" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxHomePhone=" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24DropDownListOtherPhoneIDC=" +
                        "62" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxOtherPhone=" +
                        "62217248040" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24EmergencyTextBoxGivenName=Given+name" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24EmergencyTextBoxSurname=Family+name%2FSurname" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24DropDownListMobileNo=93" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24EmergencyTextBoxMobileNo=" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24DropDownListRelationship=Other";
                    postData +=
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListTitle_0" +
                        "=" + "MR" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListGender_0" +
                        "=" + "1" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24TextBoxFirstName_0" +
                        "=" + "John" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24TextBoxLastName_0" +
                        "=" + "Smith" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListNationality_0" +
                        "=" + "ID" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListBirthDateDay_0" +
                        "=" + "23" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListBirthDateMonth_0" +
                        "=" + "6" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListBirthDateYear_0" +
                        "=" + "1992" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24TextBoxCustomerNumber_0" +
                        "=";

                    postData +=
                        @"&checkBoxInsuranceName=InsuranceInputControlAddOnsViewAjax%24CheckBoxInsuranceAccept" +
                        @"&checkBoxInsuranceId=CONTROLGROUP_InsuranceInputControlAddOnsViewAjax_CheckBoxInsuranceAccept" +
                        @"&checkBoxAUSNoInsuranceId=InsuranceInputControlAddOnsViewAjax_CheckBoxAUSNo" +
                        @"&declineInsuranceLinkButtonId=InsuranceInputControlAddOnsViewAjax_LinkButtonInsuranceDecline" +
                        @"&insuranceLinkCancelId=InsuranceInputControlAddOnsViewAjax_LinkButtonInsuranceDecline" +
                        @"&radioButtonNoInsuranceId=InsuranceInputControlAddOnsViewAjax_RadioButtonNoInsurance" +
                        @"&radioButtonYesInsuranceId=InsuranceInputControlAddOnsViewAjax_RadioButtonYesInsurance" +
                        @"&radioButton=on" +
                        @"&HiddenFieldPageBookingData=" + HttpUtility.UrlEncode(hidden) +
                        @"&__VIEWSTATEGENERATOR=05F9A2B0";

                    var travelerRequest = new RestRequest("Traveler.aspx", Method.POST);
                    travelerRequest.AddHeader("Referer", "https://booking2.airasia.com/Traveler.aspx");
                    travelerRequest.AddHeader("Accept",
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                    travelerRequest.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    travelerRequest.AddParameter("application/x-www-form-urlencoded", postData,
                        ParameterType.RequestBody);
                    var travelerResponse = client.Execute(travelerRequest);

                    if (travelerResponse.ResponseUri.AbsolutePath != "/UnitMap.aspx" ||
                        (travelerResponse.StatusCode != HttpStatusCode.OK &&
                         travelerResponse.StatusCode != HttpStatusCode.Redirect))
                        return new Currency(currencyName);

                    Thread.Sleep(1000);

                    // [POST] Select Seat

                    postData =
                        @"__EVENTTARGET=ControlGroupUnitMapView%24UnitMapViewControl%24LinkButtonAssignUnit" +
                        @"&__EVENTARGUMENT=" +
                        @"&__VIEWSTATE=%2FwEPDwUBMGRktapVDbdzjtpmxtfJuRZPDMU9XYk%3D" +
                        @"&pageToken=" +
                        @"&ControlGroupUnitMapView%24UnitMapViewControl%24compartmentDesignatorInput=" +
                        @"&ControlGroupUnitMapView%24UnitMapViewControl%24deckDesignatorInput=1" +
                        @"&ControlGroupUnitMapView%24UnitMapViewControl%24tripInput=0" +
                        @"&ControlGroupUnitMapView%24UnitMapViewControl%24passengerInput=0" +
                        @"&ControlGroupUnitMapView%24UnitMapViewControl%24HiddenEquipmentConfiguration_0_PassengerNumber_0=" +
                        @"&ControlGroupUnitMapView%24UnitMapViewControl%24EquipmentConfiguration_0_PassengerNumber_0=" +
                        @"&ControlGroupUnitMapView%24UnitMapViewControl%24EquipmentConfiguration_0_PassengerNumber_0_HiddenFee=NaN" +
                        @"&HiddenFieldPageBookingData=" + hidden +
                        @"&__VIEWSTATEGENERATOR=05F9A2B0";
                    var unitMapRequest = new RestRequest("UnitMap.aspx", Method.POST);
                    unitMapRequest.AddHeader("Referer", "https://booking2.airasia.com/UnitMap.aspx");
                    unitMapRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var unitMapResponse = client.Execute(unitMapRequest);

                    if (unitMapResponse.ResponseUri.AbsolutePath != "/Payment.aspx" ||
                        (unitMapResponse.StatusCode != HttpStatusCode.OK &&
                         unitMapResponse.StatusCode != HttpStatusCode.Redirect))
                        return new Currency(currencyName, Supplier.AirAsia);

                    Thread.Sleep(1000);

                    //Get Payment
                    const string url = @"/Payment.aspx";
                    var getItinRequest = new RestRequest(url, Method.GET);
                    searchRequest.AddHeader("Referer", "https://booking2.airasia.com/UnitMap.aspx");
                    var getItinResponse = client.Execute(getItinRequest);
                    var html = getItinResponse.Content;
                    CQ searchedHtml = html;
                    vs = searchedHtml["#viewState"];
                    vs4 = vs[0].GetAttribute("value");
                    // EZPay

                    postData = @"isEzPayParams=false";
                    var ezPayRequest = new RestRequest("EZPayAjax-resource.aspx", Method.POST);
                    ezPayRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
                    ezPayRequest.AddHeader("Referer", "https://booking2.airasia.com/Payment.aspx");
                    ezPayRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var ezPayResponse = client.Execute(ezPayRequest);

                    Thread.Sleep(1000);

                    postData =
                        @"__EVENTTARGET=CONTROLGROUPPAYMENTBOTTOM%24PaymentInputViewPaymentView" +
                        @"&__EVENTARGUMENT=AgencyAccount" +
                        @"&__VIEWSTATE=" + HttpUtility.UrlEncode(vs4) +
                        @"&pageToken=" +
                        @"&eventTarget=" +
                        @"&eventArgument=" +
                        @"&viewState=" + HttpUtility.UrlEncode(vs4) +
                        @"&pageToken=" +
                        @"&PriceDisplayPaymentView%24CheckBoxTermAndConditionConfirm=on" +
                        @"&CONTROLGROUPPAYMENTBOTTOM%24MultiCurrencyConversionViewPaymentView%24DropDownListCurrency=default" +
                        @"&MCCOriginCountry=" + FlightService.GetInstance().GetAirportCountryCode(origin) +
                        @"&CONTROLGROUPPAYMENTBOTTOM%24PaymentInputViewPaymentView%24HiddenFieldUpdatedMCC=" +
                        @"&HiddenFieldPageBookingData=" + HttpUtility.UrlEncode(hidden) +
                        @"&__VIEWSTATEGENERATOR=05F9A2B0";

                    var payRequest = new RestRequest("Payment.aspx", Method.POST);
                    payRequest.AddHeader("Referer", "https://booking2.airasia.com/Payment.aspx");
                    payRequest.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    payRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var payResponse = client.Execute(payRequest);
                    var htmlPayResp = (CQ) payResponse.Content;

                    var tableExchangeRate = htmlPayResp["#agCreditTable"];
                    var rowsExcRate = tableExchangeRate[0].ChildElements.ToList()[0].ChildElements.ToList();
                    var creditAvailableinCurr =
                        rowsExcRate[0].ChildElements.ToList()[1].InnerText.Replace(" ", "").Replace("\n", "").
                            Replace("&nbsp;", " ").Split(' ');
                    var creditAvailableinIdr = rowsExcRate[1].ChildElements.ToList()[1].InnerText.
                        Replace(" ", "").Replace("\n", "").Replace("&nbsp;", " ").Split(' ');

                    decimal credinIdr;
                    decimal credinCurr;

                    var b = decimal.TryParse(creditAvailableinIdr[1], out credinIdr);
                    var c = decimal.TryParse(creditAvailableinCurr[1], out credinCurr);

                    
                    Decimal exchangeRate;
                    if (b && c)
                    {
                        exchangeRate = credinIdr/credinCurr;
                    }
                    else
                    {
                        exchangeRate = 0;
                    }

                    Currency.SetRate(currencyName, exchangeRate, Supplier.AirAsia);
                    Console.WriteLine("The Rate for " + currencyName + " is: " + exchangeRate);
                    var currs = new Currency(currencyName, exchangeRate) {Supplier = Supplier.AirAsia};
                    return currs;
                }
                catch //(Exception e)
                {
                    return new Currency(currencyName, Supplier.AirAsia);

                }
            }
        }       
     }
 }

