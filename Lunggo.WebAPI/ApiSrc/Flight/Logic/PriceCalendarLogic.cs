using System;
using System.Linq;
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
            var priceCalendarServiceResponse = FlightService.GetInstance().
                PairsOfDateAndPrice(request.Origin, request.Destination, request.StartDate,
                    request.EndDate);

            var apiResponse = AssembleApiResponse(priceCalendarServiceResponse, request.Currency);
            return apiResponse;
            //}
            //else
            //{
            //    return new FlightSearchApiResponse
            //    {
            //        StatusCode = HttpStatusCode.BadRequest,
            //        ErrorCode = "ERFSEA01"
            //    };
            //}
        }

        private static FlightPriceCalendarApiResponse AssembleApiResponse(FlightService.LowestPrice priceCalendarServiceResponse, 
            string currency)
        {
            var curr = new Currency(currency);
            var x = Math.Round((9.6/0.5), 0)*0.5;
            foreach (var key in priceCalendarServiceResponse.PairsDateAndPrice.Keys.ToList())
            {
                priceCalendarServiceResponse.PairsDateAndPrice[key] /= curr.Rate;
                priceCalendarServiceResponse.PairsDateAndPrice[key] = Math.Round(
                    priceCalendarServiceResponse.PairsDateAndPrice[key]/
                    curr.RoundingOrder, 0)*curr.RoundingOrder;
            }
                
            var apiResponse = new FlightPriceCalendarApiResponse
            {
                Prices = priceCalendarServiceResponse.PairsDateAndPrice,
                CheapestPrice = Math.Round(priceCalendarServiceResponse.CheapestPrice / curr.RoundingOrder, 0) * curr.RoundingOrder,
                CheapestDate = priceCalendarServiceResponse.CheapestDate
            };
            return apiResponse;
        }

    }
}