﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Web;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo, FareType fareType)
        {
           
            var sementara = new FlightBookingInfo
            {
                FareId = "SJ.017.SJ.272.IN.9662.KNO.WGP?2015-11-11|1.1.1|2346000.0.3820089,3853792,1953189:X,T,Q:S:KNO:WGP:U2s5VlVrNUZXUT09",
                ContactData = new ContactData
                {
                    Name = "Fani",
                    Phone = "08172182371",
                },
                Passengers = new List<FlightPassenger>
                {
                    new FlightPassenger
                    {
                        FirstName = "Fani",
                        LastName = "Abdullah",
                        DateOfBirth = new DateTime(1976,4,3),
                        Gender = Gender.Male,
                        Title = Title.Mister,
                        PassportNumber = "9320183092141",
                        Type = PassengerType.Adult
                    },
                    //new FlightPassenger
                    //{
                    //    FirstName = "Nina",
                    //    LastName = "Luthvia",
                    //    DateOfBirth = new DateTime(1980,7,5).Date,
                    //    Gender = Gender.Female,
                    //    Title = Title.Miss,
                    //    PassportNumber = "9310182091131",
                    //    Type = PassengerType.Adult
                    //},
                    new FlightPassenger
                    {
                        FirstName = "Habibi",
                        LastName = "",
                        DateOfBirth = new DateTime(2005,11,7).Date,
                        Gender = Gender.Male,
                        Title = Title.Mister,
                        Type = PassengerType.Child
                    },
                    //new FlightPassenger
                    //{
                    //    FirstName = "Dhimas",
                    //    LastName = "Alvian",
                    //    DateOfBirth = new DateTime(2015,2,1).Date,
                    //    Gender = Gender.Male,
                    //    Type = PassengerType.Infant
                    //},
                    new FlightPassenger
                    {
                        FirstName = "Shinta",
                        LastName = "Julia",
                        DateOfBirth = new DateTime(2014,12,19).Date,
                        Gender = Gender.Female,
                        Type = PassengerType.Infant
                    }
                }
            };
            return Client.BookFlight(sementara);
        }

        private partial class SriwijayaClientHandler
        {
            internal BookFlightResult BookFlight(FlightBookingInfo bookInfo)
            {
                var hasil = new BookFlightResult();
                var Fare = bookInfo.FareId; 
                //var Fare =
                //    "SJ.017.SJ.272.IN.9662.KNO.WGP?2015-11-11|1.0.0|2346000.0.97174,3853813,1953461:X,M,T:S:KNO:WGP:U2s5VlVrNUZXUT09";
                var ParseFare = Fare.Split('.');
                var FID = ParseFare[(ParseFare.Count() - 1)];


                string FIDsegment1, FIDsegment2, FIDsegment3, ognAirport, arrAirport, penumpang;
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
                }
                else if ((ParseFID1.Count > 3) && (ParseFID1.Count <= 5))
                {
                    FIDsegment1 = ParseFID1[0];
                    FIDsegment2 = ParseFID1[1];
                    FIDsegment3 = ParseFID1[2].Substring(0, (ParseFID1[2].Length - 2));
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
                    tglBerangkat = DateTime.Parse(tglBerangkatRaw);
                    var indexHarga = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2)).IndexOf('.');
                    var hargaRaw = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2 + 1), indexHarga);
                    harga = Decimal.Parse(hargaRaw);
                }
                else if (ParseFID1.Count == 1)
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
                Client.CreateSession(client);

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

                //client.UploadString("http://agent.sriwijayaair.co.id/SJ-Eticket/application/?action=booking", agentBooking);

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
                    "radioFrom0_0=" + FIDsegment1 + "%3A" + Rbd[0] + "%3AS%3A" + ognAirport + "%3A" + arrAirport + "%3AU2s5VlVrNUZXUT09" +
                    "&radioFrom0_1=" + FIDsegment2 + "%3A" + Rbd[1] + "%3AS%3A" + ognAirport + "%3A" + arrAirport + "%3AU2s5VlVrNUZXUT09" +
                    "&radioFrom0_2=" + FIDsegment3 + "%3A" + Rbd[2] + "%3AS%3A" + ognAirport + "%3A" + arrAirport + "%3AU2s5VlVrNUZXUT09";
                int i = 0;
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Adult))
                {
                    var title = passenger.Title == Title.Mister ? "MR" : "MRS";
                    bookingParams +=
                        "&adultTitle" + i + "=" + title +
                        "&adultFirst" + i + "=" + passenger.FirstName +
                        "&adultLast" + i + "=" + passenger.LastName +
                        "&adultId" + i + "=" + passenger.PassportNumber +
                        "&adultDOB" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd") +
                        "&adultSSR" + i + "=" ;

                    i++;
                }
                i = 0;
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Child))
                {
                    var title = passenger.Title == Title.Mister ? "MSTR" : "MISS";
                    bookingParams +=
                        "&childTitle" + i + "=" + title +
                        "&childFirst" + i + "=" + passenger.FirstName +
                        "&childLast" + i + "=" + passenger.LastName +
                        "&childId" + i + "=" +
                        "&childDOB" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd") +
                        "&childSSR" + i + "=";
                    i++;
                }
                i = 0;
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PassengerType.Infant))
                {
                    bookingParams +=
                        "&infantFirst" + i + "=" + passenger.FirstName +
                        "&infantLast" + i + "=" + passenger.LastName +
                        "&infantDOB"+ i +"=" + passenger.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd") +
                        "&reffInf" + i + "=" + i;
                    i++;
                }

                bookingParams +=

                    "&contactFName=" + bookInfo.ContactData.Name +
                    "&contactLName=" + bookInfo.ContactData.Name +
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
                    "&CHD=" + penumpang[2] +
                    "&INF=" + penumpang[4] +
                    "&action=prosesBookingDirect" +
                    "&PromoCode=" +
                    "&features=RD%3ANO" +
                    "&featuring=Um5KdmJUb3dPak09&2410150551=2410150551";
                client.AutoRedirect = true;
                var bookingResult = client.UploadString("http://agent.sriwijayaair.co.id/SJ-Eticket/application/menu_others.php?reffNo=Y0hKdmMyVnpRbTl2YTJsdVowUnBjbVZqZEM0eU5ERXdNVFV3TlRVeE9uQnliM05sYzBKdmIydHBibWRFYVhKbFkzUT0=", bookingParams);

                if (client.ResponseUri.AbsoluteUri.Contains("/application/?action=Check"))
                {
                    //CQ ambilDataBooking = (CQ)bookingResult;
                    //
                    //var tunjukKodeBook = ambilDataBooking.MakeRoot()[".bookingCode"];
                    //var kodeBook = tunjukKodeBook.Select(x => x.Cq().Attr("value")).FirstOrDefault();
                    

                    client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    //client.Headers["Accept-Encoding"] = "gzip, deflate";
                    client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                    client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                    client.Headers["Upgrade-Insecure-Requests"] = "1";
                    client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                    client.Headers["Referer"] = "http://agent.sriwijayaair.co.id/SJ-Eticket/application/?action=CheckBCode&reffNo=VDWAKK";
                    client.Headers["Host"] = "agent.sriwijayaair.co.id";
                    client.Headers["Origin"] = "https://www.sriwijayaair.co.id";
                    client.AutoRedirect = true;

                    var cekparams =
                        "reffNo=VDWAKK" +
                        "&action=CheckBCode" +
                        "&step=STEP2";

                    var cekresult = client.UploadString("http://agent.sriwijayaair.co.id/SJ-Eticket/application/?", cekparams);

                    CQ ambilTimeLimit = (CQ)cekresult;

                    var tunjukTimeLimit = ambilTimeLimit.MakeRoot()[".timeLimit"];
                    var timelimit = tunjukTimeLimit.Select(x => x.Cq().Text()).FirstOrDefault();
                    var timelimitParse = timelimit.Substring(3).Split(' ');
                    var status = new BookingStatusInfo();
                    //status.BookingId = kodeBook;
                    status.BookingStatus = BookingStatus.Booked;
                    status.TimeLimit = DateTime.Parse(timelimitParse[0]+ "-" + timelimitParse[1] + "-" + timelimitParse[2]+" "+ timelimitParse[3]);
                
                    hasil.Status = status;
                    hasil.IsSuccess = true;
                    return hasil;
                
                }else
                {
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> {FlightError.FareIdNoLongerValid}
                    };
                
                };

                return hasil;
            }
        }
    }
}
