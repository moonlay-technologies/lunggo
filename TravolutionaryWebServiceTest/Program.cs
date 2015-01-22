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
            /*string session = Login(userName, password);

            if (session == null)
            {
                Console.WriteLine("session id null");
                return;
            }
            else
            {
                Console.WriteLine("session id adalah " + session);
            }
            */

            using (var cli = new DynamicDataServiceClient("BasicHttpBinding_IDynamicDataService"))
            {

                /*var searchRequest = new HotelsServiceSearchRequest()
                {
                    CheckIn = DateTime.Now.AddDays(5),
                    CheckOut = DateTime.Now.AddDays(7),
                    Nights = 2,
                    DesiredResultCurrency = "USD",
                    DetailLevel = SearchDetailLevel.Minimal, //THIS IS A VERY IMPORTANG PARAMETER
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
                       
                };*/

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
                                       AdultsCount = 4,
                                   },
                                   new HotelRoomRequest()
                                   {
                                       AdultsCount = 4,
                                   },
                                   new HotelRoomRequest()
                                   {
                                       AdultsCount = 4,
                                   },
                                   new HotelRoomRequest()
                                   {
                                       AdultsCount = 4,
                                   }
                           },
                    HotelIds = new int[]
                    {
                        4116550
                    }
                    //HotelLocation = 640255 //Prague
                };


                var newRequest = new SeedHotelsSearchRequest
                {
                    CheckIn = DateTime.Now.AddDays(1),
                    CheckOut = DateTime.Now.AddDays(2),
                    Location = 640255,
                    Nights = 1 ,
                    Residency = "ID",
                    ResultCurrency = "USD",
                    SearchDetail = HotelSearchDetailLevel.Meta
                };




                var stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
               
                var searchResponse = cli.ServiceRequest(new DynamicDataServiceRqst()
                {
                    //SessionID = session,
                    TypeOfService = ServiceType.Hotels,
                    RequestType = ServiceRequestType.Search,
                    Request = searchRequest,
                    Credentials = new Credentials
                    {
                        UserName = userName,
                        Password = password
                    }
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

                Console.WriteLine("Ditemukan " + searchResponse.HotelsSearchResponse.Result.Length + " hotel");
                stopwatch.Stop();
                Console.WriteLine("Pencarian membutuhkan waktu " + stopwatch.ElapsedMilliseconds / 1000 + " detik");

                foreach (var hotel in searchResponse.HotelsSearchResponse.Result)
                {
                    //Console.WriteLine("{0} | {1} | {2} | {3} | {4}", hotel.DisplayName, hotel.Area, hotel.District, hotel.StarRating, hotel.Packages.Min(p => p.SimplePrice));
                    Console.Out.WriteLine("Hotel ->{0,-50},LowestPrice ->{1,10} USD, HotelId ->{2,70}", hotel.DisplayName, hotel.Packages.Min(p => p.SimplePrice), hotel.ID);
                }



                var hotelTable = searchResponse.HotelsSearchResponse.Result;

                if (!hotelTable.Any())
                {
                    Console.WriteLine("ng ada isinya");
                }

                var packageTable = hotelTable[0].Packages;
                var firstPackage = packageTable[0];
                Console.WriteLine();
                foreach (var package in packageTable)
                {
                    var packagePrice = package.PackagePrice;
                    Console.WriteLine("{0} {1} {2} {3} {4}",package.PackageId,packagePrice.Currency, packagePrice.FinalPrice, 
                        packagePrice.FinalPriceInSupplierCurrency, packagePrice.OriginalPrice);
                }
                Console.WriteLine();
                Console.WriteLine();
                

                /*var firstHotel = hotelTable[5];

                Console.WriteLine("Test null {0}",null);

                Console.WriteLine("address : {0}",firstHotel.Address ?? "rama null value");
                Console.WriteLine("area : {0}", firstHotel.Area ?? "rama null value");
                //Console.WriteLine("city tax max & min & total fee : {0} {1} {2}", firstHotel.CityTax.MaxFee,firstHotel.CityTax.MinFee, firstHotel.CityTax.TotalFee);
                Console.WriteLine("defaultimage full size & pic description & thumb : {0} {1} {2}", firstHotel.DefaultImage.FullSize ?? "rama null value", firstHotel.DefaultImage.PicDescription ?? "rama null value", firstHotel.DefaultImage.Thumb ?? "rama null value");
                Console.WriteLine("display name : {0}", firstHotel.DisplayName ?? "rama null value");
                Console.WriteLine("district : {0}", firstHotel.District ?? "rama null value");
                Console.WriteLine("latitude & longitude : {0} {1}", firstHotel.GeoLocation.Latitude,firstHotel.GeoLocation.Longitude);
                Console.WriteLine("highest package price : {0}", firstHotel.HighestPackagePrice);
                Console.WriteLine("hotel id : {0}", firstHotel.ID);
                Console.WriteLine("lowest package price : {0}", firstHotel.LowestPackagePrice);
                //Console.WriteLine("packages tostring : {0}", firstHotel.Packages.ToString());
                Console.WriteLine("star rating : {0}", firstHotel.StarRating ?? "rama null value");
                Console.WriteLine("supplier lowest packageprice : {0}", firstHotel.SuppliersLowestPackagePrices);
                Console.Write("Isi dictionary : ");
                if (firstHotel.SuppliersLowestPackagePrices != null)
                {
                    foreach (var temp in firstHotel.SuppliersLowestPackagePrices)
                    {
                        Console.WriteLine("Key : {0}, Value : {1}", temp.Key, temp.Value);
                    }
                }
                else
                {
                    Console.WriteLine("dictionary price null");
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
