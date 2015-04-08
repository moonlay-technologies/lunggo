using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Query
{
    public class GetAllCityHotel : QueryBase<GetAllCityHotel, GetAllHotelQueryRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT * From Hotel WHERE City = @City ";
        }
    }
}