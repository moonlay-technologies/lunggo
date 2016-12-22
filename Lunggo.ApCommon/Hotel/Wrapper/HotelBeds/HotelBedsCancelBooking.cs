using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds
{
    public class HotelBedsCancelBooking
    {
        public HotelCancelBookingResult CancelHotelBooking(HotelCancelBookingInfo input)
        {
            var client = new HotelApiClient(HotelApiType.BookingApi);
            var cancel = new Cancellation();
            List<Tuple<string, string>> param;
            param = new List<Tuple<string, string>>
                {
                    //new Tuple<string, string>("${bookingId}", "1-3087550"),
                    new Tuple<string, string>("${bookingId}", input.BookingReference),
                    new Tuple<string, string>("${flag}", "CANCELLATION")
                };
            BookingCancellationRS bookingCancellationRs = client.Cancel(param);

            if (bookingCancellationRs == null)
            {
                return new HotelCancelBookingResult
                {
                    status = "FAILED"
                };
            }
            Console.WriteLine("Id cancelled: " + input.BookingReference);
                Console.WriteLine(JsonConvert.SerializeObject(bookingCancellationRs, Formatting.Indented, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore }));

                //Console.WriteLine("Getting detail after cancelation of id " + input.BookingReference);
                //param = new List<Tuple<string, string>>
                //{
                //    new Tuple<string, string>("${bookingId}", input.BookingReference)
                //};
                //BookingDetailRS bookingDetailRS = client.Detail(param);
                //if (bookingDetailRS != null)
                //    Console.WriteLine(JsonConvert.SerializeObject(bookingDetailRS, Formatting.Indented, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore }));
                if (bookingCancellationRs.booking.status != SimpleTypes.BookingStatus.CANCELLED)
                {
                    return new HotelCancelBookingResult
                    {
                        status = "FAILED"
                    };
                }

            return new HotelCancelBookingResult
            {
                status = "SUCCESS",
                Reference = bookingCancellationRs.booking.reference,
                ClientReference = bookingCancellationRs.booking.clientReference,
                CancellationReference = bookingCancellationRs.booking.cancellationReference,
                CancellationAmount = bookingCancellationRs.booking.hotel == null ? 0 : bookingCancellationRs.booking.hotel.cancellationAmount
            };
        }
    }
}
