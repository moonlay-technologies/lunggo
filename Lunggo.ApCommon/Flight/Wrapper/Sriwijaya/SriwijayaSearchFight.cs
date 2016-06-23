using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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
        internal override SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            //var sementarai = new SearchFlightConditions
            //{
            //    AdultCount = 2,
            //    ChildCount = 2,
            //    InfantCount = 2,
            //    CabinClass = CabinClass.Economy,
            //    Trips = new List<FlightTrip>
            //        {
            //            new FlightTrip
            //            {
            //                OriginAirport = "KNO",
            //                DestinationAirport = "WGP",
            //                DepartureDate = new DateTime(2015,11,4)
            //            }
            //        },
            //
            //};
            return Client.SearchFlight(conditions);
        }

        private partial class SriwijayaClientHandler
        {
            internal SearchFlightResult SearchFlight(SearchFlightConditions conditions)
            {
                if (conditions.Trips.Count > 1)
                    return new SearchFlightResult
                    {
                        IsSuccess = true,
                        Itineraries = new List<FlightItinerary>()
                    };

                var trip0 = conditions.Trips[0];
                trip0.OriginAirport = trip0.OriginAirport == "JKT" ? "CGK" : trip0.OriginAirport;
                trip0.DestinationAirport = trip0.DestinationAirport == "JKT" ? "CGK" : trip0.DestinationAirport;
                var client = CreateCustomerClient();
                var hasil = new SearchFlightResult();
                var convertBulan = trip0.DepartureDate.ToString("MMMM");
                var bulan3 = convertBulan.Substring(0, 3);

                //ISI BAHASA

                var url0 = "/";
                var rq0 = new RestRequest(url0, Method.GET);
                client.Execute(rq0);

                var url = "welcome.php";
                var langRequest = new RestRequest(url, Method.POST);
                var postData =
                    @"form_build_id=form-443a2fb12e018591436589029dabcde0" +
                    @"&form_id=flute_location_language_form" +
                    @"&location=ID" +
                    @"&language=ID"+
                    @"op=Choose";
                langRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var langResponse = client.Execute(langRequest);
                var temp = langResponse.ResponseUri.AbsoluteUri;
                if (langResponse.StatusCode != HttpStatusCode.OK)
                {
                    return new SearchFlightResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier },
                    };
                }

                url = "/location-block.php";
                var locRequest = new RestRequest(url, Method.GET);
                locRequest.AddHeader("X-Requested-With", "XMLHttpRequest");
                client.Execute(locRequest);
                
                //SEARCH
                url = "application/?action=booking";
                var searchRequest = new RestRequest(url, Method.POST);
                postData = @"tanggalBerangkat=" + trip0.DepartureDate.Day + "-" + bulan3 + "-" + trip0.DepartureDate.Year +
                                 @"&ADT=" + conditions.AdultCount +
                                 @"&CHD=" + conditions.ChildCount +
                                 @"&INF=" + conditions.InfantCount +
                                 @"&returndaterange=0" +
                                 @"&return=NO" +
                                 @"&Submit=Pencarian" +
                                 @"&ruteTujuan=" + trip0.DestinationAirport +
                                 @"&ruteBerangkat=" + trip0.OriginAirport +
                                 @"&vSub=YES";
                searchRequest.AddParameter("application/x-www-form-urlencoded", postData, ParameterType.RequestBody);
                var searchResponse = client.Execute(searchRequest);
                var htmlRespon = searchResponse.Content;
                

                CQ cobaAmbilTable;

                if (searchResponse.ResponseUri.AbsoluteUri == "https://www.sriwijayaair.co.id/application/?action=booking")
                {
                    cobaAmbilTable = (CQ)htmlRespon;
                }
                else
                {
                    return new SearchFlightResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier },
                    };
                }

                //if (client.ResponseUri.AbsolutePath != "/ScheduleSelect.aspx")
                //    return new SearchFlightResult { Errors = new List<FlightError> { FlightError.FareIdNoLongerValid } };

                try
                {
                    var isi = cobaAmbilTable[".tableAvailability tr"];
                    int i = isi.Count();
                    var itins = new List<FlightItinerary>();
                    for (int j = 2; j <= i; j++)
                    {
                        var FlightTrips = new FlightTrip();
                        var tunjuk = isi.MakeRoot()["tr:nth-child(" + j + ")"];
                        string FID;
                        string FIDB;
                        var ParseFID1 = new List<string>();
                        var ParseFID1B = new List<string>();

                        var cabinClass = conditions.CabinClass;
                        switch (cabinClass)
                        {

                            case CabinClass.Economy:
                                #region
                                {
                                    //FareIDEkonomi
                                    try
                                    {
                                        var ambilFID = tunjuk.MakeRoot()[".economyFare>div>input"];
                                        FID = ambilFID.Select(x => x.Cq().Attr("value")).FirstOrDefault().Trim();
                                    }
                                    catch (Exception)
                                    {
                                        new SearchFlightResult
                                        {
                                            Itineraries = new List<FlightItinerary>(),
                                            IsSuccess = true
                                        };
                                        continue;
                                    }
                                   
                                    ParseFID1 = FID.Split(',').ToList();
                                    
                                    string FIDsegmentEko1;
                                    string FIDsegmentEko2;
                                    string FIDsegmentEko3;
                                    var rbdEko = new List<string>();

                                    //1 TRANSIT
                                    if ((ParseFID1.Count > 1) && (ParseFID1.Count <= 3))
                                    {
                                        FIDsegmentEko1 = ParseFID1[0];
                                        FIDsegmentEko2 = ParseFID1[1].Substring(0, (ParseFID1[1].Length - 2));
                                        var rbdIndex1 = FID.IndexOf(":");
                                        var rbdIndex11 = FID.Substring(rbdIndex1 + 1);
                                        var rbdIndex2 = rbdIndex11.IndexOf(":");
                                        var rbdRaw = FID.Substring(rbdIndex1 + 1, (rbdIndex2));
                                        rbdEko = rbdRaw.Split(',').ToList();
                                    }

                                    //2 TRANSIT
                                    else
                                    if ((ParseFID1.Count > 3) && (ParseFID1.Count <= 5))
                                    {
                                        FIDsegmentEko1 = ParseFID1[0];
                                        FIDsegmentEko2 = ParseFID1[1];
                                        FIDsegmentEko3 = ParseFID1[2].Substring(0, (ParseFID1[2].Length - 2));
                                        var rbdIndex1 = FID.IndexOf(":");
                                        var rbdIndex11 = FID.Substring(rbdIndex1 + 1);
                                        var rbdIndex2 = rbdIndex11.IndexOf(":");
                                        var rbdRaw = FID.Substring(rbdIndex1 + 1, (rbdIndex2));
                                        rbdEko = rbdRaw.Split(',').ToList();
                                    }

                                    //TANPA TRANSIT
                                    else
                                    if (ParseFID1.Count == 1)
                                    {
                                        FIDsegmentEko1 = ParseFID1[0];
                                        var rbdIndex1 = FID.IndexOf(":");
                                        var rbdIndex11 = FID.Substring(rbdIndex1 + 1);
                                        var rbdIndex2 = rbdIndex11.IndexOf(":");
                                        var rbdRaw = FID.Substring(rbdIndex1 + 1, (rbdIndex2));
                                        rbdEko = rbdRaw.Split(',').ToList();
                                    }

                                    else
                                    {
                                        return new SearchFlightResult
                                        {
                                            IsSuccess = false,
                                            Errors = new List<FlightError> { FlightError.TechnicalError },
                                            ErrorMessages = new List<string> { "Web Layout Changed!" }
                                        };
                                    }


                                    //AJAX
                                    url =
                                        "application/pricingDetail_.php";
                                    var fareRequest = new RestRequest(url, Method.GET);
                                    fareRequest.AddQueryParameter("voteFROM", FID);
                                    fareRequest.AddQueryParameter("nameFROM", "radioFrom");
                                    fareRequest.AddQueryParameter("voteTO", null);
                                    fareRequest.AddQueryParameter("nameTO", null);
                                    fareRequest.AddQueryParameter("STI", "false");
                                    fareRequest.AddQueryParameter("RR", "NO");
                                    fareRequest.AddQueryParameter("ADM", null);
                                    var fareResponse = client.Execute(fareRequest);
                                    var responAjax = fareResponse.Content;

                                    CQ ambilDataAjax = (CQ)responAjax;
                                   

                                    var isiSegment = ambilDataAjax[".bs>dl>.flightNumBS"];
                                    int jumlahSegment = isiSegment.Count();
                                    //if (jumlahSegment == 0)
                                    //{
                                    //    return new SearchFlightResult
                                    //    {
                                    //        Itineraries = new List<FlightItinerary>(),
                                    //        IsSuccess = true
                                    //    };
                                    //}
                                    int jumlahdata = jumlahSegment * 3;
                                    var isiData = ambilDataAjax[".bs>dl>dd"];
                                    var jumlahDiIsiData = isiData.Count();
                                    //if (jumlahDiIsiData == 0)
                                    //{
                                    //    return new SearchFlightResult
                                    //    {
                                    //        Itineraries = new List<FlightItinerary>(),
                                    //        IsSuccess = true
                                    //    };
                                    //}
                                    var segments = new List<FlightSegment>();
                                    var tampungPesawat = new List<string>();
                                    string tampungPesawatString = null;

                                    //Price 

                                    fareRequest.AddQueryParameter("break", "YES");
                                    var fareResponse2 = client.Execute(fareRequest);
                                    var responAjax2 = fareResponse2.Content;

                                    CQ ambilDataAjax2 = (CQ)responAjax2;

                                    var tunjukHarga = ambilDataAjax2[".priceDetail.bold"];
                                    var ambilharga = tunjukHarga.Select(x => x.Cq().Text()).LastOrDefault();
                                    var harga = decimal.Parse(ambilharga.Split(' ')[1]);
                                    var hargaAdult = 0M;
                                    var hargaChild = 0M;
                                    var hargaInfant = 0M;
                                    try
                                    {
                                        hargaAdult = decimal.Parse(ambilDataAjax2["#fareDetailAdult .priceDetail"].Last().Text());
                                        hargaChild = decimal.Parse(ambilDataAjax2["#fareDetailChild .priceDetail"].Last().Text());
                                        hargaInfant = decimal.Parse(ambilDataAjax2["#fareDetailInfant .priceDetail"].Last().Text());
                                    }
                                    catch { }

                                    //Iterate per Segment
                                    for (int code = 0; code < jumlahSegment; code++)
                                    {
                                        //Airline
                                        var ambilACode = isiData.MakeRoot()["dd:nth-child(" + (1 + (3 * code)) + ")"];
                                        var codeRaw = ambilACode.Select(x => x.Cq().Text()).FirstOrDefault();
                                        var codeParse1 = codeRaw.Split(' ').ToList();
                                        var ambilBandara = isiData.MakeRoot()["dd:nth-child(" + ((1 + (3 * code)) + 1) + ")"];
                                        var bandaraRaw = ambilBandara.Select(x => x.Cq().Text()).FirstOrDefault();
                                        var bandaraParse1 = bandaraRaw.Split(' ').ToList();

                                        string bandara1Index11, bandara1;
                                        int bandara1Index1, bandara1Index2;

                                        //Untuk nama bandara tidak dipisah
                                        if (bandaraParse1[0].IndexOf("(") == -1)
                                            {
                                                bandara1Index1 = bandaraParse1[1].IndexOf("(");
                                                bandara1Index11 = bandaraParse1[1].Substring(bandara1Index1 + 1);
                                                bandara1Index2 = bandara1Index11.IndexOf(")");
                                                bandara1 = bandaraParse1[1].Substring(bandara1Index1 + 1, (bandara1Index2));
                                            }
                                        else
                                        //Untuk nama bandara dipisah
                                            {
                                                bandara1Index1 = bandaraParse1[0].IndexOf("(");
                                                bandara1Index11 = bandaraParse1[0].Substring(bandara1Index1 + 1);
                                                bandara1Index2 = bandara1Index11.IndexOf(")");
                                                bandara1 = bandaraParse1[0].Substring(bandara1Index1 + 1, (bandara1Index2));
                                            }
                                       

                                        string bandara2Index11, bandara2;
                                        int bandara2Index1, bandara2Index2;

                                        if ( bandaraParse1[2].IndexOf("(") == -1)
                                            {
                                                 bandara2Index1 = bandaraParse1[3].IndexOf("(");
                                                 bandara2Index11 = bandaraParse1[3].Substring(bandara2Index1 + 1);
                                                 bandara2Index2 = bandara2Index11.IndexOf(")");
                                                 bandara2 = bandaraParse1[3].Substring(bandara2Index1 + 1, (bandara2Index2));
                                            }
                                        else
                                            {
                                                bandara2Index1 = bandaraParse1[2].IndexOf("(");
                                                bandara2Index11 = bandaraParse1[2].Substring(bandara2Index1 + 1);
                                                bandara2Index2 = bandara2Index11.IndexOf(")");
                                                bandara2 = bandaraParse1[2].Substring(bandara2Index1 + 1, (bandara2Index2));
                                            }
                                        

                                        var flight = FlightService.GetInstance();

                                        //Ambil jadwal dari AJAX
                                        var ambilJadwal = isiData.MakeRoot()["dd:nth-child(" + ((1 + (3 * code)) + 2) + ")"];
                                        var jadwalRaw = ambilJadwal.Select(x => x.Cq().Text()).FirstOrDefault();
                                        var jadwalParse1 = jadwalRaw.Split(' ').ToList();

                                        var DeptTimeIndex = jadwalParse1[0].IndexOf('\t');
                                        var ArrTimeIndex = jadwalParse1[5].IndexOf('\t');
                                        DateTime departureDate = DateTime.Parse(jadwalParse1[0].Substring(DeptTimeIndex + 1));
                                        DateTime arrivalDate = DateTime.Parse(jadwalParse1[5].Substring(ArrTimeIndex + 1));
                                        var coba = departureDate.ToString("dd/MM/yyyy");
                                        var coba1 = arrivalDate.ToString("dd/MM/yyyy");
                                        string format = "dd/MM/yyyy hh.mm.ss tt";
                                        CultureInfo provider = CultureInfo.InvariantCulture;
                                        var departureTime = DateTime.ParseExact(coba + " " + jadwalParse1[1]+ " " + jadwalParse1[2], format, provider);
                                        var arrivalTime = DateTime.ParseExact(coba1 + " " + jadwalParse1[6] + " " + jadwalParse1[7], format, provider);
                                        var deptime = departureTime.AddHours(-(flight.GetAirportTimeZone(bandara1)));
                                        var arrtime = arrivalTime.AddHours(-(flight.GetAirportTimeZone(bandara2)));

                                        tampungPesawat.Add(codeParse1[0] +"."+ codeParse1[1]);
                                        tampungPesawatString = string.Join(".", tampungPesawat.ToArray());

                                        var baggage = GetBaggage(bandara1, bandara2);
                                        var isBaggageIncluded = false;
                                        if (baggage != null)
                                        {
                                            isBaggageIncluded = true;
                                        }
                                        
                                        segments.Add(new FlightSegment
                                        {
                                            AirlineCode = codeParse1[0],
                                            FlightNumber = codeParse1[1],
                                            CabinClass = CabinClass.Economy,
                                            AirlineType = AirlineType.Lcc,
                                            Rbd = rbdEko[code],
                                            DepartureAirport = bandara1,
                                            DepartureTime = DateTime.SpecifyKind(departureTime,DateTimeKind.Utc),
                                            ArrivalAirport = bandara2,
                                            ArrivalTime = DateTime.SpecifyKind(arrivalTime, DateTimeKind.Utc),
                                            OperatingAirlineCode = codeParse1[0],
                                            StopQuantity = 0,
                                            Duration = arrtime - deptime,
                                            IsMealIncluded = true,
                                            IsPscIncluded = true,
                                            IsBaggageIncluded = isBaggageIncluded,
                                            BaggageCapacity = baggage
                                        });
                                    }

                                    var prefix =
                                        "" + tampungPesawatString + "" +
                                        "." + trip0.OriginAirport + "" +
                                        "." + trip0.DestinationAirport + "" +
                                        "?" + trip0.DepartureDate.Year + "" +
                                        "-" + trip0.DepartureDate.Month + "" +
                                        "-" + trip0.DepartureDate.Day + "" +
                                        "|" + conditions.AdultCount + "" +
                                        "." + conditions.ChildCount + "" +
                                        "." + conditions.InfantCount + "" +
                                        "|" + harga + "" +
                                        "." + "1.";

                                    var itin = new FlightItinerary
                                    {
                                        AdultCount = conditions.AdultCount,
                                        ChildCount = conditions.ChildCount,
                                        InfantCount = conditions.InfantCount,
                                        CanHold = true,
                                        FareType = FareType.Published,
                                        RequireBirthDate = false,
                                        RequirePassport = false,
                                        RequireSameCheckIn = false,
                                        RequireNationality = false,
                                        RequestedCabinClass = CabinClass.Economy,
                                        TripType = TripType.OneWay,
                                        Supplier = Supplier.Sriwijaya,
                                        Price = new Price(),
                                        AdultPricePortion = hargaAdult/harga,
                                        ChildPricePortion = hargaChild/harga,
                                        InfantPricePortion = hargaInfant/harga,
                                        FareId = prefix + FID,
                                        Trips = new List<FlightTrip>
                                    {
                                       new FlightTrip()
                                       {
                                           Segments = segments,
                                               OriginAirport = trip0.OriginAirport,
                                               DestinationAirport = trip0.DestinationAirport,
                                               DepartureDate = DateTime.SpecifyKind(trip0.DepartureDate,DateTimeKind.Utc)
                                       }
                                    }
                                    };
                                    itin.Price.SetSupplier(harga, new Currency("IDR"));
                                    itins.Add(itin);
                                    hasil.IsSuccess = true;
                                    hasil.Itineraries = itins;

                                    //if (hasil.Itineraries == null)
                                    //    return new SearchFlightResult
                                    //    {
                                    //        Itineraries = new List<FlightItinerary>(),
                                    //        IsSuccess = 
                                    //    };
                                }
                                #endregion
                                break;

                            case CabinClass.Business:
                                #region
                                {
                                    //FareIDBisnis
                                    try
                                    {
                                        var ambilFIDB = tunjuk.MakeRoot()[".businessFare>div>input"];
                                        FIDB = ambilFIDB.Select(x => x.Cq().Attr("value")).FirstOrDefault().Trim();
                                    }
                                    catch (Exception)
                                    {
                                        new SearchFlightResult
                                        {
                                            Itineraries = new List<FlightItinerary>(),
                                            IsSuccess = true
                                        };
                                        continue;
                                    }
                                   
                                    ParseFID1B = FIDB.Split(',').ToList();
                                    string FIDsegmentBis1;
                                    string FIDsegmentBis2;
                                    string FIDsegmentBis3;
                                    var rbdBis = new List<string>();

                                    if ((ParseFID1B.Count > 1) && (ParseFID1B.Count <= 3))
                                    {
                                        FIDsegmentBis1 = ParseFID1B[0];
                                        FIDsegmentBis2 = ParseFID1B[1].Substring(0, (ParseFID1B[1].Length - 2));
                                        var rbdIndex1 = FIDB.IndexOf(":");
                                        var rbdIndex11 = FIDB.Substring(rbdIndex1 + 1);
                                        var rbdIndex2 = rbdIndex11.IndexOf(":");
                                        var rbdRaw = FIDB.Substring(rbdIndex1 + 1, (rbdIndex2));
                                        rbdBis = rbdRaw.Split(',').ToList();

                                    }

                                    else
                                    if ((ParseFID1B.Count > 3) && (ParseFID1B.Count <= 5))
                                    {
                                        FIDsegmentBis1 = ParseFID1B[0];
                                        FIDsegmentBis2 = ParseFID1B[1];
                                        FIDsegmentBis3 = ParseFID1B[2].Substring(0, (ParseFID1B[2].Length - 2));
                                        var rbdIndex1 = FIDB.IndexOf(":");
                                        var rbdIndex11 = FIDB.Substring(rbdIndex1 + 1);
                                        var rbdIndex2 = rbdIndex11.IndexOf(":");
                                        var rbdRaw = FIDB.Substring(rbdIndex1 + 1, (rbdIndex2));
                                        rbdBis = rbdRaw.Split(',').ToList();
                                    }

                                    else
                                    if (ParseFID1B.Count == 1)
                                    {
                                        FIDsegmentBis1 = ParseFID1B[0];
                                        var rbdIndex1 = FIDB.IndexOf(":");
                                        var rbdIndex11 = FIDB.Substring(rbdIndex1 + 1);
                                        var rbdIndex2 = rbdIndex11.IndexOf(":");
                                        var rbdRaw = FIDB.Substring(rbdIndex1 + 1, (rbdIndex2));
                                        rbdBis = rbdRaw.Split(',').ToList();
                                    }

                                    else
                                    {
                                        return new SearchFlightResult
                                        {
                                            IsSuccess = false,
                                            Errors = new List<FlightError> { FlightError.TechnicalError },
                                            ErrorMessages = new List<string> { "Web Layout Changed!" }
                                        };
                                    }

                                    //AJAX
                                    url = "application/pricingDetail_.php";
                                    var fareRequest = new RestRequest(url, Method.GET);
                                    fareRequest.AddQueryParameter("voteFROM", FIDB);
                                    fareRequest.AddQueryParameter("nameFROM", "radioFrom");
                                    fareRequest.AddQueryParameter("voteTO", null);
                                    fareRequest.AddQueryParameter("nameTO", null);
                                    fareRequest.AddQueryParameter("STI", "true");
                                    fareRequest.AddQueryParameter("RR", "NO");
                                    fareRequest.AddQueryParameter("ADM", null);
                                    var fareResponse = client.Execute(fareRequest);

                                    var responAjax = fareResponse.Content;


                                    CQ ambilDataAjax = (CQ)responAjax;


                                    var isiSegment = ambilDataAjax[".bs>dl>.flightNumBS"];
                                    int jumlahSegment = isiSegment.Count();
                                    //if (jumlahSegment == 0)
                                    //{
                                    //    return new SearchFlightResult
                                    //    {
                                    //        Itineraries = new List<FlightItinerary>(),
                                    //        IsSuccess = true
                                    //    };
                                    //}
                                    int jumlahdata = jumlahSegment * 3;
                                    var isiData = ambilDataAjax[".bs>dl>dd"];
                                    var jumlahDiIsiData = isiData.Count();
                                    //if (jumlahDiIsiData == 0)
                                    //{
                                    //    return new SearchFlightResult
                                    //    {
                                    //        Itineraries = new List<FlightItinerary>(),
                                    //        IsSuccess = true
                                    //    };
                                    //}

                                    //Price 

                                    fareRequest.AddQueryParameter("break", "YES");
                                    var fareResponse2 = client.Execute(fareRequest);
                                    var responAjax2 = fareResponse2.Content;

                                    CQ ambilDataAjax2 = (CQ)responAjax2;

                                    var tunjukHarga = ambilDataAjax2[".priceDetail.bold"];
                                    var ambilharga = tunjukHarga.Select(x => x.Cq().Text()).LastOrDefault();
                                    var harga = decimal.Parse(ambilharga.Split(' ')[1]);
                                    var hargaAdult = 0M;
                                    var hargaChild = 0M;
                                    var hargaInfant = 0M;
                                    try
                                    {
                                        hargaAdult = decimal.Parse(ambilDataAjax2["#fareDetailAdult .priceDetail"].Last().Text());
                                        hargaChild = decimal.Parse(ambilDataAjax2["#fareDetailChild .priceDetail"].Last().Text());
                                        hargaInfant = decimal.Parse(ambilDataAjax2["#fareDetailInfant .priceDetail"].Last().Text());
                                    }
                                    catch { }
                                    var isPscIncluded = ambilDataAjax2.Text().Contains("PSC");

                                    var segments = new List<FlightSegment>();
                                    var tampungPesawat = new List<string>();
                                    string tampungPesawatString = null;
                                    for (int code = 0; code < jumlahSegment; code++)
                                    {
                                        //Airline
                                        var ambilACode = isiData.MakeRoot()["dd:nth-child(" + (1 + (3 * code)) + ")"];
                                        var codeRaw = ambilACode.Select(x => x.Cq().Text()).FirstOrDefault();
                                        var codeParse1 = codeRaw.Split(' ').ToList();
                                        var ambilBandara = isiData.MakeRoot()["dd:nth-child(" + ((1 + (3 * code)) + 1) + ")"];
                                        var bandaraRaw = ambilBandara.Select(x => x.Cq().Text()).FirstOrDefault();
                                        var bandaraParse1 = bandaraRaw.Split(' ').ToList();

                                        string bandara1Index11, bandara1;
                                        int bandara1Index1, bandara1Index2;
                                        if (bandaraParse1[0].IndexOf("(") == -1)
                                            {
                                                bandara1Index1 = bandaraParse1[1].IndexOf("(");
                                                bandara1Index11 = bandaraParse1[1].Substring(bandara1Index1 + 1);
                                                bandara1Index2 = bandara1Index11.IndexOf(")");
                                                bandara1 = bandaraParse1[1].Substring(bandara1Index1 + 1, (bandara1Index2));
                                            }
                                        else
                                            {
                                                bandara1Index1 = bandaraParse1[0].IndexOf("(");
                                                bandara1Index11 = bandaraParse1[0].Substring(bandara1Index1 + 1);
                                                bandara1Index2 = bandara1Index11.IndexOf(")");
                                                bandara1 = bandaraParse1[0].Substring(bandara1Index1 + 1, (bandara1Index2));
                                            }


                                        string bandara2Index11, bandara2;
                                        int bandara2Index1, bandara2Index2;

                                        if (bandaraParse1[2].IndexOf("(") == -1)
                                            {
                                                bandara2Index1 = bandaraParse1[3].IndexOf("(");
                                                bandara2Index11 = bandaraParse1[3].Substring(bandara2Index1 + 1);
                                                bandara2Index2 = bandara2Index11.IndexOf(")");
                                                bandara2 = bandaraParse1[3].Substring(bandara2Index1 + 1, (bandara2Index2));
                                            }
                                        else
                                            {
                                                bandara2Index1 = bandaraParse1[2].IndexOf("(");
                                                bandara2Index11 = bandaraParse1[2].Substring(bandara2Index1 + 1);
                                                bandara2Index2 = bandara2Index11.IndexOf(")");
                                                bandara2 = bandaraParse1[2].Substring(bandara2Index1 + 1, (bandara2Index2));
                                            }


                                        var flight = FlightService.GetInstance();
                                        var ambilJadwal = isiData.MakeRoot()["dd:nth-child(" + ((1 + (3 * code)) + 2) + ")"];
                                        var jadwalRaw = ambilJadwal.Select(x => x.Cq().Text()).FirstOrDefault();
                                        var jadwalParse1 = jadwalRaw.Split(' ').ToList();
                                        var DeptTimeIndex = jadwalParse1[0].IndexOf('\t');
                                        var ArrTimeIndex = jadwalParse1[5].IndexOf('\t');
                                        DateTime departureDate = DateTime.Parse(jadwalParse1[0].Substring(DeptTimeIndex + 1));
                                        DateTime arrivalDate = DateTime.Parse(jadwalParse1[5].Substring(ArrTimeIndex + 1));
                                        var coba = departureDate.ToString("dd/MM/yyyy");
                                        var coba1 = arrivalDate.ToString("dd/MM/yyyy");
                                        string format = "dd/MM/yyyy hh.mm.ss tt";
                                        CultureInfo provider = CultureInfo.InvariantCulture;
                                        var departureTime = DateTime.ParseExact(coba + " " + jadwalParse1[1] + " " + jadwalParse1[2], format, provider);
                                        var arrivalTime = DateTime.ParseExact(coba1 + " " + jadwalParse1[6] + " " + jadwalParse1[7], format, provider);
                                        var deptime = departureTime.AddHours(-(flight.GetAirportTimeZone(bandara1)));
                                        var arrtime = arrivalTime.AddHours(-(flight.GetAirportTimeZone(bandara2)));
                                        tampungPesawat.Add(codeParse1[0] + "." + codeParse1[1]);
                                        tampungPesawatString = string.Join(".", tampungPesawat.ToArray());
                                        segments.Add(new FlightSegment
                                        {
                                            AirlineCode = codeParse1[0],
                                            FlightNumber = codeParse1[1],
                                            CabinClass = CabinClass.Business,
                                            Rbd = rbdBis[code],
                                            DepartureAirport = bandara1,
                                            DepartureTime = DateTime.SpecifyKind(departureTime,DateTimeKind.Utc),
                                            ArrivalAirport = bandara2,
                                            ArrivalTime = DateTime.SpecifyKind(arrivalTime,DateTimeKind.Utc),
                                            OperatingAirlineCode = codeParse1[0],
                                            StopQuantity = 0,
                                            Duration = arrtime - deptime,
                                            IsMealIncluded = true,
                                            IsPscIncluded = true
                                        });
                                    }

                                    var prefix =
                                         "" + tampungPesawatString + "" +
                                        "." + trip0.OriginAirport + "" +
                                        "." + trip0.DestinationAirport + "" +
                                        "?" + trip0.DepartureDate.Year + "" +
                                        "-" + trip0.DepartureDate.Month + "" +
                                        "-" + trip0.DepartureDate.Day + "" +
                                        "|" + conditions.AdultCount + "" +
                                        "." + conditions.ChildCount + "" +
                                        "." + conditions.InfantCount + "" +
                                        "|" + harga + "" + 
                                        "." + "2.";

                                    var itin = new FlightItinerary
                                    {
                                        AdultCount = conditions.AdultCount,
                                        ChildCount = conditions.ChildCount,
                                        InfantCount = conditions.InfantCount,
                                        CanHold = true,
                                        FareType = FareType.Published,
                                        RequireBirthDate = true,
                                        RequirePassport = false,
                                        RequireSameCheckIn = false,
                                        RequireNationality = true,
                                        RequestedCabinClass = CabinClass.Business,
                                        TripType = TripType.OneWay,
                                        Supplier = Supplier.Citilink,
                                        Price = new Price(),
                                        AdultPricePortion = hargaAdult/harga,
                                        ChildPricePortion = hargaChild/harga,
                                        InfantPricePortion = hargaInfant/harga,
                                        FareId = prefix + FIDB,
                                        Trips = new List<FlightTrip>
                                        {
                                           new FlightTrip()
                                           {
                                                Segments = segments,
                                                OriginAirport = trip0.OriginAirport,
                                                DestinationAirport = trip0.DestinationAirport,
                                                DepartureDate = DateTime.SpecifyKind(trip0.DepartureDate,DateTimeKind.Utc)
                                           }
                                        }
                                    };
                                    itin.Price.SetSupplier(harga, new Currency("IDR"));
                                    itins.Add(itin);
                                    hasil.IsSuccess = true;
                                    hasil.Itineraries = itins;

                                    //if (hasil.Itineraries == null)
                                    //    return new SearchFlightResult
                                    //    {
                                    //        Itineraries = new List<FlightItinerary>(),
                                    //        IsSuccess = true
                                    //    };
                                }
                                #endregion
                                break;
                        }
                    }
                    Logout(client);
                    return hasil;
                }
                catch
                {   
                    Logout(client);
                    return new SearchFlightResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { "Web Layout Changed!" }
                    };
                }
            }
        }
    }
}
