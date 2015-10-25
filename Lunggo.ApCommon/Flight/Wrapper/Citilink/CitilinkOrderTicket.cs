using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper.AirAsia;
using Lunggo.Framework.Web;

namespace Lunggo.ApCommon.Flight.Wrapper.Citilink
{
    internal partial class CitilinkWrapper
    {
        internal override OrderTicketResult OrderTicket(string bookingId, FareType fareType)
        {
            return Client.OrderTicket(bookingId);
        }

        private partial class CitilinkClientHandler
        {
            
            internal OrderTicketResult OrderTicket(string bookingId)
            {
                var hariIni = DateTime.Now.Day;
                var bulanIni = DateTime.Now.Month;
                var tahunIni = DateTime.Now.Year;
                var hariIni7 = DateTime.Now.Date.AddDays(7).Day;
                var bulanIni7 = DateTime.Now.Date.AddDays(7).Month;
                var tahunIni7 = DateTime.Now.Date.AddDays(7).Year;

                var client = new ExtendedWebClient();
                Client.CreateSession(client);

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

                var myBooking =
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

                client.UploadString("https://book.citilink.co.id/BookingListTravelAgent.aspx", myBooking);
                if (client.ResponseUri.AbsolutePath != "/BookingListTravelAgent.aspx")
                    return new OrderTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                    };

                client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                //Headers["Accept-Encoding"] = "gzip, deflate";
                client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                client.Headers["User-Agent"] =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.Headers["Referer"] = "https://book.citilink.co.id/BookingListTravelAgent.aspx";
                client.Headers["X-Requested-With"] = "XMLHttpRequest";
                client.Headers["Host"] = "book.citilink.co.id";
                client.Headers["Origin"] = "https://book.citilink.co.id";
                client.Headers["Content-Type"] = "application/x-www-form-urlencoded";

                var pilihBooking =
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

                client.UploadString("https://book.citilink.co.id/BookingListTravelAgent.aspx", pilihBooking);
                if (client.ResponseUri.AbsolutePath != "/Payment.aspx")
                    return new OrderTicketResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                    };

                try
                {
                    client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    //Headers["Accept-Encoding"] = "gzip, deflate";
                    client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                    client.Headers["Upgrade-Insecure-Requests"] = "1";
                    client.Headers["User-Agent"] =
                        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                    client.Headers["Referer"] = "https://book.citilink.co.id/Payment.aspx";
                    client.Headers["X-Requested-With"] = "XMLHttpRequest";
                    client.Headers["Host"] = "book.citilink.co.id";
                    client.Headers["Origin"] = "https://book.citilink.co.id";
                    client.Headers["Content-Type"] = "application/x-www-form-urlencoded";

                    var confirmBook =
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

                    client.UploadString("https://book.citilink.co.id/Payment.aspx", confirmBook);

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
                        return new OrderTicketResult
                        {
                            IsSuccess = false,
                            Errors = new List<FlightError> { FlightError.FailedOnSupplier }
                        };

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

                    //var htmlRespon = System.IO.File.ReadAllText(@"C:\Users\User\Documents\Kerja\Crawl\ItineraryCek.txt");
                    CQ ambilDataRespon = (CQ)htmlRespon;

                    var tunjukStatus = ambilDataRespon["#itineraryBody>"];
                    var tunjukTr = tunjukStatus.MakeRoot()["tr:nth-child(2)>td:nth-child(2)"];
                    var statusPembayaran = tunjukTr.Select(x => x.Cq().Text()).FirstOrDefault();
                    var hasil = new OrderTicketResult();

                    if ((statusPembayaran == "Konfirm") || (statusPembayaran == "Confirmed"))
                        {
                            hasil.BookingId = bookingId;
                            hasil.IsSuccess = true;
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
                            return new OrderTicketResult
                            {
                                IsSuccess = true,
                                BookingId = bookingId
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
