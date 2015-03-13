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
        protected abstract String GetQuery(dynamic condition=null);
        public IEnumerable<TQueryRecord> Execute(IDbConnection conn, dynamic param)
        {
            return SqlMapper.Query<TQueryRecord>(conn, GetQuery(), param);
        }
        public IEnumerable<TQueryRecord> Execute(IDbConnection conn, dynamic param, dynamic condition)
        {
            return SqlMapper.Query<TQueryRecord>(conn, GetQuery(condition), param);
        }
        public async Task<IEnumerable<TQueryRecord>> ExecuteAsync(IDbConnection conn, dynamic param)
        {
            return await SqlMapper.QueryAsync<TQueryRecord>(conn, GetQuery(), param);
        }
        public async Task<IEnumerable<TQueryRecord>> ExecuteAsync(IDbConnection conn, dynamic param, dynamic condition)
        {
            return await SqlMapper.QueryAsync<TQueryRecord>(conn, GetQuery(condition), param);
        }
    }


}
