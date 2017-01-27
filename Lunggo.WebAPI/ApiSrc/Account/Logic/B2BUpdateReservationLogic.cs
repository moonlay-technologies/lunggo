using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Http;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;

namespace Lunggo.WebAPI.ApiSrc.Account.Logic
{
    public static partial class AccountLogic
    {
        public static ApiResponseBase UpdateReservationLogic(B2BUpdateReservationRequest request)
        {
            if (!IsValid(request))
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERRUP01"
                };
            }
            if (request.RsvNo.StartsWith("1"))
            {
                //FLIGHT
            }
            else
            {
                //HOTEL
                var result = HotelService.GetInstance().UpdateReservation(request.RsvNo, request.Status);
                if (result)
                {
                    return new B2BUpdateReservationResponse
                    {
                        StatusCode = HttpStatusCode.OK
                    };
                }
                return new B2BUpdateReservationResponse
                {
                    StatusCode = HttpStatusCode.Accepted
                };
            }
            return   new B2BUpdateReservationResponse();
        }
        private static bool IsValid(B2BUpdateReservationRequest request)
        {
            return request.RsvNo != null && request.Status != null;
        }
    }
}