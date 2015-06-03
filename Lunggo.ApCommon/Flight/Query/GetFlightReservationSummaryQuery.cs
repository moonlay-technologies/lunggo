using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class GetFlightReservationSummaryQuery : QueryBase<GetFlightReservationSummaryQuery, FlightReservationTableRecord, FlightReservationTableRecord, FlightItineraryTableRecord, FlightTripTableRecord, FlightSegmentTableRecord, FlightPassengerTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            throw new NotImplementedException();
        }
    }
}
