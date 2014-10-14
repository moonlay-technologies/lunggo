using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Query
{
    public class GetCountryCodeByAreaQuery : QueryBase<GetCountryCodeByAreaQuery, GetCountryCodeQueryRecord>
    {
        private GetCountryCodeByAreaQuery()
        {

        }

        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("select CountryCode from CountryRef where CountryArea =  @CountryArea");
            return queryBuilder.ToString();
        }
        


    }
}
