using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravolutionaryWebServiceTest.Travolutionary.WebService;

namespace TravolutionaryWebServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const string userName = "rama.adhitia@travelmadezy.com";
            const string password = "d61Md7l7";


            //Before doing a search perform a login to abtain a session.
            string session = Login(userName, password);

            if (session == null)
            {
                Console.WriteLine("session id null");
                return;
            }
            else
            {
                Console.WriteLine("session id adalah " + session);
            }

            using (var cli = new DynamicDataServiceClient("BasicHttpBinding_IDynamicDataService"))
            {

                var searchRequest = new HotelsServiceSearchRequest()
                {
                    CheckIn = DateTime.Now.AddDays(5),
                    CheckOut = DateTime.Now.AddDays(7),
                    Nights = 2,
                    DesiredResultCurrency = "USD",
                    DetailLevel = SearchDetailLevel.NoPackages, //THIS IS A VERY IMPORTANG PARAMETER
                    Residency = "US",
                    Rooms = new[]
                           {
                               //One room with thow adults and one child 
                               new HotelRoomRequest()
                                   {
                                       AdultsCount = 2,
                                       KidsAges = new []{5}
                                   }, 
                           },
                    HotelLocation = 457034 //Prague
                       
                };

                var searchRqst = new HotelsServiceSearchRequest()
                {
                    CheckIn = DateTime.Now.AddDays(5),
                    CheckOut = DateTime.Now.AddDays(6),
                    ClientIP = "ngasal",
                    ContractIds = new int[] { 5,1},
                    DesiredResultCurrency = "USD",
                    DetailLevel = SearchDetailLevel.Minimal,
                    ExcludeHotelDetails = true,


                };


                Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
               
                var searchResponse = cli.ServiceRequest(new DynamicDataServiceRqst()
                {
                    SessionID = session,
                    TypeOfService = ServiceType.Hotels,
                    RequestType = ServiceRequestType.Search,
                    Request = searchRequest,
                });

                if (searchResponse.Errors != null && searchResponse.Errors.Any())
                {
                    foreach (var error in searchResponse.Errors)
                    {
                        Console.Error.WriteLine("Search Failed.");
                        Console.Out.WriteLine(error.Message);
                    }
                    return;
                }

                Console.WriteLine("Ditemukan " + searchResponse.HotelsSearchResponse.Result.Length + "hotel");
                stopwatch.Stop();
                Console.WriteLine("Pencarian membutuhkan waktu " + stopwatch.ElapsedMilliseconds / 1000 + " detik");

                /*foreach (var hotel in searchResponse.HotelsSearchResponse.Result)
                {
                    Console.Out.WriteLine("Hotel ->{0,-50},LowestPrice ->{1,10} USD", hotel.DisplayName, hotel.Packages.Min(p => p.SimplePrice));
                }*/
            }



        }

        private static string Login(string userName, string password)
        {
            using (var cli = new DynamicDataServiceClient("BasicHttpBinding_IDynamicDataService"))
            {
                var loginRequest = new DynamicDataServiceRqst
                {
                    TypeOfService = ServiceType.Unknown,
                    RequestType = ServiceRequestType.Login,
                    Credentials = new Credentials
                    {
                        UserName = userName,
                        Password = password,
                    },
                };

                var loginResponse = cli.ServiceRequest(loginRequest);

                if (loginResponse.Errors != null && loginResponse.Errors.Any())
                {
                    foreach (var error in loginResponse.Errors)
                    {
                        Console.Error.WriteLine("Login Failed.");
                        Console.Out.WriteLine(error.Message);
                    }
                    return null;
                }

                return loginResponse.SessionID;
            }
        }
    }
}
