using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Logic;
using Lunggo.ApCommon.Hotel.Logic.Search;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
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

        private static HotelsSearchServiceRequest PreProcessSearchRequest(HotelSearchApiRequest apiRequest)
        {
            var searchServiceRequest = ParameterPreProcessor.InitializeHotelsSearchServiceRequest(apiRequest);
            ParameterPreProcessor.PreProcessLangParam(searchServiceRequest, apiRequest);
            ParameterPreProcessor.PreProcessStayLengthParam(searchServiceRequest, apiRequest);
            ParameterPreProcessor.PreProcessStayDateParam(searchServiceRequest, apiRequest);
            ParameterPreProcessor.PreProcessPagingParam(searchServiceRequest,apiRequest);
            ParameterPreProcessor.PreProcessSortParam(searchServiceRequest, apiRequest);
            ParameterPreProcessor.PreProcessFilterParam(searchServiceRequest,apiRequest);
            ParameterPreProcessor.PreProcessRoomCountParam(searchServiceRequest,apiRequest);
            return searchServiceRequest;
        }
    }
}
