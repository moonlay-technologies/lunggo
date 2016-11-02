using System;
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
                    ErrorCode = "ERHSEA01"
                };
            }

            //if (apiResponse.StatusCode == HttpStatusCode.OK) return apiResponse;
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
                    (request.From != null && request.To != null);
    
            }
            else
            {
                return
                    request.Occupancies != null;
            }
        }

        private static SearchHotelInput PreprocessServiceRequest(HotelSearchApiRequest request)
        {
            var searchServiceRequest = new SearchHotelInput
            {
                SearchHotelType = request.SearchType,
                SearchId = request.SearchId,
                CheckIn = request.CheckinDate,
                Checkout = request.CheckoutDate,
                //AdultCount = request.AdultCount,
                //ChildCount = request.ChildCount,
                Nights = request.NightCount,
                Occupancies = request.Occupancies,
                //Rooms = request.RoomCount,
                Location = request.Location,
                StartPage = request.From,
                EndPage = request.To,
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
                return new HotelSearchApiResponse
                {
                    SearchId = searchServiceResponse.SearchId,
                    ReturnedHotelCount = searchServiceResponse.ReturnedHotelCount,
                    TotalHotelCount = searchServiceResponse.TotalHotelCount,
                    Hotels = searchServiceResponse.HotelDetailLists,
                    ExpiryTime = searchServiceResponse.ExpiryTime.TruncateMilliseconds(),
                    From = searchServiceResponse.StartPage,
                    To = searchServiceResponse.EndPage,
                    MaxPrice = searchServiceResponse.MaxPrice,
                    MinPrice = searchServiceResponse.MinPrice,
                    HotelFilterDisplayInfo = searchServiceResponse.HotelFilterDisplayInfo,
                    IsSpecificHotel = searchServiceResponse.IsSpecificHotel,
                    HotelCode = searchServiceResponse.HotelCode,
                    Room = searchServiceResponse.HotelRoom,
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                if (searchServiceResponse.Errors.IsNullOrEmpty())
                {
                    return new HotelSearchApiResponse
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrorCode = "ERRGEN99"
                    };
                }
                else
                {
                    switch (searchServiceResponse.Errors[0])
                    {
                        case HotelError.InvalidInputData:
                            return new HotelSearchApiResponse
                            {
                                StatusCode = HttpStatusCode.BadRequest,
                                ErrorCode = "ERHSEA01"
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
}