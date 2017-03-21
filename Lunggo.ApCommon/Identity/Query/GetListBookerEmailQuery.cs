﻿using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{
    public class GetListBookerEmailQuery : DbQueryBase<GetListBookerEmailQuery, string>
    {
        private GetListBookerEmailQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT [User].Email " +
                                "FROM [User] " +
                                "LEFT JOIN [UserRole] ON [User].Id = UserRole.UserId " +
                                "LEFT JOIN [Role] ON UserRole.RoleId = [Role].Id " +
                                "WHERE Role.Name = 'Booker'");
            return queryBuilder.ToString();
        }
    }
}