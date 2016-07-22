using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        private decimal GetLowestPrice(List<FlightItinerary> itins)
        {
            var lowestprice = itins[0].AdultPricePortion;
            return itins.Skip(1).Select(itin => itin.AdultPricePortion).Concat(new[] {lowestprice}).Min();
        }

        private string SetRoute(string origin, string destination)
        {
            return origin + destination ;
        }

        private string SetRoute(List<FlightItinerary> itins)
        {
            return itins[0].Trips[0].OriginAirport + itins[0].Trips[0].DestinationAirport;
        }

        private string SetDate(DateTime date)
        {
            return date.ToString("ddMMyy", CultureInfo.InvariantCulture);
        }

        private string SetDate(List<FlightItinerary> itins)
        {
            return itins[0].Trips[0].DepartureDate.ToString("ddMMyy", CultureInfo.InvariantCulture);
        }

        public List<string> GetLowestPricesForAMonth(string origin, string destination, string month, string year)
        {
            var startDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
            var endDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month),
                DateTime.DaysInMonth(startDate.Year,
                    startDate.Month));
            return GetLowestPricesForRangeOfDate(origin, destination, startDate, endDate);
        }
        
        public decimal GetLowestPriceInAMonth(string origin, string destination, string month, string year)
        {
            var startDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
            var endDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month),
                DateTime.DaysInMonth(startDate.Year,
                    startDate.Month));
            return GetLowestPriceInRangeOfDate(origin, destination, startDate, endDate);
        }

        public Dictionary<DateTime, string> PairsOfDateAndPrice(string origin, string destination, DateTime startDate,
            DateTime endDate)
        {
            var listofDates = new List<DateTime>();
            for (var date = startDate.Date; date <= endDate; date = date.AddDays(1))
            {
                listofDates.Add(date);
            }
            var listofPrices = GetLowestPricesForRangeOfDate(origin, destination, startDate, endDate);
            var pairs = new Dictionary<DateTime, string>();
            for (var ind = 0; ind <= listofDates.Count; ind++)
            {
                pairs.Add(listofDates.ElementAt(ind), listofPrices.ElementAt(ind));
            }
            return pairs;
        }
    }
}
