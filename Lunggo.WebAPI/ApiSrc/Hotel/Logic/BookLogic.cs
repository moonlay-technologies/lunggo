using System;
using System.Net;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Log;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Hotel.Model;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Logic
{
    public static partial class HotelLogic
    {
        public static ApiResponseBase BookLogic(HotelBookApiRequest request)
        {
            if (!IsValid(request))
                return new HotelBookApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHBOO01"
                };
            var bookServiceRequest = PreprocessServiceRequest(request);
            var bookServiceResponse = HotelService.GetInstance().BookHotel(bookServiceRequest);
            var apiResponse = AssembleApiResponse(bookServiceResponse);
                 
            if (apiResponse.TimeLimit <= DateTime.UtcNow)
            {
                return new HotelBookApiResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    ErrorCode = "ERHBOO02"
                };
            }

            if (apiResponse.IsValid == false)
            {
                return new HotelBookApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHBOO03"
                };
            }
            if (apiResponse.StatusCode == HttpStatusCode.OK) return apiResponse;
            var log = LogService.GetInstance();
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            //log.Post(
            //    "```Booking API Log```"
            //    + "\n`*Environment :* " + env.ToUpper()
            //    + "\n*REQUEST :*\n"
            //    + request.Serialize()
            //    + "\n*RESPONSE :*\n"
            //    + apiResponse.Serialize()
            //    + "\n*LOGIC RESPONSE :*\n"
            //    + selectRateServiceResponse.Serialize()
            //    + "\n*Platform :* "
            //    + Client.GetPlatformType(HttpContext.Current.User.Identity.GetClientId())
            //    + "\n*Itinerary :* \n"
            //    + HotelService.GetInstance().GetItineraryForDisplay(request.Token).Serialize());
            return apiResponse;
        }

        private static bool IsValid(HotelBookApiRequest request)
        {
            return
                request != null &&
                request.Contact != null &&
                request.Passengers != null &&
                request.Token != null;
        }

        private static BookHotelInput PreprocessServiceRequest(HotelBookApiRequest request)
        {
            var pax = HotelService.GetInstance().ConvertToPax(request.Passengers);
            var selectServiceRequest = new BookHotelInput
            {
                Passengers = pax,
                Contact = request.Contact,
                Token = request.Token,
                SpecialRequest = request.SpecialRequest,
                BookerMessageTitle = request.BookerMessageTitle,
                BookerMessageDescription = request.BookerMessageDescription
            };
            return selectServiceRequest;
        }

        private static HotelBookApiResponse AssembleApiResponse(BookHotelOutput bookHotelServiceResponse)
        {
            if (bookHotelServiceResponse == null)
            {
                return new HotelBookApiResponse();
            }
            if (!bookHotelServiceResponse.IsValid)
                return new HotelBookApiResponse
                {
                    IsValid = bookHotelServiceResponse.IsValid,
                    StatusCode = HttpStatusCode.OK,
                };
            if (bookHotelServiceResponse.IsPriceChanged)
            {
                return new HotelBookApiResponse
                {
                    IsPriceChanged = bookHotelServiceResponse.IsPriceChanged,
                    NewPrice = bookHotelServiceResponse.NewPrice,
                    IsValid = bookHotelServiceResponse.IsValid,
                    StatusCode = HttpStatusCode.OK,
                };
            }
            return new HotelBookApiResponse
            {
                IsValid = bookHotelServiceResponse.IsValid,
                RsvNo = bookHotelServiceResponse.RsvNo,
                TimeLimit = bookHotelServiceResponse.TimeLimit,
                StatusCode = HttpStatusCode.OK,
            };
        }

    }
}
