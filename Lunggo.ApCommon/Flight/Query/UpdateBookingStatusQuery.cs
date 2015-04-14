using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class UpdateBookingStatusQuery : QueryBase<UpdateBookingStatusQuery, BookingStatusInfo>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            // TODO FLIGHT : update booking status query
            return null;
        }
    }
}
