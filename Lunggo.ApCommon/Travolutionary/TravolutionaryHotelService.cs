using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;

namespace Lunggo.ApCommon.Travolutionary
{
    public class TravolutionaryHotelService
    {
        private const String DefaultResultCurrency = "USD";
        private const SearchDetailLevel DefaultHotelSearchDetailLevel = SearchDetailLevel.Meta;
        private const SearchDetailLevel DefaultHotelRoomSearchDetailLevel = SearchDetailLevel.Default;
        private const String DefaultResidency = "ID";
        private const int DefaultAdultCount = 2;

        //TODO Use configuration file do not hardcode
        private const string UserName = "rama.adhitia@travelmadezy.com";
        private const string Password = "d61Md7l7";

        public static TravolutionaryHotelSearchResponse SearchHotel(HotelsSearchServiceRequest request)
        {
            var travolutionarySearchRequest = CreateHotelSearchServiceRequest(request);
            var response = SearchInternal(travolutionarySearchRequest);
            return response;
        }

        public static TravolutionaryHotelRoomSearchResponse GetHotelRooms(HotelRoomsSearchServiceRequest request)
        {
            var travolutionarySearchRequest = CreateHotelRoomsSearchServiceRequest(request);
            var response = GetHotelRoomsInternal(travolutionarySearchRequest);
            return response;
        }

        public static TravolutionaryHotelBookResponse BookHotel(HotelBookServiceRequest request)
        {
            return null;
        }

        private static TravolutionaryHotelRoomSearchResponse GetHotelRoomsInternal(HotelsServiceSearchRequest request)
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
                var retVal = AssembleHotelRoomSearchResponse(searchResponse);
                return retVal;
            }
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
                HotelIdList = hotelArray!= null ? hotelArray.Select(p => p.ID) : null,
                SessionId = response.SessionID
            };
            return searchResponse;
        }

        private static TravolutionaryHotelRoomSearchResponse AssembleHotelRoomSearchResponse(
            DynamicDataServiceRsp response)
        {
            var retVal = InitializeTravolutionaryResponse<TravolutionaryHotelRoomSearchResponse>(response);
            if (!IsErrorTravolutionaryResponse(retVal) && ResponseContainsHotels(response))
            {
                SetSessionIdInResponse(retVal, response);
                AssembleRoomPackages(retVal, response);    
            }
            return retVal;
        }

        private static void AssembleRoomPackages(TravolutionaryHotelRoomSearchResponse response,
            DynamicDataServiceRsp rsp)
        {
            var roomPackages = ExtractRoomPackages(rsp);
            response.RoomPackages = roomPackages;
        }

        private static IEnumerable<RawRoomPackage> ExtractRoomPackages(DynamicDataServiceRsp rsp)
        {
            var hotel = rsp.HotelsSearchResponse.Result.First();
            var packages = hotel.Packages;

            //TODO Use configuration file do not hardcode
            return packages.Select(p => new RawRoomPackage
            {
                PackageId = p.PackageId.ToString(),
                PackagePrice = new Lunggo.ApCommon.Model.Price
                {
                    Currency = "USD",
                    Value = (decimal)p.PackagePrice.FinalPrice
                },
                RoomList = p.Rooms.Select(r => new RawRoom
                {
                    AdultCount = r.AdultsCount,
                    ChildrenCount = r.KidsAges == null ? 0 : r.KidsAges.Length,
                    RoomId = r.Id.ToString(CultureInfo.InvariantCulture),
                    RoomDescription = r.RoomBasis,
                    RoomName = r.RoomClass + " " + r.RoomType,
                    RoomPrice = new Model.Price
                    {
                        Currency = "USD",
                        Value = r.Price == null ? 0 : (decimal)r.Price.FinalPrice
                    }
                })
            });
        }


        private static void SetSessionIdInResponse(TravolutionaryResponseBase response, DynamicDataServiceRsp rsp)
        {
            response.SessionId = rsp.SessionID;
        }

        public static bool IsErrorTravolutionaryResponse(TravolutionaryResponseBase response)
        {
            return response.Errors != null && response.Errors.Any();
        }

        private static bool ResponseContainsHotels(DynamicDataServiceRsp response)
        {
            if (response.HotelsSearchResponse != null)
            {
                var hotelSearchResponse = response.HotelsSearchResponse;
                return (hotelSearchResponse.Result != null && hotelSearchResponse.Result.Any()) ? true : false; 
            }
            else
            {
                return false;
            }
        }

        private static T InitializeTravolutionaryResponse<T>(DynamicDataServiceRsp response) where T : TravolutionaryResponseBase,new()
        {
            IEnumerable<Lunggo.Framework.Error.Error> errorList = null;
            if (response.Errors != null && response.Errors.Any())
            {
                errorList = response.Errors.Select(TravolutionaryErrorMapper.MapErrorCode);
            }

            return new T
            {
                Errors = errorList
            };
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


        private static HotelsServiceSearchRequest CreateHotelRoomsSearchServiceRequest(HotelRoomsSearchServiceRequest request)
        {
            var searchRqst = new HotelsServiceSearchRequest()
            {
                CheckIn = request.StayDate,
                CheckOut = request.StayDate.AddDays(request.StayLength),
                ClientIP = null,
                //ContractIds = new int[] { 5 }, //Search by contract id with hotel supplier. (Can be retrieved from admin panel: Admin>Contracts)
                DesiredResultCurrency = DefaultResultCurrency, //Currency ISO (Example: "USD", "EUR", "ILS")
                DetailLevel = DefaultHotelRoomSearchDetailLevel,

                ExcludeHotelDetails = true,
                HotelIds = new int[]
                {
                    request.HotelId
                }, 
                IncludeCityTax = false,

                Nights = request.StayLength, //Mandatory. Number of nights for hotel stay (Example: 2). Cannot be “0” or “NULL”!
                Residency = DefaultResidency, //Mandatory. Lead pax residency, ISO Country Code (Example: US, CZ, IL)
                //ResponseLanguage = null, // No description in API documentation
                Rooms = InitializeHotelRoomRequest(request),
                //SupplierIds = new int[] { } //For search from specific suppliers.
            };
            return searchRqst;
        }
        private static HotelRoomRequest[] InitializeHotelRoomRequest(HotelSearchServiceRequestBase request)
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
