using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Lunggo.Framework.Extension;
using RestSharp;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket
{
    internal partial class TiketWrapper
    {
        internal override GetTripDetailsResult GetTripDetails(TripDetailsConditions conditions)
        {
            return Client.GetTripDetails(conditions);
        }

        private partial class TiketClientHandler
        {
            internal GetTripDetailsResult GetTripDetails(TripDetailsConditions conditions)
            {
                /*Take data from DB*/
                var rsvNo = FlightService.GetRsvNoByBookingIdFromDb(new List<string> { conditions.BookingId }).Single();
                var reservation = FlightService.GetInstance().GetReservationFromDb(rsvNo);
                var itinerary = reservation.Itineraries.Single(itin => itin.BookingId == conditions.BookingId);
                var segments = itinerary.Trips.SelectMany(trip => trip.Segments).ToList();

                var token = GetToken();
                var client = CreateTiketClient();
                var url = "/check_order?order_id=" + conditions.BookingId + "&email=" + reservation.Contact.Email +
                          "&token=" + token + "&output=json";
                var request = new RestRequest(url, Method.GET);
                var response = client.Execute(request);
                var dataResponse = response.Content.Deserialize<HistoryOrderResponse>();
                if (dataResponse.Diagnostic.Status != "200")
                    return null;

                var pnr = dataResponse.OrderData.OrderCartDetail == null
                    ? null
                    : dataResponse.OrderData.OrderCartDetail[0].CartDetail == null
                        ? null
                        : dataResponse.OrderData.OrderCartDetail[0].CartDetail.booking_code;
                segments.ForEach(segment => segment.Pnr = pnr);

                return new GetTripDetailsResult
                {
                    IsSuccess = true,
                    BookingId = conditions.BookingId,
                    Itinerary = itinerary,
                    FlightSegmentCount = segments.Count
                };
            }


        }
    }
}
