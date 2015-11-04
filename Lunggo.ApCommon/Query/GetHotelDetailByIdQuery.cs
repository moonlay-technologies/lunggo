using System.Text;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Query
{
    public class GetHotelDetailByIdQuery: QueryBase<GetHotelDetailBySearchParamQuery, HotelDetailBase>
    {
        private GetHotelDetailByIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            //Not implemented yet
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("");
            return queryBuilder.ToString();
        }
    }
}
