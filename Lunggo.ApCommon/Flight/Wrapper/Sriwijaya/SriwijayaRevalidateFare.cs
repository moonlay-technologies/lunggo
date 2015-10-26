using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Web;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
         internal override RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
         {
             var client = new ExtendedWebClient();
             client.Expect100Continue = false;
             client.AutoRedirect = true;
             Client.CreateSession(client);
             var hasil = new RevalidateFareResult();
             var Fare = conditions.FareId;
             var ParseFare = Fare.Split('.');
             var FID = ParseFare[(ParseFare.Count() - 1)];
             //var Fare =
             //    "SJ.017.SJ.272.IN.9662.KNO.WGP?2015-11-11|1.0.0|2346000.0.97174,3853813,1953461:X,M,T:S:KNO:WGP:U2s5VlVrNUZXUT09";
             //var FID = "3820089,3853785,1953197:X,V,T:S:KNO:WGP:U2s5VlVrNUZXUT09";
             //var fare =
             //    "3820089:X:S:KNO:WGP:U2s5VlVrNUZXUT09;;3853785:V:S:KNO:WGP:U2s5VlVrNUZXUT09;;1953197:T:S:KNO:WGP:U2s5VlVrNUZXUT09";
             var ParseFID1 = FID.Split(',').ToList();

            
             string FIDsegment1;
             string FIDsegment2;
             string FIDsegment3;
             string ognAirport;
             string arrAirport;
             string penumpang;
             decimal harga;

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
                 var hargaRaw = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2 + 1), indexHarga-1);
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
                    var hargaRaw = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2+1), indexHarga-1);
                    harga = Decimal.Parse(hargaRaw);
                }else if (ParseFID1.Count == 1)
                    {
                        FIDsegment1 = ParseFID1[0];
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
                        var hargaRaw = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2 + 1), indexHarga-1);
                        harga = Decimal.Parse(hargaRaw);
                    }
                else
                {
                    ognAirport = null;
                    arrAirport = null;
                    FIDsegment1 = null;
                    FIDsegment2 = null;
                    FIDsegment3 = null;

                    return new RevalidateFareResult
                    {
                       IsValid = false,
                       Errors = new List<FlightError> { FlightError.TechnicalError },
                       ErrorMessages = new List<string> { "Web Layout Changed!" }
                    };
                }
            

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
                 "&CHD=0" + penumpang[2] +
                 "&INF=0" + penumpang[4] +
                 "&Submit=Search" +
                 "&action=booking" +
                 "&2210150413=2210150413";
        
             var htmlRespon = client.UploadString("http://agent.sriwijayaair.co.id/SJ-Eticket/application/?action=booking", agentBooking);

             //var htmlRespon = System.IO.File.ReadAllText(@"C:\Users\User\Documents\Kerja\Crawl\sriwijayaReva.txt");

             CQ ambilFare = (CQ) htmlRespon;

             
             if ((ParseFID1.Count > 1) && (ParseFID1.Count <= 3))
             {
                 #region
                 var cekFID1 = ambilFare["[value*=" + FIDsegment1 + "]"];
                 var cekFID2 = ambilFare["[value*=" + FIDsegment2 + "]"];
                 var FIDsegments = new List<string>
                 {
                     FIDsegment1,FIDsegment2,FIDsegment3
                 };

                 if ((cekFID1.FirstElement() != null) || (cekFID2.FirstElement() != null))
                 {
                     client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                     //Headers["Accept-Encoding"] = "gzip, deflate";
                     client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                     client.Headers["Upgrade-Insecure-Requests"] = "1";
                     client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                     client.Headers["Referer"] = "http://agent.sriwijayaair.co.id/SJ-Eticket/application/?action=booking";
                     //client.Headers["X-Requested-With"] = "XMLHttpRequest";
                     client.Headers["Host"] = "agent.sriwijayaair.co.id";
                     client.Headers["Origin"] = "http://agent.sriwijayaair.co.id";
                     client.Headers["Content-Type"] = "application/x-www-form-urlencoded";

                     var cekHarga =
                         "http://agent.sriwijayaair.co.id/SJ-Eticket/application/pricingDetail_2.php?" +
                         "vote="+ FIDsegment1 +":"+ Rbd[0] +":S:"+ ognAirport +":"+ arrAirport +":U2s5VlVrNUZXUT09;;" +
                         ""+ FIDsegment2 +":"+ Rbd[1] +":S:" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;" +
                         "&name=radioFrom0_2" +
                         "&STI=false" +
                         "&PC=" +
                         "&DataR=NO ";

                     var htmlResponAjax = client.DownloadString(cekHarga);
                     CQ ambilDataAjax = (CQ)htmlResponAjax;

                     string departure;
                     string arrival;
                     string date, bandaraRaw;
                     var tampungFare = new List<string>();
                     string tampungFareString = null;

                     var tunjukSelectedgo = ambilDataAjax[".selectedgo"];
                     var segments = new List<FlightSegment>();
                     var fareCabin = (ParseFare.Count() - 2);
                     var dict = DictionaryService.GetInstance();
                     for (int i = 0; i < 2; i++)
                     {
                         var tunjukSetiapBandara = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(3)"];
                         bandaraRaw = tunjukSetiapBandara.Select(x => x.Cq().Text()).FirstOrDefault();
                         var bandara = bandaraRaw.Split('-').ToList();
                         var tunjukSetiapTanggal = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(1)"];
                         date = tunjukSetiapTanggal.Select(x => x.Cq().Text()).FirstOrDefault();
                         var tunjukSetiapJamBerangkat = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(5)"];
                         departure = tunjukSetiapJamBerangkat.Select(x => x.Cq().Text()).FirstOrDefault().Substring(0, 5);
                         var tunjukSetiapJamTiba = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(6)"];
                         arrival = tunjukSetiapJamTiba.Select(x => x.Cq().Text()).FirstOrDefault().Substring(0, 5);
                         string format = "dd-MMM-yy hh:mm";
                         //CultureInfo provider = CultureInfo;
                         var departureDate = DateTime.Parse(date + " " + departure, CultureInfo.CreateSpecificCulture("id-ID"));
                         var arrivalDate = DateTime.Parse(date + " " + arrival, CultureInfo.CreateSpecificCulture("id-ID"));
                         var deptime = departureDate.AddHours(-(dict.GetAirportTimeZone(ognAirport)));
                         var arrtime = arrivalDate.AddHours(-(dict.GetAirportTimeZone(arrAirport)));
                         tampungFare.Add("" + FIDsegments[i] + ":" + Rbd[i] + ":S:" + bandara[0] + ":" + bandara[1] + ":U2s5VlVrNUZXUT09;");
                         tampungFareString = string.Join(";", tampungFare.ToArray());
                         segments.Add(new FlightSegment
                         {
                             AirlineCode = ParseFare[i],
                             FlightNumber = ParseFare[i + 1],
                             CabinClass = (CabinClass)int.Parse(ParseFare[fareCabin]),
                             Rbd = Rbd[i],
                             DepartureAirport = bandara[0],
                             DepartureTime = DateTime.SpecifyKind(departureDate,DateTimeKind.Utc),
                             ArrivalAirport = bandara[1],
                             ArrivalTime = DateTime.SpecifyKind(arrivalDate,DateTimeKind.Utc),
                             OperatingAirlineCode = ParseFare[i],
                             Duration = arrtime - deptime,
                             StopQuantity = 0
                         });
                     }

                     var tunjukHarga = ambilDataAjax.MakeRoot()["#fareTotalAmount"];
                     var hargaBaruRaw = tunjukHarga.Select(x => x.Cq().Text()).FirstOrDefault();
                     var indexHarga = hargaBaruRaw.IndexOf('\t');
                     var hargaBaru = hargaBaruRaw.Substring(0, (indexHarga));

                     var itin = new FlightItinerary
                     {
                         AdultCount = penumpang[0],
                         ChildCount = penumpang[2],
                         InfantCount = penumpang[4],
                         CanHold = true,
                         FareType = FareType.Published,
                         RequireBirthDate = true,
                         RequirePassport = false,
                         RequireSameCheckIn = false,
                         RequireNationality = true,
                         RequestedCabinClass = CabinClass.Economy,
                         TripType = TripType.OneWay,
                         Supplier = Supplier.Sriwijaya,
                         SupplierCurrency = "IDR",
                         SupplierRate = 1,
                         SupplierPrice = decimal.Parse(hargaBaru),
                         FareId = Fare,
                         Trips = new List<FlightTrip>
                        {
                            new FlightTrip
                            {
                                OriginAirport = ognAirport,
                                DestinationAirport = arrAirport,
                                DepartureDate = tglBerangkat,
                                Segments = segments
                            }
                        }
                     };

                     hasil.IsSuccess = true;
                     hasil.IsValid = harga == Decimal.Parse(hargaBaru);
                     hasil.Itinerary = itin;

                 }
                 else
                 {
                     return new RevalidateFareResult
                     {
                         IsSuccess = false,
                         IsValid = false,
                         Errors = new List<FlightError> { FlightError.FareIdNoLongerValid },
                     };
                 }
                 #endregion
             }
             else if ((ParseFID1.Count > 3) && (ParseFID1.Count <= 5))
             {
                 #region
                 var cekFID1 = ambilFare["[value*=" + FIDsegment1 + "]"];
                 var cekFID2 = ambilFare["[value*=" + FIDsegment2 + "]"];
                 var cekFID3 = ambilFare["[value*=" + FIDsegment3 + "]"];
                 var FIDsegments = new List<string>
                 {
                     FIDsegment1,FIDsegment2,FIDsegment3
                 };
                 

                 if ((cekFID1.FirstElement() != null) || (cekFID2.FirstElement() != null) || (cekFID2.FirstElement() != null))
                 {
                     client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                     //Headers["Accept-Encoding"] = "gzip, deflate";
                     client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                     client.Headers["Upgrade-Insecure-Requests"] = "1";
                     client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                     client.Headers["Referer"] = "http://agent.sriwijayaair.co.id/SJ-Eticket/application/?action=booking";
                     //client.Headers["X-Requested-With"] = "XMLHttpRequest";
                     client.Headers["Host"] = "agent.sriwijayaair.co.id";
                     client.Headers["Origin"] = "http://agent.sriwijayaair.co.id";
                     client.Headers["Content-Type"] = "application/x-www-form-urlencoded";

                     var ambilData =
                         "http://agent.sriwijayaair.co.id/SJ-Eticket/application/pricingDetail_2.php?" +
                         "vote=" + FIDsegment1 + ":" + Rbd[0] + ":S:" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;" +
                         "" + FIDsegment2 + ":" + Rbd[1] + ":S:" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;" +
                          "" + FIDsegment3 + ":" + Rbd[2] + ":S:" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;" +
                         "&name=radioFrom0_2" +
                         "&STI=false" +
                         "&PC=" +
                         "&DataR=NO ";

                     var htmlResponAjax = client.DownloadString(ambilData);
                     CQ ambilDataAjax = (CQ) htmlResponAjax;

                     string departure;
                     string arrival;
                     string date, bandaraRaw;
                     var tampungFare = new List<string>();
                     string tampungFareString=null;

                     var tunjukSelectedgo = ambilDataAjax[".selectedgo"];
                     var segments = new List<FlightSegment>();
                     var fareCabin = (ParseFare.Count()- 2);
                     var dict = DictionaryService.GetInstance();
                     for (int i = 0; i < 3; i++)
                     {
                         var tunjukSetiapBandara = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(3)"];
                         bandaraRaw = tunjukSetiapBandara.Select(x => x.Cq().Text()).FirstOrDefault();
                         var bandara = bandaraRaw.Split('-').ToList();
                         var tunjukSetiapTanggal = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(1)"];
                         date = tunjukSetiapTanggal.Select(x => x.Cq().Text()).FirstOrDefault();
                         var tunjukSetiapJamBerangkat = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child("+ (i+1) +")>td:nth-child(5)"];
                         departure = tunjukSetiapJamBerangkat.Select(x => x.Cq().Text()).FirstOrDefault().Substring(0,5);
                         var tunjukSetiapJamTiba = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(6)"];
                         arrival = tunjukSetiapJamTiba.Select(x => x.Cq().Text()).FirstOrDefault().Substring(0, 5);
                         string format = "dd-MMM-yy hh:mm";
                         //CultureInfo provider = CultureInfo;
                         var departureDate = DateTime.Parse(date + " " + departure, CultureInfo.CreateSpecificCulture("id-ID"));
                         var arrivalDate = DateTime.Parse(date + " " + arrival, CultureInfo.CreateSpecificCulture("id-ID"));
                         var deptime = departureDate.AddHours(-(dict.GetAirportTimeZone(ognAirport)));
                         var arrtime = arrivalDate.AddHours(-(dict.GetAirportTimeZone(arrAirport)));
                         tampungFare.Add( "" + FIDsegments[i] + ":" + Rbd[i] + ":S:" + bandara[0] + ":" + bandara[1] + ":U2s5VlVrNUZXUT09;" );
                         tampungFareString = string.Join(";", tampungFare.ToArray());
                         segments.Add(new FlightSegment
                         {
                             AirlineCode = ParseFare[i],
                             FlightNumber = ParseFare[i + 1],
                             CabinClass = (CabinClass)int.Parse(ParseFare[fareCabin]),
                             Rbd = Rbd[i],
                             DepartureAirport = bandara[0],
                             DepartureTime = DateTime.SpecifyKind(departureDate,DateTimeKind.Utc),
                             ArrivalAirport = bandara[1],
                             ArrivalTime = DateTime.SpecifyKind(arrivalDate,DateTimeKind.Utc),
                             OperatingAirlineCode = ParseFare[i],
                             Duration = arrtime-deptime,
                             StopQuantity = 0
                         });
                     }

                     var tunjukHarga = ambilDataAjax.MakeRoot()["#fareTotalAmount"];
                     var hargaBaruRaw = tunjukHarga.Select(x => x.Cq().Text()).FirstOrDefault();
                     var indexHarga = hargaBaruRaw.IndexOf('\t');
                     var hargaBaru = hargaBaruRaw.Substring(0,(indexHarga));

                     var itin = new FlightItinerary
                     {
                         AdultCount = penumpang[0],
                         ChildCount = penumpang[1],
                         InfantCount = penumpang[2],
                         CanHold = true,
                         FareType = FareType.Published,
                         RequireBirthDate = true,
                         RequirePassport = false,
                         RequireSameCheckIn = false,
                         RequireNationality = true,
                         RequestedCabinClass = CabinClass.Economy,
                         TripType = TripType.OneWay,
                         Supplier = Supplier.Sriwijaya,
                         SupplierCurrency = "IDR",
                         SupplierRate = 1,
                         SupplierPrice = Decimal.Parse(hargaBaru),
                         FareId = Fare,
                         Trips = new List<FlightTrip>
                        {
                            new FlightTrip
                            {
                                OriginAirport = ognAirport,
                                DestinationAirport = arrAirport,
                                DepartureDate = tglBerangkat,
                                Segments = segments
                            }
                        }
                     };
                     hasil.IsSuccess = true;
                     hasil.IsValid = harga == Decimal.Parse(hargaBaru);
                     hasil.Itinerary = itin;
                 }
                 else
                 {
                     return new RevalidateFareResult
                     {
                         IsSuccess = false,
                         IsValid = false,
                         Errors = new List<FlightError> { FlightError.FareIdNoLongerValid },
                     };
                 }
                 #endregion
             }
             else if (ParseFID1.Count == 1)
             {
                 #region
                 var cekFID1 = ambilFare["[value*=" + FIDsegment1 + "]"];
                 var FIDsegments = new List<string>
                 {
                     FIDsegment1,FIDsegment2,FIDsegment3
                 };

                 if ((cekFID1.FirstElement() != null))
                 {
                     client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                     //Headers["Accept-Encoding"] = "gzip, deflate";
                     client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                     client.Headers["Upgrade-Insecure-Requests"] = "1";
                     client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                     client.Headers["Referer"] = "http://agent.sriwijayaair.co.id/SJ-Eticket/application/?action=booking";
                     //client.Headers["X-Requested-With"] = "XMLHttpRequest";
                     client.Headers["Host"] = "agent.sriwijayaair.co.id";
                     client.Headers["Origin"] = "http://agent.sriwijayaair.co.id";
                     client.Headers["Content-Type"] = "application/x-www-form-urlencoded";

                     var cekHarga =
                         "http://agent.sriwijayaair.co.id/SJ-Eticket/application/pricingDetail_2.php?" +
                         "vote=" + FIDsegment1 + ":" + Rbd[0] + ":S:" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;" +
                         "&name=radioFrom0_2" +
                         "&STI=false" +
                         "&PC=" +
                         "&DataR=NO ";

                     var htmlResponAjax = client.DownloadString(cekHarga);
                     CQ ambilDataAjax = (CQ)htmlResponAjax;

                     string departure;
                     string arrival;
                     string date, bandaraRaw;
                     var tampungFare = new List<string>();
                     string tampungFareString = null;

                     var tunjukSelectedgo = ambilDataAjax[".selectedgo"];
                     var segments = new List<FlightSegment>();
                     var fareCabin = (ParseFare.Count() - 2);
                     var dict = DictionaryService.GetInstance();
                     for (int i = 0; i < 1; i++)
                     {
                         var tunjukSetiapBandara = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(3)"];
                         bandaraRaw = tunjukSetiapBandara.Select(x => x.Cq().Text()).FirstOrDefault();
                         var bandara = bandaraRaw.Split('-').ToList();
                         var tunjukSetiapTanggal = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(1)"];
                         date = tunjukSetiapTanggal.Select(x => x.Cq().Text()).FirstOrDefault();
                         var tunjukSetiapJamBerangkat = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(5)"];
                         departure = tunjukSetiapJamBerangkat.Select(x => x.Cq().Text()).FirstOrDefault().Substring(0, 5);
                         var tunjukSetiapJamTiba = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(6)"];
                         arrival = tunjukSetiapJamTiba.Select(x => x.Cq().Text()).FirstOrDefault().Substring(0, 5);
                         string format = "dd-MMM-yy hh:mm";
                         //CultureInfo provider = CultureInfo;
                         var departureDate = DateTime.Parse(date + " " + departure, CultureInfo.CreateSpecificCulture("id-ID"));
                         var arrivalDate = DateTime.Parse(date + " " + arrival, CultureInfo.CreateSpecificCulture("id-ID"));
                         var deptime = departureDate.AddHours(-(dict.GetAirportTimeZone(ognAirport)));
                         var arrtime = arrivalDate.AddHours(-(dict.GetAirportTimeZone(arrAirport)));
                         tampungFare.Add("" + FIDsegments[i] + ":" + Rbd[i] + ":S:" + bandara[0] + ":" + bandara[1] + ":U2s5VlVrNUZXUT09;");
                         tampungFareString = string.Join(";", tampungFare.ToArray());
                         segments.Add(new FlightSegment
                         {
                             AirlineCode = ParseFare[i],
                             FlightNumber = ParseFare[i + 1],
                             CabinClass = (CabinClass)int.Parse(ParseFare[fareCabin]),
                             Rbd = Rbd[i],
                             DepartureAirport = bandara[0],
                             DepartureTime = departureDate,
                             ArrivalAirport = bandara[1],
                             ArrivalTime = arrivalDate,
                             OperatingAirlineCode = ParseFare[i],
                             Duration = arrtime - deptime,
                             StopQuantity = 0
                         });
                     }

                     var tunjukHarga = ambilDataAjax.MakeRoot()["#fareTotalAmount"];
                     var hargaBaruRaw = tunjukHarga.Select(x => x.Cq().Text()).FirstOrDefault();
                     var indexHarga = hargaBaruRaw.IndexOf('\t');
                     var hargaBaru = hargaBaruRaw.Substring(0, (indexHarga));

                     var itin = new FlightItinerary
                     {
                         AdultCount = penumpang[0],
                         ChildCount = penumpang[1],
                         InfantCount = penumpang[2],
                         CanHold = true,
                         FareType = FareType.Published,
                         RequireBirthDate = true,
                         RequirePassport = false,
                         RequireSameCheckIn = false,
                         RequireNationality = true,
                         RequestedCabinClass = CabinClass.Economy,
                         TripType = TripType.OneWay,
                         Supplier = Supplier.Sriwijaya,
                         SupplierCurrency = "IDR",
                         SupplierRate = 1,
                         SupplierPrice = Decimal.Parse(hargaBaru),
                         FareId = Fare,
                         Trips = new List<FlightTrip>
                        {
                            new FlightTrip
                            {
                                OriginAirport = ognAirport,
                                DestinationAirport = arrAirport,
                                DepartureDate = tglBerangkat,
                                Segments = segments
                            }
                        }
                     };
                     hasil.IsSuccess = true;
                     hasil.IsValid = harga == Decimal.Parse(hargaBaru);
                     hasil.Itinerary = itin;
                 }
                 else
                 {
                     return new RevalidateFareResult
                     {
                         IsSuccess = false,
                         IsValid = false,
                         Errors = new List<FlightError> { FlightError.FareIdNoLongerValid },
                     };
                 }
                 #endregion
             }
             else
             {
                 return new RevalidateFareResult
                 {
                     IsSuccess = false,
                     IsValid = false,
                     Errors = new List<FlightError> { FlightError.FareIdNoLongerValid },
                 };
             }
             return hasil;
        }
    }
}
