using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class GetBookedActivitiesRsvNoQuery : DbQueryBase<GetBookedActivitiesRsvNoQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return
                "SELECT r.RsvNo FROM ActivityReservation AS r " +
                "INNER JOIN Activity AS d ON r.ActivityId = d.Id " +
                "INNER JOIN Payment AS p ON p.RsvNo = r.RsvNo " +
                "WHERE r.BookingStatusCd = 'BOOK' AND p.StatusCd LIKE '%SET%'";
        }
    }
}
