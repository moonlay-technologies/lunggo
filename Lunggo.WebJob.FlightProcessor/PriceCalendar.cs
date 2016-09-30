using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using RestSharp;

namespace Lunggo.WebJob.FlightProcessor
{
    public partial class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void FlightPriceCalendar([QueueTrigger("flightpricecalendar")] string searchId)
        {
            Console.WriteLine(@"Starting Price Calendar...");
            const string clientId = "V2toa2VrOXFSWFZOUXpSM1QycEZlRTlIVlhwYWFrVjVUVVJrYlZsVVp6Vk5WRlp0VGtSR2FrOUhSWGhhYWsweFRucGpNRTE2U1RCT2VtTjNXbTFKZDFwcVFUMD0=";
            const string clientSecret = "V2tkS2FFOUVhek5QUjFsNFRucFpNVmt5UlRST2JVWnNXVmRKTTA1dFVtaFBSMDVyV1dwQk5WcEhTWGxPZWtwcVRVUkpNVTFCUFQwPQ==";
            var apiUrl = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
            var loginClient = new RestClient(apiUrl);
            var loginRequest = new RestRequest("/v1/login", Method.POST) {RequestFormat = DataFormat.Json};
            loginRequest.AddBody(new { clientId, clientSecret });
            Console.WriteLine(@"Start Login");
            //var loginResponse = loginClient.Execute(loginRequest);
            //Console.WriteLine(@"login content: ");
            //Console.WriteLine(loginResponse.ErrorMessage);
            //Console.WriteLine(loginResponse.StatusDescription);
            var loginResponse = loginClient.Execute(loginRequest).Content.Deserialize<LoginFormat>();
            Console.WriteLine(@"login succeeded");
            Console.WriteLine(@"Login status = " + loginResponse.Status);
            try
            {
                if (loginResponse.Status == "200")
                {
                    Console.WriteLine(@"Succeeded Login!");
                    var searchRequest = new RestRequest("/v1/flight/" + searchId + "/0", Method.GET);
                    searchRequest.AddHeader("Authorization", "Bearer " + loginResponse.AccessToken);
                    var searchResponse = loginClient.Execute(searchRequest).Content.Deserialize<SearchResponseFormat>();
                    if (searchResponse.Flights != null)
                    {
                        foreach (var resp in searchResponse.Flights)
                        {
                            FlightService.GetInstance().SetLowestPriceToCache(resp.Itins);
                        }
                    }
                    FlightService.GetInstance().InvalidateSearchingStatusInCache(searchId);
                    Console.WriteLine(@"Price Calendar is Successful!");
                }
                else
                {
                    Console.WriteLine(@"Can't Login!");
                }


            }
            catch(Exception e)
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

        public class SearchResponseFormat
        {
            [JsonProperty("expTime")]
            public string ExpiryTime { get; set; }
            [JsonProperty("progress")]
            public string Progress { get; set; }
            [JsonProperty("status")]
            public string Status { get; set; }
            [JsonProperty("error")]
            public string Error { get; set; }
            [JsonProperty("flights")]
            public FlightOptions[] Flights { get; set; }
        }

        public class FlightOptions
        {
            [JsonProperty("count")]
            public int Count { get; set; }
            [JsonProperty("options")]
            public List<FlightItineraryForDisplay> Itins { get; set; }
        }
    }
}
