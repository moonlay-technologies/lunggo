using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Logic;
using Lunggo.ApCommon.Hotel.Logic.Search;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.ApCommon.Util;
using Lunggo.Framework.Constant;
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

        private static IEnumerable<HotelExcerpt> ConvertToHotelExcerptList(IEnumerable<HotelDetail> sourceList, HotelSearchApiRequest request)
        {
            IEnumerable<HotelExcerpt> hotelExcerpts = null;
            if (sourceList != null)
            {
                var hotelExcerptList = sourceList.Select(
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
                    IsLatLongSet = hotel.IsLatLongSet,
                    Facilities = hotel.Facilities
                });

                var convertToHotelExcerptList = hotelExcerptList as IList<HotelExcerpt> ?? hotelExcerptList.ToList();
                foreach (var hotel in convertToHotelExcerptList)
                {
                    SetImageUrlList(hotel);
                    SetFacilitiesName(hotel, request);
                }
                hotelExcerpts = convertToHotelExcerptList;
            }
            return hotelExcerpts;
        }

        private static void SetImageUrlList(HotelExcerpt hotel)
        {
            if (hotel.ImageUrlList == null) return;
            foreach (var image in hotel.ImageUrlList)
            {
                if (!String.IsNullOrEmpty(image.FullSizeUrl))
                {
                    image.FullSizeUrl = UrlUtil.CreateFullImageUrlForHotel(hotel.HotelId, image.FullSizeUrl, true);
                }

                if (!String.IsNullOrEmpty(image.ThumbSizeUrl))
                {
                    image.ThumbSizeUrl = UrlUtil.CreateFullImageUrlForHotel(hotel.HotelId, image.ThumbSizeUrl, true);
                }
            }
        }

        private static void SetFacilitiesName(HotelExcerpt hotel, HotelSearchApiRequest request)
        {
            if (hotel.Facilities == null) return;
            var activeLanguageCode = String.IsNullOrEmpty(request.Lang)
                ? SystemConstant.IndonesianLanguageCode
                : request.Lang;

            foreach (var facility in hotel.Facilities)
            {
                facility.FacilityName = HotelFacilityUtil.GetFacilityName(facility.FacilityId, activeLanguageCode);
            }
        }

        private static HotelSearchApiResponse AssembleApiResponse(HotelsSearchServiceResponse response, HotelSearchApiRequest apiRequest)
        {
            var hotelList = ConvertToHotelExcerptList(response.HotelList,apiRequest);
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
