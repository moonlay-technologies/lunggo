using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using Lunggo.ApCommon.Flight.Model;

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
                    DestinationAirport = "SIN",
                    DepartureDate = new DateTime(2015,10,20)
                }
            };
            conditions.AdultCount = 1;
            conditions.ChildCount = 1;
            conditions.InfantCount = 1;
            return Client.SearchFlight(conditions);
        }

        private partial class AirAsiaClientHandler
        {
            internal SearchFlightResult SearchFlight(SearchFlightConditions conditions)
            {
                // WAIT

                var url = @"http://booking.airasia.com/Flight/InternalSelect" +
                          @"?o1=" + conditions.Trips[0].OriginAirport +
                          @"&d1=" + conditions.Trips[0].DestinationAirport +
                          @"&dd1=" + conditions.Trips[0].DepartureDate.ToString("yyyy-MM-dd") +
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
                CQ dom = html;
                var sel = dom["#trip_0_date_0_flight_0_fare_0"];
                return null;
            }
        }
    }
}
