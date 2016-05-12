﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Web;
using RestSharp;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
    {
        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo)
        {
            return Client.BookFlight(bookInfo);
        }

        private partial class AirAsiaClientHandler
        {
            internal BookFlightResult BookFlight(FlightBookingInfo bookInfo)
            {
                RevalidateConditions conditions = new RevalidateConditions
                {
                    Itinerary = bookInfo.Itinerary
                };
                //conditions.Itinerary = bookInfo.Itinerary;
                RevalidateFareResult revalidateResult = RevalidateFare(conditions);
                if (revalidateResult.IsItineraryChanged || revalidateResult.IsPriceChanged || (!revalidateResult.IsValid))
                {
                    return new BookFlightResult
                    {
                        IsValid = revalidateResult.IsValid,
                        ErrorMessages = revalidateResult.ErrorMessages,
                        Errors = revalidateResult.Errors,
                        IsItineraryChanged = revalidateResult.IsItineraryChanged,
                        IsPriceChanged = revalidateResult.IsPriceChanged,
                        IsSuccess = false,
                        NewItinerary = revalidateResult.NewItinerary,
                        NewPrice = revalidateResult.NewPrice,
                        Status = null
                    };
                }
                bookInfo.Itinerary = revalidateResult.NewItinerary;
                var client = CreateAgentClient();
                string origin, dest, coreFareId;
                DateTime date;
                int adultCount, childCount, infantCount;
                CabinClass cabinClass;
                decimal price;

                try
                {
                    var splittedFareId = bookInfo.Itinerary.FareId.Split('.').ToList();
                    origin = splittedFareId[0];
                    dest = splittedFareId[1];
                    date = new DateTime(int.Parse(splittedFareId[4]), int.Parse(splittedFareId[3]),
                        int.Parse(splittedFareId[2]));
                    adultCount = int.Parse(splittedFareId[5]);
                    childCount = int.Parse(splittedFareId[6]);
                    infantCount = int.Parse(splittedFareId[7]);
                    cabinClass = FlightService.ParseCabinClass(splittedFareId[8]);
                    price = decimal.Parse(splittedFareId[9]);
                    coreFareId = splittedFareId[10];
                }
                catch
                {
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.FareIdNoLongerValid }
                    };
                }

                if (!Login(client))
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { "Can't Login!" }
                    };

                var flightPart = coreFareId.Split('|')[1];
                var splitted = flightPart.Split('~');
                var airlineCode = splitted[0];
                var flightNumber = splitted[1];
                var hidden = string.Join("+", date.ToString("yyyyMMdd"), airlineCode, flightNumber,
                    origin + dest + "IDR");

                // [POST] Search Flight

                var date2 = date.AddDays(1);
                var postData =
                    @"__EVENTTARGET=" +
                    @"&__EVENTARGUMENT=" +
                    @"&__VIEWSTATE=%2FwEPDwUBMGRktapVDbdzjtpmxtfJuRZPDMU9XYk%3D" +
                    @"&pageToken=" +
                    @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24RadioButtonMarketStructure=OneWay" +
                    @"&oneWayOnly=1" +
                    @"&ControlGroupSearchView_AvailabilitySearchInputSearchVieworiginStation1=" + origin +
                    @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24TextBoxMarketOrigin1=" + origin +
                    @"&ControlGroupSearchView_AvailabilitySearchInputSearchViewdestinationStation1=" + dest +
                    @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24TextBoxMarketDestination1=" + dest +
                    @"&ControlGroupSearchView%24MultiCurrencyConversionViewSearchView%24DropDownListCurrency=default" +
                    @"&date_picker=" + date.Month + "%2F" + date.Day + "%2F" + date.Year +
                    @"&date_picker=" +
                    @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListMarketDay1=" + date.Day +
                    @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListMarketMonth1="+date.Year+"-"+date.Month +
                    @"&date_picker=" + date2.Month + "%2F" + date2.Day + "%2F" + date2.Year +
                    @"&date_picker=" +
                    @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListMarketDay2="+date2.Day +
                    @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListMarketMonth2=" + date2.Year + "-" + date2.Month +
                    @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListPassengerType_ADT="+adultCount +
                    @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListPassengerType_CHD="+childCount +
                    @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListPassengerType_INFANT="+infantCount +
                    @"&ControlGroupSearchView%24AvailabilitySearchInputSearchView%24DropDownListSearchBy=columnView" +
                    @"&ControlGroupSearchView%24ButtonSubmit=Search" +
                    @"&__VIEWSTATEGENERATOR=05F9A2B0";

                var searchRequest = new RestRequest("Search.aspx", Method.POST);
                searchRequest.AddHeader("Referer", "https://booking2.airasia.com/Search.aspx");
                searchRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var searchResponse = client.Execute(searchRequest);

                if (searchResponse.ResponseUri.AbsolutePath != "/Select.aspx" || (searchResponse.StatusCode != HttpStatusCode.OK && searchResponse.StatusCode != HttpStatusCode.Redirect))
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.FareIdNoLongerValid }
                    };

                Thread.Sleep(1000);

                // [POST] Select Flight

                postData =
                    @"__EVENTTARGET=" +
                    @"&__EVENTARGUMENT=" +
                    @"&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFP0NvbnRyb2xHcm91cFNlbGVjdFZpZXckU3BlY2lhbE5lZWRzSW5wdXRTZWxlY3RWaWV3JENoZWNrQm94U1NSc2KF%2B3FBQndP4mQD4nrPT4PNXNaR" +
                    @"&pageToken=" +
                    @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24RadioButtonMarketStructure=OneWay" +
                    @"&oneWayOnly=1" +
                    @"&ControlGroupAvailabilitySearchInputSelectView_AvailabilitySearchInputSelectVieworiginStation1=" + origin +
                    @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24TextBoxMarketOrigin1=" + origin +
                    @"&ControlGroupAvailabilitySearchInputSelectView_AvailabilitySearchInputSelectViewdestinationStation1=" + dest +
                    @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24TextBoxMarketDestination1=" + dest +
                    @"&date_picker=" + date.Month+ "%2F" + date.Day+ "%2F" + date.Year +
                    @"&date_picker=" +
                    @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListMarketDay1=" + date.Day +
                    @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListMarketMonth1="+date.Year+"-"+date.Month +
                    @"&date_picker=" + date2.Month + "%2F" + date2.Day + "%2F" + date2.Year +
                    @"&date_picker=" +
                    @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListMarketDay2=" + date2.Day +
                    @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListMarketMonth2=" + date2.Year + "-" + date2.Month +
                    @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListPassengerType_ADT="+adultCount +
                    @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListPassengerType_CHD="+childCount +
                    @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListPassengerType_INFANT="+infantCount +
                    @"&ControlGroupAvailabilitySearchInputSelectView%24MultiCurrencyConversionViewSelectView%24DropDownListCurrency=default" +
                    @"&ControlGroupAvailabilitySearchInputSelectView%24AvailabilitySearchInputSelectView%24DropDownListSearchBy=columnView" +
                    @"&ControlGroupSelectView%24AvailabilityInputSelectView%24HiddenFieldTabIndex1=4" +
                    @"&ControlGroupSelectView%24AvailabilityInputSelectView%24market1=" + HttpUtility.UrlEncode(coreFareId) +
                    @"&ControlGroupSelectView%24SpecialNeedsInputSelectView%24RadioButtonWCHYESNO=RadioButtonWCHNO" +
                    @"&ControlGroupSelectView%24ButtonSubmit=Continue" +
                    @"&__VIEWSTATEGENERATOR=C8F924D9";

                var selectRequest = new RestRequest("Select.aspx", Method.POST);
                selectRequest.AddHeader("Referer", "https://booking2.airasia.com/Select.aspx");
                selectRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var selectResponse = client.Execute(selectRequest);

                if (selectResponse.ResponseUri.AbsolutePath != "/Traveler.aspx" || (selectResponse.StatusCode != HttpStatusCode.OK && selectResponse.StatusCode != HttpStatusCode.Redirect))
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.FareIdNoLongerValid }
                    };

                Thread.Sleep(1000);

                var getTravelerRequest = new RestRequest("Traveler.aspx", Method.GET);
                getTravelerRequest.AddHeader("Referer", "https://booking2.airasia.com/Select.aspx");
                var getTravelerResponse = client.Execute(getTravelerRequest);
                var getVS = getTravelerResponse.Content;
                var vs = (CQ)getVS;
                var dataaneh = HttpUtility.UrlEncode(vs["[name='HiFlyerFare']"].Attr("value"));
                var vs4 = HttpUtility.UrlEncode(vs["#viewState"].Attr("value"));
                // [POST] Input Data

                postData =
                    @"__EVENTTARGET=CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24LinkButtonSkipToSeatMap" +
                    @"&__EVENTARGUMENT=" +
                    @"&__VIEWSTATE=" + vs4 +
                    @"&pageToken=" +
                    @"&MemberLoginTravelerView2%24TextBoxUserID=" +
                    @"&hdRememberMeEmail=" +
                    @"&MemberLoginTravelerView2%24PasswordFieldPassword=" +
                    @"&memberLogin_chk_RememberMe=on" +
                    @"&HiFlyerFare=" + dataaneh +
                    @"&isAutoSeats=false" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24HiddenSelectedCurrencyCode=IDR" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24DropDownListTitle=MS" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxFirstName=DWI" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxLastName=AGUSTINA" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxWorkPhone=62217248040" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxFax=" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxEmailAddress=dwi.agustina%40travelmadezy.com" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24DropDownListHomePhoneIDC=93" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxHomePhone=" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24DropDownListOtherPhoneIDC=" + bookInfo.ContactData.CountryCode +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24TextBoxOtherPhone=" + bookInfo.ContactData.Phone +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24EmergencyTextBoxGivenName=Given+name" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24EmergencyTextBoxSurname=Family+name%2FSurname" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24DropDownListMobileNo=93" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24EmergencyTextBoxMobileNo=" +
                    @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24ContactInputTravelerView%24DropDownListRelationship=Other";
                int i = 0;
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Adult))
                {
                    var title = passenger.Title == Title.Mister ? "MR" : "MS";
                    postData +=
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListTitle_" + i + "=" + title+
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListGender_" + i + "=" + (int) passenger.Gender +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24TextBoxFirstName_" + i + "=" + passenger.FirstName +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24TextBoxLastName_" + i + "=" + passenger.LastName +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListNationality_" + i + "=" + passenger.PassportCountry +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListBirthDateDay_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Day +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListBirthDateMonth_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Month +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListBirthDateYear_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Year +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24TextBoxCustomerNumber_" + i + "=";
                    i++;
                }
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Child))
                {
                    postData +=
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListTitle_" + i + "=CHD" +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListGender_" + i + "=" + (int)passenger.Gender +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24TextBoxFirstName_" + i + "=" + passenger.FirstName +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24TextBoxLastName_" + i + "=" + passenger.LastName +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListNationality_" + i + "=" + passenger.PassportCountry +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListBirthDateDay_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Day +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListBirthDateMonth_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Month +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListBirthDateYear_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Year +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24TextBoxCustomerNumber_" + i + "=";
                    i++;
                }
                i = 0;
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Infant))
                {
                    postData +=
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListAssign_" + i + "_" + i + "=" + i +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListGender_" + i + "_" + i + "=" + (int)passenger.Gender +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24TextBoxFirstName_" + i + "_" + i + "=" + passenger.FirstName +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24TextBoxLastName_" + i + "_" + i + "=" + passenger.LastName +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListNationality_" + i + "_" + i + "=" + passenger.PassportCountry +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListBirthDateDay_" + i + "_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Day +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListBirthDateMonth_" + i + "_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Month +
                        @"&CONTROLGROUP_OUTERTRAVELER%24CONTROLGROUPTRAVELER%24PassengerInputTravelerView%24DropDownListBirthDateYear_" + i + "_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Year;
                    i++;
                }
                postData +=
                    @"&checkBoxInsuranceName=InsuranceInputControlAddOnsViewAjax%24CheckBoxInsuranceAccept" +
                    @"&checkBoxInsuranceId=CONTROLGROUP_InsuranceInputControlAddOnsViewAjax_CheckBoxInsuranceAccept" +
                    @"&checkBoxAUSNoInsuranceId=InsuranceInputControlAddOnsViewAjax_CheckBoxAUSNo" +
                    @"&declineInsuranceLinkButtonId=InsuranceInputControlAddOnsViewAjax_LinkButtonInsuranceDecline" +
                    @"&insuranceLinkCancelId=InsuranceInputControlAddOnsViewAjax_LinkButtonInsuranceDecline" +
                    @"&radioButtonNoInsuranceId=InsuranceInputControlAddOnsViewAjax_RadioButtonNoInsurance" +
                    @"&radioButtonYesInsuranceId=InsuranceInputControlAddOnsViewAjax_RadioButtonYesInsurance" +
                    @"&radioButton=on" +
                    @"&HiddenFieldPageBookingData=" + hidden +
                    @"&__VIEWSTATEGENERATOR=05F9A2B0";

                var travelerRequest = new RestRequest("Traveler.aspx", Method.POST);
                travelerRequest.AddHeader("Referer", "https://booking2.airasia.com/Traveler.aspx");
                travelerRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var travelerResponse = client.Execute(travelerRequest);

                //GAGAL PAS MASUK KE SINI
                if (travelerResponse.ResponseUri.AbsolutePath != "/UnitMap.aspx" || (travelerResponse.StatusCode != HttpStatusCode.OK && travelerResponse.StatusCode != HttpStatusCode.Redirect))

                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.InvalidInputData }
                    };
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

                if (unitMapResponse.ResponseUri.AbsolutePath != "/Payment.aspx" || (unitMapResponse.StatusCode != HttpStatusCode.OK && unitMapResponse.StatusCode != HttpStatusCode.Redirect))
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                    };

                Thread.Sleep(1000);

                //Get Payment
                var url = @"/Payment.aspx";
                var getItinRequest = new RestRequest(url, Method.GET);
                searchRequest.AddHeader("Referer", "https://booking2.airasia.com/UnitMap.aspx");
                var getItinResponse = client.Execute(getItinRequest);
                var html = getItinResponse.Content;
                CQ searchedHtml = (CQ)html;
                try
                {
                    var newPrice = searchedHtml["#overallTotal"].First().Text().Replace(",", "");
                    var jlhSegment = searchedHtml[".row2.mtop-row"].ToList();
                    var length = jlhSegment.Count;
                    var airport = searchedHtml[".row2.mtop-row>div"].ToArray();
                    var flightDet = searchedHtml[".right-text.bold.grey1"].ToArray();
                    var flightArr = searchedHtml[".right-text.grey1"].Not(".bold").ToArray();
                    var flightDept = searchedHtml[".left-text.grey1"].ToArray();

                    var segments = new List<FlightSegment>();
                    var format = "dd MMM yyyy HHmm";
                    var format2 = "dd MMM yyyy";
                    CultureInfo provider = new CultureInfo("en-US");
                    var dict = DictionaryService.GetInstance();
                    for (int index = 0; index < jlhSegment.Count; index++)
                    {
                        var splitflightDetail = flightDet[index].InnerHTML.Split(' ');//(new string[] { "  " }, StringSplitOptions.None);
                        var splitDept = flightDept[index].InnerHTML.Trim().Split(',');
                        var deptTime = DateTime.ParseExact(splitDept[1].Trim() + " " + splitDept[0].Trim(), format, provider);
                        var splitArr = flightArr[index].InnerHTML.Trim().Split(',');
                        var arrTime = DateTime.ParseExact(splitArr[1].Trim() + " " + splitArr[0].Trim(), format, provider);
                        var departureTime = deptTime.AddHours(-(dict.GetAirportTimeZone(airport[index * 3].InnerHTML.Trim())));
                        var arrivalTime = arrTime.AddHours(-(dict.GetAirportTimeZone(airport[(index * 3) + 2].InnerHTML.Trim())));

                        segments.Add(new FlightSegment
                        {
                            AirlineCode = splitflightDetail[0],
                            FlightNumber = splitflightDetail[1],
                            CabinClass = cabinClass,
                            Rbd = bookInfo.Itinerary.Trips[0].Segments[index].Rbd,
                            DepartureAirport = airport[index * 3].InnerHTML.Trim(),
                            DepartureTime = DateTime.SpecifyKind(deptTime, DateTimeKind.Utc),
                            ArrivalAirport = airport[(index * 3) + 2].InnerHTML.Trim(),
                            ArrivalTime = DateTime.SpecifyKind(arrTime, DateTimeKind.Utc),
                            OperatingAirlineCode = splitflightDetail[0],
                            StopQuantity = 0,
                            Duration = arrivalTime - departureTime
                        });
                    }
                    var depDate = flightDept[0].InnerHTML.Trim().Split(',');
                    var itin = new FlightItinerary
                    {
                        AdultCount = adultCount,
                        ChildCount = childCount,
                        InfantCount = infantCount,
                        CanHold = true,
                        FareType = FareType.Published,
                        RequireBirthDate = true,
                        RequirePassport = RequirePassport(segments),
                        RequireSameCheckIn = false,
                        RequireNationality = true,
                        RequestedCabinClass = CabinClass.Economy,
                        TripType = TripType.OneWay,
                        Supplier = Supplier.AirAsia,
                        SupplierCurrency = "IDR",
                        SupplierPrice = decimal.Parse(newPrice),
                        FareId = bookInfo.Itinerary.FareId,
                        Trips = new List<FlightTrip>
                            {
                                new FlightTrip
                                {
                                    OriginAirport = airport[0].InnerHTML,
                                    DestinationAirport = airport[airport.Length-1].InnerHTML,
                                    DepartureDate = DateTime.SpecifyKind(DateTime.ParseExact(depDate[1].Trim(), format2, provider),DateTimeKind.Utc),
                                    Segments = segments
                                }
                            }
                    };
                    var isItinChanged = !itin.Identical(bookInfo.Itinerary);
                    if (isItinChanged) 
                    {
                        if(newPrice!=""&& decimal.Parse(newPrice)!=bookInfo.Itinerary.SupplierPrice)
                        {
                            itin.FareId = itin.FareId.Replace(bookInfo.Itinerary.SupplierPrice.ToString("0"), newPrice);
                        }
                        return new BookFlightResult
                        {
                            IsValid = true,
                            IsItineraryChanged = false,
                            IsPriceChanged = bookInfo.Itinerary.SupplierPrice != decimal.Parse(newPrice),
                            IsSuccess = false,
                            ErrorMessages = new List<string> { "Itinerary is changed!" },
                            NewItinerary = itin,
                            NewPrice = decimal.Parse(newPrice),
                            Status = null
                        };
                    }
                    else if (newPrice != "" && decimal.Parse(newPrice) != bookInfo.Itinerary.SupplierPrice) 
                    {
                        itin.FareId = itin.FareId.Replace(bookInfo.Itinerary.SupplierPrice.ToString("0"), newPrice);
                        return new BookFlightResult
                        {
                            IsValid = true,
                            IsItineraryChanged = false,
                            IsPriceChanged = bookInfo.Itinerary.SupplierPrice != decimal.Parse(newPrice),
                            IsSuccess = false,
                            ErrorMessages = new List<string> { "Itinerary is changed!" },
                            NewItinerary = itin,
                            NewPrice = decimal.Parse(newPrice),
                            Status = null
                        };
                    }
                }
                catch 
                {
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { "Web Layout Changed!, Error while revalidate in last step" }
                    };
                }

                // EZPay

                postData = @"isEzPayParams=false";
                var ezPayRequest = new RestRequest("EZPayAjax-resource.aspx", Method.POST);
                ezPayRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
                ezPayRequest.AddHeader("Referer", "https://booking2.airasia.com/Payment.aspx");
                ezPayRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var ezPayResponse = client.Execute(ezPayRequest);

                Thread.Sleep(1000);

                // SELECT HOLD (PAYMENT)

                postData =
                    @"__EVENTTARGET=CONTROLGROUPPAYMENTBOTTOM%24PaymentInputViewPaymentView" +
                    @"&__EVENTARGUMENT=HOLD" +
                    @"&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFg0FUUNPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kUGF5bWVudElucHV0Vmlld1BheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0tMSUtCQ0EtS0xJS0JDQQVRQ09OVFJPTEdST1VQUEFZTUVOVEJPVFRPTSRQYXltZW50SW5wdXRWaWV3UGF5bWVudFZpZXckUmFkaW9CdXR0b25fS0xJS0JDQS1LTElLQkNBBVdDT05UUk9MR1JPVVBQQVlNRU5UQk9UVE9NJFBheW1lbnRJbnB1dFZpZXdQYXltZW50VmlldyRSYWRpb0J1dHRvbl9DSU1CX05JQUdBLUNJTUJfTklBR0EFV0NPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kUGF5bWVudElucHV0Vmlld1BheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0NJTUJfTklBR0EtQ0lNQl9OSUFHQQVRQ09OVFJPTEdST1VQUEFZTUVOVEJPVFRPTSRQYXltZW50SW5wdXRWaWV3UGF5bWVudFZpZXckUmFkaW9CdXR0b25fS0xJS1BBWS1LTElLUEFZBVFDT05UUk9MR1JPVVBQQVlNRU5UQk9UVE9NJFBheW1lbnRJbnB1dFZpZXdQYXltZW50VmlldyRSYWRpb0J1dHRvbl9LTElLUEFZLUtMSUtQQVkFTENPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0JDQS1CQ0EFTENPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0JDQS1CQ0EFWkNPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0NJTUJfTklBR0EtQ0lNQl9OSUFHQQVaQ09OVFJPTEdST1VQUEFZTUVOVEJPVFRPTSRDb250YWN0QmlsbGluZ0lucHV0UGF5bWVudFZpZXckUmFkaW9CdXR0b25fQ0lNQl9OSUFHQS1DSU1CX05JQUdBBVRDT05UUk9MR1JPVVBQQVlNRU5UQk9UVE9NJENvbnRhY3RCaWxsaW5nSW5wdXRQYXltZW50VmlldyRSYWRpb0J1dHRvbl9LbGlrUGF5LUtsaWtQYXkFVENPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0tsaWtQYXktS2xpa1BheQVIQ09OVFJPTEdST1VQUEFZTUVOVEZMSUdIVEFORFBSSUNFJEZsaWdodERpc3BsYXlQYXltZW50Vmlld0NHJFN1cnZleUJveCQwy1xf9dVxDKosDP7li41hIuX4ZxQ%3D" +
                    @"&pageToken=" +
                    @"&eventTarget=" +
                    @"&eventArgument=" +
                    @"&viewState=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFg0FUUNPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kUGF5bWVudElucHV0Vmlld1BheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0tMSUtCQ0EtS0xJS0JDQQVRQ09OVFJPTEdST1VQUEFZTUVOVEJPVFRPTSRQYXltZW50SW5wdXRWaWV3UGF5bWVudFZpZXckUmFkaW9CdXR0b25fS0xJS0JDQS1LTElLQkNBBVdDT05UUk9MR1JPVVBQQVlNRU5UQk9UVE9NJFBheW1lbnRJbnB1dFZpZXdQYXltZW50VmlldyRSYWRpb0J1dHRvbl9DSU1CX05JQUdBLUNJTUJfTklBR0EFV0NPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kUGF5bWVudElucHV0Vmlld1BheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0NJTUJfTklBR0EtQ0lNQl9OSUFHQQVRQ09OVFJPTEdST1VQUEFZTUVOVEJPVFRPTSRQYXltZW50SW5wdXRWaWV3UGF5bWVudFZpZXckUmFkaW9CdXR0b25fS0xJS1BBWS1LTElLUEFZBVFDT05UUk9MR1JPVVBQQVlNRU5UQk9UVE9NJFBheW1lbnRJbnB1dFZpZXdQYXltZW50VmlldyRSYWRpb0J1dHRvbl9LTElLUEFZLUtMSUtQQVkFTENPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0JDQS1CQ0EFTENPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0JDQS1CQ0EFWkNPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0NJTUJfTklBR0EtQ0lNQl9OSUFHQQVaQ09OVFJPTEdST1VQUEFZTUVOVEJPVFRPTSRDb250YWN0QmlsbGluZ0lucHV0UGF5bWVudFZpZXckUmFkaW9CdXR0b25fQ0lNQl9OSUFHQS1DSU1CX05JQUdBBVRDT05UUk9MR1JPVVBQQVlNRU5UQk9UVE9NJENvbnRhY3RCaWxsaW5nSW5wdXRQYXltZW50VmlldyRSYWRpb0J1dHRvbl9LbGlrUGF5LUtsaWtQYXkFVENPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0tsaWtQYXktS2xpa1BheQVIQ09OVFJPTEdST1VQUEFZTUVOVEZMSUdIVEFORFBSSUNFJEZsaWdodERpc3BsYXlQYXltZW50Vmlld0NHJFN1cnZleUJveCQwy1xf9dVxDKosDP7li41hIuX4ZxQ%3D" +
                    @"&pageToken=" +
                    @"&PriceDisplayPaymentView%24CheckBoxTermAndConditionConfirm=on" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24MultiCurrencyConversionViewPaymentView%24DropDownListCurrency=default" +
                    @"&MCCOriginCountry=ID" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24PaymentInputViewPaymentView%24HiddenFieldUpdatedMCC=" +
                    @"&HiddenFieldPageBookingData=" + hidden +
                    @"&__VIEWSTATEGENERATOR=05F9A2B0";
                var paymentRequest = new RestRequest("Payment.aspx", Method.POST);
                paymentRequest.AddHeader("Referer", "https://booking2.airasia.com/Payment.aspx");
                paymentRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var paymentResponse = client.Execute(paymentRequest);

                Thread.Sleep(1000);

                var whileCounter = 0;
                while (paymentResponse.StatusCode == HttpStatusCode.Forbidden && whileCounter < 10)
                {
                    paymentResponse = client.Execute(paymentRequest);
                    whileCounter++;

                    Thread.Sleep(1000);
                    //return new BookFlightResult
                    //{
                    //    IsSuccess = false,
                    //    Status = new BookingStatusInfo
                    //    {
                    //        BookingStatus = BookingStatus.Failed
                    //    },
                    //    Errors = new List<FlightError> { FlightError.TechnicalError },
                    //    ErrorMessages = new List<string>{"Forbidden!"}
                    //};
                }

                if (paymentResponse.ResponseUri.AbsolutePath != "/Payment.aspx" || (paymentResponse.StatusCode != HttpStatusCode.OK && paymentResponse.StatusCode != HttpStatusCode.Redirect))
                    return new BookFlightResult { 
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                    };

                Thread.Sleep(1000);

                // EZPay

                ezPayResponse = client.Execute(ezPayRequest);

                Thread.Sleep(1000);

                // [POST] Select Hold

                postData =
                    @"__EVENTTARGET=" +
                    @"&__EVENTARGUMENT=" +
                    @"&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFg0FUUNPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kUGF5bWVudElucHV0Vmlld1BheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0tMSUtCQ0EtS0xJS0JDQQVRQ09OVFJPTEdST1VQUEFZTUVOVEJPVFRPTSRQYXltZW50SW5wdXRWaWV3UGF5bWVudFZpZXckUmFkaW9CdXR0b25fS0xJS0JDQS1LTElLQkNBBVdDT05UUk9MR1JPVVBQQVlNRU5UQk9UVE9NJFBheW1lbnRJbnB1dFZpZXdQYXltZW50VmlldyRSYWRpb0J1dHRvbl9DSU1CX05JQUdBLUNJTUJfTklBR0EFV0NPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kUGF5bWVudElucHV0Vmlld1BheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0NJTUJfTklBR0EtQ0lNQl9OSUFHQQVRQ09OVFJPTEdST1VQUEFZTUVOVEJPVFRPTSRQYXltZW50SW5wdXRWaWV3UGF5bWVudFZpZXckUmFkaW9CdXR0b25fS0xJS1BBWS1LTElLUEFZBVFDT05UUk9MR1JPVVBQQVlNRU5UQk9UVE9NJFBheW1lbnRJbnB1dFZpZXdQYXltZW50VmlldyRSYWRpb0J1dHRvbl9LTElLUEFZLUtMSUtQQVkFTENPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0JDQS1CQ0EFTENPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0JDQS1CQ0EFWkNPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0NJTUJfTklBR0EtQ0lNQl9OSUFHQQVaQ09OVFJPTEdST1VQUEFZTUVOVEJPVFRPTSRDb250YWN0QmlsbGluZ0lucHV0UGF5bWVudFZpZXckUmFkaW9CdXR0b25fQ0lNQl9OSUFHQS1DSU1CX05JQUdBBVRDT05UUk9MR1JPVVBQQVlNRU5UQk9UVE9NJENvbnRhY3RCaWxsaW5nSW5wdXRQYXltZW50VmlldyRSYWRpb0J1dHRvbl9LbGlrUGF5LUtsaWtQYXkFVENPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0tsaWtQYXktS2xpa1BheQVIQ09OVFJPTEdST1VQUEFZTUVOVEZMSUdIVEFORFBSSUNFJEZsaWdodERpc3BsYXlQYXltZW50Vmlld0NHJFN1cnZleUJveCQwy1xf9dVxDKosDP7li41hIuX4ZxQ%3D" +
                    @"&pageToken=" +
                    @"&eventTarget=" +
                    @"&eventArgument=" +
                    @"&viewState=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFg0FUUNPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kUGF5bWVudElucHV0Vmlld1BheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0tMSUtCQ0EtS0xJS0JDQQVRQ09OVFJPTEdST1VQUEFZTUVOVEJPVFRPTSRQYXltZW50SW5wdXRWaWV3UGF5bWVudFZpZXckUmFkaW9CdXR0b25fS0xJS0JDQS1LTElLQkNBBVdDT05UUk9MR1JPVVBQQVlNRU5UQk9UVE9NJFBheW1lbnRJbnB1dFZpZXdQYXltZW50VmlldyRSYWRpb0J1dHRvbl9DSU1CX05JQUdBLUNJTUJfTklBR0EFV0NPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kUGF5bWVudElucHV0Vmlld1BheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0NJTUJfTklBR0EtQ0lNQl9OSUFHQQVRQ09OVFJPTEdST1VQUEFZTUVOVEJPVFRPTSRQYXltZW50SW5wdXRWaWV3UGF5bWVudFZpZXckUmFkaW9CdXR0b25fS0xJS1BBWS1LTElLUEFZBVFDT05UUk9MR1JPVVBQQVlNRU5UQk9UVE9NJFBheW1lbnRJbnB1dFZpZXdQYXltZW50VmlldyRSYWRpb0J1dHRvbl9LTElLUEFZLUtMSUtQQVkFTENPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0JDQS1CQ0EFTENPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0JDQS1CQ0EFWkNPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0NJTUJfTklBR0EtQ0lNQl9OSUFHQQVaQ09OVFJPTEdST1VQUEFZTUVOVEJPVFRPTSRDb250YWN0QmlsbGluZ0lucHV0UGF5bWVudFZpZXckUmFkaW9CdXR0b25fQ0lNQl9OSUFHQS1DSU1CX05JQUdBBVRDT05UUk9MR1JPVVBQQVlNRU5UQk9UVE9NJENvbnRhY3RCaWxsaW5nSW5wdXRQYXltZW50VmlldyRSYWRpb0J1dHRvbl9LbGlrUGF5LUtsaWtQYXkFVENPTlRST0xHUk9VUFBBWU1FTlRCT1RUT00kQ29udGFjdEJpbGxpbmdJbnB1dFBheW1lbnRWaWV3JFJhZGlvQnV0dG9uX0tsaWtQYXktS2xpa1BheQVIQ09OVFJPTEdST1VQUEFZTUVOVEZMSUdIVEFORFBSSUNFJEZsaWdodERpc3BsYXlQYXltZW50Vmlld0NHJFN1cnZleUJveCQwy1xf9dVxDKosDP7li41hIuX4ZxQ%3D" +
                    @"&pageToken=" +
                    @"&PriceDisplayPaymentView%24CheckBoxTermAndConditionConfirm=on" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24MultiCurrencyConversionViewPaymentView%24DropDownListCurrency=default" +
                    @"&MCCOriginCountry=ID" +
                    @"&CONTROLGROUPPAYMENTBOTTOM%24ButtonSubmit=Submit+payment" +
                    @"&HiddenFieldPageBookingData=" + hidden +
                    @"&__VIEWSTATEGENERATOR=05F9A2B0";
                var paymentRequest2 = new RestRequest("Payment.aspx", Method.POST);
                paymentRequest2.AddHeader("Referer", "https://booking2.airasia.com/Payment.aspx");
                paymentRequest2.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var paymentResponse2 = client.Execute(paymentRequest2);
                
                //Ambil disini buat harga berubah, dari paymentResponse2

                Thread.Sleep(1000);

                whileCounter = 0;
                while (paymentResponse.StatusCode == HttpStatusCode.Forbidden && whileCounter < 10)
                {
                    paymentResponse2 = client.Execute(paymentRequest2);
                    whileCounter++;
                    Thread.Sleep(1000);
                }

                if (paymentResponse2.ResponseUri.AbsolutePath != "/Wait.aspx" || (paymentResponse2.StatusCode != HttpStatusCode.OK && paymentResponse2.StatusCode != HttpStatusCode.Redirect))
                    return new BookFlightResult { 
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                    };
                Thread.Sleep(1000);

                // [GET] Wait for Book

                string itinHtml = "";
                var sw = Stopwatch.StartNew();
                var retryLimit = new TimeSpan(0, 0, 30);
                var retryInterval = new TimeSpan(0, 0, 2);
                var waitRequest = new RestRequest("Wait.aspx", Method.GET);
                waitRequest.AddHeader("Referer", "https://booking2.airasia.com/Wait.aspx");
                IRestResponse waitResponse = client.Execute(waitRequest);
                while (waitResponse.ResponseUri.AbsolutePath != "/Itinerary.aspx" && sw.Elapsed <= retryLimit && (waitResponse.StatusCode == HttpStatusCode.OK || waitResponse.StatusCode == HttpStatusCode.Redirect))
                {
                    waitResponse = client.Execute(waitRequest);
                    if (waitResponse.ResponseUri.AbsolutePath != "/Itinerary.aspx" || (waitResponse.StatusCode != HttpStatusCode.OK && waitResponse.StatusCode != HttpStatusCode.Redirect))
                        Thread.Sleep(retryInterval);
                }

                var waitItin = new RestRequest("Itinerary.aspx", Method.GET);
                string temp = "";
                if (waitResponse.ResponseUri.AbsolutePath != "/Itinerary.aspx" || (waitResponse.StatusCode != HttpStatusCode.OK && waitResponse.StatusCode != HttpStatusCode.Redirect))
                {
                    waitItin.AddHeader("Referer", "https://booking2.airasia.com/Wait.aspx");
                    IRestResponse itinResponse = client.Execute(waitItin);
                    var itinRes = itinResponse.Content;
                    var cqitin = (CQ)itinRes;
                    var bookingId = cqitin["#OptionalHeaderContent_lblBookingNumber"].Text();
                    if (bookingId == "" || bookingId == null)
                    {
                        return new BookFlightResult
                        {
                            IsSuccess = false,
                            Status = new BookingStatusInfo
                            {
                                BookingStatus = BookingStatus.Failed
                            },
                            Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                        };
                    }
                    else 
                    {
                        temp = itinRes;
                    }

                    
                }
                    

                try
                {
                    itinHtml = temp;
                    var cqHtml = (CQ) itinHtml;
                    var bookingId = cqHtml["#OptionalHeaderContent_lblBookingNumber"].Text();
                    var timeLimitTexts = cqHtml["#mainContent > p"].Text().Split('\n', ',');
                    var timeLimitDateText = timeLimitTexts[2].Trim(' ', '\n');
                    var timeLimitTimeText = timeLimitTexts[4].Trim(' ', '\n');
                    var timeLimitDate = DateTime.Parse(timeLimitDateText, CultureInfo.CreateSpecificCulture("en-US"));
                    var timeLimitTime = TimeSpan.Parse(timeLimitTimeText, CultureInfo.InvariantCulture);
                    var timeLimit = timeLimitDate.Add(timeLimitTime);
                    var timeLimitFinal = timeLimit.AddHours(-7);
                    return new BookFlightResult
                    {
                        IsSuccess = true,
                        Status = new BookingStatusInfo
                        {
                            BookingId = bookingId,
                            BookingStatus = BookingStatus.Booked,
                            TimeLimit = DateTime.SpecifyKind(timeLimitFinal,DateTimeKind.Utc)
                        }
                    };
                }
                catch
                {
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { "Web Layout Changed!" }
                    };
                }
            }
        }
    }
}
