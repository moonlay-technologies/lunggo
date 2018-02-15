using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Log;
using Lunggo.Framework.Web;
using RestSharp;
using Lunggo.ApCommon.Log;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
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

        private partial class AirAsiaClientHandler
        {
            internal IssueTicketResult OrderTicket(string bookingId)
            {
                var clientx = CreateAgentClient();
                var TableLog = new GlobalLog();
                
                TableLog.PartitionKey = "FLIGHT ISSUE LOG";
                
                var log = LogService.GetInstance();
                var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
                if (!Login(clientx))
                {
                    Console.WriteLine("[Airasia] Failed to Login");
                    return new IssueTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { "[AirAsia] Can't Login!" }
                    };
                }


                // [GET] BookingList
                TableLog.Log = "[Airasia] [GET] Halaman BookingList.aspx";
                log.Post(TableLog.Log, "#logging-issueflight");
                TableLog.Logging();

                var url = "BookingList.aspx";
                var listRequest = new RestRequest(url, Method.GET);
                listRequest.AddHeader("Referer", "https://booking2.airasia.com/AgentHome.aspx");
                var listResponse = clientx.Execute(listRequest);

                if (listResponse.ResponseUri.AbsolutePath != "/BookingList.aspx" &&
                    (listResponse.StatusCode == HttpStatusCode.OK || listResponse.StatusCode == HttpStatusCode.Redirect))
                {
                    TableLog.Log = "[AirAsia][GET] Response Uri is not /BookingList.aspx or Status is not (OK Or Redirected)";
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();
                    return new IssueTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                    };
                }


                // [POST] BookingList
                TableLog.Log = "[Airasia] [Post] Halaman BookingList.aspx";
                log.Post(TableLog.Log, "#logging-issueflight");
                TableLog.Logging();

                url = "BookingList.aspx";
                var filterRequest = new RestRequest(url, Method.POST);
                filterRequest.AddHeader("Referer", "https://booking2.airasia.com/BookingList.aspx");
                var postData =
                    @"__EVENTTARGET=ControlGroupBookingListView%24BookingListSearchInputView%24LinkButtonFindBooking" +
                    @"&__EVENTARGUMENT=" +
                    @"&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFRENvbnRyb2xHcm91cEJvb2tpbmdMaXN0VmlldyRCb29raW5nTGlzdFNlYXJjaElucHV0VmlldyRSYWRpb0ZvckFnZW50BUVDb250cm9sR3JvdXBCb29raW5nTGlzdFZpZXckQm9va2luZ0xpc3RTZWFyY2hJbnB1dFZpZXckUmFkaW9Gb3JBZ2VuY3kFRUNvbnRyb2xHcm91cEJvb2tpbmdMaXN0VmlldyRCb29raW5nTGlzdFNlYXJjaElucHV0VmlldyRSYWRpb0ZvckFnZW5jeVdjug1NBbrhcyWlW33sJWOJ%2ByEA" +
                    @"&pageToken=" +
                    @"&ControlGroupBookingListView%24BookingListSearchInputView%24Search=ForAgency" +
                    @"&ControlGroupBookingListView%24BookingListSearchInputView%24DropDownListTypeOfSearch=0" +
                    @"&ControlGroupBookingListView%24BookingListSearchInputView%24TextBoxKeyword=KEYWORD" +
                    @"&__VIEWSTATEGENERATOR=05F9A2B0";
                filterRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var filterResponse = clientx.Execute(filterRequest);

                if (filterResponse.ResponseUri.AbsolutePath != "/BookingList.aspx" &&
                    (filterResponse.StatusCode == HttpStatusCode.OK ||
                     filterResponse.StatusCode == HttpStatusCode.Redirect))
                {
                    TableLog.Log = "[AirAsia][POST] Response Uri is not /BookingList.aspx or Status is not (OK Or Redirected)";
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();

                    return new IssueTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier },
                        ErrorMessages = new List<string> { "[AirAsia] || " + filterResponse.Content }
                    };
                }


                // [POST] BookingList -> ChangeItinerary
                TableLog.Log = "[Airasia] [Post] BookingList -> ChangeItinerary";
                log.Post(TableLog.Log, "#logging-issueflight");
                TableLog.Logging();

                url = "BookingList.aspx";
                var selectRequest = new RestRequest(url, Method.POST);
                selectRequest.AddHeader("Referer", "https://booking2.airasia.com/BookingList.aspx");
                postData =
                    @"__EVENTTARGET=ControlGroupBookingListView%24BookingListSearchInputView" +
                    @"&__EVENTARGUMENT=Edit%3A" + bookingId +
                    @"&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFRENvbnRyb2xHcm91cEJvb2tpbmdMaXN0VmlldyRCb29raW5nTGlzdFNlYXJjaElucHV0VmlldyRSYWRpb0ZvckFnZW50BURDb250cm9sR3JvdXBCb29raW5nTGlzdFZpZXckQm9va2luZ0xpc3RTZWFyY2hJbnB1dFZpZXckUmFkaW9Gb3JBZ2VudAVFQ29udHJvbEdyb3VwQm9va2luZ0xpc3RWaWV3JEJvb2tpbmdMaXN0U2VhcmNoSW5wdXRWaWV3JFJhZGlvRm9yQWdlbmN567nTZq194P5buYCpj4PngoYHSIU%3D" +
                    @"&pageToken=" +
                    @"&ControlGroupBookingListView%24BookingListSearchInputView%24Search=ForAgency" +
                    @"&ControlGroupBookingListView%24BookingListSearchInputView%24DropDownListTypeOfSearch=0" +
                    @"&ControlGroupBookingListView%24BookingListSearchInputView%24TextBoxKeyword=KEYWORD" +
                    @"&__VIEWSTATEGENERATOR=05F9A2B0";
                selectRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var selectResponse = clientx.Execute(selectRequest);

                if (selectResponse.ResponseUri.AbsolutePath != "/ChangeItinerary.aspx" &&
                    selectResponse.StatusCode != HttpStatusCode.OK)
                {
                    TableLog.Log = "[Airasia] Response Uri is not ChangeItinerary.aspx or Status is not OK";
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();

                    return new IssueTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier },
                        ErrorMessages = new List<string> { "[AirAsia] Error while requesting at BookingList.aspx to ChangeItinerary. Unexpected response path or response status code || " + selectResponse.Content }
                    };
                }

                // [POST] ChangeItinerary
                TableLog.Log = "[Airasia] [POST] ChangeItinerary.aspx";
                log.Post(TableLog.Log, "#logging-issueflight");
                TableLog.Logging();

                url = "ChangeItinerary.aspx";
                var itinRequest = new RestRequest(url, Method.POST);
                itinRequest.AddHeader("Referer", "https://booking2.airasia.com/ChangeItinerary.aspx");
                postData =
                    @"__EVENTTARGET=ControlGroupChangeItineraryView%24BookingDetailChangeItineraryView%24LinkButtonSubmit" +
                    @"&__EVENTARGUMENT=" +
                    @"&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFTENvbnRyb2xHcm91cENoYW5nZUl0aW5lcmFyeVZpZXckRmxpZ2h0RGlzcGxheUNoYW5nZUl0aW5lcmFyeVZpZXckU3VydmV5Qm94JDCpau1EUzh3Euedl%2FHxvru5LxFgag%3D%3D" +
                    @"&pageToken=" +
                    @"&MemberLoginChangeItineraryView2%24TextBoxUserID=" +
                    @"&hdRememberMeEmail=" +
                    @"&MemberLoginChangeItineraryView2%24PasswordFieldPassword=" +
                    @"&memberLogin_chk_RememberMe=on" +
                    @"&HiddenFieldPageBookingData=" + bookingId +
                    @"&__VIEWSTATEGENERATOR=05F9A2B0";
                itinRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var itinResponse = clientx.Execute(itinRequest);
                var changeItinHtml = itinResponse.Content;

                if (itinResponse.ResponseUri.AbsolutePath != "/PaymentChange.aspx" &&
                    (itinResponse.StatusCode == HttpStatusCode.OK || itinResponse.StatusCode == HttpStatusCode.Redirect))
                {
                    TableLog.Log = "[AirAsia]Response Uri is not PaymentChange.aspx or Status is not (OK Or Redirected)";
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();

                    return new IssueTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier },
                        ErrorMessages = new List<string> { "[AirAsia] Error while requesting at ChangeItinerary.aspx. Unexpected response path or response status code || " + itinResponse.Content }
                    };
                }

                var changeItinCq = (CQ)changeItinHtml;
                var priceValue =
                    changeItinCq["#CONTROLGROUPPAYMENTBOTTOM_PaymentInputViewPaymentView_AgencyAccount_AG_AMOUNT"].Attr(
                        "value");

                try
                {

                    // [POST] PaymentChange
                    TableLog.Log = "[Airasia] [POST] PaymentChange.aspx";
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();

                    url = "PaymentChange.aspx";
                    var paymentRequest = new RestRequest(url, Method.POST);
                    paymentRequest.AddHeader("Referer", "https://booking2.airasia.com/PaymentChange.aspx");
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
                        @"&MCCOriginCountry=ID" +
                        @"&CONTROLGROUPPAYMENTBOTTOM%24PaymentInputViewPaymentView%24AgencyAccount_AG_AMOUNT=" +
                        priceValue +
                        @"&CONTROLGROUPPAYMENTBOTTOM%24ButtonSubmit=Submit+payment" +
                        @"&HiddenFieldPageBookingData=" + bookingId +
                        @"&__VIEWSTATEGENERATOR=05F9A2B0";
                    paymentRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var paymentResponse = clientx.Execute(paymentRequest);

                    if (paymentResponse.ResponseUri.AbsolutePath != "/WaitChange.aspx" &&
                        (paymentResponse.StatusCode == HttpStatusCode.OK ||
                         paymentResponse.StatusCode == HttpStatusCode.Redirect))
                    {
                        TableLog.Log = "[AirAsia] Response Uri is not /WaitChange.aspx or Status is not (OK Or Redirected)";
                        log.Post(TableLog.Log, "#logging-issueflight");
                        TableLog.Logging();
                        throw new Exception();
                    }


                    // [GET] WaitChange
                    TableLog.Log = "[Airasia] [GET] WaitChange.aspx";
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();

                    var sw = Stopwatch.StartNew();
                    var retryLimit = new TimeSpan(0, 1, 0);
                    var retryInterval = new TimeSpan(0, 0, 2);
                    IRestResponse waitResponse = new RestResponse();
                    while (waitResponse.ResponseUri.AbsolutePath != "/ChangeFinalItinerary.aspx" && sw.Elapsed <= retryLimit && (waitResponse.StatusCode == HttpStatusCode.OK || waitResponse.StatusCode == HttpStatusCode.Redirect))
                    {
                        url = "WaitChange.aspx";
                        var waitRequest = new RestRequest(url, Method.GET);
                        waitRequest.AddHeader("Referer", "https://booking2.airasia.com/WaitChange.aspx");
                        waitResponse = clientx.Execute(waitRequest);
                        if (waitResponse.ResponseUri.AbsolutePath != "/ChangeFinalItinerary.aspx" && (waitResponse.StatusCode == HttpStatusCode.OK || waitResponse.StatusCode == HttpStatusCode.Redirect))
                            Thread.Sleep(retryInterval);
                    }

                    if (waitResponse.ResponseUri.AbsolutePath != "/ChangeFinalItinerary.aspx" &&
                        (waitResponse.StatusCode == HttpStatusCode.OK ||
                         waitResponse.StatusCode == HttpStatusCode.Redirect))
                    {
                        TableLog.Log = "[Airasia] Response Uri is not /ChangeFinalItinerary.aspx or Status is not (OK Or Redirected)";
                        log.Post(TableLog.Log, "#logging-issueflight");
                        TableLog.Logging();
                        throw new Exception();
                    }

                    TableLog.Log = "[AirAsia] Done";
                    log.Post(TableLog.Log, "#logging-issueflight");
                    TableLog.Logging();
                    return new IssueTicketResult
                    {
                        IsSuccess = true,
                        BookingId = bookingId
                    };
                }
                catch
                {
                    var isIssued = IsIssued(bookingId);
                    switch (isIssued)
                    {
                        case IssueEnum.IssueSuccess:
                            return new IssueTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(), 
                                IsSuccess = true,
                                BookingId = bookingId,
                                IsInstantIssuance = true
                            };
                        case IssueEnum.NotIssued:
                            return new IssueTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(),
                                IsSuccess = false
                            };
                        case IssueEnum.CheckingError:
                            return new IssueTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(),
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "[AirAsia] Failed to check whether deposit cut or not! Manual checking advised!" }
                            };
                        default:
                            return new IssueTicketResult
                            {
                                CurrentBalance = GetCurrentBalance(),
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "[AirAsia] Failed to check whether deposit cut or not! Manual checking advised!" }
                            };
                    }
                }
            }
        }
    }
}
