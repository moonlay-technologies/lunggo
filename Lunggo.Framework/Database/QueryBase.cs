using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Pattern;

namespace Lunggo.Framework.Database
{
    
    public abstract class QueryBase<TQuery, TQueryRecord> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery();
        public IEnumerable<TQueryRecord> Execute(IDbConnection conn, dynamic condition)
        {
            return SqlMapper.Query<TQueryRecord>(conn, GetQuery(), condition);
        }
    }


}
