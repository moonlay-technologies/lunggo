using System;
using RestSharp;

namespace Lunggo.WebJob.PriceUpdater
{
    partial class Program
    {
        public static void FlightPriceUpdater(string origin, string destination, 
            DateTime departureDate, string accesstoken, RestClient loginClient)
        {
            Console.WriteLine(@"Price updater for flight route " + origin + destination + " " + departureDate.ToString("ddMMyy"));
            try
            {
                var searchId = origin + destination + departureDate.ToString("ddMMyy") + "-100y";
                var searchRequest = new RestRequest("/v1/flight/" + searchId + "/0", Method.GET);
                searchRequest.AddHeader("Authorization", "Bearer " + accesstoken);
                loginClient.Execute(searchRequest);
                Console.WriteLine(@"Price is successfully updated!");             
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Price Calendar is FAILED!");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
