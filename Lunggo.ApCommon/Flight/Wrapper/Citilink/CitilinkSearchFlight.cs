using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Wrapper.Citilink
{
    internal partial class CitilinkWrapper
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

        private partial class CitilinkClientHandler
        {
            internal SearchFlightResult SearchFlight(SearchFlightConditions conditions)
            {
                // WAIT

                var URL = @"https://book.citilink.co.id/Search.aspx?" +
                          @"DropDownListCurrency=IDR" +
                          @"&DropDownListMarketDay1=" + conditions.Trips[0].DepartureDate.Day +
                          @"&DropDownListMarketDay2=" +
                          @"&DropDownListMarketMonth1=2015-11" + conditions.Trips[0].DepartureDate.ToString("yyyy MMMM") +
                          @"&DropDownListMarketMonth2=" +
                          @"&DropDownListPassengerType_ADT=1" + conditions.AdultCount +
                          @"&DropDownListPassengerType_CHD=0" + conditions.ChildCount +
                          @"&DropDownListPassengerType_INFANT=0" + conditions.InfantCount +
                          @"&OrganizationCode=QG" +
                          @"&Page=Select" +
                          @"&RadioButtonMarketStructure=OneWay" +
                          @"&TextBoxMarketDestination1=" + conditions.Trips[0].DestinationAirport +
                          @"&TextBoxMarketOrigin1=" + conditions.Trips[0].OriginAirport +
                          @"&culture=id-ID";

                Headers["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                Headers["Accept-Encoding"] = "gzip, deflate";
                Headers["Accept-Language"] = "en-GB,en-US;q=0.8,en;q=0.6";
                Headers["Upgrade-Insecure-Requests"] = "1";
                Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                //Headers["Origin"] = "https://booking2.airasia.com";
                Headers["Referer"] = "https://www.citilink.co.id/";

                DownloadString(URL);

                return null;
            }
        }
    }
}
