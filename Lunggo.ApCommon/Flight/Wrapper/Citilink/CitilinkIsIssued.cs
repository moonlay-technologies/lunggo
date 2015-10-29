using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                    var client = new ExtendedWebClient();

                    Client.CreateSession(client);


                    // [GET] BookingList

                    client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    //Headers["Accept-Encoding"] = "gzip, deflate";
                    client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                    client.Headers["Upgrade-Insecure-Requests"] = "1";
                    client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                    client.Headers["Referer"] = "https://book.citilink.co.id/BookingListTravelAgent.aspx";
                    client.Headers["X-Requested-With"] = "XMLHttpRequest";
                    client.Headers["Host"] = "book.citilink.co.id";
                    client.Headers["Origin"] = "https://book.citilink.co.id";
                    client.Headers["Content-Type"] = "application/x-www-form-urlencoded";

                    var myBooking2 =
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

                    client.UploadString("https://book.citilink.co.id/BookingListTravelAgent.aspx", myBooking2);

                    if (client.ResponseUri.AbsolutePath != "/BookingListTravelAgent.aspx")
                        continue;

                    
                    // [POST] Cek Itinerary

                    client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    //Headers["Accept-Encoding"] = "gzip, deflate";
                    client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                    client.Headers["Upgrade-Insecure-Requests"] = "1";
                    client.Headers["User-Agent"] =
                        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                    client.Headers["Referer"] = "Referer: https://book.citilink.co.id/BookingListTravelAgent.aspx";
                    client.Headers["X-Requested-With"] = "XMLHttpRequest";
                    client.Headers["Host"] = "book.citilink.co.id";
                    client.Headers["Origin"] = "https://book.citilink.co.id";
                    client.Headers["Content-Type"] = "application/x-www-form-urlencoded";

                    var cekItinerary =
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

                    var htmlRespon = client.UploadString("https://book.citilink.co.id/BookingListTravelAgent.aspx", cekItinerary);


                    if (client.ResponseUri.AbsolutePath != "/Itinerary.aspx" &&
                        client.StatusCode != HttpStatusCode.OK)
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
