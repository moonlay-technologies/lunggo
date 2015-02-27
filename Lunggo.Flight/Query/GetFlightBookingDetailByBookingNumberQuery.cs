using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.Flight.Query
{
    class GetFlightBookingDetailByBookingNumberQuery : QueryBase<GetFlightBookingDetailByBookingNumberQuery,FlightBookingDetail>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT * FROM %FlightBookingTable% WHERE BookingNumber = @BookingNumber";
        }
    }
}
