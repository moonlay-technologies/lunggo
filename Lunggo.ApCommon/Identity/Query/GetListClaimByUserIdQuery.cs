using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
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
