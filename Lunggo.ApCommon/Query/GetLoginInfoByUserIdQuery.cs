using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Query
{
    


    public class GetLoginInfoByUserIdQuery : QueryBase<GetLoginInfoByUserIdQuery, GetUserLoginInfoByAnyQueryRecord>
    {
        private GetLoginInfoByUserIdQuery()
        {

        }

        protected override string GetQuery()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT LoginProvider, ProviderKey FROM UserLogins where UserId = @Id");
            return queryBuilder.ToString();
        }
    }
				

}
