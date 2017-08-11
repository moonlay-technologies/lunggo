using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model;
using Lunggo.ApCommon.Product.Constant;
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
                OrderId = orderResult.MyOrder.OrderId,
                OrderDetailId = orderResult.MyOrder.Data.First().OrderDetailId
            };


            //Step 3 : Checkout Request
            var checkoutResponse = CheckOutPage(orderResult.MyOrder.OrderId, input.Token);
            if (checkoutResponse == null)
                return new RevalidateHotelResult
                {
                    IsValid = false,
                    IsPriceChanged = false,
                };

            //Step 4 : Checkout Login
            var checkoutLogResponse = CheckoutLogin(input.Token);
            if (checkoutLogResponse == null)
                return new RevalidateHotelResult
                {
                    IsValid = false,
                    IsPriceChanged = false,

                };

            //Step 5 : Checkout Page Customer
            var checkoutCustResponse = CheckoutCustomer(input, orderResult.MyOrder.Data[0].OrderDetailId);
            if (checkoutLogResponse == null)
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
            var url = input.BookUri.Split(new string[] { ".com" }, StringSplitOptions.None)[1];
            var client = tiketClient.CreateTiketClient();
            var request = new RestRequest(url, Method.GET);
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

        public TiketHotelBaseResponse CheckoutLogin(string token)
        {
            var tiketClient = new TiketClientHandler();
            var client = tiketClient.CreateTiketClient();
            var url = "/checkout/checkout_customer";
            var request = new RestRequest(url, Method.GET);
            request.AddQueryParameter("token", token);
            request.AddQueryParameter("salutation", "Mrs");
            request.AddQueryParameter("firstName", "Dwi");
            request.AddQueryParameter("lastName", "Agustina");
            request.AddQueryParameter("emailAddress", "suheri@travelmadezy.com"); // Change this
            request.AddQueryParameter("phone", "%2B811351793"); //change this
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

        public TiketHotelBaseResponse CheckoutCustomer(HotelBookInfo input, string detailOrderId)
        {
            var tiketClient = new TiketClientHandler();
            var client = tiketClient.CreateTiketClient();
            var url = "/checkout/checkout_customer";
            var firstPax = input.Passengers.FirstOrDefault();
            if (firstPax == null)
                return null;
            var request = new RestRequest(url, Method.GET);
            
            //Pax Data
            request.AddQueryParameter("salutation", TitleCd.Mnemonic(firstPax.Title));
            request.AddQueryParameter("firstName", firstPax.FirstName);
            request.AddQueryParameter("lastName", firstPax.LastName);
            request.AddQueryParameter("phone", "%2B811351793");

            //Customer Data
            request.AddQueryParameter("conSalutation", TitleCd.Mnemonic(firstPax.Title));
            var contactName = input.Contact.Name.Split(' ');
            if (contactName.Length > 2)
            {
                request.AddQueryParameter("conFirstName", contactName[0]);
                var lastName = input.Contact.Name.Replace(contactName[0], "").Trim();
                request.AddQueryParameter("conLastName", lastName);
            }
            else
            {
                request.AddQueryParameter("conFirstName", input.Contact.Name);
                request.AddQueryParameter("conLastName", input.Contact.Name);
            }
            
            request.AddQueryParameter("conEmailAddress", input.Contact.Email);
            request.AddQueryParameter("conPhone", "%2B"+ input.Contact.Phone.Substring(1));

            //Others
            request.AddQueryParameter("token", input.Token);
            request.AddQueryParameter("detailId", detailOrderId);
            request.AddQueryParameter("country", "ID");
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
