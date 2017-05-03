using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using RestSharp;

namespace Lunggo.WebJob.BookingAutomation
{
    public partial class Program
    {
        public static string SelectFlight(string searchId, int regId)
        {
            var trial = 0;
            bool success = false;
            string token = null;
            var regIdList = new List<int>();
            regIdList.Add(regId);
            if(searchId == null) return null;
            var selectRequest = new FlightSelectApiRequest
            {
                SearchId = searchId,
                RegisterNumbers = regIdList,
                EnableCombo = false
            };

            while (!success && trial < 3)
            {
                var request = new RestRequest("v1/flight/select", Method.POST);
                request.AddHeader("Authorization", "Bearer " + _accessToken);
                var requestJson = JsonExtension.Serialize(selectRequest);
                request.AddParameter("text/json", requestJson, ParameterType.RequestBody); 
                //request.AddJsonBody(selectRequest);
                var selectResponse = _client.Execute(request);
                var responseData = JsonExtension.Deserialize<FlightSelectApiResponse>(selectResponse.Content);
                if (selectResponse.StatusCode == HttpStatusCode.OK)
                {
                    if (responseData != null && responseData.StatusCode == HttpStatusCode.OK)
                    {
                        token = responseData.Token;
                        success = true;
                    }
                    if (selectResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        GetAuthAccess();
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
            return token;
        }
    }
}
