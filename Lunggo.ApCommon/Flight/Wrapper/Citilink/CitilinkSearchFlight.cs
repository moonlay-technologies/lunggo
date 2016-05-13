using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CsQuery;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
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
                    return new SearchFlightResult { Errors = new List<FlightError> { FlightError.FareIdNoLongerValid } };

                try
                {
                    var cobaAmbilTable = (CQ) htmlRespon;

                    var isi = cobaAmbilTable[".w99>tbody>tr:not([class^='trSSR'])"];
                    
                    int i = isi.Count();

                    var itins = new List<FlightItinerary>();
                    for (int j = 2; j <= i; j++)
                    {
                        var cabinClass = conditions.CabinClass;
                        switch (cabinClass)
                        {
                            case CabinClass.Economy:
                                #region
                                var FlightTrips = new FlightTrip();
                                var tunjuk = isi.MakeRoot()["tr:nth-child(" + j + ")"];

                                //FareID
                                string FID;
                                var ambilFID = tunjuk.MakeRoot()[".fareCol2>p:nth-child(1)>input"];
                                try
                                {
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

                                var ParseFID1 = FID.Split('|').ToList();
                                var ParseFID2 = ParseFID1[1].Split('~').ToList();
                                var Rbd = ParseFID1[0].Substring(2, 1);

                                //Airline

                                string Acode;
                                string Fnumber;

                                
                                url = "TaxAndFeeInclusiveDisplayAjax-resource.aspx";
                                var fareRequest = new RestRequest(url, Method.GET);
                                fareRequest.AddQueryParameter("flightKeys", FID);
                                fareRequest.AddQueryParameter("numberOfMarkets", "1");
                                fareRequest.AddQueryParameter("keyDelimeter", ",");
                                fareRequest.AddQueryParameter("ssrs", "FLEX");
                                var fareResponse = client.Execute(fareRequest);

                                var responAjax = fareResponse.Content;

                                var ambilDataAjax = (CQ) responAjax;

                                //Price 

                                var tunjukHarga = ambilDataAjax["#taxAndFeeInclusiveTotal"];
                                var ambilharga = tunjukHarga.Select(x => x.Cq().Text()).FirstOrDefault();
                                var harga = ambilharga.Split('.');

                                var segments = new List<FlightSegment>();

                                if (ParseFID2.Count > 7)
                                {
                                    int jumlahSegment = ((ParseFID2.Count) - 1)/8;
                                    int Airport = 4;
                                    for (int l = 0; l < jumlahSegment; l++)
                                    {
                                        if (ParseFID2[(8*l)].Length > 2)
                                            {
                                                Acode = ParseFID2[(8*l)].Substring(1, 2);
                                            }
                                        else
                                            {
                                                Acode = ParseFID2[(8*l)];
                                            }

                                        if (ParseFID2[(8*l) + 1].Trim().Length > 3)
                                            {
                                                Fnumber = ParseFID2[(8*l) + 1].Substring(0, 4).Trim();
                                            }
                                        else
                                            {
                                                Fnumber = ParseFID2[(8*l) + 1].Trim();
                                            }

                                        var dict = DictionaryService.GetInstance();
                                        var arrtime = DateTime.Parse(ParseFID2[Airport + 3])
                                            .AddHours(-(dict.GetAirportTimeZone(ParseFID2[Airport + 2])));
                                        var deptime =
                                            DateTime.Parse(ParseFID2[Airport + 1])
                                                .AddHours(-(dict.GetAirportTimeZone(ParseFID2[Airport])));

                                        segments.Add(new FlightSegment
                                        {

                                            AirlineCode = Acode,
                                            FlightNumber = Fnumber,
                                            CabinClass = conditions.CabinClass,
                                            Rbd = Rbd,
                                            DepartureAirport = ParseFID2[Airport],
                                            DepartureTime = DateTime.SpecifyKind(DateTime.Parse(ParseFID2[Airport + 1]),DateTimeKind.Utc),
                                            ArrivalAirport = ParseFID2[Airport + 2],
                                            ArrivalTime = DateTime.SpecifyKind(DateTime.Parse(ParseFID2[Airport + 3]), DateTimeKind.Utc),
                                            OperatingAirlineCode = Acode,
                                            StopQuantity = 0,
                                            Duration = arrtime - deptime
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
                                    "." + decimal.Parse(harga[1]) + "" +
                                    ".";

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
                                    Supplier = Supplier.Citilink,
                                    SupplierCurrency = "IDR",
                                    SupplierRate = 1,
                                    SupplierPrice = decimal.Parse(harga[1]),
                                    FareId = prefix + ParseFID1[1],
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
                                //if (itin.Trips[0].Segments.Count < 2)
                                itins.Add(itin);
                                hasil.IsSuccess = true;
                                hasil.Itineraries = itins;
                                #endregion
                                break;

                            case CabinClass.Business :
                                return new SearchFlightResult
                                {
                                    Itineraries = new List<FlightItinerary>(),
                                    IsSuccess = true
                                };
                                break;
                        }
                    }
                    if (hasil.Itineraries == null)
                        return new SearchFlightResult
                        {
                            Itineraries = new List<FlightItinerary>(),
                            IsSuccess = true
                        };
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
