using System.Collections.Generic;
using System.Text;
using Lunggo.ApCommon.Identity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{
    internal class GetAllUserByAdminQuery : DbQueryBase<GetAllUserByAdminQuery, UserData>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            //if (condition != null)
            //    queryBuilder.Append(CreateWhereClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SELECT a.Id as UserId, a.UserName, a.Email, a.FirstName, a.LastName, a.CountryCallCd, a.PhoneNumber, a.Position, a.Branch, a.Department, a.ApproverId, a.LockoutEnabled as IsLocked ");
            clauseBuilder.Append(@"FROM dbo.[User] AS a WHERE a.CompanyId = @CompanyId");
            return clauseBuilder.ToString();
        }

        //private static string CreateWhereClause(dynamic condition)
        //{
        //    var clauseBuilder = new StringBuilder();
        //    clauseBuilder.Append(@"WHERE a.CompanyId = @CompanyId ");
        //    if (condition.Name != null)
        //    {
        //        clauseBuilder.Append(@"AND a.FirstName = @Name ");
        //    }
        //    if (condition.Email != null)
        //    {
        //        clauseBuilder.Append(@"AND a.Email = @Email ");
        //    }
        //    if (condition.Position != null)
        //    {
        //        clauseBuilder.Append(@"AND a.Position = @Position ");
        //    }
        //    if (condition.Department != null)
        //    {
        //        clauseBuilder.Append(@"AND a.Department = @Department ");
        //    } if (condition.Branch != null)
        //    {
        //        clauseBuilder.Append(@"AND a.Branch = @Branch ");
        //    }

        //    return clauseBuilder.ToString();
        //}
    }
}
