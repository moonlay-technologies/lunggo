using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Extension;
using Lunggo.WebAPI.ApiSrc.Flight.Model;
using RestSharp;

namespace Lunggo.WebJob.BookingAutomation
{
    public partial class Program
    {
        public static List<FlightItineraryForDisplay> SearchFlight(string searchId)
        {
            var trial = 0;
            int progress = 0;
            var flightList = new List<FlightItineraryForDisplay>();
            while (progress < 100 && trial < 3)
            {
                var searchUrl = "v1/flight/"+ searchId +"/"+progress + "";
                var request = new RestRequest(searchUrl, Method.GET);
                request.AddHeader("Authorization", "Bearer " + _accessToken);
                var searchResponse =_client.Execute(request);
                if (searchResponse.StatusCode == HttpStatusCode.OK)
                {
                    var responseData = JsonExtension.Deserialize<FlightSearchApiResponse>(searchResponse.Content);
                    if (responseData != null && responseData.StatusCode == HttpStatusCode.OK)
                    {
                        if (responseData.Flights != null)
                        {
                            if (responseData.Flights != null)
                            {
                                foreach (var item in responseData.Flights)
                                {
                                    if (item.Itineraries != null)
                                    {
                                        flightList.AddRange(item.Itineraries);
                                    }
                                }
                            }

                            progress = responseData.Progress.GetValueOrDefault();
                        }
                    }
                    if (searchResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        //DO Login, Update _accessToken
                        //Trial dinaikin
                        GetAuthAccess();
                        trial++;
                        if (trial >= 3)
                        {
                            progress = 100;
                        }
                    }
                }
                else
                {
                    GetAuthAccess();
                    trial++;
                    if (trial >= 3)
                    {
                        progress = 100;
                    }
                }
            }
            return flightList;
        }
    }
}
