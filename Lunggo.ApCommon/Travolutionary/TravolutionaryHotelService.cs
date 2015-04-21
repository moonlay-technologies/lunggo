using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;
using Lunggo.Framework.Config;
using Lunggo.Framework.Error;
using CustomerInfo = Lunggo.ApCommon.Travolutionary.WebService.Hotel.CustomerInfo;
using Error = Lunggo.ApCommon.Travolutionary.WebService.Hotel.Error;

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
        private static readonly TravolutionaryHotelServiceErrorMapper ErrorMapper = TravolutionaryHotelServiceErrorMapper.GetInstance();

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
            TravolutionaryHotelBookResponse hotelBookResponse = null;
            var preBookCheckResponse = PreBookCheck(bookRequest);    
            if (preBookCheckResponse.IsErrorLess())
            {
                var travolutionaryHotelBookRequest = CreateHotelBookRequest(bookRequest);
            }
            else
            {
                hotelBookResponse =
                    ErrorMappingAndTravolutionaryResponseInitialization<TravolutionaryHotelBookResponse>(
                        preBookCheckResponse);
            }
            return hotelBookResponse;
        }

        private static TravolutionaryHotelBookResponse BookHotelInternal(HotelBookRequest bookRequest, String sessionId)
        {
            var bookResponse = CallHotelBookApi(bookRequest, sessionId);
            var retVal = ProcessHotelBookResponse(bookResponse);
            return retVal;
        }

        private static DynamicDataServiceRsp CallHotelBookApi(HotelBookRequest bookRequest, String sessionId)
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

        private static TravolutionaryHotelBookResponse ProcessHotelBookResponse(DynamicDataServiceRsp apiBookResponse)
        {
            var hotelBookResponse = ErrorMappingAndTravolutionaryResponseInitialization<TravolutionaryHotelBookResponse>(apiBookResponse
                , TravolutionaryHotelServiceErrorMapper.HotelBookErrorMapper);
            if (hotelBookResponse.IsErrorLess())
            {
                CopyApiBookResponse(apiBookResponse, hotelBookResponse);
            }

            return hotelBookResponse;
        }

        private static void CopyApiBookResponse(DynamicDataServiceRsp apiBookResponse,TravolutionaryHotelBookResponse serviceResponse)
        {
            var hotelSegmentsFromApi = apiBookResponse.HotelOrderBookResponse.HotelSegments;
            var hotelSegments = hotelSegmentsFromApi.Select(segment => new TravolutionaryHotelBookSegment
            {
                OrderId = segment.OrderId,
                SegmentId = segment.SegmentId,
                SegmentStatus = segment.Status,
                SupplierBookingId = segment.BookingID,
                SupplierBookingReference = segment.BookingReference
            }).ToList();
            serviceResponse.HotelSegments = hotelSegments;
        }


        private static HotelRepricePackageRequest CreateRepricePackageRequest(HotelBookServiceRequest bookRequest)
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
            var pricePackageRequest = CreateRepricePackageRequest(request);
            var response = PreBookCheckInternal(pricePackageRequest);
            return response;
        }

        private static TravolutionaryPreBookCheckResponse PreBookCheckInternal(HotelRepricePackageRequest repricePackageRequest)
        {
            var repricePackageApiResponse = CallRepricePackageApi(repricePackageRequest);
            var preBookCheckResponse = ErrorMappingAndTravolutionaryResponseInitialization<TravolutionaryPreBookCheckResponse>(repricePackageApiResponse,TravolutionaryHotelServiceErrorMapper.HotelPreBookErrorMapper);
            return preBookCheckResponse;
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
            var hotelSearchResponse =
                ErrorMappingAndTravolutionaryResponseInitialization<TravolutionaryHotelSearchResponse>(response
                    , TravolutionaryHotelServiceErrorMapper.HotelsSearchErrorMapper);

            if (hotelSearchResponse.IsErrorLess())
            {
                var hotelArray = response.HotelsSearchResponse.Result;
                hotelSearchResponse.HotelIdList = hotelArray != null ? hotelArray.Select(p => p.ID) : null;
                hotelSearchResponse.SessionId = response.SessionID;    
            }

            return hotelSearchResponse;
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

        private static T ErrorMappingAndTravolutionaryResponseInitialization<T>(DynamicDataServiceRsp response, Action<Error[],HashSet<Lunggo.Framework.Error.Error>> errorMapper) where T : TravolutionaryResponseBase,new()
        {
            HashSet<Lunggo.Framework.Error.Error> errorList = null;
            if (response == null)
            {
                errorList = new HashSet<Lunggo.Framework.Error.Error>(new ErrorComparer())
                {
                    new Framework.Error.Error()
                    {
                        Code = "E1001",
                        Message = ErrorMapper.Errors["E1001"]
                    }
                };
            }
            else
            {
                if (response.Errors != null && response.Errors.Any())
                {
                    errorList = new HashSet<Lunggo.Framework.Error.Error>(new ErrorComparer());
                    errorMapper(response.Errors,errorList);
                }    
            }

            return new T
            {
                Errors = errorList
            };
        }

        private static T ErrorMappingAndTravolutionaryResponseInitialization<T>(
            Action<HashSet<Lunggo.Framework.Error.Error>>  errorMapper)  where T : TravolutionaryResponseBase,new()
        {
            var errorList = new HashSet<Framework.Error.Error>(new ErrorComparer());
            errorMapper(errorList);

            return new T
            {
                Errors = errorList
            };
        }

        private static T ErrorMappingAndTravolutionaryResponseInitialization<T>(TravolutionaryResponseBase response) where T : TravolutionaryResponseBase, new()
        {
            return new T
            {
                Errors = response.Errors
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

    class TravolutionaryHotelServiceErrorMapper
    {
        private static readonly TravolutionaryHotelServiceErrorMapper Instance = new TravolutionaryHotelServiceErrorMapper();
        private static readonly Dictionary<String, String> ServiceErrors;
        private static readonly Dictionary<String, String> ApiErrors; 

        static TravolutionaryHotelServiceErrorMapper()
        {
            ServiceErrors = new Dictionary<string, string>
            {
                {"E1001", "API Response is null"},
                {"E1002", "Invalid API Request"},
                {"E1003", "Access is Denied / Wrong credential"},
                {"E1004", "Error from Travolutionary side"},
                {"E1005", "Room Package cannot be booked"},
                {"E1006", "Session Id is not Found / Failed to read session data"},
                {"E1007", "Room Package Prebook Error"},
                {"E1099", "Unknown Error"}
            };

            ApiErrors = new Dictionary<string, string>
            {
                {"E4000", "API response is null"},
                {"E4050", "Item cannot be booked"},
                {"E4051", "Booking is not allowed on this account"},
                {"E4060", "Item cannot be booked - Funds Deposit Issue"},
                {"E4100", "The booking cannot be confirmed, it is not available anymore"},
                {"E4101", "Cannot book the same item twice in the same session"},
                {"E3600", "Hotel Id x is not found in search results"},
                {"E3601", "Package Id x is not found in search results"},
                {"E2000", "User x is not authorized to perform y action"},
                {"E2001", "User access denied"},
                {"E2050", "Segment access denied"},
                {"E2060", "Credential is not specified"},
                {"E2061", "Couldn't verify credentials"},
                {"E2062", "Providing Both credential and session id is not allowed"},
                {"E0500", "Internal Error"},
                {"E0999", "Unknown Error"},
                {"E1000", "Supplier Error"},
                {"E0502", "Supplier Action is not implemented"},
                {"E4300", "Failed to retrieve searched details"},
                {"E0300", "Session Id is not found"},
                {"E4302", "Failed to read session data"},
                {"E4301", "No matching item is found"},
                {"E0501", "Request x y doesn't exist"},
                {"E1100", "Invalid request data"},
                {"E0400", "Action x cannot be performed on a segment with status y"},
                {"E4303", "Currency x is not supported"}
            };
        }

        public Dictionary<string, string> Errors
        {
            get { return ServiceErrors; }
        }


        private TravolutionaryHotelServiceErrorMapper()
        {
        }

        public static TravolutionaryHotelServiceErrorMapper GetInstance()
        {
            return Instance;
        }

        public static void HotelPreBookErrorMapper(Error[] apiErrorList, HashSet<Framework.Error.Error> serviceErrorList)
        {
            foreach (var error in apiErrorList)
            {
                CheckPermissionAndAuthorizationError(error,serviceErrorList);
                CheckErrorFromTravolutionarySide(error,serviceErrorList);
                CheckSessionError(error,serviceErrorList);
                CheckRoomPackagePreBookError(error,serviceErrorList);
                CheckInvalidRequestError(error,serviceErrorList);
                CheckUnknownError(error,serviceErrorList);
            }
        }

        public static void HotelBookErrorMapper(Error[] apiErrorList, HashSet<Framework.Error.Error> serviceErrorList)
        {
            foreach (var error in apiErrorList)
            {
                CheckPermissionAndAuthorizationError(error, serviceErrorList);
                CheckErrorFromTravolutionarySide(error, serviceErrorList);
                CheckSessionError(error, serviceErrorList);
                CheckRoomPackageBookingError(error,serviceErrorList);
                CheckInvalidRequestError(error, serviceErrorList);
                CheckUnknownError(error, serviceErrorList);
            }
        }

        public static void HotelRoomsSearchErrorMapper(Error[] apiErrorList, HashSet<Framework.Error.Error> serviceErrorList)
        {
            foreach (var error in apiErrorList)
            {
                CheckPermissionAndAuthorizationError(error,serviceErrorList);
                CheckErrorFromTravolutionarySide(error,serviceErrorList);
                CheckSessionError(error, serviceErrorList);
                CheckInvalidRequestError(error, serviceErrorList);
                CheckUnknownError(error, serviceErrorList);
            }
        }

        public static void HotelsSearchErrorMapper(Error[] apiErrorList, HashSet<Framework.Error.Error> serviceErrorList)
        {
            foreach (var error in apiErrorList)
            {
                CheckPermissionAndAuthorizationError(error, serviceErrorList);
                CheckErrorFromTravolutionarySide(error, serviceErrorList);
                CheckSessionError(error, serviceErrorList);
                CheckInvalidRequestError(error, serviceErrorList);
                CheckUnknownError(error, serviceErrorList);
            }
        }

        private static void CheckRoomPackageBookingError(Error apiError, HashSet<Framework.Error.Error> serviceErrorList)
        {
            var apiErrorCode = apiError.ErrorCode.Trim();
            if (
                    apiErrorCode.Equals("E4000", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E4050", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E4051", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E4060", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E4100", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E4101", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E3600", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E3601", StringComparison.CurrentCultureIgnoreCase)
                )
            {
                var serviceError = new Framework.Error.Error
                {
                    Code = "E1005",
                    Message = ServiceErrors["E1005"]
                };
                serviceErrorList.Add(serviceError);
            }
        }

        private static void CheckPermissionAndAuthorizationError(Error apiError, HashSet<Framework.Error.Error> serviceErrorList)
        {
            var apiErrorCode = apiError.ErrorCode.Trim();
            if (
                    apiErrorCode.Equals("E2000", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E2001", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E2050", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E2060", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E2061", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E2062", StringComparison.CurrentCultureIgnoreCase)
                )
            {
                var serviceError = new Framework.Error.Error
                {
                    Code = "E1003",
                    Message = ServiceErrors["E1003"]
                };
                serviceErrorList.Add(serviceError);
            }
        }

        private static void CheckErrorFromTravolutionarySide(Error apiError, HashSet<Framework.Error.Error> serviceErrorList)
        {
            var apiErrorCode = apiError.ErrorCode.Trim();
            if (
                    apiErrorCode.Equals("E0500", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E0999", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E1000", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E0502", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E4300", StringComparison.CurrentCultureIgnoreCase)
                )
            {
                var serviceError = new Framework.Error.Error
                {
                    Code = "E1004",
                    Message = ServiceErrors["E1004"]
                };
                serviceErrorList.Add(serviceError);
            }
        }

        private static void CheckSessionError(Error apiError, HashSet<Framework.Error.Error> serviceErrorList)
        {
            var apiErrorCode = apiError.ErrorCode.Trim();
            if (
                    apiErrorCode.Equals("E0300", StringComparison.CurrentCultureIgnoreCase) ||
                    apiErrorCode.Equals("E4302", StringComparison.CurrentCultureIgnoreCase)
                )
            {
                var serviceError = new Framework.Error.Error
                {
                    Code = "E1006",
                    Message = ServiceErrors["E1006"]
                };
                serviceErrorList.Add(serviceError);
            }
        }

        private static void CheckRoomPackagePreBookError(Error apiError, HashSet<Framework.Error.Error> serviceErrorList)
        {
            var apiErrorCode = apiError.ErrorCode.Trim();
            if (
                   apiErrorCode.Equals("E4301", StringComparison.CurrentCultureIgnoreCase)
                )
            {
                var serviceError = new Framework.Error.Error
                {
                    Code = "E1007",
                    Message = ServiceErrors["E1007"]
                };
                serviceErrorList.Add(serviceError);
            }
        }

        private static void CheckInvalidRequestError(Error apiError, HashSet<Framework.Error.Error> serviceErrorList)
        {
            var apiErrorCode = apiError.ErrorCode.Trim();
            if (
                   apiErrorCode.Equals("E0501", StringComparison.CurrentCultureIgnoreCase) ||
                   apiErrorCode.Equals("E1100", StringComparison.CurrentCultureIgnoreCase) ||
                   apiErrorCode.Equals("E0400", StringComparison.CurrentCultureIgnoreCase) ||
                   apiErrorCode.Equals("E4303", StringComparison.CurrentCultureIgnoreCase)
                )
            {
                var serviceError = new Framework.Error.Error
                {
                    Code = "E1002",
                    Message = ServiceErrors["E1002"]
                };
                serviceErrorList.Add(serviceError);
            }
        }

        private static void CheckUnknownError(Error apiError, HashSet<Framework.Error.Error> serviceErrorList)
        {
            var apiErrorCode = apiError.ErrorCode.Trim().ToUpperInvariant();
            string dump;
            if (!ApiErrors.TryGetValue(apiErrorCode, out dump))
            {
                var serviceError = new Framework.Error.Error
                {
                    Code = "E1099",
                    Message = ServiceErrors["E1099"]
                };
                serviceErrorList.Add(serviceError);
            }
        }
    }
}
