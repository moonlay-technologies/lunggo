using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.Flight.Query
{
    class GetFlightBookingDetailBySearchParamsQuery : QueryBase<GetFlightBookingDetailBySearchParamsQuery,FlightBookingDetail>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM %FlightBookingTable% WHERE");
            if (condition.OriginAirport != null)
                queryBuilder.Append("OriginAirport = @OriginAirport,");
            if (condition.DestinationAirport != null)
                queryBuilder.Append("DestinationAirport = @DestinationAirport,");
            if (condition.BookingDate != null)
                queryBuilder.Append("BookingDate BETWEEN @BookingDateStart AND @BookingDateEnd,");
            if (condition.IsReturning != null)
                queryBuilder.Append("IsReturning = @IsReturning,");
            if (condition.DepartureAirline != null)
                queryBuilder.Append("DepartureAirline = @DepartureAirline,");
            if (condition.ReturnAirline != null)
                queryBuilder.Append("ReturnAirline = @ReturnAirline,");
            if (condition.DepartureDate != null)
                queryBuilder.Append("DepartureDate BETWEEN @DepartureDateStart AND @DepartureDateEnd,");
            if (condition.ReturnDate != null)
                queryBuilder.Append("ReturnDate BETWEEN @ReturnDateStart AND @ReturnDateEnd,");
            if (condition.PassangerName != null)
                queryBuilder.Append("PassangerName LIKE '%@PassangerName%',");
            if (condition.BookerName != null)
                queryBuilder.Append("BookerName LIKE '%@BookerName%',");
            queryBuilder.Remove(queryBuilder.Length, 1);
            return queryBuilder.ToString();
        }
    }
}
