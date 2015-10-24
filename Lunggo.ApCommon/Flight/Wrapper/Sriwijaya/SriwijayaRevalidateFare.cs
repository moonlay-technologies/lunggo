using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using Lunggo.ApCommon.Constant;
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

             var tglBerangkat = new DateTime(2015,11,2);
             client.Expect100Continue = false;
             client.AutoRedirect = true;
             Client.CreateSession(client);

             var Fare =
                 "SJ017.SJ272.IN9662.KNO.WGP.13.11.2015.1.0.0.2346000.97174,3853813,1953461:X,M,T:S:KNO:WGP:U2s5VlVrNUZXUT09";
             var FID = "3820089,3853785,1953197:X,V,T:S:KNO:WGP:U2s5VlVrNUZXUT09";
             var ParseFID1 = FID.Split(',').ToList();

             string FIDsegment1;
             string FIDsegment2;
             string FIDsegment3;
             string ognAirport;
             string arrAirport;
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
                 "&ADT=1" +
                 "&CHD=0" +
                 "&INF=0" +
                 "&Submit=Search" +
                 "&action=booking&2210150413=2210150413";

             client.UploadString("http://agent.sriwijayaair.co.id/SJ-Eticket/application/?action=booking", agentBooking);

             var htmlRespon = System.IO.File.ReadAllText(@"C:\Users\User\Documents\Kerja\Crawl\sriwijayaReva.txt");

             CQ ambilFare = (CQ) htmlRespon;

             
             if ((ParseFID1.Count > 1) && (ParseFID1.Count <= 3))
             {
                 var cekFID1 = ambilFare["[value*=" + FIDsegment1 + "]"];
                 var cekFID2 = ambilFare["[value*=" + FIDsegment2 + "]"];

                 if ((cekFID1.FirstElement() == null) || (cekFID2.FirstElement() == null))
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

                         "vote="+ cekFID1 +":"+ Rbd[0] +":S:"+ ognAirport +":"+ arrAirport +":U2s5VlVrNUZXUT09;;" +
                         ""+ cekFID2 +":"+ Rbd[1] +":S:" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;" +
                         "&name=radioFrom0_2" +
                         "&STI=true" +
                         "&PC=" +
                         "&DataR=NO ";

                     client.UploadString("http://agent.sriwijayaair.co.id/SJ-Eticket/application/pricingDetail_2.php?", cekHarga);

                     //var segments = new List<FlightSegment>();
                     //segments.Add(new FlightSegment
                     //{
                     //    AirlineCode = Acode,
                     //    FlightNumber = Fnumber,
                     //    //CabinClass = CabinClass.Economy,
                     //    Rbd = Rbd,
                     //    DepartureAirport = ognAirport,
                     //    DepartureTime = DateTime.Parse(ParseFID2[j + 1]),
                     //    ArrivalAirport = arrAirport,
                     //    ArrivalTime = DateTime.Parse(ParseFID2[j + 3]),
                     //    OperatingAirlineCode = Acode,
                     //    //Duration = arrtime-deptime,
                     //    StopQuantity = 0
                     //});
                     //
                     //var itin = new FlightItinerary
                     //{
                     //    AdultCount = adultCount,
                     //    ChildCount = childCount,
                     //    InfantCount = infantCount,
                     //    CanHold = true,
                     //    FareType = FareType.Published,
                     //    RequireBirthDate = true,
                     //    RequirePassport = false,
                     //    RequireSameCheckIn = false,
                     //    RequireNationality = true,
                     //    RequestedCabinClass = CabinClass.Economy,
                     //    TripType = TripType.OneWay,
                     //    Supplier = Supplier.AirAsia,
                     //    SupplierCurrency = "IDR",
                     //    SupplierRate = 1,
                     //    SupplierPrice = newPrice,
                     //    FareId = prefix + foundFareId,
                     //    Trips = new List<FlightTrip>
                     //   {
                     //       new FlightTrip
                     //       {
                     //           OriginAirport = origin,
                     //           DestinationAirport = dest,
                     //           DepartureDate = date,
                     //           Segments = segments
                     //       }
                     //   }
                     //};

                 }

             }
             else if ((ParseFID1.Count > 3) && (ParseFID1.Count <= 5))
             {
                 var cekFID1 = ambilFare["[value*=" + FIDsegment1 + "]"];
                 var cekFID2 = ambilFare["[value*=" + FIDsegment2 + "]"];
                 var cekFID3 = ambilFare["[value*=" + FIDsegment3 + "]"];

                 if ((cekFID1.FirstElement() == null) || (cekFID2.FirstElement() == null) || (cekFID2.FirstElement() == null))
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

                         "vote=" + cekFID1 + ":" + Rbd[0] + ":S:" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;" +
                         "" + cekFID2 + ":" + Rbd[1] + ":S:" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;" +
                          "" + cekFID3 + ":" + Rbd[2] + ":S:" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;" +
                         "&name=radioFrom0_2" +
                         "&STI=true" +
                         "&PC=" +
                         "&DataR=NO ";

                     client.UploadString("http://agent.sriwijayaair.co.id/SJ-Eticket/application/pricingDetail_2.php?", cekHarga);
                 }

             }
             else if (ParseFID1.Count == 1)
             {
                 var cekFID1 = ambilFare["[value*=" + FIDsegment1 + "]"];

                 if ((cekFID1.FirstElement() == null))
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

                         "vote=" + cekFID1 + ":" + Rbd[0] + ":S:" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;" +
                         "&name=radioFrom0_2" +
                         "&STI=true" +
                         "&PC=" +
                         "&DataR=NO ";

                     client.UploadString("http://agent.sriwijayaair.co.id/SJ-Eticket/application/pricingDetail_2.php?", cekHarga);
                 }
             }
             else
             {
                 return new RevalidateFareResult
                 {
                     IsValid = false,
                     Errors = new List<FlightError> { FlightError.FareIdNoLongerValid },
                 };
             }

             return null;
        }
    }
}
