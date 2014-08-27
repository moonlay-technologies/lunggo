using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Query
{

    public class GetAllUserQuery : QueryBase<GetAllUserQuery,>
    {
        private GetAllUserQuery()
        {

        }

        protected override string GetQuery()
        {
            var queryBuilder = new StringBuilder();

            /***
            * Create Your Query Here
            queryBuilder.Append("SELECT FirstName,LastName FROM Person");
            **/

            return queryBuilder.ToString();
        }
    }
				
}
