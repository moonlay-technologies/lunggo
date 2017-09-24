using System;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Http;
using Lunggo.Framework.Log;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Activity.Model;
using ServiceStack.Text;

namespace Lunggo.WebAPI.ApiSrc.Activity.Logic
{
    public static partial class ActivityLogic
    {
        public static ApiResponseBase Search(ActivitySearchApiRequest request)
        {
            if (!IsValid(request))
                return new ActivitySearchApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHSEA01"
                };
            var searchServiceRequest = PreprocessServiceRequest(request);
            var searchServiceResponse = ActivityService.GetInstance().Search(searchServiceRequest);
            var apiResponse = AssembleApiResponse(searchServiceResponse);
            if (apiResponse.ExpiryTime <= DateTime.UtcNow)
            {
                return new ActivitySearchApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERHSEA04"
                };
            }

            var log = LogService.GetInstance();
            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            return apiResponse;
        }

        private static bool IsValid(ActivitySearchApiRequest request)
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
                   request.HotelCode > 0;
            }
            else
            {
                //if (request.Occupancies == null)
                //    return false;

                return
                    //request.Occupancies.TrueForAll(data => data.AdultCount > 0) &&
                    //request.Occupancies.TrueForAll(data=>data.RoomCount>0) &&
                    request.NightCount > 0 &&
                    //request.Occupancies.TrueForAll(data=>data.ChildCount == (data.ChildrenAges==null?0:data.ChildrenAges.Count)) &&
                    request.CheckinDate >= DateTime.UtcNow.Date;
                    //request.CheckoutDate >= request.CheckinDate;
                    //request.RoomCount > 0;   

            }
        }

        private static SearchActivityInput PreprocessServiceRequest(ActivitySearchApiRequest request)
        {
            var searchServiceRequest = new SearchActivityInput
            {
                //SearchHotelType = request.SearchType,
                //SearchId = request.SearchId,
                //CheckIn = request.CheckinDate,
                //Checkout = request.CheckoutDate,
                //AdultCount = request.AdultCount,
                //ChildCount = request.ChildCount,
                //Nights = request.NightCount,
                //Occupancies = request.Occupancies,
                //Rooms = request.RoomCount,
                //Location = request.Location,
                //Page = request.Page,
                //PerPage = request.PerPage,
                //FilterParam = request.Filter,
                //SortingParam = request.Sorting,
                //HotelCode = request.HotelCode,
                //RegsId = request.RegsId

            };
            //searchServiceRequest.Occupancies.ForEach(o => o.ChildrenAges = o.ChildrenAges.Take(o.ChildCount).ToList());
            return searchServiceRequest;
        }

        private static ActivitySearchApiResponse AssembleApiResponse(SearchActivityOutput searchServiceResponse)
        {
            if (searchServiceResponse.IsSuccess)
            {
                if (searchServiceResponse.ReturnedHotelCount <= 0)
                    return new ActivitySearchApiResponse()
                    {
                        StatusCode = HttpStatusCode.OK,
                        SearchId = searchServiceResponse.SearchId,
                        DestinationName = searchServiceResponse.DestinationName,
                        Page = searchServiceResponse.Page,
                        PerPage = searchServiceResponse.PerPage,
                        FilteredHotelCount = searchServiceResponse.FilteredHotelCount,
                        TotalHotelCount = searchServiceResponse.TotalHotelCount,
                        ReturnedHotelCount = searchServiceResponse.ReturnedHotelCount,
                        MaxPrice = searchServiceResponse.MaxPrice,
                        MinPrice = searchServiceResponse.MinPrice,
                        HotelFilterDisplayInfo = searchServiceResponse.HotelFilterDisplayInfo,
                        ExpiryTime = searchServiceResponse.ExpiryTime.TruncateMilliseconds()
                    };

                return new HotelSearchApiResponse
                {
                    SearchId = searchServiceResponse.SearchId,
                    DestinationName = searchServiceResponse.DestinationName,
                    ReturnedHotelCount = searchServiceResponse.ReturnedHotelCount,
                    TotalHotelCount = searchServiceResponse.TotalHotelCount,
                    Hotels = searchServiceResponse.HotelDetailLists,
                    ExpiryTime = searchServiceResponse.ExpiryTime.TruncateMilliseconds(),
                    Page = searchServiceResponse.Page,
                    PageCount = searchServiceResponse.PageCount,
                    PerPage = searchServiceResponse.PerPage,
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