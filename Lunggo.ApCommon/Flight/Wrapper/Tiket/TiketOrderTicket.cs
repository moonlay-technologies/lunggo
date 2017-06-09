using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket
{
    internal partial class TiketWrapper
    {
        internal override IssueTicketResult OrderTicket(string bookingId, bool canHold)
        {
            return Client.OrderTicket(bookingId);
        }

        private partial class TiketClientHandler
        {
            internal IssueTicketResult OrderTicket(string bookingId)
            {
                var token = FlightService.GetInstance().GetTokenBookingToCache(bookingId);

                //Step 1 : Checkout Payment Using Deposit
                var step1 = Client.CheckoutPageUsingDeposit(token);
                if (step1 == null)
                    return new IssueTicketResult
                    {
                        BookingId = bookingId,
                        IsSuccess = false,
                        IsInstantIssuance = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier },
                        ErrorMessages = new List<string> { "[Tiket] Error while request Checkout Page Using Deposit" }
                    };

                //Step 2 : Confirmation Transaction
                var confirmResponse = Client.ConfirmTransaction(token, bookingId);
                if (confirmResponse == null)
                    return new IssueTicketResult
                    {
                        BookingId = bookingId,
                        IsSuccess = false,
                        IsInstantIssuance = false,
                        Errors = new List<FlightError> { FlightError.FailedOnSupplier },
                        ErrorMessages = new List<string> { "[Tiket] Error while request Confirm Pay" }
                    };

                return new IssueTicketResult
                {
                    BookingId = bookingId,
                    IsSuccess = true,
                    IsInstantIssuance = true
                };
            }
            
            //Step3
            internal TiketBaseResponse CheckoutPageUsingDeposit(string _token)
            {
                var client = CreateTiketClient();
                var url = "/checkout/checkout_payment/8?btn_booking=1&token="+_token+"&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var flightData = JsonExtension.Deserialize<TiketBaseResponse>(response.Content);
                if (flightData == null && flightData.Diagnostic.Status != "200")
                    return null;
                return flightData;
            }


            //Step4
            internal TiketBaseResponse ConfirmTransaction(string _token, string orderId)
            {
                var client = CreateTiketClient();
                var url = "/partner/transactionApi/confirmPayment";
                var request = new RestRequest(url, Method.GET);
                request.AddQueryParameter("token", _token);
                request.AddQueryParameter("order_id", orderId);
                request.AddQueryParameter("secretkey", sharedSecret);
                request.AddQueryParameter("confirmkey", confirmKey);
                request.AddQueryParameter("username", "suheri@travelmadezy.com");
                request.AddQueryParameter("textarea_note", "confirmation"); // note for confirmation
                request.AddQueryParameter("tanggal", DateTime.UtcNow.ToString("yyyy-MM-dd"));
                request.AddQueryParameter("output", "json");
                var response = client.Execute(request);
                var confirmResponse = JsonExtension.Deserialize<TiketBaseResponse>(response.Content);
                if (confirmResponse == null && confirmResponse.Diagnostic.Status != "200")
                    return null;
                return confirmResponse;
            }
        }
    }

}
