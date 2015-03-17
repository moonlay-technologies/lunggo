using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravolutionaryWebServiceTest.Travolutionary.WebService;

namespace TravolutionaryWebServiceTest
{
    class ProgramBook
    {
        static void Main(String[] args)
        {
            const string userName = "rama.adhitia@travelmadezy.com";
            const string password = "d61Md7l7";

            using (var cli = new DynamicDataServiceClient("BasicHttpBinding_IDynamicDataService"))
            {
                var searchRequest = CreateHotelsServiceSearchRequest();
                var searchResponse = ExecuteHotelSearch(searchRequest);
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

        static DynamicDataServiceRsp ExecuteHotelSearch(HotelsServiceSearchRequest request)
        {
            var searchResponse = cli.ServiceRequest(new DynamicDataServiceRqst()
            {
                //SessionID = session,
                TypeOfService = ServiceType.Hotels,
                RequestType = ServiceRequestType.Search,
                Request = request,
                Credentials = new Credentials
                {
                    UserName = userName,
                    Password = password
                }
            });
            return searchResponse;
        }

    }
}
