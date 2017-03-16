using Lunggo.Framework.Database;

namespace Lunggo.BackendWeb.Query
{
    public class GetSegmentIdsQuery : DbQueryBase<GetSegmentIdsQuery, long>
    {
        protected override string GetQuery(dynamic condition = null)
        {

            return "SELECT s.Id FROM FlightTrip t, FlightSegment s WHERE t.ItineraryId = @ItineraryId AND t.Id = s.TripId";
        }

    }
}