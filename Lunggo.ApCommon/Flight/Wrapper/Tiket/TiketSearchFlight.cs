using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using RestSharp;
using Supplier = Lunggo.ApCommon.Flight.Constant.Supplier;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket
{
    internal partial class TiketWrapper
    {
        internal override SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            return Client.SearchFlight(conditions);
        }

        private partial class TiketClientHandler
        {
            internal SearchFlightResult SearchFlight(SearchFlightConditions conditions)
            {
                GetToken();
                var temp = conditions;
                var result = new SearchResponse();
                result = DoSearchFlight(conditions);
                var hasil = new SearchFlightResult();
                //if (CheckUpdate(conditions))
                //{
                //    result = DoSearchFlight(conditions);
                //}
                if(result == null)
                    return new SearchFlightResult();
                if (result.Departures == null || result.Departures.Result == null)
                    return new SearchFlightResult();

                var itinList = new List<FlightItinerary>();
                decimal harga = 0;
                foreach (var flight in result.Departures.Result)
                {
                    var itin = new FlightItinerary
                    {
                        FareId = flight.FlightId,
                        CanHold = true,
                        AdultCount = conditions.AdultCount,
                        ChildCount = conditions.ChildCount,
                        InfantCount = conditions.InfantCount,
                        TripType = TripType.OneWay,
                        Supplier = Supplier.Tiket,
                        FareType = FareType.Published,
                        RequireBirthDate = false,
                        RequirePassport = false,
                        RequireSameCheckIn = false,
                        RequireNationality = false,
                        RequestedCabinClass = CabinClass.Economy,
                        Price = new Price(),
                        AdultPricePortion = flight.PriceAdult/flight.PriceValue,
                        ChildPricePortion = flight.PriceChild / flight.PriceValue,
                        InfantPricePortion = flight.PriceInfant / flight.PriceValue,
                        Trips = new List<FlightTrip>
                        {
                            new FlightTrip
                            {
                                Segments = new List<FlightSegment>(),
                                OriginAirport = conditions.Trips[0].OriginAirport,
                                DestinationAirport = conditions.Trips[0].DestinationAirport,
                                DepartureDate = DateTime.SpecifyKind(conditions.Trips[0].DepartureDate,DateTimeKind.Utc)
                            }
                        }
                    };
                    harga = flight.PriceValue;
                    itin.Price.SetSupplier(harga, new Currency("IDR", Payment.Constant.Supplier.Citilink));

                    if (flight.FlightInfos != null && flight.FlightInfos.FlightInfo != null &&
                        flight.FlightInfos.FlightInfo.Count != 0)
                    {
                        foreach (var item in flight.FlightInfos.FlightInfo)
                        {
                            var durationHour = string.IsNullOrEmpty(item.DurationHour)
                                ? "0"
                                : item.DurationHour.Replace("j", string.Empty);

                            var durationMinute = string.IsNullOrEmpty(item.DurationMinute)
                                ? "0"
                                : item.DurationMinute.Replace("m", string.Empty);

                           var segment = new FlightSegment
                            {
                                AirlineCode = item.Flight_Number.Split('-')[0],
                                FlightNumber = item.Flight_Number.Split('-')[1],
                                CabinClass = conditions.CabinClass,
                                AirlineType = AirlineType.Lcc,
                                Rbd = (!string.IsNullOrEmpty(item.Class) && item.Class.Length > 1) ? null : item.Class,
                                DepartureAirport = item.DepartureCity,
                                DepartureTime = DateTime.SpecifyKind(DateTime.Parse(item.DepartureDate), DateTimeKind.Utc),
                                ArrivalAirport = item.ArrivalCity,
                                ArrivalTime = DateTime.SpecifyKind(DateTime.Parse(item.ArrivalDate), DateTimeKind.Utc),
                                OperatingAirlineCode = item.Flight_Number.Split('-')[0],
                                StopQuantity = 0,
                                Duration = TimeSpan.Parse(durationHour+":"+durationMinute),
                                IsMealIncluded = false,
                                IsPscIncluded = true,
                                IsBaggageIncluded = item.CheckInBaggage != 0,
                                BaggageCapacity = item.CheckInBaggage.ToString()
                            };
                            itin.Trips[0].Segments.Add(segment);
                        }
                    }
                    itinList.Add(itin);
                }
                hasil.Itineraries = itinList;
                hasil.IsSuccess = true;
                return hasil;
            }

            private static SearchResponse DoSearchFlight(SearchFlightConditions conditions)
            {
                var client = CreateTiketClient();

                var url = "/search/flight?d=" + conditions.Trips[0].OriginAirport + "&a=" + conditions.Trips[0].DestinationAirport + "&date=" + conditions.Trips[0].DepartureDate.ToString("yyyy-MM-dd")+ "&adult=" + conditions.AdultCount + "&child=" + conditions.ChildCount + "&infant=" + conditions.InfantCount + "&token=" + token + "&v=3&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var responseSearch = JsonExtension.Deserialize<SearchResponse>(response.Content);
                return responseSearch;
            }

            private static bool CheckUpdate(SearchFlightConditions conditions)
            {
                var client = CreateTiketClient();
                var url = "/ajax/mCheckFlightUpdated?token=" + token + "&d=" + conditions.Trips[0].OriginAirport + "&a=" + conditions.Trips[0].DestinationAirport + "&date=" + conditions.Trips[0].DepartureDate.ToString("yyyy-MM-dd") + "&adult=" + conditions.AdultCount + "&child=" + conditions.ChildCount + "&infant=" + conditions.InfantCount + "&time=134078435&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var responseSearch = JsonExtension.Deserialize<UpdateSearchResponse>(response.Content);
                if (responseSearch == null || responseSearch.Update <= 0)
                    return false;
                return true;
            }
        }

    }
}
