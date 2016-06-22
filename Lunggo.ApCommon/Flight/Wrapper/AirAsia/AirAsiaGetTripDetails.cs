using System.Collections.Generic;
using System.Linq;
using System.Net;
using CsQuery;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
    {
        internal override GetTripDetailsResult GetTripDetails(TripDetailsConditions conditions)
        {
            var rsvNo = FlightService.GetRsvNoByBookingIdFromDb(new List<string> { conditions.BookingId }).Single();
            var reservation = FlightService.GetInstance().GetReservationFromDb(rsvNo);
            var itinerary = reservation.Itineraries.Single(itin => itin.BookingId == conditions.BookingId);
            var segments = itinerary.Trips.SelectMany(trip => trip.Segments).ToList();
            segments.ForEach(segment => segment.Pnr = conditions.BookingId);
            Client.GetTerminalInfoPage(conditions.BookingId, segments);
            var result = new GetTripDetailsResult
            {
                IsSuccess = true,
                BookingId = conditions.BookingId,
                Itinerary = itinerary,
                FlightSegmentCount = segments.Count
            };

            return result;
        }

        private partial class AirAsiaClientHandler
        {
            internal void GetTerminalInfoPage(string bookingId, List<FlightSegment> segments)
            {
                var clientx = CreateAgentClient();

                if (!Login(clientx))
                    return;

                // [GET] BookingList

                var url = "BookingList.aspx";
                var listRequest = new RestRequest(url, Method.GET);
                listRequest.AddHeader("Referer", "https://booking2.airasia.com/AgentHome.aspx");
                var listResponse = clientx.Execute(listRequest);

                if (listResponse.ResponseUri.AbsolutePath != "/BookingList.aspx")
                    return;

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
                    return;

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
                var bookingList = (CQ)itinResponse.Content;
                var depAirportSets = bookingList[".itineraryCustom4:even"].ToList();
                var arrAirportSets = bookingList[".itineraryCustom4:odd"].ToList();
                var x = 0;
                var y = 0;
                foreach (var segment in segments)
                {
                    while (x < depAirportSets.Count)
                    {
                        var depSet = depAirportSets[x].InnerText;
                        var depKurung = "(" + segment.DepartureAirport + ")";
                        x++;
                        if (!depSet.Contains(depKurung))
                            continue;
                        var depKurungStart = depSet.IndexOf('(');
                        var depKurungEnd = depSet.IndexOf(')');
                        if (depSet.IndexOf(depKurung) != depKurungStart)
                        {
                            var depTerminal = depSet.Substring(depKurungStart + 1, depKurungEnd - depKurungStart - 1);
                            segment.DepartureTerminal = depTerminal;
                        }
                        break;
                    }
                    while (y < arrAirportSets.Count)
                    {
                        var arrSet = arrAirportSets[y].InnerText;
                        var arrKurung = "(" + segment.ArrivalAirport + ")";
                        y++;
                        if (!arrSet.Contains(arrKurung))
                            continue;
                        var arrKurungStart = arrSet.IndexOf('(');
                        var arrKurungEnd = arrSet.IndexOf(')');
                        if (arrSet.IndexOf(arrKurung) != arrKurungStart)
                        {
                            var arrTerminal = arrSet.Substring(arrKurungStart + 1, arrKurungEnd - arrKurungStart - 1);
                            segment.ArrivalTerminal = arrTerminal;
                        }
                        break;
                    }
                }
            }
        }
    }
}
