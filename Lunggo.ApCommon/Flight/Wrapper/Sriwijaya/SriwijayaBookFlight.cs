using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Web;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo, FareType fareType)
        {
            var hasil = new BookFlightResult();
            var book = new FlightBookingInfo
            {
                
            };

            var Fare =
                "SJ.017.SJ.272.IN.9662.KNO.WGP?2015-11-11|1.0.0|2346000.0.97174,3853813,1953461:X,M,T:S:KNO:WGP:U2s5VlVrNUZXUT09";
            var ParseFare = Fare.Split('.');
            var FID = ParseFare[(ParseFare.Count() - 1)];


            string FIDsegment1,FIDsegment2, FIDsegment3, ognAirport, arrAirport, penumpang ;
            decimal harga;
            var ParseFID1 = FID.Split(',').ToList();
            DateTime tglBerangkat;
            var Rbd = new List<string>();

            if ((ParseFID1.Count > 1) && (ParseFID1.Count <= 3))
             {
                 FIDsegment1 = ParseFID1[0];
                 FIDsegment2 = ParseFID1[1].Substring(0, (ParseFID1[1].Length - 2));
                 FIDsegment3 = null;
                 var titikIndex1 = FID.IndexOf(":");
                 var titikIndex11 = FID.Substring(titikIndex1 + 1);
                 var titikIndex2 = titikIndex11.IndexOf(":");
                 var rbdRaw = FID.Substring(titikIndex1 + 1, (titikIndex2));
                 Rbd = rbdRaw.Split(',').ToList();
                 var ambilOrigin = FID.Substring((titikIndex2 + 1) + (titikIndex1 + 1));
                 var titikIndex4 = ambilOrigin.IndexOf(":");
                 var titikIndex5 = ambilOrigin.Substring(titikIndex4 + 1).IndexOf(":");
                 ognAirport = ambilOrigin.Substring((titikIndex4 + 1), titikIndex5);
                 var titikIndex6 = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1)).IndexOf(":");
                 arrAirport = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1), titikIndex6);
                 var indexPenumpang1 = Fare.IndexOf('|');
                 var indexPenumpang2 = Fare.Substring((indexPenumpang1 + 1)).IndexOf('|');
                 penumpang = Fare.Substring((indexPenumpang1 + 1), indexPenumpang2);
                 var indextglBerangkat = Fare.IndexOf('?');
                 var tglBerangkatRaw = Fare.Substring((indextglBerangkat + 1), (indexPenumpang1 - 1 - indextglBerangkat));
                 var indexHarga = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2)).IndexOf('.');
                 tglBerangkat = DateTime.Parse(tglBerangkatRaw);
                 var hargaRaw = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2 + 1), indexHarga);
                 harga = Decimal.Parse(hargaRaw);
             }else if ((ParseFID1.Count > 3) && (ParseFID1.Count <= 5))
                {
                    FIDsegment1 = ParseFID1[0];
                    FIDsegment2 = ParseFID1[1];
                    FIDsegment3 = ParseFID1[2].Substring(0, (ParseFID1[2].Length - 2));
                    var titikIndex1 = FID.IndexOf(":");
                    var titikIndex11 = FID.Substring(titikIndex1 + 1);
                    var titikIndex2 = titikIndex11.IndexOf(":");
                    var rbdRaw = FID.Substring(titikIndex1 + 1, (titikIndex2));
                    Rbd = rbdRaw.Split(',').ToList();

                    var ambilOrigin = FID.Substring((titikIndex2+1) + (titikIndex1 + 1));
                    var titikIndex4 = ambilOrigin.IndexOf(":");
                    var titikIndex5 = ambilOrigin.Substring(titikIndex4 + 1).IndexOf(":");
                    ognAirport = ambilOrigin.Substring((titikIndex4+1),titikIndex5);
                    var titikIndex6 = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1)).IndexOf(":");
                    arrAirport = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1), titikIndex6);
                    var indexPenumpang1 = Fare.IndexOf('|');
                    var indexPenumpang2 = Fare.Substring((indexPenumpang1 + 1)).IndexOf('|');
                    penumpang = Fare.Substring((indexPenumpang1 + 1), indexPenumpang2);
                    var indextglBerangkat = Fare.IndexOf('?');
                    var tglBerangkatRaw = Fare.Substring((indextglBerangkat + 1), (indexPenumpang1-1 - indextglBerangkat));
                    tglBerangkat = DateTime.Parse(tglBerangkatRaw);
                    var indexHarga = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2)).IndexOf('.');
                    var hargaRaw = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2+1), indexHarga);
                    harga = Decimal.Parse(hargaRaw);
                }else if (ParseFID1.Count == 1)
                    {
                        FIDsegment1 = ParseFID1[1];
                        FIDsegment2 = null;
                        FIDsegment3 = null;
                        var titikIndex1 = FID.IndexOf(":");
                        var titikIndex11 = FID.Substring(titikIndex1 + 1);
                        var titikIndex2 = titikIndex11.IndexOf(":");
                        var rbdRaw = FID.Substring(titikIndex1 + 1, (titikIndex2));
                        Rbd = rbdRaw.Split(',').ToList();
                        var ambilOrigin = FID.Substring((titikIndex2 + 1) + (titikIndex1 + 1));
                        var titikIndex4 = ambilOrigin.IndexOf(":");
                        var titikIndex5 = ambilOrigin.Substring(titikIndex4 + 1).IndexOf(":");
                        ognAirport = ambilOrigin.Substring((titikIndex4 + 1), titikIndex5);
                        var titikIndex6 = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1)).IndexOf(":");
                        arrAirport = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1), titikIndex6);
                        var indexPenumpang1 = Fare.IndexOf('|');
                        var indexPenumpang2 = Fare.Substring((indexPenumpang1 + 1)).IndexOf('|');
                        penumpang = Fare.Substring((indexPenumpang1 + 1), indexPenumpang2);
                        var indextglBerangkat = Fare.IndexOf('?');
                        var tglBerangkatRaw = Fare.Substring((indextglBerangkat + 1), (indexPenumpang1 - 1 - indextglBerangkat));
                        tglBerangkat = DateTime.Parse(tglBerangkatRaw, CultureInfo.CreateSpecificCulture("id-ID"));
                        var indexHarga = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2)).IndexOf('.');
                        var hargaRaw = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2 + 1), indexHarga);
                        harga = Decimal.Parse(hargaRaw);
                    }
                else
                {
                    ognAirport = null;
                    arrAirport = null;
                    FIDsegment1 = null;
                    FIDsegment2 = null;
                    FIDsegment3 = null;
                    hasil.IsSuccess = false;
                    return hasil;
                }
            var client = new ExtendedWebClient();

            client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            //Headers["Accept-Encoding"] = "gzip, deflate";
            client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
            client.Headers["Upgrade-Insecure-Requests"] = "1";
            client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
            client.Headers["Referer"] = "http://agent.sriwijayaair.co.id/SJ-Eticket/application/index.php?action=index";
            //client.Headers["X-Requested-With"] = "XMLHttpRequest";
            client.Headers["Host"] = "agent.sriwijayaair.co.id";
            client.Headers["Origin"] = "http://agent.sriwijayaair.co.id";
            client.Headers["Content-Type"] = "application/x-www-form-urlencoded";

            var agentBooking =
                "vSub=YES" +
                "&PromoCode=" +
                "&return=NO" +
                "&ruteBerangkat=" + ognAirport +
                "&ruteTujuan=" + arrAirport +
                "&tanggalBerangkat=" + tglBerangkat.ToString("dd-MMM-yyyy") +
                "&ADT=" + penumpang[0] +
                "&CHD=0" + penumpang[1] +
                "&INF=0" + penumpang[2] +
                "&Submit=Search" +
                "&action=booking" +
                "&2210150413=2210150413";

            client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            //client.Headers["Accept-Encoding"] = "gzip, deflate";
            client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
            client.Headers["Upgrade-Insecure-Requests"] = "1";
            client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
            client.Headers["Referer"] = "http://agent.sriwijayaair.co.id/SJ-Eticket/application/?action=booking";
            client.Headers["Host"] = "agent.sriwijayaair.co.id";
            client.Headers["Origin"] = "https://www.sriwijayaair.co.id";
            client.AutoRedirect = false;
            //client.Expect100Continue = false;

            var bookingParams =
                "radioFrom0_0="+ FIDsegment1 +"%3A"+ Rbd[0] +"%3AS%3A"+ ognAirport +"%3A"+ arrAirport +"%3AU2s5VlVrNUZXUT09" +
                "&radioFrom0_1="+ FIDsegment2 +"%3A"+ Rbd[1] +"%3AS%3A"+ ognAirport +"%3A"+ arrAirport +"%3AU2s5VlVrNUZXUT09" +
                "&radioFrom0_2="+ FIDsegment3 +"%3A"+ Rbd[2] +"%3AS%3A"+ ognAirport +"%3A"+ arrAirport +"%3AU2s5VlVrNUZXUT09" ;
                int i = 0;
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Adult))
                {
                    var title = passenger.Title == Title.Mister ? "MR" : "MS";
                    bookingParams +=
                        "&adultTitle"+ i +"=" + title +
                        "&adultFirst"+ i +"=" + passenger.FirstName +
                        "&adultLast"+ i +"=" + passenger.LastName +
                        "&adultId"+ i +"=" + passenger.PassportNumber +
                        "&adultDOB"+ i +"=" + passenger.DateOfBirth +
                        "&adultSSR"+ i +"=" +
                        
                    i++;
                }
                i = 0;
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Child))
                {
                    bookingParams +=
                        "&childTitle"+ i +"=" + passenger.Title +
                        "&childFirst"+ i +"=" + passenger.FirstName +
                        "&childLast"+ i +"=" + passenger.LastName +
                        "&childId"+ i +"=" + 
                        "&childDOB"+ i +"=" + passenger.DateOfBirth +
                        "&childSSR"+ i +"=" +
                    i++;
                }
                i = 0;
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Infant))
                {
                    bookingParams +=
                        "&infantFirst"+ i +"=" + passenger.FirstName +
                        "&infantLast"+ i +"=" + passenger.LastName +
                        "&infantDOB0=" + passenger.DateOfBirth +
                        "&reffInf"+ i +"=" + i +
                    i++;
                }
                
                bookingParams +=
                    
                    "&contactFName=" + bookInfo.ContactData.Name +
                    "&contactLName=" + 
                    "&contactOriginPh=" + bookInfo.ContactData.Phone +
                    "&contactOtherPh=" +
                    "&contactAgenName=PT+TRAVEL+MADEZY+INTERNASIONAL" +
                    "&contactAgenEmail=rama.adhitia%40travelmadezy.com+" +
                    "&contactAgenPh=021-29035088" +
                    "&setoejoe=setoejoe" +
                    "&Submit=Book" +
                    "&action-TEMP=booking" +
                    "&seatADT=" +
                    "&seatCHD=" +
                    "&seatINF=" +
                    "&return=NO" +
                    "&ruteBerangkat=" + ognAirport +
                    "&ruteTujuan=" + arrAirport +
                    "&tanggalBerangkat=" + tglBerangkat.ToString("dd-MMM-yyyy") +
                    "&tanggalTujuan=" +
                    "&ADT=" + penumpang[0] +
                    "&CHD=" + penumpang[1] +
                    "&INF=" + penumpang[2] +
                    "&action=prosesBookingDirect" +
                    "&PromoCode=" +
                    "&features=RD%3ANO" +
                    "&featuring=Um5KdmJUb3dPak09&2410150551=2410150551";

            client.UploadString("http://agent.sriwijayaair.co.id/SJ-Eticket/application/menu_others.php?reffNo=Y0hKdmMyVnpRbTl2YTJsdVowUnBjbVZqZEM0eU5ERXdNVFV3TlRVeE9uQnliM05sYzBKdmIydHBibWRFYVhKbFkzUT0=", bookingParams);
            
            return null;
            
        }
    }
}
