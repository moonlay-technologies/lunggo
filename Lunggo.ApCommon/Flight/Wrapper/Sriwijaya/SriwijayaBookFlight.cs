using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Web;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo)
        {
           
            //var sementara = new FlightBookingInfo
            //{
            //    FareId = "SJ.017.SJ.272.IN.9662.KNO.WGP?2015-11-11|1.1.1|2346000.0.3820089,3853792,1953189:X,T,Q:S:KNO:WGP:U2s5VlVrNUZXUT09",
            //    ContactData = new ContactData
            //    {
            //        Name = "Fani",
            //        Phone = "08172182371",
            //    },
            //    Passengers = new List<FlightPassenger>
            //    {
            //        new FlightPassenger
            //        {
            //            FirstName = "Fani",
            //            LastName = "Abdullah",
            //            DateOfBirth = new DateTime(1976,4,3),
            //            Gender = Gender.Male,
            //            Title = Title.Mister,
            //            PassportNumber = "9320183092141",
            //            Type = PassengerType.Adult
            //        },
            //        //new FlightPassenger
            //        //{
            //        //    FirstName = "Nina",
            //        //    LastName = "Luthvia",
            //        //    DateOfBirth = new DateTime(1980,7,5).Date,
            //        //    Gender = Gender.Female,
            //        //    Title = Title.Miss,
            //        //    PassportNumber = "9310182091131",
            //        //    Type = PassengerType.Adult
            //        //},
            //        new FlightPassenger
            //        {
            //            FirstName = "Habibi",
            //            LastName = "",
            //            DateOfBirth = new DateTime(2005,11,7).Date,
            //            Gender = Gender.Male,
            //            Title = Title.Mister,
            //            Type = PassengerType.Child
            //        },
            //        //new FlightPassenger
            //        //{
            //        //    FirstName = "Dhimas",
            //        //    LastName = "Alvian",
            //        //    DateOfBirth = new DateTime(2015,2,1).Date,
            //        //    Gender = Gender.Male,
            //        //    Type = PassengerType.Infant
            //        //},
            //        new FlightPassenger
            //        {
            //            FirstName = "Shinta",
            //            LastName = "Julia",
            //            DateOfBirth = new DateTime(2014,12,19).Date,
            //            Gender = Gender.Female,
            //            Type = PassengerType.Infant
            //        }
            //    }
            //};
            return Client.BookFlight(bookInfo);
        }

        private partial class SriwijayaClientHandler
        {
            internal BookFlightResult BookFlight(FlightBookingInfo bookInfo)
            {
                var clientx = CreateAgentClient();
                var hasil = new BookFlightResult();
                var Fare = bookInfo.FareId; 
                //var Fare =
                //    "SJ.017.SJ.272.IN.9662.KNO.WGP?2015-11-11|1.0.0|2346000.0.97174,3853813,1953461:X,M,T:S:KNO:WGP:U2s5VlVrNUZXUT09";
                var ParseFare = Fare.Split('.');
                var FID = ParseFare[(ParseFare.Count() - 1)];


                string FIDsegment1, FIDsegment2, FIDsegment3, ognAirport, arrAirport, penumpangRaw, bookingParams, unknownCode;
                decimal harga;
                int jumlahSegment;
                var penumpang = new List<string>();
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
                    var findUnknownCode = FID.Substring(titikIndex1 + 1, (titikIndex2 + 2));
                    var titikU1 = findUnknownCode.IndexOf(":");
                    unknownCode = findUnknownCode.Substring(titikU1 + 1);
                    var ambilOrigin = FID.Substring((titikIndex2 + 1) + (titikIndex1 + 1));
                    var titikIndex4 = ambilOrigin.IndexOf(":");
                    var titikIndex5 = ambilOrigin.Substring(titikIndex4 + 1).IndexOf(":");
                    ognAirport = ambilOrigin.Substring((titikIndex4 + 1), titikIndex5);
                    var titikIndex6 = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1)).IndexOf(":");
                    arrAirport = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1), titikIndex6);
                    var indexPenumpang1 = Fare.IndexOf('|');
                    var indexPenumpang2 = Fare.Substring((indexPenumpang1 + 1)).IndexOf('|');
                    penumpangRaw = Fare.Substring((indexPenumpang1 + 1), indexPenumpang2);
                    penumpang = penumpangRaw.Split('.').ToList();
                    var indextglBerangkat = Fare.IndexOf('?');
                    var tglBerangkatRaw = Fare.Substring((indextglBerangkat + 1), (indexPenumpang1 - 1 - indextglBerangkat));
                    var indexHarga = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2)).IndexOf('.');
                    tglBerangkat = DateTime.Parse(tglBerangkatRaw);
                    var hargaRaw = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2 + 1), indexHarga);
                    harga = Decimal.Parse(hargaRaw);
                    jumlahSegment = 2;

                    bookingParams =
                        "radioFrom0_0=" + FIDsegment1 + "%3A" + Rbd[0] + "%3A" +unknownCode+ "%3A" + ognAirport + "%3A" + arrAirport +
                        "%3AU2s5VlVrNUZXUT09" +
                        "&radioFrom0_1=" + FIDsegment2 + "%3A" + Rbd[1] + "%3A" +unknownCode+ "%3A" + ognAirport + "%3A" + arrAirport +
                        "%3AU2s5VlVrNUZXUT09";

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
                    var findUnknownCode = FID.Substring(titikIndex1 + 1, (titikIndex2 + 2));
                    var titikU1 = findUnknownCode.IndexOf(":");
                    unknownCode = findUnknownCode.Substring(titikU1 + 1);
                    var ambilOrigin = FID.Substring((titikIndex2 + 1) + (titikIndex1 + 1));
                    var titikIndex4 = ambilOrigin.IndexOf(":");
                    var titikIndex5 = ambilOrigin.Substring(titikIndex4 + 1).IndexOf(":");
                    ognAirport = ambilOrigin.Substring((titikIndex4 + 1), titikIndex5);
                    var titikIndex6 = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1)).IndexOf(":");
                    arrAirport = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1), titikIndex6);
                    var indexPenumpang1 = Fare.IndexOf('|');
                    var indexPenumpang2 = Fare.Substring((indexPenumpang1 + 1)).IndexOf('|');
                    penumpangRaw = Fare.Substring((indexPenumpang1 + 1), indexPenumpang2);
                    penumpang = penumpangRaw.Split('.').ToList();
                    var indextglBerangkat = Fare.IndexOf('?');
                    var tglBerangkatRaw = Fare.Substring((indextglBerangkat + 1), (indexPenumpang1 - 1 - indextglBerangkat));
                    tglBerangkat = DateTime.Parse(tglBerangkatRaw);
                    var indexHarga = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2)).IndexOf('.');
                    var hargaRaw = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2 + 1), indexHarga);
                    harga = Decimal.Parse(hargaRaw);
                    jumlahSegment = 3;

                    bookingParams =
                    "radioFrom0_0=" + FIDsegment1 + "%3A" + Rbd[0] + "%3A" +unknownCode+ "%3A" + ognAirport + "%3A" + arrAirport + "%3AU2s5VlVrNUZXUT09" +
                    "&radioFrom0_1=" + FIDsegment2 + "%3A" + Rbd[1] + "%3A" +unknownCode+ "%3A" + ognAirport + "%3A" + arrAirport + "%3AU2s5VlVrNUZXUT09" +
                    "&radioFrom0_2=" + FIDsegment3 + "%3A" + Rbd[2] + "%3A" +unknownCode+ "%3A" + ognAirport + "%3A" + arrAirport + "%3AU2s5VlVrNUZXUT09";
                }
                else if (ParseFID1.Count == 1)
                {
                    FIDsegment1 = ParseFID1[0];
                    FIDsegment2 = null;
                    FIDsegment3 = null;
                    var titikIndex1 = FID.IndexOf(":");
                    var titikIndex11 = FID.Substring(titikIndex1 + 1);
                    var titikIndex2 = titikIndex11.IndexOf(":");
                    var rbdRaw = FID.Substring(titikIndex1 + 1, (titikIndex2));
                    Rbd = rbdRaw.Split(',').ToList();
                    var findUnknownCode = FID.Substring(titikIndex1 + 1, (titikIndex2 + 2));
                    var titikU1 = findUnknownCode.IndexOf(":");
                    unknownCode = findUnknownCode.Substring(titikU1 + 1);
                    var ambilOrigin = FID.Substring((titikIndex2 + 1) + (titikIndex1 + 1));
                    var titikIndex4 = ambilOrigin.IndexOf(":");
                    var titikIndex5 = ambilOrigin.Substring(titikIndex4 + 1).IndexOf(":");
                    ognAirport = ambilOrigin.Substring((titikIndex4 + 1), titikIndex5);
                    var titikIndex6 = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1)).IndexOf(":");
                    arrAirport = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1), titikIndex6);
                    var indexPenumpang1 = Fare.IndexOf('|');
                    var indexPenumpang2 = Fare.Substring((indexPenumpang1 + 1)).IndexOf('|');
                    penumpangRaw = Fare.Substring((indexPenumpang1 + 1), indexPenumpang2);
                    penumpang = penumpangRaw.Split('.').ToList();
                    var indextglBerangkat = Fare.IndexOf('?');
                    var tglBerangkatRaw = Fare.Substring((indextglBerangkat + 1), (indexPenumpang1 - 1 - indextglBerangkat));
                    tglBerangkat = DateTime.Parse(tglBerangkatRaw, CultureInfo.CreateSpecificCulture("id-ID"));
                    var indexHarga = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2)).IndexOf('.');
                    var hargaRaw = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2 + 1), indexHarga);
                    harga = Decimal.Parse(hargaRaw);
                    jumlahSegment = 1;

                    bookingParams =
                        "radioFrom=" + FIDsegment1 + "%3A" + Rbd[0] + "%3A" +unknownCode+ "%3A" + ognAirport + "%3A" + arrAirport +
                        "%3AU2s5VlVrNUZXUT09";

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

                Login(clientx);

                var tahun2dgt = tglBerangkat.Year.ToString().Substring(2, 2);
                var url = "SJ-Eticket/application/?action=booking";
                var searchRequest = new RestRequest(url, Method.POST);
                var postData =
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
                    "&" + DateTime.Now.Day.ToString("d2") + DateTime.Now.Month.ToString("d2") + tahun2dgt + (((DateTime.Now.Hour + 11) % 12) + 1) + DateTime.Now.Minute + "=" + DateTime.Now.Day.ToString("d2") + DateTime.Now.Month.ToString("d2") + tahun2dgt + (((DateTime.Now.Hour + 11) % 12) + 1) + DateTime.Now.Minute;
                searchRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var searchResponse = clientx.Execute(searchRequest);
                
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

                tahun2dgt = tglBerangkat.Year.ToString().Substring(2,2);
                var featuringEncode1 = ("From:0:" + jumlahSegment + "").Base64Encode();
                var featuringEncode2 = featuringEncode1.Base64Encode();

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
                    "&CHD=" + penumpang[1] +
                    "&INF=" + penumpang[2] +
                    "&action=prosesBookingDirect" +
                    "&PromoCode=" +
                    "&features=RD%3ANO" +
                    "&featuring=" + featuringEncode2 +
                    "&" + DateTime.Now.Day.ToString("d2") + DateTime.Now.Month.ToString("d2") + tahun2dgt + (((DateTime.Now.Hour + 11) % 12) + 1) + DateTime.Now.Minute + "=" + DateTime.Now.Day.ToString("d2") + DateTime.Now.Month.ToString("d2") + tahun2dgt + (((DateTime.Now.Hour + 11) % 12) + 1) + DateTime.Now.Minute;
                var encode1 =
                    ("prosesBookingDirect." +DateTime.Now.Day.ToString("d2") + DateTime.Now.Month.ToString("d2") + tahun2dgt + (((DateTime.Now.Hour + 11) % 12) + 1) + DateTime.Now.Minute+ ":prosesBookingDirect").Base64Encode();
                var encode2 = encode1.Base64Encode();

                url = "SJ-Eticket/application/menu_others.php?reffNo=" + encode2;
                var bookRequest = new RestRequest(url, Method.POST);
                bookRequest.AddParameter("application/x-www-form-urlencoded", bookingParams, ParameterType.RequestBody);
                var bookResponse = clientx.Execute(bookRequest);
                var bookingResult = bookResponse.Content;
                //var bookingResult = System.IO.File.ReadAllText(@"C:\Users\User\Documents\Kerja\Crawl\mbuhlali.txt");
                
                if (bookResponse.ResponseUri.AbsoluteUri.Contains("/application/?action=Check"))
                {
                    CQ ambilDataBooking = (CQ)bookingResult;
                    
                    var tunjukKodeBook = ambilDataBooking.MakeRoot()[".bookingCode>input"];
                    var kodeBook = tunjukKodeBook.Select(x => x.Cq().Attr("value")).FirstOrDefault();

                    url = "SJ-Eticket/application/?";
                    var checkRequest = new RestRequest(url, Method.POST);
                    var checkParams =
                        "reffNo=" + kodeBook +
                        "&action=CheckBCode" +
                        "&step=STEP2";
                    checkRequest.AddParameter("application/x-www-form-urlencoded", checkParams, ParameterType.RequestBody);
                    var checkResponse = clientx.Execute(checkRequest);
                    var cekresult = checkResponse.Content;
                    
                    CQ ambilTimeLimit = (CQ)cekresult;

                    var tunjukTimeLimit = ambilTimeLimit.MakeRoot()[".timeLimit"];
                    var timelimit = tunjukTimeLimit.Select(x => x.Cq().Text()).FirstOrDefault();
                    var timelimitParse = timelimit.Substring(3).Split(' ');
                    var status = new BookingStatusInfo();
                    string format = "dd/MM/yyyy h:mm:ss tt z";
                    //CultureInfo provider = CultureInfo.InvariantCulture;
                    var timeLimitDate = DateTime.Parse(timelimitParse[0]+ "/" + timelimitParse[1] + "/" + timelimitParse[2]+" "+timelimitParse[3]);
                    var timeLimitGMT = timelimitParse[4].Substring(4, 2);
                    var timeLimitFinal = DateTime.Parse(timeLimitDate + " " + timeLimitGMT);
                    
                    status.BookingId = kodeBook;
                    status.BookingStatus = BookingStatus.Booked;
                    status.TimeLimit = timeLimitFinal.ToUniversalTime();
                
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
                
                }
            }
        }
    }
}
