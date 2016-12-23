using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
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
        public static ApiResponseBase CancelBookingLogic(HotelCancelBookingApiRequest request)
        {
            if (!IsValid(request))
                return new HotelCancelBookingApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRCBA01"
                };
            var cancelServiceRequest = PreprocessServiceRequest(request);
            var cancelServiceResponse = HotelService.GetInstance().CancelHotelBooking(cancelServiceRequest);
            var apiResponse = AssembleApiResponse(cancelServiceResponse);
            if (apiResponse.BookingId == null)
            {
                return new HotelRateApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRCBA02"
                };
            }
            var log = LogService.GetInstance();
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            return apiResponse;
        }


        private static bool IsValid(HotelCancelBookingApiRequest request)
        {
            return
                request != null &&
                request.BookingId != null;
        }

        private static HotelCancelBookingInput PreprocessServiceRequest(HotelCancelBookingApiRequest request)
        {
            var cancelBookingServiceRequest = new HotelCancelBookingInput
            {
                 BookingId = request.BookingId
            };
            return cancelBookingServiceRequest;
        }

        private static HotelCancelBookingApiResponse AssembleApiResponse(HotelCancelBookingOutput cancelBookingServiceResponse)
        {
            if (cancelBookingServiceResponse == null)
            {
                return new HotelCancelBookingApiResponse();
            }

            return new HotelCancelBookingApiResponse
            {
                BookingId = cancelBookingServiceResponse.BookingId,

                StatusCode = HttpStatusCode.OK
            };
        }
    }

    
}