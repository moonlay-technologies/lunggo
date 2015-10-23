using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Web;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
    {
        private partial class AirAsiaClientHandler
        {
            private IssueEnum IsIssued(string bookingId)
            {
                const int maxRetryCount = 10;
                var counter = 0;
                var isIssued = (bool?) null;
                while (counter++ < maxRetryCount && isIssued == null)
                {
                    var client = new ExtendedWebClient();

                    if (!Login(client))
                        continue;

                    // [GET] BookingList

                    client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                    client.Headers["Accept"] =
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                    client.Headers["Accept-Encoding"] = "";
                    client.Headers["Upgrade-Insecure-Requests"] = "1";
                    client.Headers["User-Agent"] =
                        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                    client.Headers["Origin"] = "https://booking2.airasia.com";
                    client.Headers["Referer"] = "https://booking2.airasia.com/AgentHome.aspx";

                    client.DownloadString("https://booking2.airasia.com/BookingList.aspx");

                    if (client.ResponseUri.AbsolutePath != "/BookingList.aspx")
                        continue;

                    // [POST] BookingList

                    client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                    client.Headers["Accept"] =
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
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

                    if (client.ResponseUri.AbsolutePath != "/BookingList.aspx")
                        continue;

                    // [POST] BookingList -> ChangeItinerary

                    client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                    client.Headers["Accept"] =
                        "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
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

                    var bookingList = client.UploadString("https://booking2.airasia.com/BookingList.aspx", postData);

                    if (client.ResponseUri.AbsolutePath != "/ChangeItinerary.aspx" &&
                        client.StatusCode != HttpStatusCode.OK)
                        continue;

                    var bookingListCq = (CQ) bookingList;
                    var confirmElement = bookingListCq[".confirm"];
                    isIssued = confirmElement.FirstElement() != null;
                }
                switch (isIssued)
                {
                    case null:
                        return IssueEnum.CheckingError;
                    case true:
                        return IssueEnum.IssueSuccess;
                    case false:
                        return IssueEnum.NotIssued;
                    default:
                        return IssueEnum.CheckingError;
                }
            }

            private enum IssueEnum
            {
                IssueSuccess,
                NotIssued,
                CheckingError
            }

        }
    }
}
