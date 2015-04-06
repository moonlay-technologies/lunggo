using System.Web.UI.WebControls;
using Lunggo.ApCommon.Hotel.Logic;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.CustomerWeb.WebSrc.UW100.Object;

namespace Lunggo.CustomerWeb.WebSrc.UW100.Logic
{
    public class HotelSearchLogic
    {
        public static Uw100HotelSearchResponse GetHotels(Uw100HotelSearchRequest request)
        {
            var searchServiceRequest = PreProcessSearchRequest(request);
            var response = AssembleResponse(searchServiceRequest);
            return response;
        }

        private static Uw100HotelSearchResponse AssembleResponse(HotelsSearchServiceRequest searchServiceRequest)
        {
            var response = new Uw100HotelSearchResponse
            {
                Lang = searchServiceRequest.Lang,
                Country = "Indonesia",
                LocationId = searchServiceRequest.LocationId,
                Area = "Senayan",
                LocationName = "Senayan, Jakarta, Indonesia",
                Province = "Jakarta",
                ResultCount = searchServiceRequest.ResultCount,
                RoomOccupants = searchServiceRequest.RoomOccupants,
                SearchFilter = searchServiceRequest.SearchFilter,
                SearchId = searchServiceRequest.SearchId,
                SortBy = searchServiceRequest.SortBy,
                StartIndex = searchServiceRequest.StartIndex,
                StayDate = searchServiceRequest.StayDate,
                StayLength = searchServiceRequest.StayLength
            };
            return response;
        }
        
        private static HotelsSearchServiceRequest PreProcessSearchRequest(Uw100HotelSearchRequest request)
        {
            var searchServiceRequest = ParameterPreProcessor.InitializeHotelsSearchServiceRequest(request);
            ParameterPreProcessor.PreProcessLangParam(searchServiceRequest,request);
            ParameterPreProcessor.PreProcessStayLengthParam(searchServiceRequest,request);
            ParameterPreProcessor.PreProcessStayDateParam(searchServiceRequest, request);
            ParameterPreProcessor.PreProcessPagingParam(searchServiceRequest, request);
            ParameterPreProcessor.PreProcessSortParam(searchServiceRequest, request);
            ParameterPreProcessor.PreProcessFilterParam(searchServiceRequest, request);
            ParameterPreProcessor.PreProcessRoomCountParam(searchServiceRequest, request);
            return searchServiceRequest;
        }
    }
}