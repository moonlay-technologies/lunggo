using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Log;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Hotel.Model;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Logic
{
    public static partial class HotelLogic
    {
        public static ApiResponseBase Search(HotelSearchApiRequest request)
        {
            if (IsValid(request))
            {
                var searchServiceRequest = PreprocessServiceRequest(request);
                var searchServiceResponse = HotelService.GetInstance().Search(searchServiceRequest);
                var apiResponse = AssembleApiResponse(searchServiceResponse);
                if (apiResponse.StatusCode == HttpStatusCode.OK) return apiResponse;
                var log = LogService.GetInstance();
                var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
                return apiResponse;
            }
            return new HotelSelectRoomApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                ErrorCode = "ERHBOO01"
            };
        }

        private static bool IsValid(HotelSearchApiRequest request)
        {
            if (request == null)
                return false;
            if (request.SearchId != null)
            {
                return
                    request.Filter != null ||
                    request.Sorting != null ||
                    (request.From != null && request.To != null);
    
            }
            else
            {
                return
                request.AdultCount >= 1 &&
                request.ChildCount >= 0 &&
                request.CheckinDate >= DateTime.UtcNow.Date;   
            }
        }

        private static SearchHotelInput PreprocessServiceRequest(HotelSearchApiRequest request)
        {
            var searchServiceRequest = new SearchHotelInput
            {
                SearchId = request.SearchId, //TODO SEARCH ID
                CheckIn = request.CheckinDate,
                Checkout = request.CheckoutDate,
                AdultCount = request.AdultCount,
                ChildCount = request.ChildCount,
                Nights = request.NightCount,
                Rooms = request.RoomCount,
                Location = request.Location,
                StartPage = request.From,
                EndPage = request.To,//TODO START PAGE and END PAGE
                //FilterParam = request.Filter, //TODO FILTER
                //SortingParam = request.Sorting, //TODO SORTING PARAM
                
            };
            return searchServiceRequest;
        }

        private static HotelSearchApiResponse AssembleApiResponse(SearchHotelOutput searchServiceResponse)
        {
            var apiResponse = new HotelSearchApiResponse
            {
                SearchId = searchServiceResponse.SearchId,
                TotalHotel = searchServiceResponse.TotalHotel, //TODO
                Hotels = searchServiceResponse.HotelDetailLists,
                ExpiryTime = searchServiceResponse.ExpiryTime.TruncateMilliseconds(),
                From = searchServiceResponse.StartPage,
                To =  searchServiceResponse.EndPage,
                StatusCode = HttpStatusCode.OK
            };
            return apiResponse;
        }

    }
}