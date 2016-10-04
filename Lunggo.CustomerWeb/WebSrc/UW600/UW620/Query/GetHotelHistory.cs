using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.CustomerWeb.WebSrc.UW600.UW620.Query
{
    public class GetHotelHistory : DbQueryBase<GetHotelHistory, HotelReservationsTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {

            return "SELECT * FROM HotelReservations WHERE MemberCd = @idMember";
        }

    }
}