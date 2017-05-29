using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket
{
    internal partial class TiketWrapper
    {
        internal override SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            return Client.SearchFlight(conditions);
        }

        private partial class TiketClientHandler
        {
            internal SearchFlightResult SearchFlight(SearchFlightConditions conditions)
            {
                GetToken();
                var result = new SearchResponse();
                result = DoSearchFlight(conditions);
                if (CheckUpdate(conditions))
                {
                    result = DoSearchFlight(conditions);
                }
                if(result == null)
                    return new SearchFlightResult();

                //TODO Maping data hasil search ke Search Flght Result
                return  new SearchFlightResult();
            }

            private static SearchResponse DoSearchFlight(SearchFlightConditions conditions)
            {
                var client = CreateTiketClient();
                var url = "/search/flight?d=CGK&a=DPS&date=20179-0-25&ret_date=2017-09-30&adult=1&child=0&infant=0&token="+ token +"&v=3&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var responseSearch = JsonExtension.Deserialize<SearchResponse>(response.Content);
                return responseSearch;
            }

            private static bool CheckUpdate(SearchFlightConditions conditions)
            {
                var client = CreateTiketClient();
                var url = "https://api-sandbox.tiket.com/ajax/mCheckFlightUpdated?token="+token+"&d=CGK&a=DPS&date=2014-05-30&adult=1&child=0&infant=0&time=134078435&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var responseSearch = JsonExtension.Deserialize<UpdateSearchResponse>(response.Content);
                if (responseSearch == null || responseSearch.Update <= 0)
                    return false;
                return true;
            }
        }

    }
}
