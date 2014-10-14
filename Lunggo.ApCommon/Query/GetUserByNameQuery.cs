using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Query
{

    public class GetUserByNameQuery : QueryBase<GetUserByNameQuery, GetUserByAnyQueryRecord>
    {
        private GetUserByNameQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM Users WHERE LOWER(UserName) = LOWER(@userName)");
            return queryBuilder.ToString();
        }
    }
}			

