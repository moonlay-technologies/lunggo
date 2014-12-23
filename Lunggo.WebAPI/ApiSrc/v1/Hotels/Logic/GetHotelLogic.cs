using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Logic.Search;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.WebAPI.ApiSrc.v1.Hotels.Model;
using Lunggo.WebAPI.ApiSrc.v1.Hotels.Object;

namespace Lunggo.WebAPI.ApiSrc.v1.Hotels.Logic
{
    public class GetHotelLogic
    {
        public static HotelSearchApiResponse GetHotels(HotelSearchApiRequest request)
        {
            var searchServiceRequest = PreProcessSearchRequest(request);
            var searchServiceResponse = HotelsSearchService.SearchHotel(searchServiceRequest);
            var apiResponse = AssembleApiResponse(searchServiceResponse, request);
            return apiResponse;
        }

        private static IEnumerable<HotelExcerpt> ConvertToHotelExcerptList(IEnumerable<HotelDetail> sourceList)
        {
            return sourceList.Select(
                hotel => new HotelExcerpt
                {
                    Address = hotel.Address,
                    Area = hotel.Area,
                    Country = hotel.Country,
                    HotelId = hotel.HotelId,
                    HotelName = hotel.HotelName,
                    ImageUrlList = hotel.ImageUrlList,
                    Latitude = hotel.Latitude,
                    Longitude = hotel.Longitude,
                    LowestPrice = hotel.LowestPrice,
                    Province = hotel.Province,
                    StarRating = hotel.StarRating,
                });
        }

        private static HotelSearchApiResponse AssembleApiResponse(HotelsSearchServiceResponse response, HotelSearchApiRequest apiRequest)
        {
            var hotelList = ConvertToHotelExcerptList(response.HotelList);
            var apiResponse = new HotelSearchApiResponse
            {
                HotelList = hotelList,
                SearchId = response.SearchId,
                InitialRequest = apiRequest,
                TotalHotelCount = response.TotalHotelCount,
                TotalFilteredCount = response.TotalFilteredHotelCount
            };
            return apiResponse;
        }

        private static HotelsSearchServiceRequest InitializeHotelsServiceRequest(HotelSearchApiRequest request)
        {
            var searchServiceRequest = new HotelsSearchServiceRequest
            {
                Lang = request.Lang,
                LocationId = request.LocationId,
                SearchId = request.SearchId,
                SearchFilter = new HotelsSearchFilter(),
                StayLength = Int32.Parse(request.StayLength),
                StayDate = DateTime.ParseExact(request.StayDate, "yyyy-MM-dd",System.Globalization.CultureInfo.InvariantCulture),
            };
            return searchServiceRequest;
        }

        private static HotelsSearchServiceRequest PreProcessSearchRequest(HotelSearchApiRequest apiRequest)
        {
            var searchServiceRequest = InitializeHotelsServiceRequest(apiRequest);
            PreProcessPagingParam(searchServiceRequest,apiRequest);
            PreProcessSortParam(searchServiceRequest,apiRequest);
            PreProcessFilterParam(searchServiceRequest,apiRequest);
            PreProcessRoomCountParam(searchServiceRequest,apiRequest);
            return searchServiceRequest;
        }

        private static void PreProcessPagingParam(HotelsSearchServiceRequest searchServiceRequest, HotelSearchApiRequest apiRequest)
        {
            //TODO Do not hardcode use configuration file
            searchServiceRequest.StartIndex = 0;
            searchServiceRequest.ResultCount = 10;

            if (!String.IsNullOrEmpty(apiRequest.StartIndex))
            {
                searchServiceRequest.StartIndex = Convert.ToInt32(apiRequest.StartIndex);
            }

            if (!String.IsNullOrEmpty(apiRequest.ResultCount))
            {
                searchServiceRequest.ResultCount = Convert.ToInt32(apiRequest.ResultCount);
            }
        }

        private static void PreProcessSortParam(HotelsSearchServiceRequest searchServiceRequest,HotelSearchApiRequest request)
        {
            searchServiceRequest.SortBy = HotelsSearchSortType.Default;
            if (!String.IsNullOrEmpty(request.SortBy))
            {
                searchServiceRequest.SortBy =
                    (HotelsSearchSortType) Enum.Parse(typeof (HotelsSearchSortType), request.SortBy);
            }
        }

        private static void PreProcessFilterParam(HotelsSearchServiceRequest searchServiceRequest, HotelSearchApiRequest apiRequest)
        {
            PreProcessPriceFilterParam(searchServiceRequest, apiRequest);
            PreProcessStarRatingFilterParam(searchServiceRequest, apiRequest);
        }

        private static void PreProcessPriceFilterParam(HotelsSearchServiceRequest searchServiceRequest, HotelSearchApiRequest apiRequest)
        {
            searchServiceRequest.SearchFilter.MinPrice = -99999999L;
            searchServiceRequest.SearchFilter.MaxPrice = Int64.MaxValue;
            
            if (!String.IsNullOrEmpty(apiRequest.MinPrice))
            {
                searchServiceRequest.SearchFilter.MinPrice = Convert.ToInt64(apiRequest.MinPrice);
            }

            if (!String.IsNullOrEmpty(apiRequest.MaxPrice))
            {
                searchServiceRequest.SearchFilter.MaxPrice = Convert.ToInt64(apiRequest.MaxPrice);
            }
        }

        private static void PreProcessStarRatingFilterParam(HotelsSearchServiceRequest searchServiceRequest, HotelSearchApiRequest apiRequest)
        {
            if (!String.IsNullOrEmpty(apiRequest.StarRating))
            {
                searchServiceRequest.SearchFilter.StarRatingsToDisplay = apiRequest.StarRating.Split(',').ToList().Select(Int32.Parse);
            }
        }

        private static void PreProcessRoomCountParam(HotelsSearchServiceRequest searchServiceRequest, HotelSearchApiRequest apiRequest)
        {
            var roomOccupantList = new List<RoomOccupant>();
            for (var i = 0; i < apiRequest.RoomCount; i++)
            {
                roomOccupantList.Add(new RoomOccupant
                {
                    AdultCount = 2, //TODO Do not hardcode use configuration file
                    ChildrenAges = null
                });
            }
            searchServiceRequest.RoomOccupants = roomOccupantList;
        }

        

    }
}
