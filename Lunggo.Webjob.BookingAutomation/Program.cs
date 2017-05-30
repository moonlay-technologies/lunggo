using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.SharedModel;
using Lunggo.WebAPI.ApiSrc.Account.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using RestSharp;
using StackExchange.Redis;


namespace Lunggo.WebJob.BookingAutomation
{
    public partial class Program
    {

        private static string _apiUrl;
        private static string _accessToken;
        private static RestClient _client;
        private  static Dictionary<int, string> FlightSuppliers;
        public static bool isFirst;

        private static void Main(string[] args)
        {
            Init();
            Console.Write("Starting Booking Automation : ");
            var data = GetToken();
            var temp = data;
            Console.WriteLine("OK");
            //for (var index = 6; index <= 100; index++)
            //{

            //    DateTime searchDate = DateTime.Now.AddDays(index);
            //    SaveDateSearch(searchDate.Date.ToString());
            //    var searchDay = searchDate.Day < 10 ? "0" + searchDate.Day : searchDate.Day.ToString();
            //    var searchMonth = searchDate.Month < 10 ? "0" + searchDate.Month : searchDate.Month.ToString();
            //    var searchYear = searchDate.Year.ToString().Substring(searchDate.Year.ToString().Length - 2);
            //    var searchId = "CGKDPS" + searchDay + searchMonth + searchYear + "-100y";
            //    var flightList = SearchFlight(searchId);
            //    var flights = ClassifyFlightList(flightList);
            //    for (var i = 0; i < flights.Length; i++)
            //    {
            //        if (flights[i] != null && flights[i].Count != 0)
            //        {
            //            //Get One by Random
            //            Random rand = new Random();
            //            int randomFlight = rand.Next(0, flights[i].Count);
            //            var selectedRegId = flights[i].ElementAtOrDefault(randomFlight);

            //            //Do Select Here
            //            var tokenSelect = SelectFlight(searchId, selectedRegId);
            //            if (tokenSelect != null)
            //            {
            //                //Do Book
            //                Console.WriteLine("Supplier : "+ FlightSuppliers[i]);
            //                var isBookSucceed = BookFlight(tokenSelect, FlightSuppliers[i]);
            //                var test = isBookSucceed;
            //            }
            //        }
            //        else
            //        {
            //            FlightUnavailable(FlightSuppliers[i]);
            //        }
            //    }
            //}

        }

        public static RestClient CreateTiketClient()
        {
            var client = new RestClient("http://api-sandbox.tiket.com");
            client.CookieContainer = new CookieContainer();
            return client;
        }

        public  static string GetToken()
        {
            var client = CreateTiketClient();
            var url = "/apiv1/payexpress?method=getToken&secretkey=" + "31b5f2614bba57d0bb9c31e78c62fb43&output=json";
            var request = new RestRequest(url, Method.GET);
            var response = client.Execute(request);
            var responseToken = JsonExtension.Deserialize<TiketBaseResponse>(response.Content);
            if (responseToken == null)
                return null;
            return responseToken.Token;
        }
        

        public static List<int>[] ClassifyFlightList(List<FlightItineraryForDisplay> flightList)
        {
            int startMystiflyId = 333;
            int startAirAsiaId = 666;
            int startCitilinkId = 999;
            int startSriwijayaId = 1332;
           int startLionAirId = 1665;
           List<int>[] flights = new List<int>[5];
            flights[0] = new List<int>();
            flights[1] = new List<int>();
            flights[2] = new List<int>();
            flights[3] = new List<int>();
            flights[4] = new List<int>();

            foreach (var item in flightList)
            {
                //Mistifly
                if (item.RegisterNumber < startAirAsiaId)
                {
                    flights[0].Add(item.RegisterNumber.GetValueOrDefault());
                }

                //Airasia
                if (item.RegisterNumber >= startAirAsiaId && item.RegisterNumber < startCitilinkId)
                {
                    flights[1].Add(item.RegisterNumber.GetValueOrDefault());
                }

                //Citilink
                if (item.RegisterNumber >= startCitilinkId && item.RegisterNumber < startSriwijayaId)
                {
                    flights[2].Add(item.RegisterNumber.GetValueOrDefault());
                }

                //Sriwijaya
                if (item.RegisterNumber >= startSriwijayaId && item.RegisterNumber < startLionAirId)
                {
                    flights[3].Add(item.RegisterNumber.GetValueOrDefault());
                }

                //LionAir
                if (item.RegisterNumber >= startLionAirId && item.RegisterNumber < 1998)
                {
                    flights[4].Add(item.RegisterNumber.GetValueOrDefault());
                }
            }
            return flights;
        } 

        public static void GetAuthAccess()
        {   

            var request = new RestRequest("/v1/login", Method.POST);
            var loginReq = new LoginApiRequest
            {
                ClientId = "V2toa2VrOXFSWFZOUXpSM1QycEZlRTlIVlhwYWFrVjVUVVJrYlZsVVp6Vk5WRlp0VGtSR2FrOUhSWGhhYWsweFRucGpNRTE2U1RCT2VtTjNXbTFKZDFwcVFUMD0=",
                ClientSecret = "V2tkS2FFOUVhek5QUjFsNFRucFpNVmt5UlRST2JVWnNXVmRKTTA1dFVtaFBSMDVyV1dwQk5WcEhTWGxPZWtwcVRVUkpNVTFCUFQwPQ=="
            };
            request.AddJsonBody(loginReq);
            // execute the request
            var response = _client.Execute<LoginApiResponse>(request);
            if (response.Data != null)
            {
                _accessToken = response.Data.AccessToken;
            }
        }

    }
}
