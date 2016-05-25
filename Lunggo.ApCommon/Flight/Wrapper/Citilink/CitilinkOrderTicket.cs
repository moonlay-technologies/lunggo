using System;
using System.Collections.Generic;
using System.Linq;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Web;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Citilink
{
    internal partial class CitilinkWrapper
    {
        internal override IssueTicketResult OrderTicket(string bookingId, bool canHold)
        {
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            if (env == "production")
                return Client.OrderTicket(bookingId);
            else
                return new IssueTicketResult
                {
                    IsSuccess = true,
                    BookingId = bookingId,
                    IsInstantIssuance = true
                };
        }

        private partial class CitilinkClientHandler
        {
            
            internal IssueTicketResult OrderTicket(string bookingId)
            {
                var hariIni = DateTime.Now.Day;
                var bulanIni = DateTime.Now.Month;
                var tahunIni = DateTime.Now.Year;
                var hariIni7 = DateTime.Now.Date.AddDays(7).Day;
                var bulanIni7 = DateTime.Now.Date.AddDays(7).Month;
                var tahunIni7 = DateTime.Now.Date.AddDays(7).Year;

                var clientx = CreateAgentClient();
                Login(clientx);

                var url = "BookingListTravelAgent.aspx";
                var listRequest = new RestRequest(url, Method.POST);
                var postData =
                    "__EVENTTARGET=ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24LinkButtonFindBooking" +
                    "&__EVENTARGUMENT=" +
                    "&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFWkNvbnRyb2xHcm91cEJvb2tpbmdMaXN0VHJhdmVsQWdlbnRWaWV3JEJvb2tpbmdMaXN0Qm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckUmFkaW9Gb3JBZ2VudAVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeQVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeTXhy2ltZry%2FVwOL4DGYiD%2Br%2FS9H" +
                    "&pageToken=" +
                    "&AvailabilitySearchInputBookingListTravelAgentVieworiginStation1=" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketOrigin1=" +
                    "&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation1=" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketDestination1=" +
                    "&AvailabilitySearchInputBookingListTravelAgentVieworiginStation2=" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketOrigin2=" +
                    "&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation2=" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketDestination2=" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24RadioButtonMarketStructure=RoundTrip" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketDay1=" + hariIni +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketMonth1=" + tahunIni + "-" + bulanIni +
                    "&date_picker=" + tahunIni + "-" + bulanIni + "-" + hariIni +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketDay2=" + hariIni7 +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketMonth2=" + tahunIni7 + "-" + bulanIni7 +
                    "&date_picker= " + tahunIni7 + "-" + bulanIni7 + "-" + hariIni7 +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListPassengerType_ADT=1" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListPassengerType_CHD=0" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListPassengerType_INFANT=0" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListSearchBy=columnView" +
                    "&ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24Search=ForAgent" +
                    "&ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24DropDownListTypeOfSearch=0" +
                    "&ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24TextBoxKeyword=";
                listRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var listResponse = clientx.Execute(listRequest);
                if (listResponse.ResponseUri.AbsolutePath != "/BookingListTravelAgent.aspx")
                    return new IssueTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                    };

                url = "BookingListTravelAgent.aspx";
                var selectRequest = new RestRequest(url, Method.POST);
                postData = 
                    "__EVENTTARGET=ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView" +
                    "&__EVENTARGUMENT=payment:" + bookingId +
                    "&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFWkNvbnRyb2xHcm91cEJvb2tpbmdMaXN0VHJhdmVsQWdlbnRWaWV3JEJvb2tpbmdMaXN0Qm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckUmFkaW9Gb3JBZ2VudAVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeQVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeTXhy2ltZry%2FVwOL4DGYiD%2Br%2FS9H" +
                    "&pageToken=" +
                    "&AvailabilitySearchInputBookingListTravelAgentVieworiginStation1=" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketOrigin1=" +
                    "&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation1=" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketDestination1=" +
                    "&AvailabilitySearchInputBookingListTravelAgentVieworiginStation2=" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketOrigin2=" +
                    "&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation2=" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketDestination2=" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24RadioButtonMarketStructure=RoundTrip" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketDay1=" + hariIni +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketMonth1=" + tahunIni + "-" + bulanIni +
                    "&date_picker=" + tahunIni + "-" + bulanIni + "-" + hariIni +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketDay2=" + hariIni7 +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketMonth2=" + tahunIni + "-" + bulanIni +
                    "&date_picker=" + tahunIni7 + "-" + bulanIni7 + "-" + hariIni7 +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListPassengerType_ADT=1" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListPassengerType_CHD=0" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListPassengerType_INFANT=0" +
                    "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListSearchBy=columnView" +
                    "&ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24Search=ForAgent" +
                    "&ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24DropDownListTypeOfSearch=0" +
                    "&ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24TextBoxKeyword=";
                selectRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var selectResponse = clientx.Execute(selectRequest);
                if (selectResponse.ResponseUri.AbsolutePath != "/Payment.aspx")
                    return new IssueTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                    };

                try
                {
                    url = "Payment.aspx";
                    var paymentRequest = new RestRequest(url, Method.POST);
                    postData =
                        "__EVENTTARGET=" +
                        "&__EVENTARGUMENT=" +
                        "&__VIEWSTATE=%2FwEPDwUBMGRkBsrCYiDYbQKCOcoq%2FUTudEf14vk%3D&pageToken=" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24DropDownListPaymentMethodCode=AgencyAccount%3AAG" +
                        "&DropDownListPaymentMethodCode=PrePaid%3AKB" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardNumber=" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtVcc=" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24DdlExpMonth=1" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24DdlExpYear=2015" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderName=Dwi+Agustina" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderAddress=Jl.+Jend+Sudirman+Kav.+52-53&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderCity=Jakarta+Selatan" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderProvince=" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderZipCode=" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24DdlCardHolderCountry=AD" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderEmail=dwi.agustina%40travelmadezy.com" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24TxtCardHolderPhone=6289023456" +
                        "&DropDownListPaymentMethodCode=ExternalAccount%3AMC" +
                        "&DropDownListPaymentMethodCode=Voucher%3AVO" +
                        "&TextBoxVoucherAccount_VO_ACCTNO=&AMOUNT=" +
                        "&DropDownListPaymentMethodCode=PrePaid%3AHOLD" +
                        "&DropDownListPaymentMethodCode=PrePaid%3AHOLD" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24AgreementPaymentInputViewPaymentView%24CheckBoxAgreement=on" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ControlGroupPaymentInputViewPaymentView%24storedPaymentId=" +
                        "&CONTROLGROUPPAYMENTBOTTOM%24ButtonSubmit=Lanjutkan";
                    paymentRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var paymentResponse = clientx.Execute(paymentRequest);

                    url = "BookingListTravelAgent.aspx";
                    var listRequest2 = new RestRequest(url, Method.POST);
                    postData =
                        "__EVENTTARGET=ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24LinkButtonFindBooking" +
                        "&__EVENTARGUMENT=" +
                        "&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFWkNvbnRyb2xHcm91cEJvb2tpbmdMaXN0VHJhdmVsQWdlbnRWaWV3JEJvb2tpbmdMaXN0Qm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckUmFkaW9Gb3JBZ2VudAVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeQVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeTXhy2ltZry%2FVwOL4DGYiD%2Br%2FS9H" +
                        "&pageToken=" +
                        "&AvailabilitySearchInputBookingListTravelAgentVieworiginStation1=" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketOrigin1=" +
                        "&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation1=" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketDestination1=" +
                        "&AvailabilitySearchInputBookingListTravelAgentVieworiginStation2=" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketOrigin2=" +
                        "&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation2=" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketDestination2=" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24RadioButtonMarketStructure=RoundTrip" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketDay1=" + hariIni +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketMonth1=" + tahunIni + "-" + bulanIni +
                        "&date_picker=" + tahunIni + "-" + bulanIni + "-" + hariIni +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketDay2=" + hariIni7 +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketMonth2=" + tahunIni7 + "-" + bulanIni7 +
                        "&date_picker= " + tahunIni7 + "-" + bulanIni7 + "-" + hariIni7 +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListPassengerType_ADT=1" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListPassengerType_CHD=0" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListPassengerType_INFANT=0" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListSearchBy=columnView" +
                        "&ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24Search=ForAgent" +
                        "&ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24DropDownListTypeOfSearch=0" +
                        "&ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24TextBoxKeyword=";
                    listRequest2.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var listResponse2 = clientx.Execute(listRequest2);
                    if (listResponse2.ResponseUri.AbsolutePath != "/BookingListTravelAgent.aspx")
                        return new IssueTicketResult
                        {
                            IsSuccess = false,
                            Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                        };

                    url = "BookingListTravelAgent.aspx";
                    var checkRequest = new RestRequest(url, Method.POST);
                    postData =
                        "__EVENTTARGET=ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView" +
                        "&__EVENTARGUMENT=View:" + bookingId +
                        "&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFWkNvbnRyb2xHcm91cEJvb2tpbmdMaXN0VHJhdmVsQWdlbnRWaWV3JEJvb2tpbmdMaXN0Qm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckUmFkaW9Gb3JBZ2VudAVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeQVbQ29udHJvbEdyb3VwQm9va2luZ0xpc3RUcmF2ZWxBZ2VudFZpZXckQm9va2luZ0xpc3RCb29raW5nTGlzdFRyYXZlbEFnZW50VmlldyRSYWRpb0ZvckFnZW5jeTXhy2ltZry%2FVwOL4DGYiD%2Br%2FS9H" +
                        "&pageToken=" +
                        "&AvailabilitySearchInputBookingListTravelAgentVieworiginStation1=" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketOrigin1=" +
                        "&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation1=" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketDestination1=" +
                        "&AvailabilitySearchInputBookingListTravelAgentVieworiginStation2=" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketOrigin2=" +
                        "&AvailabilitySearchInputBookingListTravelAgentViewdestinationStation2=" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24TextBoxMarketDestination2=" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24RadioButtonMarketStructure=RoundTrip" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketDay1=" + hariIni +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketMonth1=" + tahunIni + "-" + bulanIni +
                        "&date_picker=" + tahunIni + "-" + bulanIni + "-" + hariIni +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketDay2=" + hariIni7 +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListMarketMonth2=" + tahunIni7 + "-" + bulanIni7 +
                        "&date_picker=" + tahunIni7 + "-" + bulanIni7 + "-" + hariIni7 +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListPassengerType_ADT=1" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListPassengerType_CHD=0" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListPassengerType_INFANT=0" +
                        "&AvailabilitySearchInputBookingListTravelAgentView%24DropDownListSearchBy=columnView" +
                        "&ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24Search=ForAgent" +
                        "&ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24DropDownListTypeOfSearch=0" +
                        "&ControlGroupBookingListTravelAgentView%24BookingListBookingListTravelAgentView%24TextBoxKeyword=";
                    checkRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var checkResponse = clientx.Execute(checkRequest);
                    var htmlRespon = checkResponse.Content;

                    //var htmlRespon = System.IO.File.ReadAllText(@"C:\Users\User\Documents\Kerja\Crawl\ItineraryCek.txt");
                    var ambilDataRespon = (CQ)htmlRespon;

                    var tunjukStatus = ambilDataRespon["#itineraryBody>"];
                    var tunjukTr = tunjukStatus.MakeRoot()["tr:nth-child(2)>td:nth-child(2)"];
                    var statusPembayaran = tunjukTr.Select(x => x.Cq().Text()).FirstOrDefault();
                    var hasil = new IssueTicketResult();

                    if ((statusPembayaran == "Konfirm") || (statusPembayaran == "Confirmed"))
                        {
                            hasil.BookingId = bookingId;
                            hasil.IsSuccess = true;
                            hasil.IsInstantIssuance = true;
                        }
                    else
                        {
                            hasil.IsSuccess = false;
                            hasil.Errors = new List<FlightError> { FlightError.FailedOnSupplier };
                        }

                    return hasil;
                }

                catch (Exception)
                {
                    var isIssued = IsIssued(bookingId);
                    switch (isIssued)
                    {
                        case IssueEnum.IssueSuccess:
                            return new IssueTicketResult
                            {
                                IsSuccess = true,
                                BookingId = bookingId
                            };
                        case IssueEnum.NotIssued:
                            return new IssueTicketResult
                            {
                                IsSuccess = false
                            };
                        case IssueEnum.CheckingError:
                            return new IssueTicketResult
                            {
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "Failed to check whether deposit cut or not! Manual checking advised!" }
                            };
                        default:
                            return new IssueTicketResult
                            {
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "Failed to check whether deposit cut or not! Manual checking advised!" }
                            };
                    }
                }
            }
        }
    }
}
