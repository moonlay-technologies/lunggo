using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Encoder;
using Lunggo.Framework.Web;
using RestSharp;
using Lunggo.ApCommon.Constant;

namespace Lunggo.ApCommon.Flight.Wrapper.Sriwijaya
{
    internal partial class SriwijayaWrapper
    {
        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo)
        {
            return Client.BookFlight(bookInfo);
        }

        private partial class SriwijayaClientHandler
        {
            internal BookFlightResult BookFlight(FlightBookingInfo bookInfo)
            {
                /*Revalidate Flight Before Book*/
                RevalidateConditions conditions = new RevalidateConditions
                {
                    Itinerary = bookInfo.Itinerary
                };
                //conditions.Itinerary = bookInfo.Itinerary;
                RevalidateFareResult revalidateResult = RevalidateFare(conditions);
                if (revalidateResult.IsItineraryChanged || revalidateResult.IsPriceChanged || (!revalidateResult.IsValid))
                {
                    return new BookFlightResult
                    {
                        IsValid = revalidateResult.IsValid,
                        ErrorMessages = revalidateResult.ErrorMessages,
                        Errors = revalidateResult.Errors,
                        IsItineraryChanged = revalidateResult.IsItineraryChanged,
                        IsPriceChanged = revalidateResult.IsPriceChanged,
                        IsSuccess = false,
                        NewItinerary = revalidateResult.NewItinerary,
                        NewPrice = revalidateResult.NewPrice,
                        Status = null
                    };
                }
                bookInfo.Itinerary = revalidateResult.NewItinerary;

                var clientx = CreateAgentClient();
                var hasil = new BookFlightResult();
                var Fare = bookInfo.Itinerary.FareId; 
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

                //Untuk 2 Segment
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
                        "radioFrom0_0=" + FIDsegment1 + "%3A" + Rbd[0] + "%3A" + unknownCode + "%3A" + ognAirport + "%3A" + arrAirport +
                        "%3AU2s5VlVrNUZXUT09" +
                        "&radioFrom0_1=" + FIDsegment2 + "%3A" + Rbd[1] + "%3A" + unknownCode + "%3A" + ognAirport + "%3A" + arrAirport +
                        "%3AU2s5VlVrNUZXUT09";

                }
                //Untuk 3 Segment
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
                    "radioFrom0_0=" + FIDsegment1 + "%3A" + Rbd[0] + "%3A" + unknownCode + "%3A" + ognAirport + "%3A" + arrAirport + "%3AU2s5VlVrNUZXUT09" +
                    "&radioFrom0_1=" + FIDsegment2 + "%3A" + Rbd[1] + "%3A" + unknownCode + "%3A" + ognAirport + "%3A" + arrAirport + "%3AU2s5VlVrNUZXUT09" +
                    "&radioFrom0_2=" + FIDsegment3 + "%3A" + Rbd[2] + "%3A" + unknownCode + "%3A" + ognAirport + "%3A" + arrAirport + "%3AU2s5VlVrNUZXUT09";
                }
                //Untuk 1 Segment
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
                        "radioFrom=" + FIDsegment1 + "%3A" + Rbd[0] + "%3A" + unknownCode + "%3A" + ognAirport + "%3A" + arrAirport +
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
                var htmlRespon = searchResponse.Content;
                CQ ambilFare = (CQ)htmlRespon;

                /*LAKUKAN DISINI GET BUAT PRICING DETAIL*/
                if (ParseFID1.Count == 1)
                {
                    #region
                    var cekFID1 = ambilFare["[value*=" + FIDsegment1 + "]"];
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
                        var fareResponse = clientx.Execute(fareRequest);
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
                        for (int y = 0; y < 1; y++)
                        {
                            var tunjukSetiapBandara = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (y + 1) + ")>td:nth-child(3)"];
                            bandaraRaw = tunjukSetiapBandara.Select(x => x.Cq().Text()).FirstOrDefault();
                            var bandara = bandaraRaw.Split('-').ToList();
                            var tunjukSetiapTanggal = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (y + 1) + ")>td:nth-child(1)"];
                            date = tunjukSetiapTanggal.Select(x => x.Cq().Text()).FirstOrDefault();
                            var tunjukSetiapJamBerangkat = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (y + 1) + ")>td:nth-child(5)"];
                            departure = tunjukSetiapJamBerangkat.Select(x => x.Cq().Text()).FirstOrDefault().Substring(0, 5);
                            var tunjukSetiapJamTiba = tunjukSelectedgo.MakeRoot()[".selectedgo:nth-child(" + (y + 1) + ")>td:nth-child(6)"];
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
                            var flight = FlightService.GetInstance();
                            var departureDate = DateTime.Parse(date + " " + departure, CultureInfo.CreateSpecificCulture("id-ID"));
                            var arrivalDate = (DateTime.Parse(date + " " + arrival, CultureInfo.CreateSpecificCulture("id-ID"))).AddDays(plusHari);
                            var deptime = departureDate.AddHours(-(flight.GetAirportTimeZone(ognAirport)));
                            var arrtime = arrivalDate.AddHours(-(flight.GetAirportTimeZone(arrAirport)));
                            tampungFare.Add("" + FIDsegments[y] + ":" + Rbd[y] + ":S:" + bandara[0] + ":" + bandara[1] + ":U2s5VlVrNUZXUT09;");
                            tampungFareString = string.Join(";", tampungFare.ToArray());
                            segments.Add(new FlightSegment
                            {
                                AirlineCode = ParseFare[y],
                                FlightNumber = ParseFare[y + 1],
                                CabinClass = (CabinClass)int.Parse(ParseFare[fareCabin]),
                                Rbd = Rbd[y],
                                DepartureAirport = bandara[0],
                                DepartureTime = DateTime.SpecifyKind(departureDate, DateTimeKind.Utc),
                                ArrivalAirport = bandara[1],
                                ArrivalTime = DateTime.SpecifyKind(arrivalDate, DateTimeKind.Utc),
                                OperatingAirlineCode = ParseFare[y],
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
                            TripType = TripType.OneWay,
                            Supplier = Supplier.Sriwijaya,
                            Price = new Price(),
                            AdultPricePortion = hargaAdult / hargaBaru,
                            ChildPricePortion = hargaChild / hargaBaru,
                            InfantPricePortion = hargaInfant / hargaBaru,
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
                        itin.Price.SetSupplier(hargaBaru, new Currency("IDR"));

                        var fixFlight = new BookFlightResult();
                        fixFlight.IsSuccess = true;
                        fixFlight.IsValid = true;
                        fixFlight.IsPriceChanged = bookInfo.Itinerary.Price.Supplier != hargaBaru;
                        fixFlight.IsItineraryChanged = !bookInfo.Itinerary.Identical(itin);
                        if (hasil.IsPriceChanged)
                        {
                            hasil.NewPrice = hargaBaru;
                        }
                        fixFlight.NewItinerary = itin;
                        if (revalidateResult.IsItineraryChanged || revalidateResult.IsPriceChanged || (!revalidateResult.IsValid))
                        {
                            return fixFlight;
                        }
                    }
                    else
                    {
                        return new BookFlightResult
                        {
                            IsSuccess = false,
                            Status = new BookingStatusInfo
                            {
                                BookingStatus = BookingStatus.Failed
                            },
                            Errors = new List<FlightError> { FlightError.FareIdNoLongerValid }
                        };
                    }
                    #endregion
                }

                /*END HERE*/
                
                int i = 0;
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PaxType.Adult))
                {
                    var title = passenger.Title == Title.Mister ? "MR" : "MRS";
                    bookingParams +=
                        "&adultTitle" + i + "=" + title +
                        "&adultFirst" + i + "=" + passenger.FirstName +
                        "&adultLast" + i + "=" + passenger.LastName +
                        "&adultId" + i + "=" + passenger.PassportNumber +
                        "&adultDOB" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd") +
                        "&adultSSR" + i + "=";

                    i++;
                }
                i = 0;
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PaxType.Child))
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
                foreach (var passenger in bookInfo.Passengers.Where(p => p.Type == PaxType.Infant))
                {
                    bookingParams +=
                        "&infantFirst" + i + "=" + passenger.FirstName +
                        "&infantLast" + i + "=" + passenger.LastName +
                        "&infantDOB" + i + "=" + passenger.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd") +
                        "&reffInf" + i + "=" + i;
                    i++;
                }

                tahun2dgt = tglBerangkat.Year.ToString().Substring(2, 2);
                var featuringEncode1 = ("From:0:" + jumlahSegment + "").Base64Encode();
                var featuringEncode2 = featuringEncode1.Base64Encode();

                bookingParams +=

                    "&contactFName=" + bookInfo.Contact.Name +
                    "&contactLName=" + bookInfo.Contact.Name +
                    "&contactOriginPh=" + bookInfo.Contact.Phone +
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
                    ("prosesBookingDirect." + DateTime.Now.Day.ToString("d2") + DateTime.Now.Month.ToString("d2") + tahun2dgt + (((DateTime.Now.Hour + 11) % 12) + 1) + DateTime.Now.Minute + ":prosesBookingDirect").Base64Encode();
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
                    var timeLimitDate = DateTime.Parse(timelimitParse[0] + "/" + timelimitParse[1] + "/" + timelimitParse[2] + " " + timelimitParse[3]);
                    var timeLimitGMT = timelimitParse[4].Substring(4, 2);
                    var timeLimitFinal = DateTime.Parse(timeLimitDate + " " + timeLimitGMT);
                    
                    status.BookingId = kodeBook;
                    status.BookingStatus = BookingStatus.Booked;
                    status.TimeLimit = timeLimitFinal.ToUniversalTime();
                
                    hasil.Status = status;
                    hasil.IsSuccess = true;
                    return hasil;
                
                }
                else
                {
                    return new BookFlightResult
                    {
                        IsSuccess = false,
                        Status = new BookingStatusInfo
                        {
                            BookingStatus = BookingStatus.Failed
                        },
                        Errors = new List<FlightError> { FlightError.FareIdNoLongerValid }
                    };
                
                }
            }
        }
    }
}
