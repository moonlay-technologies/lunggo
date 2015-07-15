﻿using System.Text;
using Lunggo.ApCommon.Identity.Query.Record;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query
{

    public class GetUserByEmailQuery : QueryBase<GetUserByEmailQuery, GetUserByAnyQueryRecord>
    {
        private GetUserByEmailQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("select * from Users where Email = @Email");
            return queryBuilder.ToString();
        }
    }
				
}