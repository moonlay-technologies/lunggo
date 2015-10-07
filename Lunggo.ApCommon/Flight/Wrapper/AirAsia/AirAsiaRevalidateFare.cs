using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CsQuery;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Flight.Wrapper.AirAsia
{
    internal partial class AirAsiaWrapper
    {
        internal override RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
        {
            conditions.FareId =
                "CGK.KUL.19.10.2015.1.0.0.y.0~Z~~Z01H00~AAB1~~73~X|AK~ 381~ ~~CGK~10/19/2015 08:35~KUL~10/19/2015 11:35~";
            return Client.RevalidateFare(conditions);
        }

        private partial class AirAsiaClientHandler
        {
            internal RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
            {
                var splittedFareId = conditions.FareId.Split('.').ToList();
                var origin = splittedFareId[0];
                var dest = splittedFareId[1];
                var date = new DateTime(int.Parse(splittedFareId[4]), int.Parse(splittedFareId[3]),
                    int.Parse(splittedFareId[2]));
                var adultCount = int.Parse(splittedFareId[5]);
                var childCount = int.Parse(splittedFareId[6]);
                var infantCount = int.Parse(splittedFareId[7]);
                var cabinClass = FlightService.ParseCabinClass(splittedFareId[8]);
                var coreFareId = splittedFareId[9];

                var url = @"http://booking.airasia.com/Flight/InternalSelect" +
                          @"?o1=" + origin +
                          @"&d1=" + dest +
                          @"&dd1=" + date.ToString("yyyy-MM-dd") +
                          @"&ADT=" + adultCount +
                          @"&CHD=" + childCount +
                          @"&inl=" + infantCount +
                          @"&s=true" +
                          @"&mon=true" +
                          @"&culture=id-ID" +
                          @"&cc=IDR";
                Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                Headers["Upgrade-Insecure-Requests"] = "1";
                Headers["User-Agent"] =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                Headers["Origin"] = "https://booking2.airasia.com";
                Headers["Referer"] = "https://booking2.airasia.com/Payment.aspx";
                var html = DownloadString(url);
                var searchedHtml = (CQ) html;
                var fareIdSuffix = coreFareId.Split('|').Last();
                CQ radio;
                switch (cabinClass)
                {
                    case CabinClass.Economy:
                        radio = searchedHtml["[value$=" + fareIdSuffix + "][id$=0"];
                        break;
                    case CabinClass.Business:
                        radio = searchedHtml["[value$=" + fareIdSuffix + "][id$=1"];
                        break;
                    default:
                        radio = null;
                        break;
                }

                if (radio.FirstElement() != null)
                {
                    var foundFareId = radio.Attr("value");
                    var fareIdPrefix = origin + "." + dest + "." + date.ToString("dd.MM.yyyy") + "." + adultCount + "." +
                                       childCount + "." + infantCount + "." + cabinClass + ".";

                    url = "https://booking.airasia.com/Flight/PriceItinerary" +
                          "?SellKeys%5B%5D=" + HttpUtility.UrlEncode(foundFareId);
                    var itinHtml = (CQ) DownloadString(url);
                    var price = itinHtml[".section-total-display-price > span:first"].Text().Trim(' ', '\n');
                    var segmentsHtml = itinHtml[".price-display-segment"].Select(segHtml => segHtml.Cq().MakeRoot());
                    var segments = new List<FlightSegment>();
                    foreach (var segmentHtml in segmentsHtml)
                    {
                        var flightNumberSet =
                            (segmentHtml[".price-display-segment-designator > span"]).Text().Split(' ');
                        var depArr = segmentHtml[".price-display-segment-text"];
                        var dep = depArr[0].InnerText;
                        var arr = depArr[1].InnerText;
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
                            Rbd = foundFareId.Split('~')[1],
                            DepartureAirport = dep,
                            DepartureTime = departure,
                            ArrivalAirport = arr,
                            ArrivalTime = arrival,
                            OperatingAirlineCode = flightNumberSet[0],
                            StopQuantity = 0
                        });
                    }
                    var itin = new FlightItinerary
                    {
                        AdultCount = adultCount,
                        ChildCount = childCount,
                        InfantCount = infantCount,
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
                        FareId = fareIdPrefix + foundFareId,
                        FlightTrips = new List<FlightTrip>
                        {
                            new FlightTrip
                            {
                                OriginAirport = origin,
                                DestinationAirport = dest,
                                DepartureDate = date,
                                Segments = segments
                            }
                        }
                    };

                    var fareRow = radio.Parent().Parent().Parent().Parent().Parent();
                    var durationRows = fareRow.Children().First().MakeRoot()[".fare-light-row"];
                    itin.FlightTrips[0].Segments = segments.Zip(durationRows, (segment, durationRow) =>
                    {
                        var durationTexts =
                            durationRow.LastElementChild.FirstElementChild.InnerHTML.Trim().Split(' ').ToList();
                        var duration = new TimeSpan();
                        duration =
                            duration.Add(TimeSpan.ParseExact(durationTexts[0], "h'h'", CultureInfo.InvariantCulture));
                        if (durationTexts.Count > 1)
                            duration =
                                duration.Add(TimeSpan.ParseExact(durationTexts[1], "m'm'", CultureInfo.InvariantCulture));
                        segment.Duration = duration;
                        return segment;
                    }).ToList();
                    return new RevalidateFareResult
                    {
                        IsSuccess = true,
                        IsValid = itin.FareId == conditions.FareId,
                        Itinerary = itin
                    };
                }
                else
                {
                    return new RevalidateFareResult
                    {
                        IsSuccess = true,
                        IsValid = false
                    };
                }
            }
        }
    }
}
