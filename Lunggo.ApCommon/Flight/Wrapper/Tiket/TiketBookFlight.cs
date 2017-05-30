using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Extension;
using RestSharp;
using Supplier = Lunggo.ApCommon.Payment.Constant.Supplier;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket
{
    internal partial class TiketWrapper
    {
        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo)
        {
            return Client.AddOrder(bookInfo);
        }


        private partial class TiketClientHandler
        {
            internal BookFlightResult AddOrder(FlightBookingInfo bookInfo)
            {
                var client = CreateTiketClient();
                var url = "/order/add/flight &conEmailAddress=you_julin@yahoo.com&firstnamea1=susi&lastnamea1=wijaya&ida1=1116057107900001&titlea1=Mr&titlec1=Ms&firstnamec1=carreen&lastnamec1=athalia&birthdatec1=2005-02-02&titlei1=Mr&parenti1=1&firstnamei1=wendy&lastnamei1=suprato&birthdatei1=2011-06-29&output=json"; //TODO Change this
                var request = new RestRequest(url, Method.GET);
                request.AddQueryParameter("token", token);
                request.AddQueryParameter("flight_id", "CHANGETHIS");
                request.AddQueryParameter("child", bookInfo.Passengers.Count(x => x.Type == PaxType.Child).ToString());
                request.AddQueryParameter("adult", bookInfo.Passengers.Count(x => x.Type == PaxType.Adult).ToString());
                request.AddQueryParameter("infant", bookInfo.Passengers.Count(x => x.Type == PaxType.Infant).ToString());
                request.AddQueryParameter("conSalutation", TitleCd.Mnemonic(bookInfo.Contact.Title));
                request.AddQueryParameter("conFirstName", bookInfo.Contact.Name);
                request.AddQueryParameter("conLastName", bookInfo.Contact.Name);
                request.AddQueryParameter("conPhone", bookInfo.Contact.CountryCallingCode + bookInfo.Contact.Phone);
                request.AddQueryParameter("conEmailAddress", bookInfo.Contact.Email);
                //request.AddQueryParameter("conOtherPhone", "");
                var response = client.Execute(request);
                var AddOrderResponse = JsonExtension.Deserialize<TiketBaseResponse>(response.Content);
                var temp = AddOrderResponse;
                if (AddOrderResponse == null)
                    return new BookFlightResult();
                var orderResult = Order(token);
                Console.WriteLine("Fisnihed Add Order");
                //TODO Operate the data from Order Result
                return new BookFlightResult();
                
            }

            internal OrderResponse Order(string token)
            {
                var client = CreateTiketClient();
                var url = "/order?token="+token+"&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var orderResponse = JsonExtension.Deserialize<OrderResponse>(response.Content);
                return orderResponse;
            }
        }

        internal override RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
        {
            return new RevalidateFareResult();
        }

        internal override IssueTicketResult OrderTicket(string bookingId, bool canHold)
        {
            return new IssueTicketResult();
        }

        internal override GetTripDetailsResult GetTripDetails(TripDetailsConditions conditions)
        {
            return new GetTripDetailsResult();
        }

        internal override Currency CurrencyGetter(string currency)
        {
            return new Currency(currency, Supplier.Sriwijaya);
        }

        internal override decimal GetDeposit()
        {
            return 0;
        }

        internal override List<BookingStatusInfo> GetBookingStatus()
        {
            throw new NotImplementedException();
        }
    }
}
