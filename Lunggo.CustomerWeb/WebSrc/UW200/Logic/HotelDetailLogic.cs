using System;
using Lunggo.ApCommon.Hotel.Logic;
using Lunggo.ApCommon.Hotel.Logic.Search;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.CustomerWeb.WebSrc.UW200.Object;

namespace Lunggo.CustomerWeb.WebSrc.UW200.Logic
{
    public class HotelDetailLogic
    {
        public static Uw200HotelDetailResponse GetHotelDetail(Uw200HotelDetailRequest request)
        {
            var searchServiceRequest = PreProcessHotelDetailRequest(request);
            var hotelDetail = HotelsSearchService.GetHotelDetail(searchServiceRequest.HotelId);
            var response = AssembleResponse(hotelDetail, searchServiceRequest);
            return response;
        }

        private static Uw200HotelDetailResponse AssembleResponse(HotelDetail hotelDetail, HotelRoomsSearchServiceRequest searchServiceRequest)
        {
            var response = new Uw200HotelDetailResponse
            {
                HotelName = hotelDetail.HotelName,
                HotelId = Int32.Parse(hotelDetail.HotelId),
                HotelDescription = hotelDetail.HotelDescription,
                Country = hotelDetail.Country,
                SearchId = searchServiceRequest.SearchId,
                RoomOccupants = searchServiceRequest.RoomOccupants,
                Lang = searchServiceRequest.Lang,
                LowestPrice = hotelDetail.LowestPrice,
                StayDate = searchServiceRequest.StayDate,
                Address = hotelDetail.Address,
                Area = hotelDetail.Area,
                Latitude = hotelDetail.Latitude,
                Longitude = hotelDetail.Longitude,
                Province = hotelDetail.Province,
                StarRating = hotelDetail.StarRating,
                StayLength = searchServiceRequest.StayLength,
                ImageUrlList = hotelDetail.ImageUrlList
            };
            return response;
        }

        private static HotelRoomsSearchServiceRequest PreProcessHotelDetailRequest(Uw200HotelDetailRequest request)
        {
            var searchServiceRequest = ParameterPreProcessor.InitializeHotelRoomsSearchServiceRequest(request);
            ParameterPreProcessor.PreProcessLangParam(searchServiceRequest, request);
            ParameterPreProcessor.PreProcessStayLengthParam(searchServiceRequest, request);
            ParameterPreProcessor.PreProcessStayDateParam(searchServiceRequest, request);
            ParameterPreProcessor.PreProcessRoomCountParam(searchServiceRequest, request);
            return searchServiceRequest;
        }
    }
}