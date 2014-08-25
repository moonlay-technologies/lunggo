using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Database
{
    public abstract class QueryBase<T> where T : QueryRecord
    {
        protected abstract String GetQuery();

        public IEnumerable<T> Execute(IDbConnection conn, dynamic condition)
        {
            return SqlMapper.Query<T>(conn, GetQuery(), condition);
        }

    }
}
