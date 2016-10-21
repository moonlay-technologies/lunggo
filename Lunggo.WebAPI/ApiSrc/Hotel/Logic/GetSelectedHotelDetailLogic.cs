using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Hotel.Model;
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
        public static ApiResponseBase GetSelectedHotelDetailLogic(HotelSelectedRoomApiRequest request)
        {
            if (IsValid(request))
            {
                //var searchServiceRequest = PreprocessServiceRequest(request);
                var searchServiceResponse = HotelService.GetInstance().GetSelectedHotelDetailsFromCache(request.Token);
                var apiResponse = AssembleApiResponse(searchServiceResponse);
                if (apiResponse.StatusCode == HttpStatusCode.OK) return apiResponse;
                return apiResponse;
            }
            return new HotelDetailApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERHGSH01"
            };
        }

        private static bool IsValid(HotelSelectedRoomApiRequest request)
        {
            if (request == null)
                return false;
            return
                request.Token != null;

        }

       
        private static HotelSelectedRoomApiResponse AssembleApiResponse(HotelDetailsBase response)
        {
            var apiResponse = new HotelSelectedRoomApiResponse
            {
                HotelDetails = response
            };
            return apiResponse;
        }
    }
}