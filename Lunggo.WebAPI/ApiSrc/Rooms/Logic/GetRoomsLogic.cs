using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Logic;
using Lunggo.ApCommon.Hotel.Logic.Search;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.WebAPI.ApiSrc.Rooms.Object;

namespace Lunggo.WebAPI.ApiSrc.Rooms.Logic
{
    public class GetRoomsLogic
    {
        public static RoomSearchApiResponse GetRooms(RoomSearchApiRequest request)
        {
            var searchServiceRequest = PreprocessSearchRequest(request);
            var searchServiceResponse = HotelRoomsSearchService.GetRooms(searchServiceRequest);
            var apiResponse = AssembleApiResponse(searchServiceResponse, request);
            return apiResponse;
        }

        public static IEnumerable<RoomPackageExcerpt> ConvertToRoomPackageExcerpts(IEnumerable<RoomPackage> roomPackages)
        {
            return roomPackages.Select(
                roomPackage => new RoomPackageExcerpt
                {
                    FinalPackagePrice   = roomPackage.FinalPackagePrice,
                    PackageId = roomPackage.PackageId,
                    RoomList = roomPackage.RoomList.Select(
                        room => new RoomExcerpt
                        {
                            AdultCount = room.AdultCount,
                            ChildrenCount = room.ChildrenCount,
                            RoomId = room.RoomId,
                            RoomName = room.RoomName,
                            FinalRoomPrice = room.FinalRoomPrice,
                            RoomDescription = room.RoomDescription
                        })
                });
        }

        public static RoomSearchApiResponse AssembleApiResponse(HotelRoomsSearchServiceResponse response,
            RoomSearchApiRequest request)
        {
            var packageList = ConvertToRoomPackageExcerpts(response.RoomPackages);
            var roomPackageExcerpts = packageList as IList<RoomPackageExcerpt> ?? packageList.ToList();
            var apiResponse = new RoomSearchApiResponse
            {
                InitialRequest = request,
                PackageList = roomPackageExcerpts,
                TotalPackageCount = roomPackageExcerpts.Count()
            };
            return apiResponse;
        }

        private static HotelRoomsSearchServiceRequest PreprocessSearchRequest(RoomSearchApiRequest request)
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