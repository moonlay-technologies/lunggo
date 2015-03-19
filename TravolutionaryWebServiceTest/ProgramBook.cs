using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TravolutionaryWebServiceTest.Travolutionary.WebService;

namespace TravolutionaryWebServiceTest
{
    class ProgramBook
    {
        const string UserName = "rama.adhitia@travelmadezy.com";
        const string Password = "d61Md7l7";

        static void Main(String[] args)
        {
            using (var cli = new DynamicDataServiceClient("BasicHttpBinding_IDynamicDataService"))
            {
                var searchRequest = CreateHotelsServiceSearchRequest();
                var searchResponse = ExecuteHotelSearch(searchRequest,cli);

                if (searchResponse.Errors != null && searchResponse.Errors.Any())
                {
                    foreach (var error in searchResponse.Errors)
                    {
                        Console.Error.WriteLine("Search Failed.");
                        Console.Out.WriteLine(error.Message);
                    }
                    return;
                }
                ExecuteHotelBook(searchResponse,cli);
                Console.WriteLine("Sukses");
            }
        }

        static HotelsServiceSearchRequest CreateHotelsServiceSearchRequest()
        {
            var searchRequest = new HotelsServiceSearchRequest()
            {
                CheckIn = DateTime.Now.AddDays(3),
                CheckOut = DateTime.Now.AddDays(4),
                Nights = 1,
                DesiredResultCurrency = "USD",
                DetailLevel = SearchDetailLevel.Default, //THIS IS A VERY IMPORTANG PARAMETER
                Residency = "ID",
                Rooms = new[]
                           {
                               //One room with thow adults and one child 
                               new HotelRoomRequest()
                                   {
                                       AdultsCount = 2,
                                   }
                           },
                HotelIds = new int[]
                    {
                        4116550
                    }
                //HotelLocation = 640255 //Prague
            };
            return searchRequest;
        }

        static DynamicDataServiceRsp ExecuteHotelSearch(HotelsServiceSearchRequest request,DynamicDataServiceClient cli)
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
            return searchResponse;
        }

        static void ExecuteHotelBook(DynamicDataServiceRsp searchResponse, DynamicDataServiceClient cli)
        {
            var hotelTable = searchResponse.HotelsSearchResponse.Result;
            var sessionId = searchResponse.SessionID;

            var packageTable = hotelTable[0].Packages;
            var firstPackage = packageTable[0];
            var leadPaxId = Guid.NewGuid().ToString();

            var bookRequest = new HotelBookRequest
            {
                ClientIP = "139.228.112.122", //mandatory
                BookingPrice = 100, //mandatory
                HotelID = hotelTable[0].ID, //mandatory
                InternalAgentRef1 = "No Data", //optional
                InternalAgentRef2 = "No Data", //optional
                PackageID = firstPackage.PackageId, //mandatory
                LeadPaxId = leadPaxId, //mandatory
                LeadPaxRoomId = firstPackage.Rooms[0].Id, //mandatory
                Passengers = new[]
                    {
                        new CustomerInfo
                        {
                            Allocation = firstPackage.Rooms[0].Id,
                            PersonDetails = new Person
                            {
                                Name = new PersonName
                                {
                                    GivenName = "Rama",
                                    NamePrefix = "Mr",
                                    Surname = "Adhitia"
                                },
                                Type = PersonType.Adult
                            },
                            Id = leadPaxId
                        },
                        new CustomerInfo
                        {
                            Allocation = firstPackage.Rooms[0].Id,
                            PersonDetails = new Person
                            {
                                Name = new PersonName
                                {
                                    GivenName = "Rachel",
                                    NamePrefix = "Mrs",
                                    Surname = "Adhitia"
                                },
                                Type = PersonType.Adult
                            },
                            Id = Guid.NewGuid().ToString()
                        }
                    },
                RoomsRemarks = new Dictionary<String, String>()
                    {
                        {firstPackage.Rooms[0].Id,null}
                    },
                SelectedPaymentMethod = PaymentMethod.Cash
            };

            var bookResponse = cli.ServiceRequest(new DynamicDataServiceRqst()
            {
                SessionID = sessionId,
                TypeOfService = ServiceType.Hotels,
                RequestType = ServiceRequestType.Book,
                Request = bookRequest
            });

            var hotelBookSegments = bookResponse.HotelOrderBookResponse.HotelSegments;
            foreach(var bookSegment in hotelBookSegments)
            {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Booking Id {0}",bookSegment.BookingID);
                Console.WriteLine("Booking Reference {0}", bookSegment.BookingReference);
                Console.WriteLine("Booking Order ID {0}", bookSegment.OrderId);
                Console.WriteLine("Booking Segment Id {0}", bookSegment.SegmentId);
                Console.WriteLine("Boooking Status {0}", bookSegment.Status);
                Console.WriteLine("--------------------------------------------------");
            }
        }
    }
}
