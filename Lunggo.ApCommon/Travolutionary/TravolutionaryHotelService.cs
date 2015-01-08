using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;

namespace Lunggo.ApCommon.Travolutionary
{
    public class TravolutionaryHotelService
    {
        private const String DefaultResultCurrency = "USD";
        private const SearchDetailLevel DefaultHotelSearchDetailLevel = SearchDetailLevel.Meta;
        private const String DefaultResidency = "ID";
        private const int DefaultAdultCount = 2;
        private const string UserName = "rama.adhitia@travelmadezy.com";
        private const string Password = "d61Md7l7";

        public static TravolutionaryHotelSearchResponse SearchHotel(HotelsSearchServiceRequest request)
        {
            var travolutionarySearchRequest = CreateHotelSearchServiceRequest(request);
            var response = SearchInternal(travolutionarySearchRequest);
            return response;
        }

        private static TravolutionaryHotelSearchResponse SearchInternal(HotelsServiceSearchRequest request)
        {
            using (var cli = new DynamicDataServiceClient("BasicHttpBinding_IDynamicDataService"))
            {
                var searchResponse = cli.ServiceRequest(new DynamicDataServiceRqst()
                {
                    //SessionID = session,
                    TypeOfService = ServiceType.Hotels,
                    RequestType = ServiceRequestType.Search,
                    Request = request,
                    Credentials = new Credentials
                    {
                        UserName = UserName,
                        Password = Password
                    }
                });
                var retVal = AssembleHotelSearchResponse(searchResponse);
                return retVal;
            }   
        }

        private static TravolutionaryHotelSearchResponse AssembleHotelSearchResponse(DynamicDataServiceRsp response)
        {
            var hotelArray = response.HotelsSearchResponse.Result;
            var searchResponse = new TravolutionaryHotelSearchResponse
            {
                HotelIdList = hotelArray!= null ? hotelArray.Select(p=> p.ID) : null,
                SessionId = response.SessionID
            };
            return searchResponse;
        }

        private static HotelsServiceSearchRequest CreateHotelSearchServiceRequest(HotelsSearchServiceRequest request)
        {
            var searchRqst = new HotelsServiceSearchRequest()
            {
                CheckIn = request.StayDate,
                CheckOut = request.StayDate.AddDays(request.StayLength),
                ClientIP = null,
                //ContractIds = new int[] { 5 }, //Search by contract id with hotel supplier. (Can be retrieved from admin panel: Admin>Contracts)
                DesiredResultCurrency = DefaultResultCurrency, //Currency ISO (Example: "USD", "EUR", "ILS")
                DetailLevel = DefaultHotelSearchDetailLevel,

                ExcludeHotelDetails = true,
                //HotelIds = new int[] { }, 
                HotelLocation = request.LocationId,
                IncludeCityTax = false,

                Nights = request.StayLength, //Mandatory. Number of nights for hotel stay (Example: 2). Cannot be “0” or “NULL”!
                Residency = DefaultResidency, //Mandatory. Lead pax residency, ISO Country Code (Example: US, CZ, IL)
                //ResponseLanguage = null, // No description in API documentation
                Rooms = InitializeHotelRoomRequest(request),
                //SupplierIds = new int[] { } //For search from specific suppliers.
            };
            return searchRqst;
        }

        private static HotelRoomRequest[] InitializeHotelRoomRequest(HotelsSearchServiceRequest request)
        {
            var roomCount = request.RoomOccupants.Count();
            var hotelRoomRequest = new HotelRoomRequest[roomCount];
            for (var i = 0; i < roomCount; i++)
            {
                hotelRoomRequest[i] = new HotelRoomRequest
                {
                    AdultsCount = DefaultAdultCount,
                    SeperatedBeds = false
                };
            }
            return hotelRoomRequest;
        }


    }
}
