using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model;
using Lunggo.ApCommon.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Context;
using Lunggo.Framework.Extension;
using RestSharp;
using Price = Lunggo.ApCommon.Product.Model.Price;
using Supplier = Lunggo.ApCommon.Hotel.Constant.Supplier;

namespace Lunggo.ApCommon.Hotel.Wrapper.Tiket
{
    public class TiketSearchHotel
    {
        public SearchHotelResult SearchHotel(SearchHotelCondition condition)
        {
            int totalPage = 0;
            var result = new SearchHotelResult();
            var hotelList = new List<HotelDetail>();
            //Do Search for 1st time
            var hotels = DoSearchHotel(condition, 1, out totalPage);
            if (hotels != null && hotels.Count != 0)
            {
                hotelList.AddRange(hotels);
                if (totalPage > 1)
                {
                    for (int i = 2; i <= totalPage; i++)
                    {
                        var response = DoSearchHotel(condition, i, out totalPage);
                        if (response != null && response.Count != 0)
                        {
                            hotelList.AddRange(response);
                        }
                    }
                }
            }
            result.HotelDetails = hotelList;
            result.CheckIn = condition.CheckIn;
            result.CheckOut = condition.Checkout;
            result.DestinationName = condition.Destination;
            return result;
        }



        public List<HotelDetail> DoSearchHotel(SearchHotelCondition condition, int page, out int totalPage)
        {
            totalPage = 0;
            var tiketClient = new TiketClientHandler();
            var client = tiketClient.CreateTiketClient();
            var token = tiketClient.GetToken();
            var url = "/search/hotel";
            var adultCount = condition.Occupancies.Sum(x => x.AdultCount);
            var childCount = condition.Occupancies.Sum(x => x.ChildCount);
            var roomCount = condition.Occupancies.Sum(x => x.RoomCount);
            var request = new RestRequest(url, Method.GET);
            request.AddQueryParameter("token", token);
            request.AddQueryParameter("q", condition.Location);
            request.AddQueryParameter("startdate", condition.CheckIn.ToString("yyyy-MM-dd"));
            request.AddQueryParameter("night", condition.Nights.ToString());
            request.AddQueryParameter("enddate", condition.Checkout.ToString("yyyy-MM-dd"));
            request.AddQueryParameter("room", roomCount.ToString());
            request.AddQueryParameter("offset", "50");
            request.AddQueryParameter("page", page.ToString());
            request.AddQueryParameter("adult", adultCount.ToString());
            request.AddQueryParameter("child", childCount.ToString());
            request.AddQueryParameter("output", "json");
            var response = client.Execute(request);
            var searchResponse = JsonExtension.Deserialize<HotelSearchResponse>(response.Content);
            if (searchResponse == null && searchResponse.Diagnostic.Status != "200" && searchResponse.Results == null && searchResponse.Results.Result.Count == 0)
                return null;
            List<HotelDetail> hotels = new List<HotelDetail>();

            totalPage = searchResponse.Pagination.LastPage;
            //var hotels = new HotelDetail();
            var lang = OnlineContext.GetActiveLanguageCode();
            var allCurrencies = HotelService.GetInstance().GetAllCurrenciesFromCache(condition.SearchId);
            // Mapping data response into search result
            foreach (var data in searchResponse.Results.Result)
            {
                var hotel = new HotelDetail
                {
                    SearchId = condition.SearchId,
                    Supplier = Supplier.Tiket,
                    Latitude = data.Latitude == null ? null : (decimal?)decimal.Parse(data.Latitude),
                    Longitude = data.Longitude == null ? null : (decimal?)decimal.Parse(data.Longitude),
                    HotelName = data.Name,
                    Id = data.Id,
                    HotelCode = data.hotel_id,
                    Address = data.address,
                    DestinationName = data.Province,
                    ZoneCode = (string.IsNullOrEmpty(data.Kecamatan) || data.Kecamatan == " ") ? condition.Location : data.Kecamatan,
                    AreaCode = data.Kelurahan,
                    PhotoPrimary = data.PhotoPrimary,
                    StarRating = data.Rating,
                    CheckInDate = condition.CheckIn,
                    CheckOutDate = condition.Checkout,
                    OriginalCheapestFare = data.Price,
                    NetCheapestFare = data.Price,
                    OriginalCheapestTotalFare = data.total_price,
                    NetCheapestTotalFare = data.total_price,
                    NightCount = condition.Nights,
                    TotalAdult = adultCount,
                    TotalChildren = childCount,
                    WifiAccess = !string.IsNullOrEmpty(data.Wifi),
                    StarCode = string.IsNullOrEmpty(data.StarRating) ? 0 : int.Parse(data.StarRating),
                    HotelUri = data.BusinessUri,
                    Rooms = new List<HotelRoom>()
                };
                var room = new HotelRoom
                {
                    Rates = new List<HotelRate>()
                };
                room.Rates.Add(new HotelRate
                {
                   AdultCount = adultCount,
                   ChildCount = childCount,
                   NightCount = condition.Nights,
                   RateCount = 1,
                   RateKey = ConstructRateKey(condition),
                    Price = new Price()
                });
                room.Rates[0].Price.SetSupplier(data.Price,
                                    searchResponse.Diagnostic.Currency != null
                                        ? allCurrencies[searchResponse.Diagnostic.Currency]
                                        : ConfigManager.GetInstance().GetConfigValue("general", "environment") == "production"
                                            ? allCurrencies["IDR"]
                                            : data.Price < 10000
                                                ? allCurrencies["USD"]
                                                : allCurrencies["IDR"]);

                hotel.Rooms.Add(room);
                hotels.Add(hotel);
            }
            return hotels;
        }

        public string ConstructRateKey(SearchHotelCondition condition)
        {
            string rateKey = condition.CheckIn.ToString("yyyy") + "" + condition.CheckIn.ToString("MM") + "" + condition.CheckIn.ToString("dd") + "|" 
                             + condition.Checkout.ToString("yyyy") + "" + condition.Checkout.ToString("MM") + "" + condition.Checkout.ToString("dd") + "|" 
                             + "W|1|6914|DUS.ST|CG-TODOS|BB||" + condition.Nights + "~" + condition.AdultCount + "~" + condition.ChildCount + "||N@D4EF2AC67207410FB2DB7DAEB33FCA13";
            return rateKey;
        }


        internal SearchHotelResult SearchHotelByArea(SearchHotelCondition condition)
        {
            var tiketClient = new TiketClientHandler();
            var client = tiketClient.CreateTiketClient();
            var token = tiketClient.GetToken();
            var url = "/search/search_area";
            var request = new RestRequest(url, Method.GET);
            request.AddQueryParameter("token", token);
            request.AddQueryParameter("uid", "city:178");
            request.AddQueryParameter("output", "json");
            var response = client.Execute(request);
            var searchResponse = JsonExtension.Deserialize<TiketHotelBaseResponse>(response.Content);
            if (searchResponse == null && searchResponse.Diagnostic.Status != "200")
                return null;

            return new SearchHotelResult();
        }
    }
}
