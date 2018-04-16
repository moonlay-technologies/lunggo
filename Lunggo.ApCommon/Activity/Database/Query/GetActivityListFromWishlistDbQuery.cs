using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    public class GetActivityListFromWishlistDbQuery : DbQueryBase<GetActivityListFromWishlistDbQuery, long>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "Select DISTINCT ActivityId From Wishlist WHERE UserId = @user";
        }
    }
}
