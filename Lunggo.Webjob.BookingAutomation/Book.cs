using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using RestSharp;

namespace Lunggo.WebJob.BookingAutomation
{
    public partial class Program
    {
        public static bool BookFlight(string token)
        {
            var trial = 0;
            bool success = false;
            if (token == null) return false;
            var paxList = new List<PaxForDisplay>();
            var paxRequest = new PaxForDisplay
            {
                Name = "Handoko Mulyani",
                DateOfBirth = DateTime.Now.AddYears(-25),
                Title = Title.Mister,
                Type = PaxType.Adult,
                PassportExpiryDate = DateTime.Now.AddYears(4),
                PassportNumber = "ADBYI21UYT",
                Nationality = "ID",
                PassportCountry = "ID"
            };
            
            paxList.Add(paxRequest);
            var bookRequest = new FlightBookApiRequest
            {
                Token = token,
                Test = true,
                Passengers = paxList,
                LanguageCode = "ID",
                Contact = new Contact
                {
                    Name = "Handoko Mulyani",
                    Title = Title.Mister,
                    Phone = "853603412",
                    Email = "test@travorama.com",
                    CountryCallingCode = "62"
                }
            };

            while (!success && trial < 3)
            {
                var request = new RestRequest("v1/flight/book", Method.POST);
                request.AddHeader("Authorization", "Bearer " + _accessToken);
                //request.AddJsonBody(bookRequest);
                var requestJson = JsonExtension.Serialize(bookRequest);
                request.AddParameter("text/json", requestJson, ParameterType.RequestBody);
                var bookResponse = _client.Execute(request);
                var responseData = JsonExtension.Deserialize<FlightBookApiResponse>(bookResponse.Content);
                //var bookResponse = _client.Execute<FlightBookApiResponse>(request);
                if (bookResponse.StatusCode == HttpStatusCode.OK)
                {
                    if (bookResponse != null && bookResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var rsvNo = responseData.RsvNo;
                        success = true;
                        trial = 3;
                    }
                    else
                    {
                        if (bookResponse != null && bookResponse.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            GetAuthAccess();
                        }

                        trial++;
                        if (trial >= 3)
                        {
                            success = true;
                        }
                    }
                }
                else
                {
                    GetAuthAccess();
                    trial++;
                    if (trial >= 3)
                    {
                        success = true;
                    }
                }
                

            }
            return success;
        }
    }
}
