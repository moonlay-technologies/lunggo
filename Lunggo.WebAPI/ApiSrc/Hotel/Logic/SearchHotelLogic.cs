using System;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Http;
using Lunggo.Framework.Log;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Hotel.Model;
using ServiceStack.Text;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Logic
{
    public static partial class HotelLogic
    {
        public static ApiResponseBase Search(HotelSearchApiRequest request)
        {
            if (!IsValid(request))
                return new HotelSearchApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHSEA01"
                };
            var searchServiceRequest = PreprocessServiceRequest(request);
            var searchServiceResponse = HotelService.GetInstance().Search(searchServiceRequest);
            var apiResponse = AssembleApiResponse(searchServiceResponse);
            if (apiResponse.ExpiryTime <= DateTime.UtcNow)
            {
                return new HotelSearchApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHSEA04"
                };
            }

            var log = LogService.GetInstance();
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            return apiResponse;
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
                    (request.Page > 0 && request.PerPage > 0);
            }
            else if (request.RegsId != null)
            {
                return
                    request.Occupancies != null &&
                    request.HotelCode > 0;
            }
            else
            {
                if (request.Occupancies == null)
                    return false;

                return
                    request.Occupancies.TrueForAll(data => data.AdultCount > 0) &&
                    request.Occupancies.TrueForAll(data=>data.RoomCount>0) &&
                    request.NightCount > 0 &&
                    //request.Occupancies.TrueForAll(data=>data.ChildCount == (data.ChildrenAges==null?0:data.ChildrenAges.Count)) &&
                    request.CheckinDate >= DateTime.UtcNow.Date;
                    //request.CheckoutDate >= request.CheckinDate;
                    //request.RoomCount > 0;   

            }
        }

        private static SearchHotelInput PreprocessServiceRequest(HotelSearchApiRequest request)
        {
            var searchServiceRequest = new SearchHotelInput
            {
                SearchHotelType = request.SearchType,
                SearchId = request.SearchId,
                CheckIn = request.CheckinDate,
                //Checkout = request.CheckoutDate,
                //AdultCount = request.AdultCount,
                //ChildCount = request.ChildCount,
                Nights = request.NightCount,
                Occupancies = request.Occupancies,
                //Rooms = request.RoomCount,
                Location = request.Location,
                Page = request.Page,
                PerPage = request.PerPage,
                FilterParam = request.Filter,
                SortingParam = request.Sorting,
                HotelCode = request.HotelCode,
                RegsId = request.RegsId

            };
            return searchServiceRequest;
        }

        private static HotelSearchApiResponse AssembleApiResponse(SearchHotelOutput searchServiceResponse)
        {
            if (searchServiceResponse.IsSuccess)
            {
                if (searchServiceResponse.ReturnedHotelCount <= 0)
                    return new HotelSearchApiResponse()
                    {
                        StatusCode = HttpStatusCode.OK,
                        SearchId = searchServiceResponse.SearchId,
                        Page = searchServiceResponse.Page,
                        PerPage = searchServiceResponse.PerPage,
                        FilteredHotelCount = searchServiceResponse.FilteredHotelCount,
                        TotalHotelCount = searchServiceResponse.TotalHotelCount,
                        ReturnedHotelCount = searchServiceResponse.ReturnedHotelCount,
                        MaxPrice = searchServiceResponse.MaxPrice,
                        MinPrice = searchServiceResponse.MinPrice,
                        HotelFilterDisplayInfo = searchServiceResponse.HotelFilterDisplayInfo
                    };

                return new HotelSearchApiResponse
                {
                    SearchId = searchServiceResponse.SearchId,
                    ReturnedHotelCount = searchServiceResponse.ReturnedHotelCount,
                    TotalHotelCount = searchServiceResponse.TotalHotelCount,
                    Hotels = searchServiceResponse.HotelDetailLists,
                    ExpiryTime = searchServiceResponse.ExpiryTime.TruncateMilliseconds(),
                    Page = searchServiceResponse.Page,
                    PageCount = searchServiceResponse.PageCount,
                    PerPage = searchServiceResponse.PerPage,
                    PageCount = searchServiceResponse.PageCount,
                    MaxPrice = searchServiceResponse.MaxPrice,
                    MinPrice = searchServiceResponse.MinPrice,
                    HotelFilterDisplayInfo = searchServiceResponse.HotelFilterDisplayInfo,
                    IsSpecificHotel = searchServiceResponse.IsSpecificHotel,
                    HotelCode = searchServiceResponse.HotelCode,
                    Room = searchServiceResponse.HotelRoom,
                    FilteredHotelCount = searchServiceResponse.FilteredHotelCount,
                    
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                if (searchServiceResponse.Errors == null)
                    return new HotelSearchApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERHSEA99"
                    };

                switch (searchServiceResponse.Errors[0])
                {
                    case HotelError.InvalidInputData:
                        return new HotelSearchApiResponse
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrorCode = "ERRGEN99"
                        };
                    case HotelError.SearchIdNoLongerValid:
                        return new HotelSearchApiResponse
                        {
                            StatusCode = HttpStatusCode.Accepted,
                            ErrorCode = "ERHSEA02"
                        };
                    case HotelError.RateKeyNotFound:
                        return new HotelSearchApiResponse
                        {
                            StatusCode = HttpStatusCode.Accepted,
                            ErrorCode = "ERHSEA03"
                        };
                    default:
                        return new HotelSearchApiResponse
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrorCode = "ERRGEN99"
                        };
                }


            }

        }

    }
}