using System;
using Lunggo.Framework.Config;
using Lunggo.Framework.Http.Rest;

//using RestSharp;

namespace Lunggo.Webjob.PriceCalendarUpdater
{
    partial class Program
    {
        public static void HotelPriceUpdater()
        {
            Console.WriteLine(@"Starting Price Calendar...");
            const string clientId = "V2toa2VrOXFSWFZOUXpSM1QycEZlRTlIVlhwYWFrVjVUVVJrYlZsVVp6Vk5WRlp0VGtSR2FrOUhSWGhhYWsweFRucGpNRTE2U1RCT2VtTjNXbTFKZDFwcVFUMD0=";
            const string clientSecret = "V2tkS2FFOUVhek5QUjFsNFRucFpNVmt5UlRST2JVWnNXVmRKTTA1dFVtaFBSMDVyV1dwQk5WcEhTWGxPZWtwcVRVUkpNVTFCUFQwPQ==";
            var apiUrl = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
            var loginClient = new RestClient(apiUrl);
            var loginRequest = new RestRequest("/v1/login", Method.POST) { RequestFormat = DataFormat.Json };
            loginRequest.AddBody(new { clientId, clientSecret });
            Console.WriteLine(@"Start Login");
        }

    }
}
