using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Query
{

    public class GetListClaimByUserIdQuery : QueryBase<GetListClaimByUserIdQuery, dynamic>
    {
        private GetListClaimByUserIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("select * from UserClaims where UserId = @Id");
            return queryBuilder.ToString();
        }
    }
				
}
