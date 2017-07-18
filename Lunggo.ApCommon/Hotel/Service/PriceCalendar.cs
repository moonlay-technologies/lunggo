using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public decimal GetLowestPrice(List<HotelDetailForDisplay> itins)
        {
            return itins.Min(p => p.NetCheapestFare);
        }

        public string SetDate(DateTime date)
        {
            return date.ToString("ddMMyy", CultureInfo.InvariantCulture);
        }

        public List<decimal> GetLowestPricesForAMonth(string location, string month, string year)
        {
            var startDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
            var endDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month),
                DateTime.DaysInMonth(startDate.Year,
                    startDate.Month));
            return GetLowestPricesForRangeOfDate(location, startDate, endDate);
        }
        
        public LowestPrice GetLowestPriceInAMonth(string location, string month, string year)
        {
            var startDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
            var endDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month),
                DateTime.DaysInMonth(startDate.Year,
                    startDate.Month));
            return GetLowestPriceInRangeOfDate(location, startDate, endDate);
        }

        public LowestPrice PairsOfDateAndPrice(string location, DateTime startDate,
            DateTime endDate)
        {
            var listofDates = new List<String>();
            for (var date = startDate.Date; date <= endDate; date = date.AddDays(1))
            {
                listofDates.Add(date.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));
            }
           
            var listofPrices = GetLowestPricesForRangeOfDate(location, startDate, endDate);
            var pairs = new Dictionary<string, decimal>();
            decimal minPrice;
            string minDate;
            if (listofPrices.Any(f => f > 0))
            {
                minPrice = listofPrices.Where(f => f > 0).Min();
                var index = listofPrices.IndexOf(minPrice);
                minDate = listofDates.ElementAt(index);
                for (var ind = 0; ind < listofDates.Count; ind++)
                {
                    pairs.Add(listofDates.ElementAt(ind), listofPrices.ElementAt(ind));
                }
            }
            else
            {
                minPrice = -1;
                minDate = "";
            }

            return new LowestPrice
            {
                CheapestDate = minDate,
                CheapestPrice = minPrice,
                Location = location,
                PairsDateAndPrice = pairs
            };
        }

        public LowestPrice PairsOfDateAndPrice(string location, string month, string year)
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

            var listofPrices = GetLowestPricesForRangeOfDate(location, startDate, endDate);
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
                Location = location,
                PairsDateAndPrice = pairs
            };
        }
        public class LowestPrice
        {
            public string CheapestDate { get; set; }
            public decimal CheapestPrice { get; set; }
            public Dictionary<string, decimal> PairsDateAndPrice { get; set; }
            public string Location { get; set; }
            public string Currency { get; set; }
        }
    }
}
