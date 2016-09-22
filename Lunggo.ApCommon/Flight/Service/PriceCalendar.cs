using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public decimal GetLowestPrice(List<FlightItinerary> itins)
        {
            var lowestprice = itins[0].AdultPricePortion / itins[0].AdultCount *itins[0].Price.OriginalIdr;
            for (var ind = 1; ind < itins.Count; ind++)
            {
                if (itins[ind].AdultPricePortion/itins[ind].AdultCount*itins[ind].Price.OriginalIdr < lowestprice)
                {
                    lowestprice = itins[ind].AdultPricePortion/itins[ind].AdultCount*itins[ind].Price.OriginalIdr;
                }
            }
            return lowestprice;
        }

        public string SetRoute(string origin, string destination)
        {
            return origin + destination ;
        }

        public string SetRoute(List<FlightItinerary> itins)
        {
            if (itins != null)
            {
                return itins[0].Trips[0].OriginAirport + itins[0].Trips[0].DestinationAirport;
            }
            return "";
        }

        public string SetDate(DateTime date)
        {
            return date.ToString("ddMMyy", CultureInfo.InvariantCulture);
        }

        public string SetDate(List<FlightItinerary> itins)
        {
            return itins[0].Trips[0].DepartureDate.ToString("ddMMyy", CultureInfo.InvariantCulture);
        }

        public List<decimal> GetLowestPricesForAMonth(string origin, string destination, string month, string year)
        {
            var startDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
            var endDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month),
                DateTime.DaysInMonth(startDate.Year,
                    startDate.Month));
            return GetLowestPricesForRangeOfDate(origin, destination, startDate, endDate);
        }
        
        public LowestPrice GetLowestPriceInAMonth(string origin, string destination, string month, string year)
        {
            var startDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
            var endDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month),
                DateTime.DaysInMonth(startDate.Year,
                    startDate.Month));
            return GetLowestPriceInRangeOfDate(origin, destination, startDate, endDate);
        }

        public LowestPrice PairsOfDateAndPrice(string origin, string destination, DateTime startDate,
            DateTime endDate)
        {
            var listofDates = new List<String>();
            for (var date = startDate.Date; date <= endDate; date = date.AddDays(1))
            {
                listofDates.Add(date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));
            }
           
            var listofPrices = GetLowestPricesForRangeOfDate(origin, destination, startDate, endDate);
            var pairs = new Dictionary<string, decimal>();
            decimal minPrice;
            string minDate;
            if (listofPrices.Any(f => f > 0))
            {
                minPrice = listofPrices.Where(f => f > 0).Min();
                var index = listofPrices.IndexOf(minPrice);
                minDate = listofDates.ElementAt(index);
            }
            else
            {
                minPrice = -1;
                minDate = "";
            }
            
            for (var ind = 0; ind < listofDates.Count; ind++)
            {
                pairs.Add(listofDates.ElementAt(ind), listofPrices.ElementAt(ind));
            }

            return new LowestPrice
            {
                CheapestDate = minDate,
                CheapestPrice = minPrice,
                Origin = origin,
                Destination = destination,
                PairsDateAndPrice = pairs
            };
        }

        public LowestPrice PairsOfDateAndPrice(string origin, string destination, string month, string year)
        {
            var listofDates = new List<String>();
            var startDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
            var endDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month),
                DateTime.DaysInMonth(startDate.Year,
                    startDate.Month));
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                listofDates.Add(date.ToString("ddMMyy", CultureInfo.InvariantCulture));
            }

            var listofPrices = GetLowestPricesForRangeOfDate(origin, destination, startDate, endDate);
            var pairs = new Dictionary<string, decimal>();
            var minPrice = Convert.ToDecimal(listofPrices.ElementAt(0));
            var minDate = listofDates.ElementAt(0);
            for (var ind = 0; ind <= listofDates.Count; ind++)
            {
                pairs.Add(listofDates.ElementAt(ind), listofPrices.ElementAt(ind));
                if (ind == 0) continue;
                var currentPrice = Convert.ToDecimal(listofPrices.ElementAt(ind));
                if (currentPrice >= minPrice) continue;
                minPrice = currentPrice;
                minDate = listofDates.ElementAt(ind);
            }

            return new LowestPrice
            {
                CheapestDate = minDate,
                CheapestPrice = minPrice,
                Origin = origin,
                Destination = destination,
                PairsDateAndPrice = pairs
            };
        }
        public class LowestPrice
        {
            public string CheapestDate { get; set; }
            public decimal CheapestPrice { get; set; }
            public Dictionary<string, decimal> PairsDateAndPrice { get; set; }
            public string Origin { get; set; }
            public string Destination { get; set; }
            public string Currency { get; set; }
        }
    }
}
