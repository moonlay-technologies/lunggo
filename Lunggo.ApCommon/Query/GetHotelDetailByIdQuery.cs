using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
