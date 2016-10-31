using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetListClaimByUserIdQuery : DbQueryBase<GetListClaimByUserIdQuery, dynamic>
    {
        private GetListClaimByUserIdQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * " +
                                "FROM [UserClaim] " +
                                "WHERE UserId = @Id");
            return queryBuilder.ToString();
        }
    }
				
}
