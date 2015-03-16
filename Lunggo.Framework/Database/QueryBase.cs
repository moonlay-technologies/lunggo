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
        protected abstract String GetQuery(dynamic condition = null);
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
    public abstract class QueryBase<TQuery, TQueryRecord, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition=null);
        public IEnumerable<TQueryRecord> ExecuteMultiMap(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TQueryRecord>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TQueryRecord>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public IEnumerable<TQueryRecord> ExecuteMultiMap(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TQueryRecord>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TQueryRecord>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }

        public async Task<IEnumerable<TQueryRecord>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TQueryRecord>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TQueryRecord>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public async Task<IEnumerable<TQueryRecord>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TQueryRecord>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TQueryRecord>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }
    }
    public abstract class QueryBase<TQuery, TQueryRecord, TFirst, TSecond, TThird, TFourth, TFifth, TSixth> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition = null);
        public IEnumerable<TQueryRecord> ExecuteMultiMap(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TQueryRecord>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TQueryRecord>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public IEnumerable<TQueryRecord> ExecuteMultiMap(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TQueryRecord>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TQueryRecord>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }

        public async Task<IEnumerable<TQueryRecord>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TQueryRecord>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TQueryRecord>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public async Task<IEnumerable<TQueryRecord>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TQueryRecord>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TQueryRecord>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }
    }

    public abstract class QueryBase<TQuery, TQueryRecord, TFirst, TSecond, TThird, TFourth, TFifth> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition = null);
        public IEnumerable<TQueryRecord> ExecuteMultiMap(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TFifth, TQueryRecord>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TQueryRecord>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public IEnumerable<TQueryRecord> ExecuteMultiMap(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TFifth, TQueryRecord>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TQueryRecord>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }

        public async Task<IEnumerable<TQueryRecord>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TFifth, TQueryRecord>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TQueryRecord>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public async Task<IEnumerable<TQueryRecord>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TFifth, TQueryRecord>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TQueryRecord>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }
    }
    public abstract class QueryBase<TQuery, TQueryRecord, TFirst, TSecond, TThird, TFourth> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition = null);
        public IEnumerable<TQueryRecord> ExecuteMultiMap(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TQueryRecord>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TQueryRecord>(conn, GetQuery(), map, splitOn: splitOn);
        }
        public IEnumerable<TQueryRecord> ExecuteMultiMap(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TQueryRecord>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TQueryRecord>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }

        public async Task<IEnumerable<TQueryRecord>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TQueryRecord>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TQueryRecord>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public async Task<IEnumerable<TQueryRecord>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TQueryRecord>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TQueryRecord>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }
    }
    public abstract class QueryBase<TQuery, TQueryRecord, TFirst, TSecond, TThird> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition = null);
        public IEnumerable<TQueryRecord> ExecuteMultiMap(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TQueryRecord>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TQueryRecord>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public IEnumerable<TQueryRecord> ExecuteMultiMap(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TQueryRecord>map, string splitOn = "Id")
        {
            var x = GetQuery(condition);
            return SqlMapper.Query<TFirst, TSecond, TThird, TQueryRecord>(conn, x, map, param, splitOn: splitOn);
        }

        public async Task<IEnumerable<TQueryRecord>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TQueryRecord>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TQueryRecord>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public async Task<IEnumerable<TQueryRecord>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TQueryRecord>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TQueryRecord>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }
    }
    public abstract class QueryBase<TQuery, TQueryRecord, TFirst, TSecond> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition = null);
        public IEnumerable<TQueryRecord> ExecuteMultiMap(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TQueryRecord>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TQueryRecord>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public IEnumerable<TQueryRecord> ExecuteMultiMap(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TQueryRecord>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TQueryRecord>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }

        public async Task<IEnumerable<TQueryRecord>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TQueryRecord>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TQueryRecord>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public async Task<IEnumerable<TQueryRecord>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TQueryRecord>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TQueryRecord>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }
    }
    
}
