using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Hotel.Wrapper.Tiket
{
    public class TiketHotelDetail
    {
        public SearchHotelResult GetHotelDetail(string hotelUri, string _token)
        {
            var tiketClient = new TiketClientHandler();
            var client = tiketClient.CreateTiketClient();
            var url = "https://api-sandbox.tiket.com/hotel/indonesia/jakarta/jakarta/orchardz-jayakarta?startdate=2017-07-07&night=1&room=1&adult=1&child=0&is_partner=0&star_rating%5B0%5D=0&star_rating%5B1%5D=1&star_rating%5B2%5D=2&star_rating%5B3%5D=3&star_rating%5B4%5D=4&star_rating%5B5%5D=5&hotel_chain=0&facilities=0&latitude=0&longitude=0&distance=0&uid=business%3A19485325&token=0c30e45f921bdb00bfeef0851a08eb77e31d728a&output=json";
            var request = new RestRequest(url, Method.GET);
            var result = tiketClient.GetToken();
            var token = result;
            var response = client.Execute(request);
            var searchResponse = JsonExtension.Deserialize<TiketHotelDetailResponse>(response.Content);
            if (searchResponse == null && searchResponse.Diagnostic.Status != "200")
                return null;


            return new SearchHotelResult();   
        }

    }
}
