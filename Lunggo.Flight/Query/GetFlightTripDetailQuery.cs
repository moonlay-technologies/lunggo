using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Flight.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.Flight.Query
{
    public class GetFlightTripDetailQuery : QueryBase<GetFlightTripDetailQuery, FlightTripDetailTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(
                condition == FlightReservationIntegrated.QueryType.Overview
                    ? @"SELECT TripDetailId, SequenceNo, CarrierCd, FlightNumber, DepartureAirportCd, " +
                        @"ArrivalAirportCd, DepartureTime, ArrivalTime, Duration "
                    : @"SELECT * ");
            queryBuilder.Append(
                @"FROM FlightTripDetail ");
            queryBuilder.Append(
                @"WHERE TripId = @TripId ");
            queryBuilder.Append(
                @"ORDER BY SequenceNo");
            return queryBuilder.ToString();
        }
    }
}
