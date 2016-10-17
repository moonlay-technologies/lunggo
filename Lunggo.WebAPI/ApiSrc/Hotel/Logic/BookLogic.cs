using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Identity.Auth;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Util;
using Lunggo.Framework.Config;
using Lunggo.Framework.Constant;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Hotel.Model;
using Lunggo.ApCommon.Product.Service;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Logic
{
    public static partial class HotelLogic
    {
        public static ApiResponseBase Book(HotelBookApiRequest request)
        {
            if (IsValid(request))
            {
                var bookServiceRequest = PreprocessServiceRequest(request);
                var bookServiceResponse = HotelService.GetInstance().BookHotel(bookServiceRequest);
                var apiResponse = AssembleApiResponse(bookServiceResponse);
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
            return new HotelSelectRoomApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERHBOO01"
            };
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
                SpecialRequest = request.SpecialRequest
            };
            return selectServiceRequest;
        }

        private static HotelBookApiResponse AssembleApiResponse(BookHotelOutput bookHotelServiceResponse)
        {
            if (bookHotelServiceResponse == null)
            {
                return new HotelBookApiResponse();
            }
            else
            {
                if (bookHotelServiceResponse.IsValid)
                {
                    if (bookHotelServiceResponse.IsPriceChanged)
                    {
                        return new HotelBookApiResponse
                        {
                            IsPriceChanged = bookHotelServiceResponse.IsPriceChanged,
                            NewPrice = bookHotelServiceResponse.NewPrice,
                            IsValid = bookHotelServiceResponse.IsValid
                        };
                    }
                    else
                    {
                        return new HotelBookApiResponse
                        {
                            IsValid = bookHotelServiceResponse.IsValid,
                            RsvNo = bookHotelServiceResponse.RsvNo,
                            TimeLimit = bookHotelServiceResponse.TimeLimit
                        };
                    }
                }
                else
                {
                    return new HotelBookApiResponse
                    {
                        IsValid = bookHotelServiceResponse.IsValid
                    };
                }
            }
            
        }

        internal static object g(HotelRoomDetailApiRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
