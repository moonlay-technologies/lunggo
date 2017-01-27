using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Newtonsoft.Json;
using RestSharp;

namespace Lunggo.WebJob.PriceUpdater
{
    partial class Program
    {
        public static void HotelPriceUpdater(string destination, long location, DateTime checkinDate, string accesstoken, RestClient loginClient)
        {
            try
            {
                Console.WriteLine(@"Hotel Price Updater for " + destination + " " + checkinDate.ToString("ddMMyy"));
                var searchRequest = new RestRequest("/v1/hotel/search", Method.POST) { RequestFormat = DataFormat.Json };
                searchRequest.AddHeader("Authorization", "Bearer " + accesstoken);
                const int nights = 1;
                var checkoutDate = checkinDate.AddDays(nights);
                const int adultCount = 2;
                const int roomCount = 1;
                const int childCount = 0;
                const int page = 1;
                const string searchHotelType = "Location";
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

                searchRequest.AddBody(new
                {
                    occupancies,
                    checkinDate,
                    checkoutDate,
                    page,
                    perPage,
                    searchHotelType,
                    nightCount,
                    hotelSorting,
                    location
                });
                loginClient.Execute(searchRequest);
                Console.WriteLine(@"Price Calendar is Successful!");
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
