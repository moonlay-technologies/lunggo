using Dapper;
using Lunggo.Framework.Pattern;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Lunggo.Framework.Database
{
    public abstract class ExecuteNonQueryBase<TQuery> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition = null);
        public int Execute(IDbConnection conn, dynamic param)
        {
            return SqlMapper.Execute(conn, GetQuery(), param);
        }
        public int Execute(IDbConnection conn, dynamic param, dynamic condition)
        {
            return SqlMapper.Execute(conn, GetQuery(condition), param);
        }

        public async Task<int> ExecuteAsync(IDbConnection conn, dynamic param)
        {
            return await SqlMapper.ExecuteAsync(conn, GetQuery(), param);
        }
        public async Task<int> ExecuteAsync(IDbConnection conn, dynamic param, dynamic condition)
        {
            return await SqlMapper.ExecuteAsync(conn, GetQuery(condition), param);
        }
    }
}
