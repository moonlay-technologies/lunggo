using Lunggo.Framework.Database;
using Lunggo.CustomerWeb.WebSrc.UW600.UW620.Model;
using Lunggo.Hotel.ViewModels;
using Lunggo.Repository.TableRecord;

namespace Lunggo.CustomerWeb.WebSrc.UW600.UW620.Query
{
    public class GetHotelHistory : QueryBase<GetHotelHistory, HotelReservationsTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {

            return "SELECT * FROM HotelReservations WHERE MemberCd = @idMember";
        }

    }
}