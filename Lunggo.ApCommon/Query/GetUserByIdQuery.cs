using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Query
{
    public class GetUserByIdQuery : QueryBase<GetUserByIdQuery, GetUserByAnyQueryRecord>
    {
        private GetUserByIdQuery()
        {
            
        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM Users WHERE Id = @Id");
            return queryBuilder.ToString();
        }
    }
				
}
