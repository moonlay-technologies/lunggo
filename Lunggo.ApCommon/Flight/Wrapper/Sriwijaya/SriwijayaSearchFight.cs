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
                
                // WAIT
                var client = new ExtendedWebClient();
                var hasil = new SearchFlightResult();
                var convertBulan = conditions.Trips[0].DepartureDate.ToString("MMMM");
                var bulan3 = convertBulan.Substring(0, 3);
                Client.CreateSession(client);

                client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                //Headers["Accept-Encoding"] = "gzip, deflate";
                client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                //Headers["Origin"] = "https://booking2.airasia.com";
                client.Headers["Referer"] = "https://www.sriwijayaair.co.id/welcome.php";
                client.Headers["Host"] = "www.sriwijayaair.co.id";

                var frontpageParams =
                    @"form_build_id=form-443a2fb12e018591436589029dabcde0" +
                    @"&form_id=flute_location_language_form" +
                    @"&form_id=flute_location_language_form" +
                    @"&location=ID" +
                    @"op=Choose";
                client.Expect100Continue = false;
                client.UploadString(@"https://www.sriwijayaair.co.id/welcome.php", frontpageParams);


                var searchParams = @"tanggalBerangkat=" + conditions.Trips[0].DepartureDate.Day + "-" + bulan3 + "-" + conditions.Trips[0].DepartureDate.Year +
                                 @"&ADT=" + conditions.AdultCount +
                                 @"&CHD=" + conditions.ChildCount +
                                 @"&INF=" + conditions.InfantCount +
                                 @"&returndaterange=0" +
                                 @"&return=NO" +
                                 @"&Submit=Pencarian" +
                                 @"&ruteTujuan=" + conditions.Trips[0].DestinationAirport +
                                 @"&ruteBerangkat=" + conditions.Trips[0].OriginAirport +
                                 @"&vSub=YES";

                client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                //client.Headers["Accept-Encoding"] = "gzip, deflate";
                client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                client.Headers["Upgrade-Insecure-Requests"] = "1";
                client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                client.Headers["Referer"] = "https://www.sriwijayaair.co.id/application/?action=index";
                client.Headers["Host"] = "www.sriwijayaair.co.id";
                client.Headers["Origin"] = "https://www.sriwijayaair.co.id";
                client.AutoRedirect = false;
                //client.Expect100Continue = false;

                var htmlRespon = client.UploadString(@"https://www.sriwijayaair.co.id/application/?action=booking", searchParams);
                CQ cobaAmbilTable;

                if (client.ResponseUri.AbsoluteUri == "https://www.sriwijayaair.co.id/application/?action=booking")
                {
                    cobaAmbilTable = (CQ)htmlRespon;
                }
                else
                {
                    return new SearchFlightResult { Errors = new List<FlightError> { FlightError.FailedOnSupplier } };
                }

                //if (client.ResponseUri.AbsolutePath != "/ScheduleSelect.aspx")
                //    return new SearchFlightResult { Errors = new List<FlightError> { FlightError.FareIdNoLongerValid } };

                try
                {
                    var isi = cobaAmbilTable[".tableAvailability tr"];
                    int i = isi.Count();
                    if (i == 0)
                    {
                        return new SearchFlightResult
                        {
                            Itineraries = new List<FlightItinerary>(),
                            IsSuccess = true
                        };
                    }

                    var itins = new List<FlightItinerary>();
                    for (int j = 2; j <= i; j++)
                    {
                        var FlightTrips = new FlightTrip();
                        var tunjuk = isi.MakeRoot()["tr:nth-child(" + j + ")"];
                        if (tunjuk == null)
                        {
                            return new SearchFlightResult
                        {
                            Itineraries = new List<FlightItinerary>(),
                            IsSuccess = true
                        };
                        }
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
                                    var ambilFID = tunjuk.MakeRoot()[".economyFare>div>input"];                                  
                                    FID = ambilFID.Select(x => x.Cq().Attr("value")).FirstOrDefault().Trim();
                                    ParseFID1 = FID.Split(',').ToList();
                                    
                                    string FIDsegmentEko1;
                                    string FIDsegmentEko2;
                                    string FIDsegmentEko3;
                                    var rbdEko = new List<string>();

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
                                    if (ParseFID1.Count == 1)
                                    {
                                        FIDsegmentEko1 = ParseFID1[0];
                                        var rbdIndex1 = FID.IndexOf(":");
                                        var rbdIndex11 = FID.Substring(rbdIndex1 + 1);
                                        var rbdIndex2 = rbdIndex11.IndexOf(":");
                                        var rbdRaw = FID.Substring(rbdIndex1 + 1, (rbdIndex2));
                                        rbdEko = rbdRaw.Split(',').ToList();
                                    }

                                    //AJAX
                                    client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                                    //Headers["Accept-Encoding"] = "gzip, deflate";
                                    client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                                    client.Headers["Upgrade-Insecure-Requests"] = "1";
                                    client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                                    client.Headers["Referer"] = "https://www.sriwijayaair.co.id/application/?action=booking";
                                    //client.Headers["X-Requested-With"] = "XMLHttpRequest";
                                    client.Headers["Host"] = "www.sriwijayaair.co.id";

                                    var url =
                                        "https://www.sriwijayaair.co.id/application/pricingDetail_.php?" +
                                        "voteFROM=" + FID +
                                        "&nameFROM=radioFrom" +
                                        "&voteTO=" +
                                        "&nameTO=" +
                                        "&STI=true" +
                                        "&RR=NO&ADM=";


                                    var responAjax = client.DownloadString(url);

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
                                        

                                        var dict = DictionaryService.GetInstance();
                                        var ambilJadwal = isiData.MakeRoot()["dd:nth-child(" + ((1 + (3 * code)) + 2) + ")"];
                                        var jadwalRaw = ambilJadwal.Select(x => x.Cq().Text()).FirstOrDefault();
                                        var jadwalParse1 = jadwalRaw.Split(' ').ToList();
                                        var DeptTimeIndex = jadwalParse1[0].IndexOf('\t');
                                        var ArrTimeIndex = jadwalParse1[5].IndexOf('\t');
                                        DateTime departureDate = DateTime.Parse(jadwalParse1[0].Substring(DeptTimeIndex + 1));
                                        DateTime arrivalDate = DateTime.Parse(jadwalParse1[5].Substring(ArrTimeIndex + 1));
                                        var coba = departureDate.ToString("dd/MM/yyyy");
                                        var coba1 = arrivalDate.ToString("dd/MM/yyyy");
                                        string format = "dd/MM/yyyy hh.mm.ss";
                                        CultureInfo provider = CultureInfo.InvariantCulture;
                                        var departureTime = DateTime.ParseExact(coba + " " + jadwalParse1[1], format, provider);
                                        var arrivalTime = DateTime.ParseExact(coba1 + " " + jadwalParse1[6], format, provider);
                                        var deptime = departureTime.AddHours(-(dict.GetAirportTimeZone(bandara1)));
                                        var arrtime = arrivalTime.AddHours(-(dict.GetAirportTimeZone(bandara2)));
                                        tampungPesawat.Add(codeParse1[0] +"."+ codeParse1[1]);
                                        tampungPesawatString = string.Join(".", tampungPesawat.ToArray());
                                        segments.Add(new FlightSegment
                                        {

                                            AirlineCode = codeParse1[0],
                                            FlightNumber = codeParse1[1],
                                            CabinClass = CabinClass.Economy,
                                            Rbd = rbdEko[code],
                                            DepartureAirport = bandara1,
                                            DepartureTime = DateTime.SpecifyKind(departureTime,DateTimeKind.Utc),
                                            ArrivalAirport = bandara2,
                                            ArrivalTime = DateTime.SpecifyKind(arrivalTime, DateTimeKind.Utc),
                                            OperatingAirlineCode = codeParse1[0],
                                            StopQuantity = 0,
                                            Duration = arrtime - deptime
                                        });
                                    }

                                    //Price 

                                    var tunjukHarga = ambilDataAjax[".totalPrice"];
                                    var ambilharga = tunjukHarga.Select(x => x.Cq().Text()).FirstOrDefault();
                                    var harga = ambilharga.Split(' ');

                                    var prefix =
                                        "" + tampungPesawatString + "" +
                                        "." + conditions.Trips[0].OriginAirport + "" +
                                        "." + conditions.Trips[0].DestinationAirport + "" +
                                        "?" + conditions.Trips[0].DepartureDate.Year + "" +
                                        "-" + conditions.Trips[0].DepartureDate.Month + "" +
                                        "-" + conditions.Trips[0].DepartureDate.Day + "" +
                                        "|" + conditions.AdultCount + "" +
                                        "." + conditions.ChildCount + "" +
                                        "." + conditions.InfantCount + "" +
                                        "|" + decimal.Parse(harga[1]) + "" +
                                        "." + "1.";

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
                                        RequestedCabinClass = CabinClass.Economy,
                                        TripType = TripType.OneWay,
                                        Supplier = Supplier.Sriwijaya,
                                        SupplierCurrency = "IDR",
                                        SupplierRate = 1,
                                        SupplierPrice = decimal.Parse(harga[1]),
                                        FareId = prefix + FID,
                                        Trips = new List<FlightTrip>
                                    {
                                       new FlightTrip()
                                       {
                                           Segments = segments,
                                           OriginAirport = conditions.Trips[0].OriginAirport,
                                           DestinationAirport = conditions.Trips[0].DestinationAirport,
                                           DepartureDate = conditions.Trips[0].DepartureDate
                                       }
                                    }
                                    };
                                    itins.Add(itin);
                                    hasil.IsSuccess = true;
                                    hasil.Itineraries = itins;

                                    if (hasil.Itineraries == null)
                                        return new SearchFlightResult
                                        {
                                            Itineraries = new List<FlightItinerary>(),
                                            IsSuccess = true
                                        };

                                }
                                #endregion
                                break;

                            case CabinClass.Business:
                                #region
                                {
                                    //FareIDBisnis
                                    
                                    var ambilFIDB = tunjuk.MakeRoot()[".businessFare>div>input"];
                                    if (ambilFIDB.FirstElement() != null)
                                    {
                                        FIDB = ambilFIDB.Select(x => x.Cq().Attr("value")).FirstOrDefault().Trim();
                                        ParseFID1B = FIDB.Split(',').ToList();
                                        string FIDsegmentBis1;
                                        string FIDsegmentBis2;
                                        string FIDsegmentBis3;
                                        var rbdBis = new List<string>();

                                        if ((ParseFID1B.Count > 1) && (ParseFID1B.Count <= 3))
                                        {
                                            FIDsegmentBis1 = ParseFID1[0];
                                            FIDsegmentBis2 = ParseFID1[1].Substring(0, (ParseFID1[1].Length - 2));
                                            var rbdIndex1 = FIDB.IndexOf(":");
                                            var rbdIndex11 = FIDB.Substring(rbdIndex1 + 1);
                                            var rbdIndex2 = rbdIndex11.IndexOf(":");
                                            var rbdRaw = FIDB.Substring(rbdIndex1 + 1, (rbdIndex2));
                                            rbdBis = rbdRaw.Split(',').ToList();

                                        }
                                        if ((ParseFID1B.Count > 3) && (ParseFID1B.Count <= 5))
                                        {
                                            FIDsegmentBis1 = ParseFID1[0];
                                            FIDsegmentBis2 = ParseFID1[1];
                                            FIDsegmentBis3 = ParseFID1[2].Substring(0, (ParseFID1[2].Length - 2));
                                            var rbdIndex1 = FIDB.IndexOf(":");
                                            var rbdIndex11 = FIDB.Substring(rbdIndex1 + 1);
                                            var rbdIndex2 = rbdIndex11.IndexOf(":");
                                            var rbdRaw = FIDB.Substring(rbdIndex1 + 1, (rbdIndex2));
                                            rbdBis = rbdRaw.Split(',').ToList();
                                        }
                                        if (ParseFID1B.Count == 1)
                                        {
                                            FIDsegmentBis1 = ParseFID1[0];
                                            var rbdIndex1 = FIDB.IndexOf(":");
                                            var rbdIndex11 = FIDB.Substring(rbdIndex1 + 1);
                                            var rbdIndex2 = rbdIndex11.IndexOf(":");
                                            var rbdRaw = FIDB.Substring(rbdIndex1 + 1, (rbdIndex2));
                                            rbdBis = rbdRaw.Split(',').ToList();
                                        }

                                        //AJAX
                                        client.Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                                        //Headers["Accept-Encoding"] = "gzip, deflate";
                                        client.Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                                        client.Headers["Upgrade-Insecure-Requests"] = "1";
                                        client.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                                        client.Headers["Referer"] = "https://www.sriwijayaair.co.id/application/?action=booking";
                                        //client.Headers["X-Requested-With"] = "XMLHttpRequest";
                                        client.Headers["Host"] = "www.sriwijayaair.co.id";

                                        var url =
                                            "https://www.sriwijayaair.co.id/application/pricingDetail_.php?" +
                                            "voteFROM=" + FIDB +
                                            "&nameFROM=radioFrom" +
                                            "&voteTO=" +
                                            "&nameTO=" +
                                            "&STI=true" +
                                            "&RR=NO&ADM=";


                                        var responAjax = client.DownloadString(url);


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
                                        for (int code = 0; code < jumlahSegment; code++)
                                        {
                                            //Airline
                                            var ambilACode = isiData.MakeRoot()["dd:nth-child(" + (1 + (3 * code)) + ")"];
                                            var codeRaw = ambilACode.Select(x => x.Cq().Text()).FirstOrDefault();
                                            var codeParse1 = codeRaw.Split(' ').ToList();
                                            var ambilBandara = isiData.MakeRoot()["dd:nth-child(" + ((1 + (3 * code)) + 1) + ")"];
                                            var bandaraRaw = ambilBandara.Select(x => x.Cq().Text()).FirstOrDefault();
                                            var bandaraParse1 = bandaraRaw.Split(' ').ToList();

                                            var bandara1Index1 = bandaraParse1[0].IndexOf("(");
                                            var bandara1Index11 = bandaraParse1[0].Substring(bandara1Index1 + 1);
                                            var bandara1Index2 = bandara1Index11.IndexOf(")");
                                            var bandara1 = bandaraParse1[0].Substring(bandara1Index1 + 1, (bandara1Index2));

                                            var bandara2Index1 = bandaraParse1[2].IndexOf("(");
                                            var bandara2Index11 = bandaraParse1[2].Substring(bandara2Index1 + 1);
                                            var bandara2Index2 = bandara2Index11.IndexOf(")");
                                            var bandara2 = bandaraParse1[2].Substring(bandara2Index1 + 1, (bandara2Index2));

                                            var dict = DictionaryService.GetInstance();
                                            var ambilJadwal = isiData.MakeRoot()["dd:nth-child(" + ((1 + (3 * code)) + 2) + ")"];
                                            var jadwalRaw = ambilJadwal.Select(x => x.Cq().Text()).FirstOrDefault();
                                            var jadwalParse1 = jadwalRaw.Split(' ').ToList();
                                            var DeptTimeIndex = jadwalParse1[0].IndexOf('\t');
                                            var ArrTimeIndex = jadwalParse1[5].IndexOf('\t');
                                            var departureTime = DateTime.Parse(jadwalParse1[0].Substring(DeptTimeIndex + 1));
                                            var arrivalTime = DateTime.Parse(jadwalParse1[5].Substring(ArrTimeIndex + 1));
                                            var arrtime = departureTime.AddHours(-(dict.GetAirportTimeZone(bandara1)));
                                            var deptime = arrivalTime.AddHours(-(dict.GetAirportTimeZone(bandara2)));
                                            tampungPesawat.Add(codeParse1[0] + codeParse1[1]);
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
                                                Duration = arrtime - deptime
                                            });
                                        }

                                        //Price 

                                        var tunjukHarga = ambilDataAjax[".totalPrice"];
                                        var ambilharga = tunjukHarga.Select(x => x.Cq().Text()).FirstOrDefault();
                                        var harga = ambilharga.Split(' ');

                                        

                                        var prefix =
                                             "" + tampungPesawatString + "" +
                                            "." + conditions.Trips[0].OriginAirport + "" +
                                            "." + conditions.Trips[0].DestinationAirport + "" +
                                            "?" + conditions.Trips[0].DepartureDate.Year + "" +
                                            "-" + conditions.Trips[0].DepartureDate.Month + "" +
                                            "-" + conditions.Trips[0].DepartureDate.Day + "" +
                                            "|" + conditions.AdultCount + "" +
                                            "." + conditions.ChildCount + "" +
                                            "." + conditions.InfantCount + "" +
                                            "|" + decimal.Parse(harga[1]) + "" + 
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
                                            SupplierCurrency = "IDR",
                                            SupplierRate = 1,
                                            SupplierPrice = decimal.Parse(harga[1]),
                                            FareId = prefix + FIDB,
                                            Trips = new List<FlightTrip>
                                    {
                                       new FlightTrip()
                                       {
                                           Segments = segments,
                                           OriginAirport = conditions.Trips[0].OriginAirport,
                                           DestinationAirport = conditions.Trips[0].DestinationAirport
                                       }
                                    }
                                        };
                                        itins.Add(itin);
                                        hasil.IsSuccess = true;
                                        hasil.Itineraries = itins;

                                        if (hasil.Itineraries == null)
                                            return new SearchFlightResult
                                            {
                                                Itineraries = new List<FlightItinerary>(),
                                                IsSuccess = true
                                            };
    
                                    }
                                }
                                #endregion
                                break;
                        }
                    }
                    return hasil;
                }
                catch
                {
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
