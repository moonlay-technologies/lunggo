using System;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.Framework.Database;
using Lunggo.Framework.Redis;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Payment.Model
{
    public class Currency
    {
        public string Symbol { get; private set; }
        public decimal Rate { get; private set; }
        public decimal RoundingOrder { get; private set; }

        [JsonConstructor]
        public Currency(string symbol)
        {
            if (!ValidateSymbol(symbol))
                return;
            Symbol = symbol.ToUpper();
            Rate = GetLatestRate();
            RoundingOrder = GetRoundingOrder();
        }

        public Currency(string symbol, decimal rate)
        {
            if (!ValidateSymbol(symbol))
                return;
            Symbol = symbol.ToUpper();
            Rate = rate;
            RoundingOrder = GetRoundingOrder();
        }

        public decimal GetRoundingOrder()
        {
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "currencyRoundingOrder:" + Symbol;
            var roundingOrder = (decimal)redisDb.StringGet(redisKey);
            return roundingOrder;
        }

        public decimal GetLatestRate()
        {
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "currencyRate:" + Symbol;
            var rate = (decimal)redisDb.StringGet(redisKey);
            return rate;
        }

        public static void SyncCurrencyData()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var records = GetCurrencyQuery.GetInstance().Execute(conn, null);
                var redis = RedisService.GetInstance();
                var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
                foreach (var record in records)
                {
                    var rateKey = "currencyRate:" + record.Symbol;
                    var roundingOrderKey = "currencyRoundingOrder:" + record.Symbol;
                    redisDb.StringSet(rateKey, (RedisValue)record.Rate.GetValueOrDefault());
                    redisDb.StringSet(roundingOrderKey, (RedisValue)record.RoundingOrder.GetValueOrDefault());
                }
            }
        }

        public static decimal GetLatestRate(string symbol)
        {
            return !ValidateSymbol(symbol) ? 0M : new Currency(symbol).Rate;
        }

        public static void SetRate(string symbol, decimal rate)
        {
            if (!ValidateSymbol(symbol))
                return;
            symbol = symbol.ToUpper();
            using (var conn = DbService.GetInstance().GetOpenConnection())
                CurrencyTableRepo.GetInstance().Update(conn, new CurrencyTableRecord { Symbol = symbol, Rate = rate });
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            var rateKey = "currencyRate:" + symbol;
            redisDb.StringSet(rateKey, (RedisValue)rate);
        }

        public static void SetRoundingOrder(string symbol, decimal roundingOrder)
        {
            if (!ValidateSymbol(symbol))
                return;
            symbol = symbol.ToUpper();
            using (var conn = DbService.GetInstance().GetOpenConnection())
                CurrencyTableRepo.GetInstance().Update(conn, new CurrencyTableRecord { Symbol = symbol, RoundingOrder = roundingOrder });
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            var roundingOrderKey = "currencyRoundingOrder:" + symbol;
            redisDb.StringSet(roundingOrderKey, (RedisValue)roundingOrder);
        }

        public override string ToString()
        {
            return Symbol;
        }

        public static implicit operator string(Currency currency)
        {
            return currency.ToString();
        }

        static bool ValidateSymbol(string symbol)
        {
            return symbol != null && symbol.Length == 3;
        }

        private class GetCurrencyQuery : QueryBase<GetCurrencyQuery, CurrencyTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT Symbol, Rate, RoundingOrder " +
                       "FROM Currency";
            }
        }
    }
}
