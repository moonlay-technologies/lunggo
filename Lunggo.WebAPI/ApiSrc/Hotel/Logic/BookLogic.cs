using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Environment;
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
            var env = EnvVariables.Get("general", "environment");
            return apiResponse;
        }

        private static bool IsValid(HotelBookApiRequest request)
        {
            try
            {
                return
                    request != null &&
                    !string.IsNullOrEmpty(request.Token) &&
                    !string.IsNullOrEmpty(request.LanguageCode) &&
                    request.Contact != null &&
                    request.Contact.Title != Title.Undefined &&
                    !string.IsNullOrEmpty(request.Contact.Name) &&
                    !string.IsNullOrEmpty(request.Contact.Phone) &&
                    !string.IsNullOrEmpty(request.Contact.Email) &&
                    new MailAddress(request.Contact.Email) != null &&
                    !string.IsNullOrEmpty(request.Contact.CountryCallingCode) &&
                    request.Passengers != null;
                //request.Passengers.Any() &&
                //request.Passengers.TrueForAll(p => !string.IsNullOrEmpty(p.Name)) &&
                //request.Passengers.TrueForAll(p => p.Title != Title.MethodNotSet) &&
                //request.Passengers.TrueForAll(p => p.Type != PaxType.MethodNotSet);
            }
            catch
            {
                return false;
            }
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
