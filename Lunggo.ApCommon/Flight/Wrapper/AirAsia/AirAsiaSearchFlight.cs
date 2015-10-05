using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CsQuery;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.Framework.Extension;
using CabinClass = Lunggo.ApCommon.Flight.Constant.CabinClass;

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
            conditions.CabinClass = CabinClass.Economy;
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
                var searchedHtml = (CQ) html;
                var availableFares = searchedHtml[".radio-markets"];
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
                var trip0 = conditions.Trips[0];
                var fareIdPrefix = trip0.OriginAirport + "." + trip0.DestinationAirport + "." + trip0.DepartureDate.ToString("dd.MM.yyyy") + "." + conditions.AdultCount + "." + conditions.ChildCount + "." + conditions.InfantCount + ".";
                Parallel.ForEach(fareIds, fareId =>
                {
                    url = "https://booking.airasia.com/Flight/PriceItinerary" +
                          "?SellKeys%5B%5D=" + HttpUtility.UrlEncode(fareId);
                    var itinHtml = (CQ) DownloadString(url);
                    
                });
                return null;
            }
        }
    }
}
