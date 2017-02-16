using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.Flight.Logic
{
    public static partial class FlightLogic
    {
        public static ApiResponseBase FindLowestPrices(FlightPriceCalendarApiRequest request)
        {
            if (!IsValid(request))
            {
                return new FlightPriceCalendarApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERFPRC01"
                };
            }
            var priceCalendarServiceResponse = FlightService.GetInstance().
                PairsOfDateAndPrice(request.Origin, request.Destination, request.StartDate,
                    request.EndDate);

            var apiResponse = AssembleApiResponse(priceCalendarServiceResponse, request.Currency);
            return apiResponse;
        }

        private static bool IsValid(FlightPriceCalendarApiRequest request)
        {
            return (request.EndDate >= request.StartDate);
        }

        private static FlightPriceCalendarApiResponse AssembleApiResponse(FlightService.LowestPrice priceCalendarServiceResponse, 
            string currency)
        {
            var curr = new Currency(currency);
            var dailyprices = new List<DailyPrice>();
            foreach (var key in priceCalendarServiceResponse.PairsDateAndPrice.Keys.ToList())
            {
                priceCalendarServiceResponse.PairsDateAndPrice[key] /= curr.Rate;
                priceCalendarServiceResponse.PairsDateAndPrice[key] = Math.Round(
                    priceCalendarServiceResponse.PairsDateAndPrice[key]/
                    curr.RoundingOrder, 0)*curr.RoundingOrder;
                dailyprices.Add(new DailyPrice { Date = key, Price = priceCalendarServiceResponse.PairsDateAndPrice[key] });
            }

            //var keys = priceCalendarServiceResponse.PairsDateAndPrice.Keys.ToList();
            //var i = 0;
            
            //var mydata = new PriceCalendar{Years = new List<Year>()};
            //while (i < keys.Count) 
            //{
            //    var currentYear = 2000 + Convert.ToInt32(keys[i].Substring(4, 2));
            //    var thisyear = new Year 
            //    {
            //        Name = currentYear,
            //        Months = new List<Month>() 
            //    };
            //    while (i < keys.Count && 2000 + Convert.ToInt32(keys[i].Substring(4, 2)) == currentYear)
            //    {
            //        var currentMonth = Convert.ToInt32(keys[i].Substring(2, 2));
            //        var thismonth = new Month { Name = keys[i].Substring(2, 2) };
            //        var month = new Dictionary<string, decimal>();
            //        while (i < keys.Count && Convert.ToInt32(keys[i].Substring(2, 2)) == currentMonth)
            //        {
            //            month.Add(keys[i].Substring(0, 2), priceCalendarServiceResponse.PairsDateAndPrice[keys[i]]);
            //            i++;
            //        }
            //        thismonth.DailyPrice = month;
            //        thisyear.Months.Add(thismonth);
            //    }
            //    mydata.Years.Add(thisyear);
            //}
         
            var apiResponse = new FlightPriceCalendarApiResponse
            {
                Prices = dailyprices,
                CheapestPrice = Math.Round(priceCalendarServiceResponse.CheapestPrice / curr.RoundingOrder, 0) * curr.RoundingOrder,
                CheapestDate = priceCalendarServiceResponse.CheapestDate
            };

            return apiResponse;
        }
    }
}