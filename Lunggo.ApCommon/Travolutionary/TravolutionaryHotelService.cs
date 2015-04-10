using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;
using Lunggo.Framework.Config;
using Room = Lunggo.ApCommon.Travolutionary.WebService.Hotel.Room;

namespace Lunggo.ApCommon.Travolutionary
{
    public class TravolutionaryHotelService
    {
        private const String DefaultResultCurrency = "USD";
        private const SearchDetailLevel DefaultHotelSearchDetailLevel = SearchDetailLevel.Meta;
        private const SearchDetailLevel DefaultHotelRoomSearchDetailLevel = SearchDetailLevel.Default;
        private const String DefaultResidency = "ID";
        private const int DefaultAdultCount = 2;
        private const String RoomAvailable = "Available";
        private const String RoomUnavailable = "Unavailable";
        private const int DefaultChildAge = 8;

        private readonly static String UserName = ConfigManager.GetInstance().GetConfigValue("travolutionary", "apiUserName");
        private readonly static String Password = ConfigManager.GetInstance().GetConfigValue("travolutionary", "apiPassword");
        private static readonly TravolutionaryHotelServiceErrorDictionary ErrorDictionary = TravolutionaryHotelServiceErrorDictionary.GetInstance();

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

        public static TravolutionaryHotelBookResponse BookHotel(HotelBookServiceRequest bookRequest)
        {
            
            var preBookCheckResponse = PreBookCheck(bookRequest);    
            if (preBookCheckResponse.IsBookable)
            {
                
            }
            else
            {
                var travolutionaryHotelBookRequest = CreateHotelBookRequest(bookRequest);
            }
            return null;
        }

        public static TravolutionaryHotelBookResponse BookHotelInternal(HotelBookRequest bookRequest, String sessionId)
        {
            var bookResponse = CallHotelBookApi(bookRequest, sessionId);
            var retVal = ProcessHotelBookResponse(bookResponse);
            return retVal;
        }

        public static DynamicDataServiceRsp CallHotelBookApi(HotelBookRequest bookRequest, String sessionId)
        {
            using (var cli = new DynamicDataServiceClient("BasicHttpBinding_IDynamicDataService"))
            {
                var bookResponse = cli.ServiceRequest(new DynamicDataServiceRqst
                {
                    SessionID = sessionId,
                    TypeOfService = ServiceType.Hotels,
                    RequestType = ServiceRequestType.Book,
                    Request = bookRequest,
                });
                return bookResponse;
            }
        }

        public static TravolutionaryHotelBookResponse ProcessHotelBookResponse(DynamicDataServiceRsp searchResponse)
        {
            return null;
        }

        static HotelRepricePackageRequest CreateRepricePackageRequest(HotelBookServiceRequest bookRequest)
        {
            return new HotelRepricePackageRequest
            {
                HotelId = bookRequest.HotelId,
                PackageId = new Guid(bookRequest.Package.PackageId),
                Rooms = bookRequest.Package.Rooms.Select(p => new RepriceRoomRequest
                {
                    Adults = p.AdultCount,
                    Availability = p.Available? RoomAvailable : RoomUnavailable,
                    Id = p.RoomId,
                    KidsAges = GetKidAgesArray(p.ChildCount),
                    RoomBasis = p.RoomBasis,
                    RoomClass = p.RoomClass,
                    RoomKind = p.RoomType
                }).ToArray(),
                SearchRequest = CreateHotelRoomsSearchServiceRequest(bookRequest.SearchRequest),
                TotalPrice = Convert.ToDouble(bookRequest.Package.FinalPriceFromSupplier)
            };
        }

        private static int[] GetKidAgesArray(int childCount)
        {
            var kidAgesArray = new int[childCount];
            for (var i=0;i<childCount;i++)
            {
                kidAgesArray[i] = DefaultChildAge;
            }
            return kidAgesArray;
        }

        private static TravolutionaryPreBookCheckResponse PreBookCheck(HotelBookServiceRequest request)
        {
            //var response = PreBookCheckInternal(request);
            return null;
        }

        private static TravolutionaryPreBookCheckResponse PreBookCheckInternal(HotelRepricePackageRequest repricePackageRequest)
        {
            var repricePackageApiResponse = CallRepricePackageApi(repricePackageRequest);
            var preBookCheckResponse = ErrorMappingAndTravolutionaryResponseInitialization<TravolutionaryPreBookCheckResponse>(repricePackageApiResponse,TravolutionaryHotelServiceErrorMapper.RepricePackageErrorMapper);


            return null;
        }

        private static DynamicDataServiceRsp CallRepricePackageApi(HotelRepricePackageRequest request)
        {
            using (var cli = new DynamicDataServiceClient("BasicHttpBinding_IDynamicDataService"))
            {
                var repricePackageResponse = cli.ServiceRequest(new DynamicDataServiceRqst
                {
                    TypeOfService = ServiceType.Hotels,
                    RequestType = ServiceRequestType.RepriceItem,
                    Request = request,
                    Credentials = new Credentials
                    {
                        UserName = UserName,
                        Password = Password
                    }
                });
                return repricePackageResponse;
            }
        }
        
        private static HotelBookRequest CreateHotelBookRequest(HotelBookServiceRequest bookRequest)
        {
            var customerInfoArray = CreateCustomerInfoArray(bookRequest);

            var travolutionaryBookRequest = new HotelBookRequest
            {
                ClientIP = bookRequest.ClientIp, //mandatory
                //TODO create price logic
                BookingPrice = 100, //mandatory
                HotelID = bookRequest.HotelId, //mandatory
                InternalAgentRef1 = "No Data", //optional
                InternalAgentRef2 = "No Data", //optional
                PackageID = new Guid(bookRequest.Package.PackageId), //mandatory
                LeadPaxId = customerInfoArray.First().Id,
                LeadPaxRoomId = customerInfoArray.First().Allocation, //mandatory
                Passengers = customerInfoArray,
                RoomsRemarks = CreateRoomsRemarks(bookRequest.Package),
                SelectedPaymentMethod = PaymentMethod.Cash
            };
            return travolutionaryBookRequest;
        }

        private static Dictionary<String, String> CreateRoomsRemarks(PackageDetailForBooking package)
        {
            return package.Rooms.ToDictionary<RoomDetailForBooking, string, string>(room => room.RoomId, room => null);
        }

        private static CustomerInfo[] CreateCustomerInfoArray(HotelBookServiceRequest bookRequest)
        {
            var customerInfoList = new List<CustomerInfo>();
            var roomCounter = 0;
            foreach (var name in bookRequest.LeadRoomOccupantNames)
            {
                var roomId = bookRequest.Package.Rooms.ElementAt(roomCounter).RoomId;
                var customerInfo1 = CreateCustomerInfo(name, roomId);
                var customerInfo2 = CreateCustomerInfo(name, roomId);
                customerInfoList.Add(customerInfo1);
                customerInfoList.Add(customerInfo2);
                roomCounter++;
            }
            return customerInfoList.ToArray();
        }

        private static CustomerInfo CreateCustomerInfo(String name, String roomId)
        {
            var nameParts = name.Split(null);
            var customerInfo = new CustomerInfo
            {
                Allocation = roomId,
                PersonDetails = new Person
                {
                    Name = new PersonName
                    {
                        GivenName = nameParts[0],
                        NamePrefix = "Mr/Mrs",
                        Surname = nameParts.Count() > 1 ? nameParts[1] : nameParts[0]
                    },
                    Type = PersonType.Adult
                },
                Id = Guid.NewGuid().ToString()
            };
            return customerInfo;
        }

        private static TravolutionaryHotelRoomSearchResponse GetHotelRoomsInternal(HotelsServiceSearchRequest request)
        {
            using (var cli = new DynamicDataServiceClient("BasicHttpBinding_IDynamicDataService"))
            {
                var searchResponse = cli.ServiceRequest(new DynamicDataServiceRqst
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
                var searchResponse = cli.ServiceRequest(new DynamicDataServiceRqst
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
            var retVal = ErrorMappingAndTravolutionaryResponseInitialization<TravolutionaryHotelRoomSearchResponse>(response,TravolutionaryHotelServiceErrorMapper.HotelRoomsSearchErrorMapper);
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

            return packages.Select(p => new RawRoomPackage
            {
                PackageId = p.PackageId.ToString(),
                PackagePrice = new Model.Price
                {
                    Currency = DefaultResultCurrency,
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
                        Currency = DefaultResultCurrency,
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

        private static T ErrorMappingAndTravolutionaryResponseInitialization<T>(DynamicDataServiceRsp response, Action<Error[],List<Lunggo.Framework.Error.Error>> errorMapper) where T : TravolutionaryResponseBase,new()
        {
            List<Lunggo.Framework.Error.Error> errorList = null;
            if (response == null)
            {
                errorList = new List<Lunggo.Framework.Error.Error>
                {
                    new Framework.Error.Error()
                    {
                        Code = "E1001",
                        Message = ErrorDictionary.Errors["E1001"]
                    }
                };
            }
            else
            {
                if (response.Errors != null && response.Errors.Any())
                {
                    errorList = new List<Lunggo.Framework.Error.Error>();
                    errorMapper(response.Errors,errorList);
                }    
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

    class TravolutionaryHotelServiceErrorDictionary
    {
        private static readonly TravolutionaryHotelServiceErrorDictionary Instance = new TravolutionaryHotelServiceErrorDictionary();
        private readonly Dictionary<String, String> _errors;
 
        public Dictionary<String, String> Errors
        {
            get { return _errors; }
        }
        private TravolutionaryHotelServiceErrorDictionary()
        {
            _errors = new Dictionary<String, String>
            {
                {"E1001", "API Response is null"},
                {"E1002", "Invalid API Request"},
                {"E1003", "Access is Denied / Wrong credential"},
                {"E1004", "Error from Travolutionary side"},
                {"E1005", "Room Package cannot be booked"},
                {"E1006", "Session Id is not Found / Failed to read session data"},
                {"E1099", "Unknown Error"}
            };
        }

        public static TravolutionaryHotelServiceErrorDictionary GetInstance()
        {
            return Instance;
        }
    }

    class TravolutionaryHotelServiceErrorMapper
    {
        public static void RepricePackageErrorMapper(Error[] apiErrorList, List<Lunggo.Framework.Error.Error> serviceErrorList)
        {
                   
        }

        public static void HotelRoomsSearchErrorMapper(Error[] apiErrorList, List<Lunggo.Framework.Error.Error> serviceErrorList)
        {
               
        }
    }
}
