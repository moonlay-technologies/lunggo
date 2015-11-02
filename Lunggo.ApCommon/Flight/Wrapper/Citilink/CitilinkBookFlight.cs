using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Web;

namespace Lunggo.ApCommon.Flight.Wrapper.Citilink
{
    internal partial class CitilinkWrapper
    {
        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo)
        {            
            return Client.BookFlight(bookInfo);
        }

        private partial class CitilinkClientHandler
        {
            internal BookFlightResult BookFlight(FlightBookingInfo bookInfo)
            {
                var client = new ExtendedWebClient();
                Client.CreateSession(client);

                var splittedFareId = bookInfo.FareId.Split('.').ToList();
                var date = new DateTime(int.Parse(splittedFareId[4]), int.Parse(splittedFareId[3]), int.Parse(splittedFareId[2]));
                var adultCount = int.Parse(splittedFareId[5]);
                var childCount = int.Parse(splittedFareId[6]);
                var infantCount = int.Parse(splittedFareId[7]);
                var airlineCode = splittedFareId[8];
                var flightNumber = splittedFareId[9];
                var coreFareId = splittedFareId[11];
                var splitcoreFareId = coreFareId.Split('~').ToList();
                string origin;
                string dest;
                int index;
                if (splitcoreFareId.Count > 16)
                {
                    index = 18;
                    origin = splitcoreFareId[index];
                    dest = splitcoreFareId[index + 10];
                }
                else
                {
                    origin = splitcoreFareId[11];
                    dest = splitcoreFareId[13];
                }
                
                
                
                
                // SEARCH
                Client.CreateSession(client);
                var date2 = date.AddDays(1);
                string searchURI = @"https://book.citilink.co.id/BookingListTravelAgent.aspx";
                client.Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
                client.Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                client.Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                client.//Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/BookingListTravelAgent.aspx";
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Headers["Origin"] = "https://book.citilink.co.id";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                
                
                string searchParamaters =
                     @"AvailabilitySearchInputBookingListTravelAgentView$ButtonSubmit=Find+Flights" +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListMarketDay1=" + date.Day +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListMarketDay2=" + date2.Day +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListMarketMonth1=" + date.Year + "-" + date.Month +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListMarketMonth2=" + date.Year + "-" +date.Month+
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListPassengerType_ADT=" + adultCount +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListPassengerType_CHD=" + childCount +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListPassengerType_INFANT=" + infantCount +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListSearchBy=columnView" +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$RadioButtonMarketStructure=OneWay" +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$TextBoxMarketDestination1=" + dest +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$TextBoxMarketDestination2=" +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$TextBoxMarketOrigin1=" + origin +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$TextBoxMarketOrigin2=" +
                     @"&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation1=" + dest  +
                     @"&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation2=" +
                     @"&AvailabilitySearchInputBookingListTravelAgentVieworiginStation1=" + origin +
                     @"&AvailabilitySearchInputBookingListTravelAgentVieworiginStation2=" +
                     @"&ControlGroupBookingListTravelAgentView$BookingListBookingListTravelAgentView$DropDownListTypeOfSearch=0" +
                     @"&ControlGroupBookingListTravelAgentView$BookingListBookingListTravelAgentView$Search=ForAgent" +
                     @"&ControlGroupBookingListTravelAgentView$BookingListBookingListTravelAgentView$TextBoxKeyword=" +
                     @"&__EVENTARGUMENT=" +
                     @"&__EVENTTARGET=" +
                    //@"&__VIEWSTATE=/wEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFWkNvbnRyb2xHcm91cEJvb2tpbmdMaXN0VHJhdmVsQWdlbnRWaWV3JEJvb2tpbmdMaXN0Qm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckUmFkaW9Gb3JBZ2VudAVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeQVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeTXhy2ltZry/VwOL4DGYiD+r/S9H" +
                     @"&date_picker=" + date.Year + "-" +date.Month+ "-" +date.Day+
                     @"&date_picker=" + date2.Day + "-" +date2.Month+ "-" +date2.Day+
                     @"&pageToken=";
                
                client.UploadString(searchURI, searchParamaters);
                
                
                if (client.ResponseUri.AbsolutePath != "/ScheduleSelect.aspx")
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.FareIdNoLongerValid }
                    };
                
                // SELECT
                
                string selectURI = @"https://book.citilink.co.id/ScheduleSelect.aspx";
                client.Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
                client.Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                client.Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                //Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                client.Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/ScheduleSelect.aspx";
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Headers["Origin"] = "https://book.citilink.co.id";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                
                string selectParameters =
                   @"AvailabilitySearchInputScheduleSelectView$DdlCurrencyDynamic=IDR" +
                   @"&AvailabilitySearchInputScheduleSelectView$DropDownListMarketDay1=" + date.Day +
                   @"&AvailabilitySearchInputScheduleSelectView$DropDownListMarketDay2=" + date2.Day +
                   @"&AvailabilitySearchInputScheduleSelectView$DropDownListMarketMonth1=" + date.Year +"-"+ date.Month+
                   @"&AvailabilitySearchInputScheduleSelectView$DropDownListMarketMonth2=" + date2.Year + "-" + date.Month +
                   @"&AvailabilitySearchInputScheduleSelectView$DropDownListPassengerType_ADT=" + adultCount +
                   @"&AvailabilitySearchInputScheduleSelectView$DropDownListPassengerType_CHD=" + childCount + 
                   @"&AvailabilitySearchInputScheduleSelectView$DropDownListPassengerType_INFANT=" + infantCount +
                   @"&AvailabilitySearchInputScheduleSelectView$DropDownListSearchBy=columnView" +
                   @"&AvailabilitySearchInputScheduleSelectView$RadioButtonMarketStructure=OneWay" +
                   @"&AvailabilitySearchInputScheduleSelectView$TextBoxMarketDestination1=" + dest +
                   @"&AvailabilitySearchInputScheduleSelectView$TextBoxMarketDestination2=" +
                   @"&AvailabilitySearchInputScheduleSelectView$TextBoxMarketOrigin1=" + origin +
                   @"&AvailabilitySearchInputScheduleSelectView$TextBoxMarketOrigin2=" +
                   @"&AvailabilitySearchInputScheduleSelectViewdestinationStation1=" + dest +
                   @"&AvailabilitySearchInputScheduleSelectViewdestinationStation2=" +
                   @"&AvailabilitySearchInputScheduleSelectVieworiginStation1=" + origin +
                   @"&AvailabilitySearchInputScheduleSelectVieworiginStation2=" +
                   @"&ControlGroupScheduleSelectView$AvailabilityInputScheduleSelectView$HiddenFieldTabIndex1=1" +
                   @"&ControlGroupScheduleSelectView$AvailabilityInputScheduleSelectView$market1=" + HttpUtility.UrlEncode(coreFareId) +
                   @"&ControlGroupScheduleSelectView$ButtonSubmit=Lanjutkan" +
                   @"&__EVENTARGUMENT=" +
                   @"&__EVENTTARGET=" +
                    //@"&__VIEWSTATE=/wEPDwUBMGRkBsrCYiDYbQKCOcoq/UTudEf14vk=" +
                   @"&date_picker=" + date.Year + "-" + date.Month + "-" + date.Day +
                   @"&date_picker=" + date2.Year + "-" + date2.Month + "-" + date2.Day +
                   @"&pageToken=";
                
                client.UploadString(selectURI, selectParameters);
                
                if (client.ResponseUri.AbsolutePath != "/Passenger.aspx")
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.FareIdNoLongerValid }
                    };
                
                // INPUT DATA (TRAVELER)
                
                string passURI = @"https://book.citilink.co.id/Passenger.aspx";
                client.Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
                client.Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                client.Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                //Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                client.Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/Passenger.aspx";
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Headers["Origin"] = "https://book.citilink.co.id";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                
                string passParameters =
                    @"CONTROLGROUPPASSENGER$ButtonSubmit=Continue" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$DropDownListCountry=" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$DropDownListStateProvince=" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$DropDownListTitle=MR" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxAddressLine1=Jl. Jend Sudirman Kav. 52-53" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxAddressLine2=Equity Tower, 25th Floor" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxAddressLine3=SCBD Lot 9" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxCity=Jakarta Selatan" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxEmailAddress=dwi.agustina@travelmadezy.com" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxFax=021-29035099" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxFirstName=Yoga" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxHomePhone=" + bookInfo.ContactData.CountryCode + bookInfo.ContactData.Phone +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxLastName=Sukma" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxMiddleName=Dwi" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxPostalCode=zip/postal" +
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxWorkPhone=085728755848" +
                    @"&CONTROLGROUPPASSENGER$ItineraryDistributionInputPassengerView$Distribution=2" +
                    @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$RadioButtonInsurance=No";
                    int i = 0;
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Adult))
                {
                    var title = passenger.Title == Title.Mister ? "MR" : "MS";
                    passParameters +=
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListTitle_" + i + "=" + title +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxFirstName_" + i + "=" + passenger.FirstName +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxLastName_" + i + "=" + passenger.LastName +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListNationality_" + i + "=" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateDay_" + i + "=1" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateMonth_" + i + "=1" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateYear_" + i + "=1970" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentCountry0_" + i + "=" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentDateDay0_" + i + "=1" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentDateMonth0_" + i + "=1" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentDateYear0_" + i + "=" + date.Year +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentType0_" + i + "=" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListResidentCountry_" + i + "=" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxDocumentNumber0_" + i + "=" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxMiddleName_" + i + "=middle";
                
                    i++;
                }
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Child))
                {
                    var title = passenger.Title == Title.Mister ? "MR" : "MS";
                    passParameters +=
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListTitle_" + i + "=" + title +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxFirstName_" + i + "=" + passenger.FirstName +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxLastName_" + i + "=" + passenger.LastName +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateDay_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Day +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateMonth_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Month +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateYear_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Year +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentCountry0_" + i + "=" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentDateDay0_" + i + "=1" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentDateMonth0_" + i + "=1" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentDateYear0_" + i + "=" + date.Year +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListDocumentType0_" + i + "=" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListNationality_" + i + "=" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListResidentCountry_" + i + "=" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxDocumentNumber0_" + i + "=" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxMiddleName_" + i + "=middle";
                    i++;
                }
                i = 0;
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Infant))
                    {
                        passParameters +=
                            @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListTitle_" + i + "_"+ i +"=" +
                            @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListAssign_" + i + "_" + i + "=" + i +
                            @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateDay_" + i + "_"+ i +"=" + passenger.DateOfBirth.GetValueOrDefault().Day +
                            @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateMonth_" + i + "_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Month +
                            @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateYear_" + i + "_"+ i +"=" + passenger.DateOfBirth.GetValueOrDefault().Year +
                            @"&CONTROLGROUPPASSENGER%24PassengerInputViewPassengerView%24TextBoxFirstName_" + i + "_" + i + "=" + passenger.FirstName +
                            @"&CONTROLGROUPPASSENGER%24PassengerInputViewPassengerView%24TextBoxLastName_" + i + "_" + i + "=" + passenger.LastName +
                            @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListGender_" + i + "_"+ i +"=" + (int)passenger.Gender +
                            @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListNationality_" + i + "_"+ i +"=" +
                            @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListResidentCountry_" + i + "_"+ i +"=" +
                            @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxMiddleName_" + i + "_" + i + "=middle" ;
                        i++;
                    }
                    
                passParameters +=
                    @"&__EVENTARGUMENT=" +
                    @"&__EVENTTARGET=" +
                    //@"&__VIEWSTATE=/wEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgIFR0NPTlRST0xHUk9VUFBBU1NFTkdFUiRQYXNzZW5nZXJJbnB1dFZpZXdQYXNzZW5nZXJWaWV3JENoZWNrQm94SW5zdXJhbmNlBUFDT05UUk9MR1JPVVBQQVNTRU5HRVIkUGFzc2VuZ2VySW5wdXRWaWV3UGFzc2VuZ2VyVmlldyRDaGVja0JveFBNSXZkWh6Cdtm1mad5oP+7VGz2nQKe
                    @"&pageToken=";
                
                client.UploadString(passURI, passParameters);
                
                if (client.ResponseUri.AbsolutePath != "/SeatMap.aspx")
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.InvalidInputData }
                    };
                
                // SELECT SEAT (UNITMAP)
                
                string kursiURI = @"https://book.citilink.co.id/SeatMap.aspx";
                client.Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
                client.Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                client.Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                //Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                client.Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/SeatMap.aspx";
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Headers["Origin"] = "https://book.citilink.co.id";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                
                string kursiParameter = 
                          @"ControlGroupUnitMapView$UnitMapViewControl$compartmentDesignatorInput=" +
                          @"&ControlGroupUnitMapView$UnitMapViewControl$deckDesignatorInput=1" +
                          @"&ControlGroupUnitMapView$UnitMapViewControl$passengerInput=0" +
                          @"&ControlGroupUnitMapView$UnitMapViewControl$tripInput=0" +
                          @"&__EVENTARGUMENT=" +
                          @"&__EVENTTARGET=ControlGroupUnitMapView$UnitMapViewControl$LinkButtonAssignUnit" +
                               //"&__VIEWSTATE=/wEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFN0NvbnRyb2xHcm91cFVuaXRNYXBWaWV3JFVuaXRNYXBWaWV3Q29udHJvbCRDaGVja0JveFNlYXRC9WoNScpqWuAJVhj4Iqw3MUfIjw=="
                          @"&pageToken=";
                    int j = 0;
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Adult))
                {
                     kursiParameter += 
                          @"&ControlGroupUnitMapView$UnitMapViewControl$EquipmentConfiguration_0_PassengerNumber_" + j + "=" +
                          @"&ControlGroupUnitMapView$UnitMapViewControl$HiddenEquipmentConfiguration_0_PassengerNumber_" + j + "=";
                    j++;
                }
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Child))
                {
                    kursiParameter +=
                         @"&ControlGroupUnitMapView$UnitMapViewControl$EquipmentConfiguration_0_PassengerNumber_" + j + "=" +
                         @"&ControlGroupUnitMapView$UnitMapViewControl$HiddenEquipmentConfiguration_0_PassengerNumber_" + j + "=";
                    j++;
                }
                client.UploadString(kursiURI, kursiParameter);
                
                if (client.ResponseUri.AbsolutePath != "/Payment.aspx")
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.InvalidInputData }
                    };
                
                // SELECT HOLD (PAYMENT)
                
                string paymentURI = @"https://book.citilink.co.id/Payment.aspx";
                client.Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
                client.Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                client.Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                //Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                client.Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/Payment.aspx";
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Headers["Origin"] = "https://book.citilink.co.id";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                
                string payParameter =
                   @"__EVENTTARGET=" +
                   @"&__EVENTARGUMENT=" +
                   @"&__VIEWSTATE=%2FwEPDwUBMGRkBsrCYiDYbQKCOcoq%2FUTudEf14vk%3D" +
                   @"&pageToken=" +
                   @"&DropDownListPaymentMethodCode=AgencyAccount%3AAG" +
                   @"&DropDownListPaymentMethodCode=PrePaid%3AKB" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardNumber=" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtVcc=" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24DdlExpMonth=1 " +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24DdlExpYear=2015" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderName=Dwi+Agustina" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderAddress=Jl.+Jend+Sudirman+Kav.+52-53" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderCity=Jakarta+Selatan" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderProvince=" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderZipCode=" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24DdlCardHolderCountry=AD" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderEmail=dwi.agustina%40travelmadezy.com" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderPhone=" + bookInfo.ContactData.CountryCode + bookInfo.ContactData.Phone +
                   @"&DropDownListPaymentMethodCode=ExternalAccount%3AMC" +
                   @"&DropDownListPaymentMethodCode=Voucher%3AVO" +
                   @"&TextBoxVoucherAccount_VO_ACCTNO=" +
                   @"&AMOUNT=" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24DropDownListPaymentMethodCode=PrePaid%3AHOLD" +
                   @"&DropDownListPaymentMethodCode=PrePaid%3AHOLD" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24AgreementPaymentInputViewPaymentView%24CheckBoxAgreement=on" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24storedPaymentId=" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ButtonSubmit=Lanjutkan";
                
                client.UploadString(paymentURI, payParameter);
                
                if (client.ResponseUri.AbsolutePath != "/Wait.aspx")
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.TechnicalError }
                    };
                
                // WAIT
                
                string waitURI = "https://book.citilink.co.id/Wait.aspx";
                
                client.Headers[HttpRequestHeader.Host] = "book.citilink.co.id";
                client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:41.0) Gecko/20100101 Firefox/41.0";
                client.Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                client.Headers[HttpRequestHeader.AcceptLanguage] = "en-GB,en-US;q=0.8,en;q=0.6";
                //Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                client.Headers[HttpRequestHeader.Referer] = "https://book.citilink.co.id/Wait.aspx";
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Headers["Origin"] = "https://book.citilink.co.id";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                
                var htmlResult = client.DownloadString(waitURI);
                while (!(client.ResponseUri.AbsolutePath.Contains("Itinerary.aspx")))
                {
                   htmlResult = client.DownloadString(waitURI);
                }

                //var htmlResult = System.IO.File.ReadAllText(@"C:\Users\User\Documents\Kerja\Crawl\Itin.txt");

                CQ ambilDataItin = (CQ) htmlResult;


                var hasil = new BookFlightResult();


                var tunjukDataNomorBooking = ambilDataItin.MakeRoot()["#SpanRecordLocator"];
                var NomorBooking = tunjukDataNomorBooking.Select(x => x.Cq().Text()).FirstOrDefault();
                var tunjukDataTimeLimit = ambilDataItin.MakeRoot()["#itineraryBody>p"];
                //var tunjukDataTimeLimit1 = tunjukDataTimeLimit["p:nth-child(1)"];
                var ambilTimeLimit = tunjukDataTimeLimit.Select(x => x.Cq().Text()).FirstOrDefault();
                var timelimitIndex = ambilTimeLimit.IndexOf("[");
                var timelimitParse3 = ambilTimeLimit.Split(' ');
                var timelimitString = ambilTimeLimit.Substring(timelimitIndex+1, 20);
                var timelimitParse = timelimitString.Split(',');
                var timelimitParse2 = timelimitParse[1].Split(' ');
                var ambiltahun = timelimitParse[2].Trim();
                var timeLimitUTC = timelimitParse3[54].Substring(4,2);
                string tahun;
                if (ambiltahun.Length > 4)
                {
                    tahun = ambiltahun.Substring(0, 4);
                }
                else
                {
                    tahun = ambiltahun;
                }


                if (timelimitParse2[1] == "Nop")
                    timelimitParse2[1] = "Nov";

                var timelimit = DateTime.Parse(timelimitParse2[2] + "-" + timelimitParse2[1] + "-" + tahun +" "+ timelimitParse3[41]+" "+timeLimitUTC,CultureInfo.CreateSpecificCulture("id-ID"));

                var status = new BookingStatusInfo();
                status.BookingId = NomorBooking;
                status.BookingStatus = BookingStatus.Booked;
                status.TimeLimit = timelimit.ToUniversalTime();

                hasil.Status = status;
                hasil.IsSuccess = true;
                return hasil;
            }
        }
    }
}
