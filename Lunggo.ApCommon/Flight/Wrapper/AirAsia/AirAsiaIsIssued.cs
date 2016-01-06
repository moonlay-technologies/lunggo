using System.Net;
using CsQuery;
using Lunggo.Framework.Web;
using RestSharp;

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
                    var clientx = CreateAgentClient();

                    if (!Login(clientx))
                        continue;

                    // [GET] BookingList

                    var url = "BookingList.aspx";
                    var listRequest = new RestRequest(url, Method.GET);
                    listRequest.AddHeader("Referer", "https://booking2.airasia.com/AgentHome.aspx");
                    var listResponse = clientx.Execute(listRequest);

                    if (listResponse.ResponseUri.AbsolutePath != "/BookingList.aspx")
                        continue;

                    // [POST] BookingList

                    url = "BookingList.aspx";
                    var selectRequest = new RestRequest(url, Method.POST);
                    selectRequest.AddHeader("Referer", "https://booking2.airasia.com/BookingList.aspx");
                    var postData =
                        @"__EVENTTARGET=ControlGroupBookingListView%24BookingListSearchInputView%24LinkButtonFindBooking" +
                        @"&__EVENTARGUMENT=" +
                        @"&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFRENvbnRyb2xHcm91cEJvb2tpbmdMaXN0VmlldyRCb29raW5nTGlzdFNlYXJjaElucHV0VmlldyRSYWRpb0ZvckFnZW50BUVDb250cm9sR3JvdXBCb29raW5nTGlzdFZpZXckQm9va2luZ0xpc3RTZWFyY2hJbnB1dFZpZXckUmFkaW9Gb3JBZ2VuY3kFRUNvbnRyb2xHcm91cEJvb2tpbmdMaXN0VmlldyRCb29raW5nTGlzdFNlYXJjaElucHV0VmlldyRSYWRpb0ZvckFnZW5jeVdjug1NBbrhcyWlW33sJWOJ%2ByEA" +
                        @"&pageToken=" +
                        @"&ControlGroupBookingListView%24BookingListSearchInputView%24Search=ForAgency" +
                        @"&ControlGroupBookingListView%24BookingListSearchInputView%24DropDownListTypeOfSearch=0" +
                        @"&ControlGroupBookingListView%24BookingListSearchInputView%24TextBoxKeyword=KEYWORD" +
                        @"&__VIEWSTATEGENERATOR=05F9A2B0";
                    selectRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var selectResponse = clientx.Execute(selectRequest);

                    if (selectResponse.ResponseUri.AbsolutePath != "/BookingList.aspx")
                        continue;

                    // [POST] BookingList -> ChangeItinerary

                    url = "BookingList.aspx";
                    var itinRequest = new RestRequest(url, Method.POST);
                    listRequest.AddHeader("Referer", "https://booking2.airasia.com/BookingList.aspx");
                    postData =
                        @"__EVENTTARGET=ControlGroupBookingListView%24BookingListSearchInputView" +
                        @"&__EVENTARGUMENT=Edit%3A" + bookingId +
                        @"&__VIEWSTATE=%2FwEPDwUBMGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgMFRENvbnRyb2xHcm91cEJvb2tpbmdMaXN0VmlldyRCb29raW5nTGlzdFNlYXJjaElucHV0VmlldyRSYWRpb0ZvckFnZW50BURDb250cm9sR3JvdXBCb29raW5nTGlzdFZpZXckQm9va2luZ0xpc3RTZWFyY2hJbnB1dFZpZXckUmFkaW9Gb3JBZ2VudAVFQ29udHJvbEdyb3VwQm9va2luZ0xpc3RWaWV3JEJvb2tpbmdMaXN0U2VhcmNoSW5wdXRWaWV3JFJhZGlvRm9yQWdlbmN567nTZq194P5buYCpj4PngoYHSIU%3D" +
                        @"&pageToken=" +
                        @"&ControlGroupBookingListView%24BookingListSearchInputView%24Search=ForAgency" +
                        @"&ControlGroupBookingListView%24BookingListSearchInputView%24DropDownListTypeOfSearch=0" +
                        @"&ControlGroupBookingListView%24BookingListSearchInputView%24TextBoxKeyword=KEYWORD" +
                        @"&__VIEWSTATEGENERATOR=05F9A2B0";
                    itinRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var itinResponse = clientx.Execute(itinRequest);
                    var bookingList = itinResponse.Content;

                    if (itinResponse.ResponseUri.AbsolutePath != "/ChangeItinerary.aspx" &&
                        itinResponse.StatusCode != HttpStatusCode.OK)
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
