using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CsQuery;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Log;
using Lunggo.Framework.Web;
using Lunggo.Framework.Extension;
using RestSharp;


namespace Lunggo.ApCommon.Flight.Wrapper.Citilink
{
    internal partial class CitilinkWrapper
    {
        internal override SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            return Client.SearchFlight(conditions);
        }

        private partial class CitilinkClientHandler
        {
            internal SearchFlightResult SearchFlight(SearchFlightConditions conditions)
            {
                if (conditions.Trips.Count > 1)
                    return new SearchFlightResult
                    {
                        IsSuccess = true,
                        Itineraries = new List<FlightItinerary>()
                    };

                var log = LogService.GetInstance();
                var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
                // WAIT
                var client = CreateCustomerClient();
                var hasil = new SearchFlightResult();

                var url = @"Search.aspx";
                var searchRequest = new RestRequest(url, Method.GET);
                searchRequest.AddQueryParameter("DropDownListCurrency", "IDR");
                searchRequest.AddQueryParameter("DropDownListMarketDay1", conditions.Trips[0].DepartureDate.Day.ToString(CultureInfo.InvariantCulture));
                searchRequest.AddQueryParameter("DropDownListMarketDay2", null);
                searchRequest.AddQueryParameter("DropDownListMarketMonth1", conditions.Trips[0].DepartureDate.ToString("yyyy MMMM"));
                searchRequest.AddQueryParameter("DropDownListMarketMonth2", null);
                searchRequest.AddQueryParameter("DropDownListPassengerType_ADT", conditions.AdultCount.ToString(CultureInfo.InvariantCulture));
                searchRequest.AddQueryParameter("DropDownListPassengerType_CHD", conditions.ChildCount.ToString(CultureInfo.InvariantCulture));
                searchRequest.AddQueryParameter("DropDownListPassengerType_INFANT", conditions.InfantCount.ToString(CultureInfo.InvariantCulture));
                searchRequest.AddQueryParameter("OrganizationCode", "QG");
                searchRequest.AddQueryParameter("Page", "Select");
                searchRequest.AddQueryParameter("RadioButtonMarketStructure", "OneWay");
                searchRequest.AddQueryParameter("TextBoxMarketDestination1", conditions.Trips[0].DestinationAirport);
                searchRequest.AddQueryParameter("TextBoxMarketOrigin1", conditions.Trips[0].OriginAirport);
                searchRequest.AddQueryParameter("culture", "id-ID");
                var searchResponse = client.Execute(searchRequest);
                var htmlRespon = searchResponse.Content;

                if (searchResponse.ResponseUri.AbsolutePath != "/ScheduleSelect.aspx")
                {
                    log.Post("[Citilink Search] Error while requesting at Search.aspx. Unexpected RensponseUri absolute path", "#logging-dev");
                    return new SearchFlightResult
                    {
                        Errors = new List<FlightError> { FlightError.FareIdNoLongerValid },
                        ErrorMessages = new List<string> { "[Citilink] Error while requesting at Search.aspx. Unexpected RensponseUri absolute path || " + searchResponse.Content }
                    };
                }

                try
                {
                    var cobaAmbilTable = (CQ)htmlRespon;

                    var isi = cobaAmbilTable["#availabilityTable0>label"].MakeRoot();

                    int i = isi.Count();

                    var itins = new List<FlightItinerary>();
                    for (int j = 0; j < i; j++)
                    {
                        var cabinClass = conditions.CabinClass;
                        switch (cabinClass)
                        {
                            case CabinClass.Economy:
                                #region
                                var FlightTrips = new FlightTrip();
                                var tunjuk = isi[j];

                                //FareID
                                var ambilFID = tunjuk.Cq().MakeRoot()["[type='radio']"];
                                var FID = ambilFID.Select(x => x.GetAttribute("value")).FirstOrDefault();
                                if (FID == null)
                                    continue;
                                FID = FID.Trim();

                                var ParseFID1 = FID.Split('|').ToList();
                                var ParseFID2 = ParseFID1[1].Split('~').ToList();
                                var Rbd = ParseFID1[0].Substring(2, 1);

                                //Airline

                                string Acode;
                                string Fnumber;

                                //if ((ParseFID2.Count - 1) / 8 == 1)
                                //{
                                    url = "TaxAndFeeInclusiveDisplayAjax-resource.aspx";
                                    var fareRequest = new RestRequest(url, Method.GET);
                                    fareRequest.AddQueryParameter("flightKeys", FID);
                                    fareRequest.AddQueryParameter("numberOfMarkets", "1");
                                    fareRequest.AddQueryParameter("defaultSelection", FID);
                                    fareRequest.AddQueryParameter("keyDelimeter", ",");
                                    fareRequest.AddQueryParameter("ssrs", "FLEX");
                                    var fareResponse = client.Execute(fareRequest);

                                    var responAjax = fareResponse.Content;

                                    var ambilDataAjax = (CQ)responAjax;

                                    //Price 

                                    var tunjukHarga = ambilDataAjax["#taxAndFeeInclusiveTotal"];
                                    var ambilharga = tunjukHarga.Select(x => x.Cq().Text()).FirstOrDefault();
                                    var harga = decimal.Parse(ambilharga.Split('.')[1]);
                                    var breakdownHarga = ambilDataAjax[".right.stripeMe>tbody"];
                                    var pscRow = breakdownHarga[0].ChildElements.ToList()[1];
                                    var adultPsc = decimal.Parse(pscRow.ChildElements.ToList()[1].InnerText.Split('.')[1]);
                                    var childPsc = decimal.Parse("0");
                                    if (conditions.ChildCount > 0)
                                    {
                                        childPsc = decimal.Parse(pscRow.ChildElements.ToList()[2].InnerText.Split('.')[1]);
                                    }

                                    var insChrg = breakdownHarga[0].ChildElements.ToList()[2];
                                    var adultIns = decimal.Parse(insChrg.ChildElements.ToList()[1].InnerText.Split('.')[1]);
                                    var childIns = decimal.Parse("0");
                                    if (conditions.ChildCount > 0)
                                    {
                                        childIns = decimal.Parse(insChrg.ChildElements.ToList()[2].InnerText.Split('.')[1]);
                                    }

                                    var vatRow = breakdownHarga[0].ChildElements.ToList()[3];
                                    var adultVat = decimal.Parse(vatRow.ChildElements.ToList()[1].InnerText.Split('.')[1]);
                                    var childVat = decimal.Parse("0");
                                    if (conditions.ChildCount > 0)
                                    {
                                        childVat = decimal.Parse(vatRow.ChildElements.ToList()[2].InnerText.Split('.')[1]);
                                    }

                                    var hargaAdult = 0M;
                                    var hargaChild = 0M;
                                    var hargaInfant = 0M;
                                    var adultTax = adultIns + adultPsc + adultVat;
                                    var childTax = childPsc + childIns + childVat;

                                    var taxTable = ambilDataAjax[".twoblocks.resume-block>div:last-of-type>strong"];
                                    try
                                    {
                                        var iIdx = conditions.ChildCount > 0 ? 2 : 1;
                                        hargaAdult = decimal.Parse(taxTable[0].InnerText.Split('.')[1]) + adultTax;
                                        if (conditions.ChildCount > 0)
                                            hargaChild = decimal.Parse(taxTable[1].InnerText.Split('.')[1]) + childTax;
                                        if (conditions.InfantCount > 0)
                                            hargaInfant = decimal.Parse(taxTable[iIdx].InnerText.Split('.')[1]);
                                        if (conditions.InfantCount > 0)
                                        {
                                            var infantTax = harga - (hargaAdult + hargaChild + hargaInfant);
                                            hargaInfant += infantTax;
                                        }
                                        
                                    } 
                                    catch { }

                                    var segments = new List<FlightSegment>();

                                    if (ParseFID2.Count > 7)
                                    {
                                        int jumlahSegment = ((ParseFID2.Count) - 1) / 8;
                                        int Airport = 4;
                                        for (int l = 0; l < jumlahSegment; l++)
                                        {
                                            if (ParseFID2[(8 * l)].Length > 2)
                                            {
                                                Acode = ParseFID2[(8 * l)].Substring(1, 2);
                                            }
                                            else
                                            {
                                                Acode = ParseFID2[(8 * l)];
                                            }

                                            if (ParseFID2[(8 * l) + 1].Trim().Length > 3)
                                            {
                                                Fnumber = ParseFID2[(8 * l) + 1].Substring(0, 4).Trim();
                                            }
                                            else
                                            {
                                                Fnumber = ParseFID2[(8 * l) + 1].Trim();
                                            }

                                            var flight = FlightService.GetInstance();
                                            var arrtime = DateTime.Parse(ParseFID2[Airport + 3])
                                                .AddHours(-(flight.GetAirportTimeZone(ParseFID2[Airport + 2])));
                                            var deptime =
                                                DateTime.Parse(ParseFID2[Airport + 1])
                                                    .AddHours(-(flight.GetAirportTimeZone(ParseFID2[Airport])));

                                            var baggage = GetBaggage();
                                            var isBaggageIncluded = false;
                                            if (baggage != null)
                                            {
                                                isBaggageIncluded = true;
                                            }
                                            segments.Add(new FlightSegment
                                            {

                                                AirlineCode = Acode,
                                                FlightNumber = Fnumber,
                                                CabinClass = conditions.CabinClass,
                                                AirlineType = AirlineType.Lcc,
                                                Rbd = Rbd,
                                                DepartureAirport = ParseFID2[Airport],
                                                DepartureTime = DateTime.SpecifyKind(DateTime.Parse(ParseFID2[Airport + 1]), DateTimeKind.Utc),
                                                ArrivalAirport = ParseFID2[Airport + 2],
                                                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse(ParseFID2[Airport + 3]), DateTimeKind.Utc),
                                                OperatingAirlineCode = Acode,
                                                StopQuantity = 0,
                                                Duration = arrtime - deptime,
                                                IsMealIncluded = false,
                                                IsPscIncluded = true,
                                                IsBaggageIncluded = isBaggageIncluded,
                                                BaggageCapacity = baggage
                                            });
                                            Airport = Airport + 8;
                                        }
                                    }

                                    var prefix =
                                        "" + conditions.Trips[0].OriginAirport + "" +
                                        "." + conditions.Trips[0].DestinationAirport + "" +
                                        "." + conditions.Trips[0].DepartureDate.Day + "" +
                                        "." + conditions.Trips[0].DepartureDate.Month + "" +
                                        "." + conditions.Trips[0].DepartureDate.Year + "" +
                                        "." + conditions.AdultCount + "" +
                                        "." + conditions.ChildCount + "" +
                                        "." + conditions.InfantCount + "" +
                                        "." + ParseFID2[0].Trim() + "" +
                                        "." + ParseFID2[1].Trim() + "" +
                                        "." + harga + "" +
                                        ".";

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
                                        Supplier = Supplier.Citilink,
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
                                            OriginAirport = conditions.Trips[0].OriginAirport,
                                            DestinationAirport = conditions.Trips[0].DestinationAirport,
                                            DepartureDate = DateTime.SpecifyKind(conditions.Trips[0].DepartureDate,DateTimeKind.Utc)
                                        }
                                    }
                                    };
                                    itin.Price.SetSupplier(harga, new Currency("IDR", Payment.Constant.Supplier.Citilink));
                                    //if (itin.Trips[0].Segments.Count < 2)
                                    itins.Add(itin);
                                //}

                                hasil.IsSuccess = true;
                                #endregion
                                break;

                            case CabinClass.Business:
                                return new SearchFlightResult
                                {
                                    Itineraries = new List<FlightItinerary>(),
                                    IsSuccess = true
                                };
                                break;
                        }
                    }

                    hasil.Itineraries = itins.Where(it => it.Price.SupplierCurrency.Rate != 0).ToList();

                    if (hasil.Itineraries == null)
                        return new SearchFlightResult
                        {
                            Itineraries = new List<FlightItinerary>(),
                            IsSuccess = true
                        };
                    return hasil;
                }
                catch(Exception e)
                {
                    log.Post("[Citilink Search] Error While process data to get Flight List", "#logging-dev");
                    return new SearchFlightResult
                    {
                        IsSuccess = false,
                        Errors = new List<FlightError> { FlightError.TechnicalError },
                        ErrorMessages = new List<string> { "[Citilink] " + e.Message }
                    };
                }
            }
        }
    }
}
