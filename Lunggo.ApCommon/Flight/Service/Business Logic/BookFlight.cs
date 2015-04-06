using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public BookFlightOutput BookFlight(BookFlightInput input)
        {
            var output = new BookFlightOutput();
            var bookInfo = new FlightBookingInfo
            {
                FareId = input.BookingInfo.FareId,
                ContactEmail = input.BookingInfo.ContactEmail,
                ContactPhone = input.BookingInfo.ContactPhone,
                PassengerFareInfos = input.BookingInfo.PassengerFareInfos
            };
            var response = BookFlightInternal(bookInfo);
            if (response.IsBookSuccess)
            {
                output.BookResult.BookingId = response.Status.BookingId;
                output.BookResult.BookingStatus = response.Status.BookingStatus;
                if (response.Status.BookingStatus == BookingStatus.Booked)
                    output.BookResult.TimeLimit = response.Status.TimeLimit;
            }
            if (input.ReturnBookingInfo.FareId != null)
            {
                bookInfo.FareId = input.ReturnBookingInfo.FareId;
                response = BookFlightInternal(bookInfo);
                if (response.IsBookSuccess)
                {
                    output.ReturnBookResult.BookingId = response.Status.BookingId;
                    output.ReturnBookResult.BookingStatus = response.Status.BookingStatus;
                    if (response.Status.BookingStatus == BookingStatus.Booked)
                        output.ReturnBookResult.TimeLimit = response.Status.TimeLimit;
                }
            }
            return output;
        }
    }
}
