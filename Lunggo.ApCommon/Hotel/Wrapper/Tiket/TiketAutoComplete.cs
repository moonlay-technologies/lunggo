using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model;
using Lunggo.ApCommon.Model;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Hotel.Wrapper.Tiket
{
    public partial class TiketHotelWrapper
    {
        public List<AutocompleResult> SearchAutoComplete(string data)
        {
            var tiketHandler = new TiketClientHandler();
            var client = tiketHandler.CreateTiketClient();
            var token = tiketHandler.GetToken();
            var url = "/search/autocomplete/hotel";
            var request = new RestRequest(url, Method.GET);
            request.AddQueryParameter("token", token);
            request.AddQueryParameter("q", data);
            request.AddQueryParameter("output", "json");
            var response = client.Execute(request);
            var searchResponse = JsonExtension.Deserialize<TiketAutoCompleteResponse>(response.Content);
            if (searchResponse == null && searchResponse.Diagnostic.Status != "200")
                return null;
            if(searchResponse.Results == null && searchResponse.Results.ResultList == null && searchResponse.Results.ResultList.Count == 0)
            return null;

            return searchResponse.Results.ResultList;
        }

    }
}
