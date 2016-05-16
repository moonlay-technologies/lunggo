using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.Framework.Web;
using RestSharp;
using System.Diagnostics;
using Lunggo.ApCommon.Constant;

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
                Login(client);

                var splittedFareId = bookInfo.Itinerary.FareId.Split('.').ToList();
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
                    index = 11;
                    origin = splitcoreFareId[index];
                    dest = splitcoreFareId[21];
                }
                else
                {
                    origin = splitcoreFareId[11];
                    dest = splitcoreFareId[13];
                }




                // SEARCH
                var date2 = date.AddDays(1);
                string searchUrl = @"BookingListTravelAgent.aspx";

                var searchRequest = new RestRequest(searchUrl, Method.POST);
                var searchPostData =
                     @"AvailabilitySearchInputBookingListTravelAgentView$ButtonSubmit=Find+Flights" +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListMarketDay1=" + date.Day +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListMarketDay2=" + date2.Day +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListMarketMonth1=" + date.Year + "-" + date.Month +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListMarketMonth2=" + date.Year + "-" + date.Month +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListPassengerType_ADT=" + adultCount +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListPassengerType_CHD=" + childCount +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListPassengerType_INFANT=" + infantCount +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$DropDownListSearchBy=columnView" +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$RadioButtonMarketStructure=OneWay" +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$TextBoxMarketDestination1=" + dest +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$TextBoxMarketDestination2=" +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$TextBoxMarketOrigin1=" + origin +
                     @"&AvailabilitySearchInputBookingListTravelAgentView$TextBoxMarketOrigin2=" +
                     @"&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation1=" + dest +
                     @"&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation2=" +
                     @"&AvailabilitySearchInputBookingListTravelAgentVieworiginStation1=" + origin +
                     @"&AvailabilitySearchInputBookingListTravelAgentVieworiginStation2=" +
                     @"&ControlGroupBookingListTravelAgentView$BookingListBookingListTravelAgentView$DropDownListTypeOfSearch=0" +
                     @"&ControlGroupBookingListTravelAgentView$BookingListBookingListTravelAgentView$Search=ForAgent" +
                     @"&ControlGroupBookingListTravelAgentView$BookingListBookingListTravelAgentView$TextBoxKeyword=" +
                     @"&__EVENTARGUMENT=" +
                     @"&__EVENTTARGET=" +
                    //@"&__VIEWSTATE=/wEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFWkNvbnRyb2xHcm91cEJvb2tpbmdMaXN0VHJhdmVsQWdlbnRWaWV3JEJvb2tpbmdMaXN0Qm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckUmFkaW9Gb3JBZ2VudAVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeQVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeTXhy2ltZry/VwOL4DGYiD+r/S9H" +
                     @"&date_picker=" + date.Year + "-" + date.Month + "-" + date.Day +
                     @"&date_picker=" + date2.Day + "-" + date2.Month + "-" + date2.Day +
                     @"&pageToken=";
                searchRequest.AddParameter("application/x-www-form-urlencoded", searchPostData, ParameterType.RequestBody);
                var searchResponse = client.Execute(searchRequest);

                if (searchResponse.ResponseUri.AbsolutePath != "/ScheduleSelect.aspx")
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

                var selectUri = @"ScheduleSelect.aspx";

                var selectRequest = new RestRequest(selectUri, Method.POST);
                var selectPostData =
                   @"AvailabilitySearchInputScheduleSelectView$DdlCurrencyDynamic=IDR" +
                   @"&AvailabilitySearchInputScheduleSelectView$DropDownListMarketDay1=" + date.Day +
                   @"&AvailabilitySearchInputScheduleSelectView$DropDownListMarketDay2=" + date2.Day +
                   @"&AvailabilitySearchInputScheduleSelectView$DropDownListMarketMonth1=" + date.Year + "-" + date.Month +
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

                selectRequest.AddParameter("application/x-www-form-urlencoded", selectPostData, ParameterType.RequestBody);
                var selectResponse = client.Execute(selectRequest);

                if (selectResponse.ResponseUri.AbsolutePath != "/Passenger.aspx")
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

                string passUrl = @"Passenger.aspx";

                var passRequest = new RestRequest(passUrl, Method.POST);
                var passPostData =
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
                    @"&CONTROLGROUPPASSENGER$ContactInputPassengerView$TextBoxHomePhone=" + bookInfo.Contact.CountryCallingCode + bookInfo.Contact.Phone +
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
                    passPostData +=
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
                    passPostData +=
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
                    passPostData +=
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListTitle_" + i + "_" + i + "=" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListAssign_" + i + "_" + i + "=" + i +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateDay_" + i + "_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Day +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateMonth_" + i + "_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Month +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListBirthDateYear_" + i + "_" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().Year +
                        @"&CONTROLGROUPPASSENGER%24PassengerInputViewPassengerView%24TextBoxFirstName_" + i + "_" + i + "=" + passenger.FirstName +
                        @"&CONTROLGROUPPASSENGER%24PassengerInputViewPassengerView%24TextBoxLastName_" + i + "_" + i + "=" + passenger.LastName +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListGender_" + i + "_" + i + "=" + (int)passenger.Gender +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListNationality_" + i + "_" + i + "=" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$DropDownListResidentCountry_" + i + "_" + i + "=" +
                        @"&CONTROLGROUPPASSENGER$PassengerInputViewPassengerView$TextBoxMiddleName_" + i + "_" + i + "=middle";
                    i++;
                }

                passPostData +=
                    @"&__EVENTARGUMENT=" +
                    @"&__EVENTTARGET=" +
                    //@"&__VIEWSTATE=/wEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgIFR0NPTlRST0xHUk9VUFBBU1NFTkdFUiRQYXNzZW5nZXJJbnB1dFZpZXdQYXNzZW5nZXJWaWV3JENoZWNrQm94SW5zdXJhbmNlBUFDT05UUk9MR1JPVVBQQVNTRU5HRVIkUGFzc2VuZ2VySW5wdXRWaWV3UGFzc2VuZ2VyVmlldyRDaGVja0JveFBNSXZkWh6Cdtm1mad5oP+7VGz2nQKe
                    @"&pageToken=";

                passRequest.AddParameter("application/x-www-form-urlencoded", passPostData, ParameterType.RequestBody);
                var passResponse = client.Execute(passRequest);

                if (passResponse.ResponseUri.AbsolutePath != "/SeatMap.aspx")
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

                var seatUrl = @"SeatMap.aspx";
                var seatRequest = new RestRequest(seatUrl, Method.POST);

                var seatPostData =
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
                    seatPostData +=
                         @"&ControlGroupUnitMapView$UnitMapViewControl$EquipmentConfiguration_0_PassengerNumber_" + j + "=" +
                         @"&ControlGroupUnitMapView$UnitMapViewControl$HiddenEquipmentConfiguration_0_PassengerNumber_" + j + "=";
                    j++;
                }
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Child))
                {
                    seatPostData +=
                         @"&ControlGroupUnitMapView$UnitMapViewControl$EquipmentConfiguration_0_PassengerNumber_" + j + "=" +
                         @"&ControlGroupUnitMapView$UnitMapViewControl$HiddenEquipmentConfiguration_0_PassengerNumber_" + j + "=";
                    j++;
                }
                seatRequest.AddParameter("application/x-www-form-urlencoded", seatPostData, ParameterType.RequestBody);
                var seatResponse = client.Execute(seatRequest);

                if (seatResponse.ResponseUri.AbsolutePath != "/Payment.aspx")
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.InvalidInputData }
                    };

                /*Buat dapat Info Itinerary dan Harga*/
                var getPaymenturl = @"Payment.aspx";
                var paymentGetRequest = new RestRequest(getPaymenturl, Method.GET);
                paymentGetRequest.AddHeader("Referer", "https://book.citilink.co.id/SeatMap.aspx");
                var paymentGetresponse = client.Execute(paymentGetRequest);
                var html = paymentGetresponse.Content;
                CQ detailFlight = (CQ)html;
                var getPrice = detailFlight["#priceDisplayBody>table:last"].Children().Children().Last().Last().Text().Trim().Split('\n');
                var harga = getPrice[1].Trim().Replace("Rp.","").Replace(",","");
                var fixPrice = decimal.Parse(harga);

                //Cek Harga di Final
                if (bookInfo.Itinerary.Price.Supplier != fixPrice) 
                {
                    var fixItin = bookInfo.Itinerary;
                    fixItin.Price.Supplier = fixPrice;
                    fixItin.FareId = fixItin.FareId.Replace(bookInfo.Itinerary.Price.Supplier.ToString(),fixPrice.ToString());
                    
                    return new BookFlightResult
                    {
                        IsValid = true,
                        IsItineraryChanged = false,
                        IsPriceChanged = bookInfo.Itinerary.Price.Supplier != fixPrice,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Price is changed!" },
                        NewItinerary = fixItin,
                        NewPrice = fixPrice,
                        Status = null
                    };
                }
                

                // SELECT HOLD (PAYMENT)

                var paymentUrl = @"Payment.aspx";
                var paymentRequest = new RestRequest(paymentUrl, Method.POST);

                var paymentPostData =
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
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderPhone=" + bookInfo.Contact.CountryCallingCode + bookInfo.Contact.Phone +
                   @"&DropDownListPaymentMethodCode=ExternalAccount%3AMC" +
                   @"&DropDownListPaymentMethodCode=Voucher%3AVO" +
                   @"&TextBoxVoucherAccount_VO_ACCTNO=" +
                   @"&AMOUNT=" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24DropDownListPaymentMethodCode=PrePaid%3AHOLD" +
                   @"&DropDownListPaymentMethodCode=PrePaid%3AHOLD" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24AgreementPaymentInputViewPaymentView%24CheckBoxAgreement=on" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24storedPaymentId=" +
                   @"&CONTROLGROUPPAYMENTBOTTOM%24ButtonSubmit=Lanjutkan";
                
                paymentRequest.AddParameter("application/x-www-form-urlencoded", paymentPostData, ParameterType.RequestBody);
                var paymentResponse = client.Execute(paymentRequest);


                if (paymentResponse.ResponseUri.AbsolutePath != "/Wait.aspx")
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

                var waitUrl = "Wait.aspx";
                var waitRequest = new RestRequest(waitUrl, Method.GET);
                var waitResponse = client.Execute(waitRequest);

                while (!(waitResponse.ResponseUri.AbsolutePath.Contains("Itinerary.aspx")))
                {
                    waitResponse = client.Execute(waitRequest);
                }

                //var htmlResult = System.IO.File.ReadAllText(@"C:\Users\User\Documents\Kerja\Crawl\Itin.txt");

                var ambilDataItin = (CQ)waitResponse.Content;


                var hasil = new BookFlightResult();


                var tunjukDataNomorBooking = ambilDataItin.MakeRoot()["#SpanRecordLocator"];
                var NomorBooking = tunjukDataNomorBooking.Select(x => x.Cq().Text()).FirstOrDefault();
                var tunjukDataTimeLimit = ambilDataItin.MakeRoot()["#itineraryBody>p"];
                //var tunjukDataTimeLimit1 = tunjukDataTimeLimit["p:nth-child(1)"];
                var ambilTimeLimit = tunjukDataTimeLimit.Select(x => x.Cq().Text()).FirstOrDefault();
                var timelimitIndex = ambilTimeLimit.IndexOf("[");
                var timelimitIndex2 = ambilTimeLimit.IndexOf("]");
                var timelimitParse3 = ambilTimeLimit.Split(' ');
                var timelimitString = ambilTimeLimit.Substring(timelimitIndex + 1, timelimitIndex2 - timelimitIndex - 1);
                var timelimitSplitComma = timelimitString.Split(',');
                var timelimit1SplitSpace = timelimitSplitComma[1].Trim().Split(' ');
                var timelimit2SplitSpace = timelimitSplitComma[2].Trim().Split(' ');
                var timelimitDate = timelimit1SplitSpace[1].Trim();
                var timelimitMonth = timelimit1SplitSpace[0].Trim();
                var timelimitYear = timelimit2SplitSpace[0].Trim();
                var timelimitTime = timelimit2SplitSpace[28].Trim();
                string tahun;
                if (timelimitYear.Length > 4)
                {
                    tahun = timelimitYear.Substring(0, 4);
                }
                else
                {
                    tahun = timelimitYear;
                }


                if (timelimitMonth == "Nop")
                    timelimitMonth = "Nov";

                var timelimit = DateTime.Parse(timelimitDate + "-" + timelimitMonth + "-" + tahun + " " + timelimitTime, CultureInfo.CreateSpecificCulture("id-ID"));

                var status = new BookingStatusInfo();
                status.BookingId = NomorBooking;
                status.BookingStatus = BookingStatus.Booked;
                status.TimeLimit = DateTime.SpecifyKind(timelimit, DateTimeKind.Utc);

                hasil.Status = status;
                hasil.IsSuccess = true;
                return hasil;
            }
        }
    }
}
