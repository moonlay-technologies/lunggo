﻿using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Payment.Database.Query
{
    internal class GetCartIdFromDbQuery : DbQueryBase<GetCartIdFromDbQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT DISTINCT CartId FROM Carts WHERE RsvNoList = @RsvNo";
        }
    }
}