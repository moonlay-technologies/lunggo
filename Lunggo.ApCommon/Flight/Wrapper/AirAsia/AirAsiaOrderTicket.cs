using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Web;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
    {
        internal override OrderTicketResult OrderTicket(string bookingId, FareType fareType)
        {
            return Client.OrderTicket(bookingId);
        }

        private partial class AirAsiaClientHandler
        {
            internal OrderTicketResult OrderTicket(string bookingId)
            {
                var client = new ExtendedWebClient();

                if (!Login(client))
                    return new OrderTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { "Can't Login!" }
                    };

                // [GET] BookingList

                client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                client.Headers["Accept-Encoding"] = "";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                client.Headers["User-Agent"] =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.Headers["Origin"] = "https://booking2.airasia.com";
                client.Headers["Referer"] = "https://booking2.airasia.com/AgentHome.aspx";

                client.DownloadString("https://booking2.airasia.com/BookingList.aspx");

                if (client.ResponseUri.AbsolutePath != "/BookingList.aspx" && (client.StatusCode == HttpStatusCode.OK || client.StatusCode == HttpStatusCode.Redirect))
                    return new OrderTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                    };

                // [POST] BookingList

                client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                client.Headers["Accept-Encoding"] = "";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                client.Headers["User-Agent"] =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.Headers["Origin"] = "https://booking2.airasia.com";
                client.Headers["Referer"] = "https://booking2.airasia.com/BookingList.aspx";
                var postData =
                    @"__EVENTTARGET=ControlGroupBookingListView%24BookingListSearchInputView%24LinkButtonFindBooking" +
                    @"&__EVENTARGUMENT=" +
                    @"&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFRENvbnRyb2xHcm91cEJvb2tpbmdMaXN0VmlldyRCb29raW5nTGlzdFNlYXJjaElucHV0VmlldyRSYWRpb0ZvckFnZW50BUVDb250cm9sR3JvdXBCb29raW5nTGlzdFZpZXckQm9va2luZ0xpc3RTZWFyY2hJbnB1dFZpZXckUmFkaW9Gb3JBZ2VuY3kFRUNvbnRyb2xHcm91cEJvb2tpbmdMaXN0VmlldyRCb29raW5nTGlzdFNlYXJjaElucHV0VmlldyRSYWRpb0ZvckFnZW5jeVdjug1NBbrhcyWlW33sJWOJ%2ByEA" +
                    @"&pageToken=" +
                    @"&ControlGroupBookingListView%24BookingListSearchInputView%24Search=ForAgency" +
                    @"&ControlGroupBookingListView%24BookingListSearchInputView%24DropDownListTypeOfSearch=0" +
                    @"&ControlGroupBookingListView%24BookingListSearchInputView%24TextBoxKeyword=KEYWORD" +
                    @"&__VIEWSTATEGENERATOR=05F9A2B0";

                client.UploadString("https://booking2.airasia.com/BookingList.aspx", postData);

                if (client.ResponseUri.AbsolutePath != "/BookingList.aspx" && (client.StatusCode == HttpStatusCode.OK || client.StatusCode == HttpStatusCode.Redirect))
                    return new OrderTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                    };

                // [POST] BookingList -> ChangeItinerary

                client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                client.Headers["Accept-Encoding"] = "";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                client.Headers["User-Agent"] =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.Headers["Origin"] = "https://booking2.airasia.com";
                client.Headers["Referer"] = "https://booking2.airasia.com/BookingList.aspx";
                postData =
                    @"__EVENTTARGET=ControlGroupBookingListView%24BookingListSearchInputView" +
                    @"&__EVENTARGUMENT=Edit%3A" + bookingId +
                    @"&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFRENvbnRyb2xHcm91cEJvb2tpbmdMaXN0VmlldyRCb29raW5nTGlzdFNlYXJjaElucHV0VmlldyRSYWRpb0ZvckFnZW50BURDb250cm9sR3JvdXBCb29raW5nTGlzdFZpZXckQm9va2luZ0xpc3RTZWFyY2hJbnB1dFZpZXckUmFkaW9Gb3JBZ2VudAVFQ29udHJvbEdyb3VwQm9va2luZ0xpc3RWaWV3JEJvb2tpbmdMaXN0U2VhcmNoSW5wdXRWaWV3JFJhZGlvRm9yQWdlbmN567nTZq194P5buYCpj4PngoYHSIU%3D" +
                    @"&pageToken=" +
                    @"&ControlGroupBookingListView%24BookingListSearchInputView%24Search=ForAgency" +
                    @"&ControlGroupBookingListView%24BookingListSearchInputView%24DropDownListTypeOfSearch=0" +
                    @"&ControlGroupBookingListView%24BookingListSearchInputView%24TextBoxKeyword=KEYWORD" +
                    @"&__VIEWSTATEGENERATOR=05F9A2B0";

                client.UploadString("https://booking2.airasia.com/BookingList.aspx", postData);

                if (client.ResponseUri.AbsolutePath != "/ChangeItinerary.aspx" && client.StatusCode != HttpStatusCode.OK)
                    return new OrderTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                    };

                // [POST] ChangeItinerary

                client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                client.Headers["Accept-Encoding"] = "";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                client.Headers["User-Agent"] =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.Headers["Origin"] = "https://booking2.airasia.com";
                client.Headers["Referer"] = "https://booking2.airasia.com/ChangeItinerary.aspx";
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

                var changeItinHtml = client.UploadString("https://booking2.airasia.com/ChangeItinerary.aspx", postData);

                if (client.ResponseUri.AbsolutePath != "/PaymentChange.aspx" && (client.StatusCode == HttpStatusCode.OK || client.StatusCode == HttpStatusCode.Redirect))
                    return new OrderTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                    };

                var changeItinCq = (CQ)changeItinHtml;
                var priceValue =
                    changeItinCq["#CONTROLGROUPPAYMENTBOTTOM_PaymentInputViewPaymentView_AgencyAccount_AG_AMOUNT"].Attr(
                        "value");

                try
                {

                    // [POST] PaymentChange

                    client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                    client.Headers["Accept"] =
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                    client.Headers["Accept-Encoding"] = "";
                    client.Headers["Upgrade-Insecure-Requests"] = "1";
                    client.Headers["User-Agent"] =
                        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                    client.Headers["Origin"] = "https://booking2.airasia.com";
                    client.Headers["Referer"] = "https://booking2.airasia.com/PaymentChange.aspx";
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

                    client.UploadString("https://booking2.airasia.com/PaymentChange.aspx", postData);

                    if (client.ResponseUri.AbsolutePath != "/WaitChange.aspx" && (client.StatusCode == HttpStatusCode.OK || client.StatusCode == HttpStatusCode.Redirect))
                        throw new Exception();

                    // [GET] WaitChange

                    var sw = Stopwatch.StartNew();
                    var retryLimit = new TimeSpan(0, 1, 0);
                    var retryInterval = new TimeSpan(0, 0, 2);
                    while (client.ResponseUri.AbsolutePath != "/ChangeFinalItinerary.aspx" && sw.Elapsed <= retryLimit && (client.StatusCode == HttpStatusCode.OK || client.StatusCode == HttpStatusCode.Redirect))
                    {
                        client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                        client.Headers["Accept"] =
                            "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                        client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                        client.Headers["Accept-Encoding"] = "";
                        client.Headers["Upgrade-Insecure-Requests"] = "1";
                        client.Headers["User-Agent"] =
                            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                        client.Headers["Origin"] = "https://booking2.airasia.com";
                        client.Headers["Referer"] = "https://booking2.airasia.com/WaitChange.aspx";

                        client.DownloadString("https://booking2.airasia.com/WaitChange.aspx");
                        if (client.ResponseUri.AbsolutePath != "/ChangeFinalItinerary.aspx" && (client.StatusCode == HttpStatusCode.OK || client.StatusCode == HttpStatusCode.Redirect))
                            Thread.Sleep(retryInterval);
                    }

                    if (client.ResponseUri.AbsolutePath != "/ChangeFinalItinerary.aspx" && (client.StatusCode == HttpStatusCode.OK || client.StatusCode == HttpStatusCode.Redirect))
                        throw new Exception();

                    return new OrderTicketResult
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
                            return new OrderTicketResult
                            {
                                IsSuccess = true,
                                BookingId = bookingId,
                                IsInstantIssuance = true
                            };
                        case IssueEnum.NotIssued:
                            return new OrderTicketResult
                            {
                                IsSuccess = false
                            };
                        case IssueEnum.CheckingError:
                            return new OrderTicketResult
                            {
                                IsSuccess = false,
                                Errors = new List<FlightError> { FlightError.TechnicalError },
                                ErrorMessages = new List<string> { "Failed to check whether deposit cut or not! Manual checking advised!" }
                            };
                        default:
                            return new OrderTicketResult
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
