using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Hotel.Model;
using DailyPrice = Lunggo.WebAPI.ApiSrc.Flight.Model.DailyPrice;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Logic
{
    public static partial class HotelLogic
    {
        public static ApiResponseBase FindLowestPrices(HotelPriceCalendarApiRequest request)
        {
            if (!IsValid(request))
            {
                return new HotelPriceCalendarApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHPRC01"
                };
            }
            var priceCalendarServiceResponse = HotelService.GetInstance().
                PairsOfDateAndPrice(request.LocationCode, request.StartDate,
                    request.EndDate);

            var apiResponse = AssembleApiResponse(priceCalendarServiceResponse, request.Currency);
            return apiResponse;
        }

        private static bool IsValid(HotelPriceCalendarApiRequest request)
        {
            return (request.EndDate > request.StartDate);
        }

        private static HotelPriceCalendarApiResponse AssembleApiResponse(HotelService.LowestPrice priceCalendarServiceResponse, 
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
            
            var apiResponse = new HotelPriceCalendarApiResponse
            {
                Prices = dailyprices,
                CheapestPrice = Math.Round(priceCalendarServiceResponse.CheapestPrice / curr.RoundingOrder, 0) * curr.RoundingOrder,
                CheapestDate = priceCalendarServiceResponse.CheapestDate
            };

            return apiResponse;
        }
    }
}