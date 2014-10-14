using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Query
{

    public class GetUserByLoginInfoQuery : QueryBase<GetUserByLoginInfoQuery, GetUserByAnyQueryRecord>
    {
        private GetUserByLoginInfoQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(
                "SELECT u.* FROM Users u inner join UserLogins l on l.Id = u.Id where l.LoginProvider = @loginProvider and l.ProviderKey = @providerKey"
            );
            return queryBuilder.ToString();
        }
    }
				

}
