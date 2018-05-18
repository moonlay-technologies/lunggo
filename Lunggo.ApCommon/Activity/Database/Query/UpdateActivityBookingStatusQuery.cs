using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class UpdateActivityBookingStatusQuery : NoReturnDbQueryBase<UpdateActivityBookingStatusQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "UPDATE ActivityReservation SET BookingStatusCd = @bookingStatusCd, UpdateDate = @updateDate WHERE RsvNo = @rsvNo";
        }
    }
}
