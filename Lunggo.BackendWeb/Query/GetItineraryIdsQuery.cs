using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.BackendWeb.Query
{
    public class GetItineraryIdsQuery : DbQueryBase<GetItineraryIdsQuery, FlightItineraryTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            
            return "SELECT i.Id, i.BookingId FROM FlightItinerary i WHERE i.RsvNo = @RsvNo";
        }

    }
}