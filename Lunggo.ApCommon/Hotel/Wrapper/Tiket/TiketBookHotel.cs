using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Hotel.Wrapper.Tiket
{


    public class TiketBookHotel
    {

        public RevalidateHotelResult BookHotel(HotelBookInfo input)
        {
            var bookResult = new RevalidateHotelResult();

            //Step 1 : Add Order
            var addOrderResponse = AddOrder(input);
            if (addOrderResponse == null)
                return new RevalidateHotelResult
                {
                    IsValid = false,
                    IsPriceChanged = false,

                };

            //Step 2 : Order
            var orderResult = Order(input.Token);
            if (orderResult == null)
                return new RevalidateHotelResult
                {
                    IsValid = false,
                    IsPriceChanged = false
                };

            bookResult = new RevalidateHotelResult
            {
                IsValid = true,
                IsPriceChanged = false,
                OrderId = orderResult.MyOrder.OrderId
            };


            //Step 3 : Checkout Request
            var checkoutResponse = CheckOutPage(orderResult.MyOrder.OrderId, input.Token);
            if (checkoutResponse == null)
                return new RevalidateHotelResult
                {
                    IsValid = false,
                    IsPriceChanged = false,
                };

            //Step 4 : Checkout Page Customer
            var checkoutCustResponse = CheckoutCustomer(input.Token);
            if (checkoutCustResponse == null)
                return new RevalidateHotelResult
                {
                    IsValid = false,
                    IsPriceChanged = false,

                };

            return bookResult;
        }

        public TiketHotelBaseResponse AddOrder(HotelBookInfo input)
        {
            var tiketClient = new TiketClientHandler();
            //var client = new RestClient(input.BookUri);
            //client.CookieContainer = new CookieContainer();
            var url = input.BookUri.Split(new string[] { ".com" }, StringSplitOptions.None)[1];
            var client = tiketClient.CreateTiketClient();
            //var url = "/order/add/hotel";
            var request = new RestRequest(url, Method.GET);
            //request.AddQueryParameter("startdate", input.CheckIn.ToString("yyyy-MM-dd"));
            //request.AddQueryParameter("enddate", input.Checkout.ToString("yyyy-MM-dd"));
            //request.AddQueryParameter("night", input.Nights.ToString());
            //request.AddQueryParameter("room", input.Rooms.ToString());
            //request.AddQueryParameter("adult", input.AdultCount.ToString());
            //request.AddQueryParameter("child", input.ChildCount.ToString());
            ////request.AddQueryParameter("hotelname", input.HotelName);
            //request.AddQueryParameter("room_id", input.RoomId);
            request.AddQueryParameter("token", input.Token);
            request.AddQueryParameter("output", "json");
            var response = client.Execute(request);
            var addOderResponse = JsonExtension.Deserialize<TiketHotelBaseResponse>(response.Content);
            if (addOderResponse == null && addOderResponse.Diagnostic.Status != "200")
                return null;
            if (addOderResponse.Diagnostic != null && addOderResponse.Diagnostic.ErrorMessage != null)
                return null;
            return addOderResponse;
        }

        public TiketHotelOrderResponse Order(string token)
        {
            var tiketClient = new TiketClientHandler();
            var client = tiketClient.CreateTiketClient();
            var url = "/order?token=" + token + "&output=json";
            var request = new RestRequest(url, Method.GET);
            var response = client.Execute(request);
            var orderResponse = JsonExtension.Deserialize<TiketHotelOrderResponse>(response.Content);
            if (orderResponse == null && orderResponse.Diagnostic.Status != "200")
                return null;
            if (orderResponse.Diagnostic != null && orderResponse.Diagnostic.ErrorMessage != null)
                return null;
            return orderResponse;
        }

        public TiketHotelBaseResponse CheckOutPage(string orderId, string token)
        {
            var tiketClient = new TiketClientHandler();
            var client = tiketClient.CreateTiketClient();
            var url = "order/checkout/" + orderId + "/IDR?token=" + token + "&output=json";
            var request = new RestRequest(url, Method.GET);
            var response = client.Execute(request);
            var checkoutResponse = JsonExtension.Deserialize<TiketHotelBaseResponse>(response.Content);
            if (checkoutResponse == null && checkoutResponse.Diagnostic.Status != "200")
                return null;
            if (checkoutResponse.Diagnostic != null && checkoutResponse.Diagnostic.ErrorMessage != null)
                return null;
            return checkoutResponse;
        }

        public TiketHotelBaseResponse CheckoutCustomer(string token)
        {
            var tiketClient = new TiketClientHandler();
            var client = tiketClient.CreateTiketClient();
            var url = "/checkout/checkout_customer";
            var request = new RestRequest(url, Method.GET);
            request.AddQueryParameter("token", token);
            request.AddQueryParameter("salutation", "Mr");
            request.AddQueryParameter("firstName", "Suheri");
            request.AddQueryParameter("lastName", "Marpaung");
            request.AddQueryParameter("emailAddress", "suheri@travelmadezy.com");
            request.AddQueryParameter("phone", "%2B85360343300");
            request.AddQueryParameter("saveContinue", "2");
            request.AddQueryParameter("output", "json");
            var response = client.Execute(request);
            var responseCust = JsonExtension.Deserialize<TiketHotelBaseResponse>(response.Content);
            if (responseCust == null && responseCust.Diagnostic.Status != "200")
                return null;
            if (responseCust.Diagnostic != null && responseCust.Diagnostic.ErrorMessage != null)
                return null;
            return responseCust;
        }

    }
}
