using System;
using System.Linq;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.WebJob.PriceUpdater
{
    partial class Program
    {
        static void Main()
        {
            Init();
            var todaydate = DateTime.UtcNow;
            //var todaydate = new DateTime(2017, 6, 1, 0, 0, 0, DateTimeKind.Utc);
            var endofmonth = DateTime.DaysInMonth(todaydate.Year, todaydate.Month);
            var apiUrl = EnvVariables.Get("api", "apiUrl");
            Console.WriteLine(apiUrl);
            var loginClient = new RestClient(apiUrl);
            var difference = endofmonth - todaydate.Day;
            var baliCode = HotelService.AutoCompletes.First(c => c.Code == "BAI").Id;
            Console.WriteLine("id for Bali is " + baliCode);
            var jktCode = HotelService.AutoCompletes.First(c => c.Code == "JAV").Id;
            Console.WriteLine("id for Jakarta is " + jktCode);
            var bdoCd = HotelService.AutoCompletes.First(c => c.Code == "BDO").Id;
            Console.WriteLine("id for Bandung is " + bdoCd);
            var jogCd = HotelService.AutoCompletes.First(c => c.Code == "JOG").Id;
            Console.WriteLine("id for Jogja is " + jogCd);
            var subCd = HotelService.AutoCompletes.First(c => c.Code == "SUB").Id;
            Console.WriteLine("id for Surabaya is " + subCd);
            var loginResponse = "";
            var accessToken = "";
            while (loginResponse != "200")
            {
                var loginResult = Login(loginClient);
                accessToken = loginResult.AccessToken;
                loginResponse = loginResult.Status;
            }

            Console.WriteLine(@"Succeeded Login!");
            for (var i = 0; i <= difference; i++)
            {
                //FlightPriceUpdater("JKT", "DPS", todaydate.AddDays(i), accessToken, loginClient);
                //FlightPriceUpdater("JKT", "KNO", todaydate.AddDays(i), accessToken, loginClient);
                FlightPriceUpdater("JKT", "JOG", todaydate.AddDays(i), accessToken, loginClient);
                //FlightPriceUpdater("JKT", "SUB", todaydate.AddDays(i), accessToken, loginClient);
                //FlightPriceUpdater("DPS", "JKT", todaydate.AddDays(i), accessToken, loginClient);
                //HotelPriceUpdater("Bali", baliCode, todaydate.AddDays(i), accessToken, loginClient);
                //HotelPriceUpdater("Jakarta", jktCode, todaydate.AddDays(i), accessToken, loginClient);
                //HotelPriceUpdater("Bandung", bdoCd, todaydate.AddDays(i), accessToken, loginClient);
                //HotelPriceUpdater("Yogyakarta", jogCd, todaydate.AddDays(i), accessToken, loginClient);
                //HotelPriceUpdater("Surabaya", subCd, todaydate.AddDays(i), accessToken, loginClient);
            }
        }

        static LoginFormat Login(RestClient loginClient)
        {
            const string clientId = "V2toa2VrOXFSWFZOUXpSM1QycEZlRTlIVlhwYWFrVjVUVVJrYlZsVVp6Vk5WRlp0VGtSR2FrOUhSWGhhYWsweFRucGpNRTE2U1RCT2VtTjNXbTFKZDFwcVFUMD0=";
            const string clientSecret = "V2tkS2FFOUVhek5QUjFsNFRucFpNVmt5UlRST2JVWnNXVmRKTTA1dFVtaFBSMDVyV1dwQk5WcEhTWGxPZWtwcVRVUkpNVTFCUFQwPQ==";
            var loginRequest = new RestRequest("/v1/login", Method.POST) { RequestFormat = DataFormat.Json };
            loginRequest.AddBody(new { clientId, clientSecret });
            Console.WriteLine(@"Start Login");
            var loginResponse = loginClient.Execute(loginRequest).Content.Deserialize<LoginFormat>();
            Console.WriteLine(@"login succeeded");
            Console.WriteLine(@"Login status = " + loginResponse.Status);
            return loginResponse;
        }
    }
}
