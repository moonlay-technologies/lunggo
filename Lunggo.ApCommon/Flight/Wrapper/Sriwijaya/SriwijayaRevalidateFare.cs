using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using CsQuery;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Web;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        internal override RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
        {
            return Client.RevalidateFare(conditions);
        }

        private partial class SriwijayaClientHandler
        {
            internal RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
            {
                {
                    var client = CreateAgentClient();
                    Login(client);
                    var hasil = new RevalidateFareResult();
                    var Fare = conditions.Itinerary.FareId;
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
                    string unknownCode;
                    var penumpang = new List<string>();
                    decimal harga;

                    DateTime tglBerangkat;
                    var Rbd = new List<string>();

                    // PARSING FARE

                    //Penerbangan 2 Segment
                    if ((ParseFID1.Count > 1) && (ParseFID1.Count <= 3))
                    {
                        FIDsegment1 = ParseFID1[0];
                        FIDsegment2 = ParseFID1[1].Substring(0, (ParseFID1[1].Length - 2));
                        FIDsegment3 = null;
                        var titikIndex1 = FID.IndexOf(":");
                        var titikIndex11 = FID.Substring(titikIndex1 + 1);
                        var titikIndex2 = titikIndex11.IndexOf(":");
                        var rbdRaw = FID.Substring(titikIndex1 + 1, (titikIndex2));
                        var findUnknownCode = FID.Substring(titikIndex1 + 1, (titikIndex2 + 2));
                        var titikU1 = findUnknownCode.IndexOf(":");
                        unknownCode = findUnknownCode.Substring(titikU1 + 1);
                        Rbd = rbdRaw.Split(',').ToList();
                        var ambilOrigin = FID.Substring((titikIndex2 + 1) + (titikIndex1 + 1));
                        var titikIndex4 = ambilOrigin.IndexOf(":");
                        var titikIndex5 = ambilOrigin.Substring(titikIndex4 + 1).IndexOf(":");
                        ognAirport = ambilOrigin.Substring((titikIndex4 + 1), titikIndex5);
                        var titikIndex6 = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1)).IndexOf(":");
                        arrAirport = ambilOrigin.Substring((titikIndex4 + 1) + (titikIndex5 + 1), titikIndex6);
                        var indexPenumpang1 = Fare.IndexOf('|');
                        var indexPenumpang2 = Fare.Substring((indexPenumpang1 + 1)).IndexOf('|');
                        penumpang = Fare.Substring((indexPenumpang1 + 1), indexPenumpang2).Split('.').ToList();
                        var indextglBerangkat = Fare.IndexOf('?');
                        var tglBerangkatRaw = Fare.Substring((indextglBerangkat + 1), (indexPenumpang1 - 1 - indextglBerangkat));
                        var indexHarga = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2)).IndexOf('.');
                        tglBerangkat = DateTime.Parse(tglBerangkatRaw);
                        var hargaRaw = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2 + 1), indexHarga - 1);
                        harga = Decimal.Parse(hargaRaw);
                    }

                    //Penerbangan 3 segment
                    else
                        if ((ParseFID1.Count > 3) && (ParseFID1.Count <= 5))
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
                            penumpang = Fare.Substring((indexPenumpang1 + 1), indexPenumpang2).Split('.').ToList();
                            var indextglBerangkat = Fare.IndexOf('?');
                            var tglBerangkatRaw = Fare.Substring((indextglBerangkat + 1), (indexPenumpang1 - 1 - indextglBerangkat));
                            tglBerangkat = DateTime.Parse(tglBerangkatRaw);
                            var indexHarga = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2)).IndexOf('.');
                            var hargaRaw = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2 + 1), indexHarga - 1);
                            harga = Decimal.Parse(hargaRaw);
                        }

                        //Penerbangan 1 segment
                        else
                            if (ParseFID1.Count == 1)
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
                                penumpang = Fare.Substring((indexPenumpang1 + 1), indexPenumpang2).Split('.').ToList();
                                var indextglBerangkat = Fare.IndexOf('?');
                                var tglBerangkatRaw = Fare.Substring((indextglBerangkat + 1), (indexPenumpang1 - 1 - indextglBerangkat));
                                tglBerangkat = DateTime.Parse(tglBerangkatRaw, CultureInfo.CreateSpecificCulture("id-ID"));
                                var indexHarga = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2)).IndexOf('.');
                                var hargaRaw = Fare.Substring((indexPenumpang1 + 1) + (indexPenumpang2 + 1), indexHarga - 1);
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



                    //AMBIL AJAX

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
                    var searchResponse = client.Execute(searchRequest);
                    var htmlRespon = searchResponse.Content;

                    //var htmlRespon = System.IO.File.ReadAllText(@"C:\Users\User\Documents\Kerja\Crawl\sriwijayaReva.txt");

                    CQ ambilFare = (CQ)htmlRespon;

                    //Proses untuk 2 segment
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
                            url = "SJ-Eticket/application/pricingDetail_2.php";
                            var fareRequest = new RestRequest(url, Method.GET);
                            fareRequest.AddQueryParameter("vote", FIDsegment1 + ":" + Rbd[0] + ":" + unknownCode + ":" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;" + "" + FIDsegment2 + ":" + (Rbd.Count > 1 ? Rbd[1] : Rbd[0]) + ":" + unknownCode + ":" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;");
                            fareRequest.AddQueryParameter("name", "radioFrom0_2");
                            fareRequest.AddQueryParameter("STI", "false");
                            fareRequest.AddQueryParameter("PC", null);
                            fareRequest.AddQueryParameter("DataR", "NO ");
                            var fareResponse = client.Execute(fareRequest);
                            var htmlResponAjax = fareResponse.Content;
                            CQ ambilDataAjax = (CQ)htmlResponAjax;

                            string departure;
                            string arrivalRaw;
                            string arrival;
                            double plusHari;
                            string date, bandaraRaw;
                            var tampungFare = new List<string>();
                            string tampungFareString = null;

                            var tunjukSelectedgo = ambilDataAjax[".selectedgo"];
                            var segments = new List<FlightSegment>();
                            var fareCabin = (ParseFare.Count() - 2);
                            var flight = FlightService.GetInstance();
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
                                arrivalRaw = tunjukSetiapJamTiba.Select(x => x.Cq().Text()).FirstOrDefault();
                                if (arrivalRaw.Length > 8)
                                {
                                    arrival = arrivalRaw.Substring(0, 5);
                                    var style = NumberStyles.Number;
                                    var x = arrivalRaw.Substring(7, 2);
                                    plusHari = double.Parse((arrivalRaw.Substring(6, 2)), style);
                                }
                                else
                                {
                                    arrival = arrivalRaw.Substring(0, 5);
                                    plusHari = 0;
                                }
                                string format = "dd-MMM-yy hh:mm";
                                //CultureInfo provider = CultureInfo;
                                var departureDate = DateTime.Parse(date + " " + departure, CultureInfo.CreateSpecificCulture("id-ID"));
                                var arrivalDate = (DateTime.Parse(date + " " + arrival, CultureInfo.CreateSpecificCulture("id-ID"))).AddDays(plusHari);
                                var deptime = departureDate.AddHours(-(flight.GetAirportTimeZone(bandara[0])));
                                var arrtime = arrivalDate.AddHours(-(flight.GetAirportTimeZone(bandara[1])));
                                tampungFare.Add("" + FIDsegments[i] + ":" + (Rbd.Count > i ? Rbd[i] : Rbd[0]) + ":S:" + bandara[0] + ":" + bandara[1] + ":U2s5VlVrNUZXUT09;");
                                tampungFareString = string.Join(";", tampungFare.ToArray());
                                segments.Add(new FlightSegment
                                {
                                    AirlineCode = ParseFare[0].Split(';')[i * 2],
                                    FlightNumber = ParseFare[0].Split(';')[i * 2 + 1],
                                    CabinClass = (CabinClass)int.Parse(ParseFare[fareCabin]),
                                    AirlineType = AirlineType.Lcc,
                                    Rbd = Rbd.Count > i ? Rbd[i] : Rbd[0],
                                    DepartureAirport = bandara[0],
                                    DepartureTime = DateTime.SpecifyKind(departureDate, DateTimeKind.Utc),
                                    ArrivalAirport = bandara[1],
                                    ArrivalTime = DateTime.SpecifyKind(arrivalDate, DateTimeKind.Utc),
                                    OperatingAirlineCode = ParseFare[0].Split(';')[i * 2],
                                    Duration = arrtime - deptime,
                                    StopQuantity = 0,
                                    IsMealIncluded = true,
                                    IsPscIncluded = true
                                });
                            }

                            var tunjukHarga = ambilDataAjax.MakeRoot()["#fareTotalAmount"];
                            var hargaBaruRaw = tunjukHarga.Select(x => x.Cq().Text()).FirstOrDefault();
                            var indexHarga = hargaBaruRaw.IndexOf('\t');
                            var hargaBaru = decimal.Parse(hargaBaruRaw.Substring(0, (indexHarga)));
                            var hargaAdult = 0M;
                            var hargaChild = 0M;
                            var hargaInfant = 0M;
                            try
                            {
                                hargaAdult = decimal.Parse(ambilDataAjax["#fareDetailAdult .priceDetail"].Reverse().Skip(1).Take(1).Single().InnerText);
                                hargaChild = decimal.Parse(ambilDataAjax["#fareDetailChild .priceDetail"].Reverse().Skip(1).Take(1).Single().InnerText);
                                hargaInfant = decimal.Parse(ambilDataAjax["#fareDetailInfant .priceDetail"].Reverse().Skip(1).Take(1).Single().InnerText);
                            }
                            catch { }


                            var itin = new FlightItinerary
                            {
                                AdultCount = int.Parse(penumpang[0]),
                                ChildCount = int.Parse(penumpang[1]),
                                InfantCount = int.Parse(penumpang[2]),
                                CanHold = true,
                                FareType = FareType.Published,
                                RequireBirthDate = false,
                                RequirePassport = false,
                                RequireSameCheckIn = false,
                                RequireNationality = false,
                                RequestedCabinClass = CabinClass.Economy,
                                RequestedTripType = conditions.Itinerary.RequestedTripType,
                                TripType = TripType.OneWay,
                                Supplier = Supplier.Sriwijaya,
                                Price = new Price(),
                                AdultPricePortion = hargaAdult / hargaBaru,
                                ChildPricePortion = hargaChild / hargaBaru,
                                InfantPricePortion = hargaInfant / hargaBaru,
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
                            itin.Price.SetSupplier(hargaBaru, new Currency("IDR"));

                            var prefix =
                                        string.Join(";", segments.Select(s => s.AirlineCode + ";" + s.FlightNumber)) +
                                        "." + itin.Trips[0].OriginAirport +
                                        "." + itin.Trips[0].DestinationAirport +
                                        "?" + itin.Trips[0].DepartureDate.Year +
                                        "-" + itin.Trips[0].DepartureDate.Month +
                                        "-" + itin.Trips[0].DepartureDate.Day +
                                        "|" + itin.AdultCount +
                                        "." + itin.ChildCount +
                                        "." + itin.InfantCount +
                                        "|" + hargaBaru +
                                        "." + (int)itin.RequestedCabinClass + ".";
                            itin.FareId = prefix + Fare.Split('.').Last();

                            hasil.IsSuccess = true;
                            hasil.IsValid = true;
                            hasil.IsPriceChanged = harga != hargaBaru;
                            hasil.IsItineraryChanged = !conditions.Itinerary.Identical(itin);
                            if (hasil.IsPriceChanged)
                            {
                                hasil.NewPrice = hargaBaru;
                            }
                            hasil.NewItinerary = itin;
                        }
                        else
                        {
                            SriwijayaClientHandler.Logout(client);
                            return new RevalidateFareResult
                            {
                                IsSuccess = false,
                                IsValid = false,
                                Errors = new List<FlightError> { FlightError.FareIdNoLongerValid },
                            };
                        }
                        #endregion
                    }

                    //Proses untuk 3 Segement
                    else
                        if ((ParseFID1.Count > 3) && (ParseFID1.Count <= 5))
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
                                url = "SJ-Eticket/application/pricingDetail_2.php";
                                var fareRequest = new RestRequest(url, Method.GET);
                                fareRequest.AddQueryParameter("vote", FIDsegment1 + ":" + Rbd[0] + ":" + unknownCode + ":" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;" + "" + FIDsegment2 + ":" + (Rbd.Count > 1 ? Rbd[1] : Rbd[0]) + ":" + unknownCode + ":" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;" + "" + FIDsegment3 + ":" + (Rbd.Count > 2 ? Rbd[2] : Rbd[0]) + ":" + unknownCode + ":" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;");
                                fareRequest.AddQueryParameter("name", "radioFrom0_2");
                                fareRequest.AddQueryParameter("STI", "false");
                                fareRequest.AddQueryParameter("PC", null);
                                fareRequest.AddQueryParameter("DataR", "NO ");
                                var fareResponse = client.Execute(fareRequest);
                                var htmlResponAjax = fareResponse.Content;
                                CQ ambilDataAjax = (CQ)htmlResponAjax;

                                string departure;
                                string arrivalRaw;
                                string arrival;
                                double plusHari;
                                string date, bandaraRaw;
                                var tampungFare = new List<string>();
                                string tampungFareString = null;

                                var tunjukSelectedgo = ambilDataAjax[".selectedgo"];
                                var segments = new List<FlightSegment>();
                                var fareCabin = (ParseFare.Count() - 2);
                                var flight = FlightService.GetInstance();
                                for (int i = 0; i < 3; i++)
                                {
                                    var tunjukSetiapBandara = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(3)"];
                                    bandaraRaw = tunjukSetiapBandara.Select(x => x.Cq().Text()).FirstOrDefault();
                                    var bandara = bandaraRaw.Split('-').ToList();
                                    var tunjukSetiapTanggal = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(1)"];
                                    date = tunjukSetiapTanggal.Select(x => x.Cq().Text()).FirstOrDefault();
                                    var tunjukSetiapJamBerangkat = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(5)"];
                                    departure = tunjukSetiapJamBerangkat.Select(x => x.Cq().Text()).FirstOrDefault().Substring(0, 5);
                                    var tunjukSetiapJamTiba = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (i + 1) + ")>td:nth-child(6)"];
                                    arrivalRaw = tunjukSetiapJamTiba.Select(x => x.Cq().Text()).FirstOrDefault();
                                    if (arrivalRaw.Length > 8)
                                    {
                                        arrival = arrivalRaw.Substring(0, 5);
                                        var style = NumberStyles.Number;
                                        plusHari = double.Parse((arrivalRaw.Substring(6, 2)), style);
                                    }
                                    else
                                    {
                                        arrival = arrivalRaw.Substring(0, 5);
                                        plusHari = 0;
                                    }
                                    string format = "dd-MMM-yy hh:mm";
                                    //CultureInfo provider = CultureInfo;
                                    var departureDate = DateTime.Parse(date + " " + departure, CultureInfo.CreateSpecificCulture("id-ID"));
                                    var arrivalDate = (DateTime.Parse(date + " " + arrival, CultureInfo.CreateSpecificCulture("id-ID"))).AddDays(plusHari);
                                    var deptime = departureDate.AddHours(-(flight.GetAirportTimeZone(bandara[0])));
                                    var arrtime = arrivalDate.AddHours(-(flight.GetAirportTimeZone(bandara[1])));
                                    tampungFare.Add("" + FIDsegments[i] + ":" + (Rbd.Count > i ? Rbd[i] : Rbd[0]) + ":S:" + bandara[0] + ":" + bandara[1] + ":U2s5VlVrNUZXUT09;");
                                    tampungFareString = string.Join(";", tampungFare.ToArray());
                                    segments.Add(new FlightSegment
                                    {
                                        AirlineCode = ParseFare[0].Split(';')[i * 2],
                                        FlightNumber = ParseFare[0].Split(';')[i * 2 + 1],
                                        CabinClass = (CabinClass)int.Parse(ParseFare[fareCabin]),
                                        AirlineType = AirlineType.Lcc,
                                        Rbd = Rbd.Count > i ? Rbd[i] : Rbd[0],
                                        DepartureAirport = bandara[0],
                                        DepartureTime = DateTime.SpecifyKind(departureDate, DateTimeKind.Utc),
                                        ArrivalAirport = bandara[1],
                                        ArrivalTime = DateTime.SpecifyKind(arrivalDate, DateTimeKind.Utc),
                                        OperatingAirlineCode = ParseFare[0].Split(';')[i * 2],
                                        Duration = arrtime - deptime,
                                        StopQuantity = 0,
                                        IsMealIncluded = true,
                                        IsPscIncluded = true
                                    });
                                }

                                var tunjukHarga = ambilDataAjax.MakeRoot()["#fareTotalAmount"];
                                var hargaBaruRaw = tunjukHarga.Select(x => x.Cq().Text()).FirstOrDefault();
                                var indexHarga = hargaBaruRaw.IndexOf('\t');
                                var hargaBaru = decimal.Parse(hargaBaruRaw.Substring(0, (indexHarga)));
                                var hargaAdult = 0M;
                                var hargaChild = 0M;
                                var hargaInfant = 0M;
                                try
                                {
                                    hargaAdult = decimal.Parse(ambilDataAjax["#fareDetailAdult .priceDetail"].Reverse().Skip(1).Take(1).Single().InnerText);
                                    hargaChild = decimal.Parse(ambilDataAjax["#fareDetailChild .priceDetail"].Reverse().Skip(1).Take(1).Single().InnerText);
                                    hargaInfant = decimal.Parse(ambilDataAjax["#fareDetailInfant .priceDetail"].Reverse().Skip(1).Take(1).Single().InnerText);
                                }
                                catch { }

                                var itin = new FlightItinerary
                                {
                                    AdultCount = int.Parse(penumpang[0]),
                                    ChildCount = int.Parse(penumpang[1]),
                                    InfantCount = int.Parse(penumpang[2]),
                                    CanHold = true,
                                    FareType = FareType.Published,
                                    RequireBirthDate = false,
                                    RequirePassport = false,
                                    RequireSameCheckIn = false,
                                    RequireNationality = false,
                                    RequestedCabinClass = CabinClass.Economy,
                                    RequestedTripType = conditions.Itinerary.RequestedTripType,
                                    TripType = TripType.OneWay,
                                    Supplier = Supplier.Sriwijaya,
                                    Price = new Price(),
                                    AdultPricePortion = hargaAdult / hargaBaru,
                                    ChildPricePortion = hargaChild / hargaBaru,
                                    InfantPricePortion = hargaInfant / hargaBaru,
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
                                itin.Price.SetSupplier(hargaBaru, new Currency("IDR"));

                                var prefix =
                                        string.Join(";", segments.Select(s => s.AirlineCode + ";" + s.FlightNumber)) +
                                        "." + itin.Trips[0].OriginAirport +
                                        "." + itin.Trips[0].DestinationAirport +
                                        "?" + itin.Trips[0].DepartureDate.Year +
                                        "-" + itin.Trips[0].DepartureDate.Month +
                                        "-" + itin.Trips[0].DepartureDate.Day +
                                        "|" + itin.AdultCount +
                                        "." + itin.ChildCount +
                                        "." + itin.InfantCount +
                                        "|" + hargaBaru +
                                        "." + (int)itin.RequestedCabinClass + ".";
                                itin.FareId = prefix + Fare.Split('.').Last();

                                hasil.IsSuccess = true;
                                hasil.IsValid = true;
                                hasil.IsPriceChanged = harga != hargaBaru;
                                hasil.IsItineraryChanged = !conditions.Itinerary.Identical(itin);
                                if (hasil.IsPriceChanged)
                                {
                                    hasil.NewPrice = hargaBaru;
                                }
                                hasil.NewItinerary = itin;
                            }
                            else
                            {
                                SriwijayaClientHandler.Logout(client);
                                return new RevalidateFareResult
                                {
                                    IsSuccess = false,
                                    IsValid = false,
                                    Errors = new List<FlightError> { FlightError.FareIdNoLongerValid },
                                };
                            }
                            #endregion
                        }

                        //Proses untuk 1 segment
                        else
                            if (ParseFID1.Count == 1)
                            {
                                #region
                                var cekFID1 = ambilFare["[value*='" + FIDsegment1 + "']"];
                                var FIDsegments = new List<string>
                 {
                     FIDsegment1,FIDsegment2,FIDsegment3
                 };

                                if ((cekFID1.FirstElement() != null))
                                {
                                    url = "SJ-Eticket/application/pricingDetail_2.php";
                                    var fareRequest = new RestRequest(url, Method.GET);
                                    fareRequest.AddQueryParameter("vote", FIDsegment1 + ":" + Rbd[0] + ":" + unknownCode + ":" + ognAirport + ":" + arrAirport + ":U2s5VlVrNUZXUT09;;");
                                    fareRequest.AddQueryParameter("name", "radioFrom0_2");
                                    fareRequest.AddQueryParameter("STI", "false");
                                    fareRequest.AddQueryParameter("PC", null);
                                    fareRequest.AddQueryParameter("DataR", "NO ");
                                    var fareResponse = client.Execute(fareRequest);
                                    var htmlResponAjax = fareResponse.Content;
                                    CQ ambilDataAjax = (CQ)htmlResponAjax;

                                    string departure;
                                    string arrivalRaw;
                                    string arrival;
                                    double plusHari;
                                    string date, bandaraRaw;
                                    var tampungFare = new List<string>();
                                    string tampungFareString = null;

                                    var tunjukSelectedgo = ambilDataAjax[".selectedgo"];
                                    var segments = new List<FlightSegment>();
                                    var fareCabin = (ParseFare.Count() - 2);
                                    var flight = FlightService.GetInstance();
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
                                        arrivalRaw = tunjukSetiapJamTiba.Select(x => x.Cq().Text()).FirstOrDefault();
                                        if (arrivalRaw.Length > 8)
                                        {
                                            arrival = arrivalRaw.Substring(0, 5);
                                            var style = NumberStyles.Number;
                                            plusHari = double.Parse((arrivalRaw.Substring(6, 2)), style);
                                        }
                                        else
                                        {
                                            arrival = arrivalRaw.Substring(0, 5);
                                            plusHari = 0;
                                        }
                                        string format = "dd-MMM-yy hh:mm";
                                        //CultureInfo provider = CultureInfo;
                                        var departureDate = DateTime.Parse(date + " " + departure, CultureInfo.CreateSpecificCulture("id-ID"));
                                        var arrivalDate = (DateTime.Parse(date + " " + arrival, CultureInfo.CreateSpecificCulture("id-ID"))).AddDays(plusHari);
                                        var deptime = departureDate.AddHours(-(flight.GetAirportTimeZone(ognAirport)));
                                        var arrtime = arrivalDate.AddHours(-(flight.GetAirportTimeZone(arrAirport)));
                                        tampungFare.Add("" + FIDsegments[i] + ":" + (Rbd.Count > i ? Rbd[i] : Rbd[0]) + ":S:" + bandara[0] + ":" + bandara[1] + ":U2s5VlVrNUZXUT09;");
                                        tampungFareString = string.Join(";", tampungFare.ToArray());
                                        segments.Add(new FlightSegment
                                        {
                                            AirlineCode = ParseFare[0].Split(';')[i * 2],
                                            FlightNumber = ParseFare[0].Split(';')[i * 2 + 1],
                                            CabinClass = (CabinClass)int.Parse(ParseFare[fareCabin]),
                                            AirlineType = AirlineType.Lcc,
                                            Rbd = Rbd.Count > i ? Rbd[i] : Rbd[0],
                                            DepartureAirport = bandara[0],
                                            DepartureTime = DateTime.SpecifyKind(departureDate, DateTimeKind.Utc),
                                            ArrivalAirport = bandara[1],
                                            ArrivalTime = DateTime.SpecifyKind(arrivalDate, DateTimeKind.Utc),
                                            OperatingAirlineCode = ParseFare[0].Split(';')[i * 2],
                                            Duration = arrtime - deptime,
                                            StopQuantity = 0,
                                            IsMealIncluded = true,
                                            IsPscIncluded = true
                                        });
                                    }

                                    var tunjukHarga = ambilDataAjax.MakeRoot()["#fareTotalAmount"];
                                    var hargaBaruRaw = tunjukHarga.Select(x => x.Cq().Text()).FirstOrDefault();
                                    var indexHarga = hargaBaruRaw.IndexOf('\t');
                                    var hargaBaru = decimal.Parse(hargaBaruRaw.Substring(0, (indexHarga)));
                                    var hargaAdult = 0M;
                                    var hargaChild = 0M;
                                    var hargaInfant = 0M;
                                    try
                                    {
                                        hargaAdult = decimal.Parse(ambilDataAjax["#fareDetailAdult .priceDetail"].Reverse().Skip(1).Take(1).Single().InnerText);
                                        hargaChild = decimal.Parse(ambilDataAjax["#fareDetailChild .priceDetail"].Reverse().Skip(1).Take(1).Single().InnerText);
                                        hargaInfant = decimal.Parse(ambilDataAjax["#fareDetailInfant .priceDetail"].Reverse().Skip(1).Take(1).Single().InnerText);
                                    }
                                    catch { }

                                    var itin = new FlightItinerary
                                    {
                                        AdultCount = int.Parse(penumpang[0]),
                                        ChildCount = int.Parse(penumpang[1]),
                                        InfantCount = int.Parse(penumpang[2]),
                                        CanHold = true,
                                        FareType = FareType.Published,
                                        RequireBirthDate = false,
                                        RequirePassport = false,
                                        RequireSameCheckIn = false,
                                        RequireNationality = false,
                                        RequestedCabinClass = CabinClass.Economy,
                                        RequestedTripType = conditions.Itinerary.RequestedTripType,
                                        TripType = TripType.OneWay,
                                        Supplier = Supplier.Sriwijaya,
                                        Price = new Price(),
                                        AdultPricePortion = hargaAdult / hargaBaru,
                                        ChildPricePortion = hargaChild / hargaBaru,
                                        InfantPricePortion = hargaInfant / hargaBaru,
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
                                    itin.Price.SetSupplier(hargaBaru, new Currency("IDR"));

                                    var prefix =
                                        string.Join(";", segments.Select(s => s.AirlineCode + ";" + s.FlightNumber)) +
                                        "." + itin.Trips[0].OriginAirport +
                                        "." + itin.Trips[0].DestinationAirport +
                                        "?" + itin.Trips[0].DepartureDate.Year +
                                        "-" + itin.Trips[0].DepartureDate.Month +
                                        "-" + itin.Trips[0].DepartureDate.Day +
                                        "|" + itin.AdultCount +
                                        "." + itin.ChildCount +
                                        "." + itin.InfantCount +
                                        "|" + hargaBaru +
                                        "." + (int)itin.RequestedCabinClass + ".";
                                    itin.FareId = prefix + Fare.Split('.').Last();

                                    hasil.IsSuccess = true;
                                    hasil.IsValid = true;
                                    hasil.IsPriceChanged = harga != hargaBaru;
                                    hasil.IsItineraryChanged = !conditions.Itinerary.Identical(itin);
                                    if (hasil.IsPriceChanged)
                                    {
                                        hasil.NewPrice = hargaBaru;
                                    }
                                    hasil.NewItinerary = itin;
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
                                Logout(client);
                                return new RevalidateFareResult
                                {
                                    IsSuccess = false,
                                    IsValid = false,
                                    Errors = new List<FlightError> { FlightError.FareIdNoLongerValid },
                                };
                            }
                    Logout(client);
                    return hasil;
                }
            }
        }
    }
}
