using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Hotel.Wrapper.Tiket
{
    public class TiketHotelDetail
    {
        public TiketHotelDetailResponse GetHotelDetail(string hotelUri)
        {
            var tiketClient = new TiketClientHandler();
            var client = tiketClient.CreateTiketClient();
            hotelUri = hotelUri.Replace("https://api-sandbox.tiket.com","");
            var token = tiketClient.GetToken();
            var request = new RestRequest(hotelUri, Method.GET);
            request.AddQueryParameter("token", token);
            request.AddQueryParameter("output", "json");
            var response = client.Execute(request);
            var searchResponse = JsonExtension.Deserialize<TiketHotelDetailResponse>(response.Content);
            if (searchResponse == null && searchResponse.Diagnostic.Status != "200")
                return null;

            return searchResponse;   
        }

    }
}
