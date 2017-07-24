using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using RestSharp;


namespace Lunggo.ApCommon.Hotel.Wrapper.Tiket
{
    public class TiketIssue
    {
        private readonly string sharedKey = ConfigManager.GetInstance().GetConfigValue("tiket", "apiSecret");
        private readonly string confirmKey = ConfigManager.GetInstance().GetConfigValue("tiket", "apiConfirmKey");
        public HotelIssueTicketResult IssueHotel(HotelIssueInfo hotelIssueInfo)
        {
           return new HotelIssueTicketResult();
        }

        //Step1
        internal TiketHotelBaseResponse CheckoutPageUsingDeposit(string _token)
        {
            var tiketClient = new TiketClientHandler();
            var client = tiketClient.CreateTiketClient();
            var url = "/checkout/checkout_payment/8?btn_booking=1&token=" + _token + "&output=json";
            var request = new RestRequest(url, Method.GET);
            var response = client.Execute(request);
            var flightData = JsonExtension.Deserialize<TiketHotelBaseResponse>(response.Content);
            if (flightData == null && flightData.Diagnostic.Status != "200")
                return null;
            return flightData;
        }


        //Step2
        internal TiketHotelBaseResponse ConfirmTransaction(string _token, string orderId)
        {
            var tiketClient = new TiketClientHandler();
            var client = tiketClient.CreateTiketClient();
            var url = "/partner/transactionApi/confirmPayment";
            var request = new RestRequest(url, Method.GET);
            request.AddQueryParameter("token", _token);
            request.AddQueryParameter("order_id", orderId);
            request.AddQueryParameter("secretkey", sharedKey);
            request.AddQueryParameter("confirmkey", confirmKey);
            request.AddQueryParameter("username", "suheri@travelmadezy.com");
            request.AddQueryParameter("textarea_note", "confirmation"); // note for confirmation
            request.AddQueryParameter("tanggal", DateTime.UtcNow.ToString("yyyy-MM-dd"));
            request.AddQueryParameter("output", "json");
            var response = client.Execute(request);
            var confirmResponse = JsonExtension.Deserialize<TiketHotelBaseResponse>(response.Content);
            if (confirmResponse == null && confirmResponse.Diagnostic.Status != "200")
                return null;
            return confirmResponse;
        }
    }

    
}
