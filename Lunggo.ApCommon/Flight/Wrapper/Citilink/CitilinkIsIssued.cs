using System;
using System.Linq;
using System.Net;
using CsQuery;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Web;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Citilink
{
    internal partial class CitilinkWrapper
    {
        private partial class CitilinkClientHandler
        {
            private IssueEnum IsIssued(string bookingId)
            {
                var hariIni = DateTime.Now.Day;
                var bulanIni = DateTime.Now.Month;
                var tahunIni = DateTime.Now.Year;
                var hariIni7 = DateTime.Now.Date.AddDays(7).Day;
                var bulanIni7 = DateTime.Now.Date.AddDays(7).Month;
                var tahunIni7 = DateTime.Now.Date.AddDays(7).Year;

                const int maxRetryCount = 10;
                var counter = 0;
                var isIssued = (bool?)null;
                while (counter++ < maxRetryCount && isIssued == null)
                {
                    var clientx = CreateAgentClient();

                    Login(clientx);


                    // [GET] BookingList

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
                        continue;

                    
                    // [POST] Cek Itinerary

                    url = "BookingListTravelAgent.aspx";
                    var selectRequest = new RestRequest(url, Method.POST);
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
                    selectRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                    var selectResponse = clientx.Execute(selectRequest);
                    var htmlRespon = selectResponse.Content;


                    if (selectResponse.ResponseUri.AbsolutePath != "/Itinerary.aspx" &&
                        selectResponse.StatusCode != HttpStatusCode.OK)
                        continue;

                    CQ ambilDataRespon = (CQ)htmlRespon;

                    var tunjukStatus = ambilDataRespon["#itineraryBody>"];
                    var tunjukTr = tunjukStatus.MakeRoot()["tr:nth-child(2)>td:nth-child(2)"];
                    var statusPembayaran = tunjukTr.Select(x => x.Cq().Text()).FirstOrDefault();
                    isIssued = tunjukTr.FirstElement() != null;

                    var hasil = new OrderTicketResult();
                    
                    if ((statusPembayaran == "Konfirm") || (statusPembayaran == "Confirmed"))
                    {
                        isIssued = true;
                    }
                    else
                    {
                        isIssued = false;
                    }   
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
