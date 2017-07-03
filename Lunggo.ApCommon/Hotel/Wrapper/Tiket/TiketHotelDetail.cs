using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Hotel.Wrapper.Tiket
{
    public class TiketHotelDetail
    {
        internal SearchHotelResult GetHotelDetail(string hotelUri)
        {
            var tiketClient = new TiketClientHandler();
            var client = tiketClient.CreateTiketClient();
            var token = tiketClient.GetToken();
            hotelUri = hotelUri + "&token=" + token + "&output=json";
            var request = new RestRequest(hotelUri, Method.GET);
            var response = client.Execute(request);
            var searchResponse = JsonExtension.Deserialize<TiketHotelDetailResponse>(response.Content);
            if (searchResponse == null && searchResponse.Diagnostic.Status != "200")
                return null;


            return new SearchHotelResult();   
        }

    }
}
