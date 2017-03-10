using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Identity.Query;
using Lunggo.ApCommon.Identity.Users;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Roles
{
    public class Role : IdentityRole<String>
    {
        public static List<string> GetFromDb(string userId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var role = GetListRoleByUserIdQuery.GetInstance().Execute(conn, new { UserId = userId }).ToList();
                return role;
            }
        }
    }
}
