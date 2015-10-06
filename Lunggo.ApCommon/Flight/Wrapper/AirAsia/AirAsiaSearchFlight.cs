using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.Framework.Extension;
using CabinClass = Lunggo.ApCommon.Flight.Constant.CabinClass;
using FareType = Lunggo.ApCommon.Flight.Constant.FareType;
using FlightSegment = Lunggo.ApCommon.Flight.Model.FlightSegment;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
    {
        internal override SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            conditions = new SearchFlightConditions();
            conditions.Trips = new List<FlightTrip>
            {
                new FlightTrip
                {
                    OriginAirport = "CGK",
                    DestinationAirport = "HND",
                    DepartureDate = new DateTime(2015,10,13)
                }
            };
            conditions.AdultCount = 1;
            conditions.ChildCount = 1;
            conditions.InfantCount = 1;
            conditions.CabinClass = CabinClass.Economy;
            return Client.SearchFlight(conditions);
        }

        private partial class AirAsiaClientHandler
        {
            internal SearchFlightResult SearchFlight(SearchFlightConditions conditions)
            {
                var trip0 = conditions.Trips[0];
                var url = @"http://booking.airasia.com/Flight/InternalSelect" +
                          @"?o1=" + trip0.OriginAirport +
                          @"&d1=" + trip0.DestinationAirport +
                          @"&dd1=" + trip0.DepartureDate.ToString("yyyy-MM-dd") +
                          @"&ADT=" + conditions.AdultCount +
                          @"&CHD=" + conditions.ChildCount +
                          @"&inl=" + conditions.InfantCount +
                          @"&s=true" +
                          @"&mon=true" +
                          @"&culture=id-ID" +
                          @"&cc=IDR";
                Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                Headers["Upgrade-Insecure-Requests"] = "1";
                Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                Headers["Origin"] = "https://booking2.airasia.com";
                Headers["Referer"] = "https://booking2.airasia.com/Payment.aspx";
                var html = DownloadString(url);
                var searchedHtml = (CQ) html;
                var availableFares = searchedHtml[".radio-markets"].MakeRoot();
                IEnumerable<string> fareIds;
                switch (conditions.CabinClass)
                {
                    case CabinClass.Economy:
                        fareIds = availableFares.Where(dom => dom.Id.Last() == '0').Select(dom => dom.Value);
                        break;
                    case CabinClass.Business:
                        fareIds = availableFares.Where(dom => dom.Id.Last() == '1').Select(dom => dom.Value);
                        break;
                    default:
                        fareIds = new List<string>();
                        break;
                }
                var itins = new List<FlightItinerary>();
                var fareIdPrefix = trip0.OriginAirport + "." + trip0.DestinationAirport + "." + trip0.DepartureDate.ToString("dd.MM.yyyy") + "." + conditions.AdultCount + "." + conditions.ChildCount + "." + conditions.InfantCount + ".";
                foreach (var fareId in fareIds)
                {
                    url = "https://booking.airasia.com/Flight/PriceItinerary" +
                          "?SellKeys%5B%5D=" + HttpUtility.UrlEncode(fareId);
                    var itinHtml = (CQ) DownloadString(url);
                    var price = itinHtml[".section-total-display-price > span:first"].Text().Trim(' ', '\n');
                    var segmentsHtml = itinHtml[".price-display-segment"].Select(segHtml => segHtml.Cq().MakeRoot());
                    var segments = new List<FlightSegment>();
                    foreach (var segmentHtml in segmentsHtml)
                    {
                        var flightNumberSet =
                            (segmentHtml[".price-display-segment-designator > span"]).Text().Split(' ');
                        var oriDest = segmentHtml[".price-display-segment-text"];
                        var ori = oriDest[0].InnerText;
                        var dest = oriDest[1].InnerText;
                        var timingSet = segmentHtml[".price-display-segment-dates > span"];
                        var departure = DateTime.ParseExact(timingSet[0].InnerText, "HHmm', 'dd' 'MMMM' 'yyyy",
                            CultureInfo.CreateSpecificCulture("id-ID"));
                        var arrival = DateTime.ParseExact(timingSet[1].InnerText, "HHmm', 'dd' 'MMMM' 'yyyy",
                            CultureInfo.CreateSpecificCulture("id-ID"));
                        segments.Add(new FlightSegment
                        {
                            AirlineCode = flightNumberSet[0],
                            FlightNumber = flightNumberSet[1],
                            CabinClass = CabinClass.Economy,
                            DepartureAirport = ori,
                            DepartureTime = departure,
                            ArrivalAirport = dest,
                            ArrivalTime = arrival,
                            OperatingAirlineCode = flightNumberSet[0],
                            StopQuantity = 0
                        });
                    }
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
                        Supplier = FlightSupplier.AirAsia,
                        SupplierCurrency = "IDR",
                        SupplierRate = 1,
                        SupplierPrice = decimal.Parse(price, CultureInfo.CreateSpecificCulture("id-ID")),
                        FareId = fareIdPrefix + fareId,
                        FlightTrips = new List<FlightTrip> {trip0}
                    };
                    itin.FlightTrips[0].Segments = segments;
                    itins.Add(itin);
                }
                foreach (var itin in itins)
                {
                    var oriFareId = itin.FareId.Split('.').Last();
                    var radio = availableFares.Single(fare => fare.Value == oriFareId).Cq();
                    var fareRow = radio.Parent().Parent().Parent().Parent().Parent();
                    var durationRows = fareRow.Children().First().MakeRoot()[".fare-light-row"];
                    var segments = itin.FlightTrips[0].Segments;
                    itin.FlightTrips[0].Segments = segments.Zip(durationRows, (segment, durationRow) =>
                    {
                        var durationTexts = durationRow.LastElementChild.FirstElementChild.InnerHTML.Trim().Split(' ').ToList();
                        var duration = new TimeSpan();
                        duration = duration.Add(TimeSpan.ParseExact(durationTexts[0], "h'h'", CultureInfo.InvariantCulture));
                        if (durationTexts.Count > 1)
                            duration = duration.Add(TimeSpan.ParseExact(durationTexts[1], "m'm'", CultureInfo.InvariantCulture));
                        segment.Duration = duration;
                        return segment;
                    }).ToList();
                }
                return new SearchFlightResult
                {
                    IsSuccess = true,
                    FlightItineraries = itins
                };
            }
        }
    }
}
