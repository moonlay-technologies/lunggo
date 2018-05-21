using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Activity.Model.Logic;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Activity.Constant;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetMyBookingsOutput GetMyBookings(GetMyBookingsInput input)
        {
            var getMyBookingsOutput = GetMyBookingsFromDb(input);
            return getMyBookingsOutput;
        }

        public bool CheckBookingStatusCd(string bookingStatusCd)
        {
            var bookingStatus = BookingStatusCd.Mnemonic(bookingStatusCd);
            return bookingStatus != BookingStatus.Undefined;
        }
    }
}