using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
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
    public abstract class QueryBase<TQuery, TResult, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition=null);
        public IEnumerable<TResult> ExecuteMultiMap(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public IEnumerable<TResult> ExecuteMultiMap(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }

        public async Task<IEnumerable<TResult>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public async Task<IEnumerable<TResult>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TResult>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }
    }
    public abstract class QueryBase<TQuery, TResult, TFirst, TSecond, TThird, TFourth, TFifth, TSixth> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition = null);
        public IEnumerable<TResult> ExecuteMultiMap(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public IEnumerable<TResult> ExecuteMultiMap(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }

        public async Task<IEnumerable<TResult>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public async Task<IEnumerable<TResult>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TResult>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }
    }

    public abstract class QueryBase<TQuery, TResult, TFirst, TSecond, TThird, TFourth, TFifth> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition = null);
        public IEnumerable<TResult> ExecuteMultiMap(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public IEnumerable<TResult> ExecuteMultiMap(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }

        public async Task<IEnumerable<TResult>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public async Task<IEnumerable<TResult>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TFifth, TResult>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TResult>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }
    }
    public abstract class QueryBase<TQuery, TResult, TFirst, TSecond, TThird, TFourth> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition = null);
        public IEnumerable<TResult> ExecuteMultiMap(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TResult>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TResult>(conn, GetQuery(), map, splitOn: splitOn);
        }
        public IEnumerable<TResult> ExecuteMultiMap(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TResult>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TFourth, TResult>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }

        public async Task<IEnumerable<TResult>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TFourth, TResult>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TResult>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public async Task<IEnumerable<TResult>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TFourth, TResult>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TFourth, TResult>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }
    }
    public abstract class QueryBase<TQuery, TResult, TFirst, TSecond, TThird> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition = null);
        public IEnumerable<TResult> ExecuteMultiMap(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TResult>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TThird, TResult>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public IEnumerable<TResult> ExecuteMultiMap(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TResult>map, string splitOn = "Id")
        {
            var x = GetQuery(condition);
            return SqlMapper.Query<TFirst, TSecond, TThird, TResult>(conn, x, map, param, splitOn: splitOn);
        }

        public async Task<IEnumerable<TResult>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TThird, TResult>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TResult>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public async Task<IEnumerable<TResult>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TThird, TResult>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TThird, TResult>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }
    }
    public abstract class QueryBase<TQuery, TResult, TFirst, TSecond> : SingletonBase<TQuery> where TQuery : SingletonBase<TQuery>
    {
        protected abstract String GetQuery(dynamic condition = null);
        public IEnumerable<TResult> ExecuteMultiMap(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TResult>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TResult>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public IEnumerable<TResult> ExecuteMultiMap(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TResult>map, string splitOn = "Id")
        {
            return SqlMapper.Query<TFirst, TSecond, TResult>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }

        public async Task<IEnumerable<TResult>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, Func<TFirst, TSecond, TResult>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TResult>(conn, GetQuery(), map, param, splitOn: splitOn);
        }
        public async Task<IEnumerable<TResult>> ExecuteMultiMapAsync(IDbConnection conn, dynamic param, dynamic condition, Func<TFirst, TSecond, TResult>map, string splitOn = "Id")
        {
            return await SqlMapper.QueryAsync<TFirst, TSecond, TResult>(conn, GetQuery(condition), map, param, splitOn: splitOn);
        }
    }
    
}
