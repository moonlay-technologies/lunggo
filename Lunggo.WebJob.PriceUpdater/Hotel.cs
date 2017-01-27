using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Newtonsoft.Json;
using RestSharp;

namespace Lunggo.WebJob.PriceUpdater
{
    partial class Program
    {
        public static void HotelPriceUpdater(long locationCd, DateTime checkinDate)
        {
            Console.WriteLine(@"Price updater for " + locationCd);
            const string clientId = "V2toa2VrOXFSWFZOUXpSM1QycEZlRTlIVlhwYWFrVjVUVVJrYlZsVVp6Vk5WRlp0VGtSR2FrOUhSWGhhYWsweFRucGpNRTE2U1RCT2VtTjNXbTFKZDFwcVFUMD0=";
            const string clientSecret = "V2tkS2FFOUVhek5QUjFsNFRucFpNVmt5UlRST2JVWnNXVmRKTTA1dFVtaFBSMDVyV1dwQk5WcEhTWGxPZWtwcVRVUkpNVTFCUFQwPQ==";
            var apiUrl = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
            var loginClient = new RestClient(apiUrl);
            var loginRequest = new RestRequest("/v1/login", Method.POST) { RequestFormat = DataFormat.Json };
            loginRequest.AddBody(new { clientId, clientSecret });
            Console.WriteLine(@"Start Login");
            var loginResponse = loginClient.Execute(loginRequest).Content.Deserialize<LoginFormat>();
            Console.WriteLine(@"login succeeded");
            Console.WriteLine(@"Login status = " + loginResponse.Status);
            try
            {
                if (loginResponse.Status == "200")
                {
                    Console.WriteLine(@"Succeeded Login!");
                    var searchRequest = new RestRequest("/v1/hotel/search", Method.POST) { RequestFormat = DataFormat.Json };
                    searchRequest.AddHeader("Authorization", "Bearer " + loginResponse.AccessToken);

                    const int nights = 1;
                    var checkoutDate = checkinDate.AddDays(nights);
                    const int adultCount = 2;
                    const int roomCount = 1;
                    const int childCount = 0;
                    const int page = 1;
                    const int pageCount = 1;
                    const int perPage = 20;
                    const string hotelSorting = "ASCENDINGPRICE";
                    var childrenAges = new List<int>() { 0, 0, 0, 0 };
                    const int nightCount = 1;
                    var occupancies = new List<Occupancy>
                    {
                        new Occupancy
                        {
                            AdultCount = adultCount,
                            ChildCount = childCount,
                            RoomCount = roomCount,
                            ChildrenAges = childrenAges
                        }
                    };

                    HotelService.GetInstance().Search(new SearchHotelInput
                    {
                        Occupancies = occupancies,
                        CheckIn = checkinDate,
                        Checkout = checkoutDate,
                        Page = page,
                        PerPage = perPage,
                        SearchHotelType = SearchHotelType.Location,
                        Nights = nightCount,
                        SortingParam = hotelSorting,
                        Location = locationCd
                    });

                    var searchResponse = loginClient.Execute(searchRequest);
                    Console.WriteLine(@"Price Calendar is Successful!");
                }
                else
                {
                    Console.WriteLine(@"Can't Login!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Price Calendar is FAILED!");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        public class LoginFormat
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }
            [JsonProperty("expTime")]
            public string ExpiryTime { get; set; }
            [JsonProperty("refreshToken")]
            public string RefreshToken { get; set; }
            [JsonProperty("status")]
            public string Status { get; set; }
            [JsonProperty("error")]
            public string Error { get; set; }
        }

    }
}
