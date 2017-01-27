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
            var endofmonth = DateTime.DaysInMonth(todaydate.Year, todaydate.Month);
            var apiUrl = ConfigManager.GetInstance().GetConfigValue("api", "apiUrl");
            var loginClient = new RestClient(apiUrl);
            var difference = endofmonth - todaydate.Day;
            var baliCode = HotelService.AutoCompletes.First(c => c.Code == "BAI").Id;
            var jktCode = HotelService.AutoCompletes.First(c => c.Code == "JAV").Id;
            var bdoCd = HotelService.AutoCompletes.First(c => c.Code == "BDO").Id;
            var jogCd = HotelService.AutoCompletes.First(c => c.Code == "JOG").Id;
            var subCd = HotelService.AutoCompletes.First(c => c.Code == "SUB").Id;
            var soloCd = HotelService.AutoCompletes.First(c => c.Code == "SOC").Id;
            var mdnCd = HotelService.AutoCompletes.First(c => c.Code == "MES").Id;
            var plmCd = HotelService.AutoCompletes.First(c => c.Code == "ID6").Id;
            var mlgCd = HotelService.AutoCompletes.First(c => c.Code == "MLG").Id;
            var bgrCd = HotelService.AutoCompletes.First(c => c.Code == "ID5").Id;
            var sinCd = HotelService.AutoCompletes.First(c => c.Code == "SIN").Id;
            var kulCd = HotelService.AutoCompletes.First(c => c.Code == "KUL").Id;
            var bgkCd = HotelService.AutoCompletes.First(c => c.Code == "BKK").Id;
            var hgkCd = HotelService.AutoCompletes.First(c => c.Code == "HKG").Id;

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
                FlightPriceUpdater("JKT", "JOG", todaydate.AddDays(i), accessToken, loginClient);
                FlightPriceUpdater("JKT", "KUL", todaydate.AddDays(i), accessToken, loginClient);
                FlightPriceUpdater("JKT", "SIN", todaydate.AddDays(i), accessToken, loginClient);
                FlightPriceUpdater("SUB", "BDO", todaydate.AddDays(i), accessToken, loginClient);
                FlightPriceUpdater("JKT", "HKG", todaydate.AddDays(i), accessToken, loginClient);
                FlightPriceUpdater("JKT", "SUB", todaydate.AddDays(i), accessToken, loginClient);
                FlightPriceUpdater("DPS", "JKT", todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Bali", baliCode, todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Jakarta", jktCode, todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Bandung", bdoCd, todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Solo", soloCd, todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Yogyakarta", jogCd, todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Surabaya", subCd, todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Medan", mdnCd, todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Palembang", plmCd, todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Bogor", bgrCd, todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Malang", mlgCd, todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Kuala Lumpur", kulCd, todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Singapore", sinCd, todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Bangkok", bgkCd, todaydate.AddDays(i), accessToken, loginClient);
                HotelPriceUpdater("Hong Kong", hgkCd, todaydate.AddDays(i), accessToken, loginClient);
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
