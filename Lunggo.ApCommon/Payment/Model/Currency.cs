using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Redis;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Payment.Model
{
    public class Currency
    {
        public string Symbol { get; set; }
        public decimal Rate { get; set; }
        public decimal RoundingOrder { get; private set; }

        public Supplier Supplier { get; set; }
        public bool IsAvailable { get; set; }
        [JsonConstructor]
        public Currency(string symbol)
        {
            try
            {
                if (!ValidateSymbol(symbol))
                {
                    IsAvailable = false;
                    return;
                }

                Symbol = symbol.ToUpper();
                Rate = GetLatestRate();
                RoundingOrder = GetRoundingOrder();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public Currency(string symbol, Supplier supplier)
        {
            if (!ValidateSymbol(symbol))
            {
                IsAvailable = false;
                Supplier = supplier;
                return;
            }

            Symbol = symbol.ToUpper();
            Rate = GetLatestRate(Symbol, supplier);
            RoundingOrder = GetRoundingOrder();
        }
        public Currency(string symbol, decimal rate)
        {
            if (!ValidateSymbol(symbol))
            {
                IsAvailable = false;
                return;
            }
            Symbol = symbol.ToUpper();
            Rate = rate;
            RoundingOrder = GetRoundingOrder();
        }

        public Currency(string symbol, decimal rate, decimal roundingOrder)
        {
            if (!ValidateSymbol(symbol))
            {
                IsAvailable = false;
                return;
            }
            Symbol = symbol.ToUpper();
            Rate = rate;
            RoundingOrder = roundingOrder;
        }

        public decimal GetRoundingOrder()
        {
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "currencyRoundingOrder:" + Symbol + ":" + Supplier;
            var roundingOrder = (decimal)redisDb.StringGet(redisKey);
            return roundingOrder;
        }

        public decimal GetLatestRate()
        {
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = "currencyRate:" + Symbol + ":" + Supplier;
            var rate = (decimal)redisDb.StringGet(redisKey);
            return rate;
        }

        public static Dictionary<string, Currency> GetAllCurrencies()
        {
            var currencyList = GetCurrencyList();
            return currencyList.ToDictionary(c => c, c => new Currency(c));
        }

        public static Dictionary<string, Currency> GetAllCurrencies(Supplier supplier)
        {
            var currencyList = GetCurrencyList(supplier);
            return currencyList.ToDictionary(c => c, c => new Currency(c));
        }

        public static void SyncCurrencyData()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var records = GetCurrencyQuery.GetInstance().Execute(conn, null).ToList();
                var redis = RedisService.GetInstance();
                var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
                foreach (var record in records)
                {
                    var rateKey = "currencyRate:" + record.Symbol + ":" + record.SupplierCd;
                    var roundingOrderKey = "currencyRoundingOrder:" + record.Symbol + ":" + record.SupplierCd;
                    redisDb.StringSet(rateKey, (RedisValue)record.Rate.GetValueOrDefault());
                    redisDb.StringSet(roundingOrderKey, (RedisValue)record.RoundingOrder.GetValueOrDefault());
                }
                var currencyListKey = "currencies";
                var currencyList = records.Select(r => r.Symbol);
                redisDb.StringSet(currencyListKey, currencyList.Serialize());
            }
        }

        public static void SyncCurrencyData(Supplier supplier)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var records = GetCurrencyQuery.GetInstance().Execute(conn, null).ToList();
                var redis = RedisService.GetInstance();
                var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
                foreach (var record in records)
                {
                    var rateKey = "currencyRate:" + record.Symbol + ":" + supplier;
                    var roundingOrderKey = "currencyRoundingOrder:" + record.Symbol + ":" + supplier;
                    redisDb.StringSet(rateKey, (RedisValue)record.Rate.GetValueOrDefault());
                    redisDb.StringSet(roundingOrderKey, (RedisValue)record.RoundingOrder.GetValueOrDefault());
                }
                var currencyListKey = "currencies" + ":" + supplier;
                var currencyList = records.Select(r => r.Symbol);
                redisDb.StringSet(currencyListKey, currencyList.Serialize());
            }
        }

        public static decimal GetLatestRate(string symbol)
        {
            var supplier = Supplier.Travorama;
            return !ValidateSymbol(symbol) ? 0M : new Currency(symbol, supplier).Rate;
        }

        public static decimal GetLatestRate(string symbol, Supplier supplier)
        {
            return !ValidateSymbol(symbol) ? 0M : new Currency(symbol, supplier).Rate;
        }

        public static void SetRate(string symbol, decimal rate)
        {
            if (!ValidateSymbol(symbol))
                return;
            symbol = symbol.ToUpper();
            var supplier = Supplier.Travorama;
            using (var conn = DbService.GetInstance().GetOpenConnection())
                CurrencyTableRepo.GetInstance().Update(conn, new CurrencyTableRecord { Symbol = symbol, Rate = rate, SupplierCd = SupplierCd.Mnemonic(supplier)});
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            var rateKey = "currencyRate:" + symbol + ":" + supplier;
            redisDb.StringSet(rateKey, (RedisValue)rate);
        }

        public static void SetRate(string symbol, decimal rate, Supplier supplier)
        {
            if (!ValidateSymbol(symbol))
                return;
            symbol = symbol.ToUpper();
            using (var conn = DbService.GetInstance().GetOpenConnection())
                CurrencyTableRepo.GetInstance().Update(conn, new CurrencyTableRecord { Symbol = symbol, Rate = rate, SupplierCd = SupplierCd.Mnemonic(supplier) });
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            var rateKey = "currencyRate:" + symbol + ":" + supplier;
            redisDb.StringSet(rateKey, (RedisValue)rate);
        }

        public static void SetRoundingOrder(string symbol, decimal roundingOrder)
        {
            if (!ValidateSymbol(symbol))
                return;
            symbol = symbol.ToUpper();
            var supplier = Supplier.Travorama;
            using (var conn = DbService.GetInstance().GetOpenConnection())
                CurrencyTableRepo.GetInstance().Update(conn, new CurrencyTableRecord { Symbol = symbol, RoundingOrder = roundingOrder, SupplierCd = SupplierCd.Mnemonic(supplier) });
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            var roundingOrderKey = "currencyRoundingOrder:" + symbol + ":" + supplier;
            redisDb.StringSet(roundingOrderKey, (RedisValue)roundingOrder);
        }

        public static void SetRoundingOrder(string symbol, decimal roundingOrder, Supplier supplier)
        {
            if (!ValidateSymbol(symbol))
                return;
            symbol = symbol.ToUpper();
            using (var conn = DbService.GetInstance().GetOpenConnection())
                CurrencyTableRepo.GetInstance().Update(conn, new CurrencyTableRecord { Symbol = symbol, RoundingOrder = roundingOrder, SupplierCd = SupplierCd.Mnemonic(supplier) });
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            var roundingOrderKey = "currencyRoundingOrder:" + symbol;
            redisDb.StringSet(roundingOrderKey, (RedisValue)roundingOrder);
        }

        private static IEnumerable<string> GetCurrencyList()
        {
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            var currencyListKey = "currencies";
            var currencyList = ((string) redisDb.StringGet(currencyListKey)).Deserialize<IEnumerable<string>>();
            return currencyList;
        }

        private static IEnumerable<string> GetCurrencyList(Supplier supplier)
        {
            var redis = RedisService.GetInstance();
            var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            var currencyListKey = "currencies:" + supplier;
            var currencyList = ((string)redisDb.StringGet(currencyListKey)).Deserialize<IEnumerable<string>>();
            return currencyList;
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
                return "SELECT Symbol, Rate, RoundingOrder, SupplierCd " +
                       "FROM Currency";
            }
        }
    }
}
