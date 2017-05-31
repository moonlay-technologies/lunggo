using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket
{
    internal partial class TiketWrapper
    {
        private partial class TiketClientHandler
        {
            internal void GetFlightData(string flightId, DateTime date)
            {
                var client = CreateTiketClient();
                var url = "/flight_api/get_flight_data?flight_id="+flightId+"&token="+token+"&date="+date.ToString("yyyy-MM-dd")+"&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var flightData = JsonExtension.Deserialize<GetFlightDataResponse>(response.Content);
                var temp = flightData;
                Console.WriteLine("Fisnihed Get Flight Data");
                //Operate Data which useful for Order Data
            }
        }
    }

}
