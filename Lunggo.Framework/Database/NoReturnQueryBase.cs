using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Lunggo.Framework.Pattern;

namespace Lunggo.Framework.Database
{
    public abstract class NoReturnQueryBase<TQuery> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition = null);
        public void Execute(IDbConnection conn, dynamic param)
        {
            SqlMapper.Execute(conn, GetQuery(), param);
        }
        public void Execute(IDbConnection conn, dynamic param, dynamic condition)
        {
            SqlMapper.Execute(conn, GetQuery(condition), param);
        }

        public async void ExecuteAsync(IDbConnection conn, dynamic param)
        {
            await SqlMapper.ExecuteAsync(conn, GetQuery(), param);
        }
        public async void ExecuteAsync(IDbConnection conn, dynamic param, dynamic condition)
        {
            await SqlMapper.ExecuteAsync(conn, GetQuery(condition), param);
        }
    }
}
