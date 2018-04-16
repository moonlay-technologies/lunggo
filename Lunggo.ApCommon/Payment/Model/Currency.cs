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

        public Currency(string symbol)
        {
            if (!ValidateSymbol(symbol))
                return;

            Supplier = Supplier.Travorama;
            Symbol = symbol.ToUpper();
            Rate = GetLatestRate();
            RoundingOrder = GetRoundingOrder();
        }

        public Currency(string symbol, Supplier supplier = Supplier.Travorama)
        {
            if (!ValidateSymbol(symbol))
                return;

            Supplier = supplier;
            Symbol = symbol.ToUpper();
            Rate = GetLatestRate();
            RoundingOrder = GetRoundingOrder();
        }

        public Currency(string symbol, decimal rate, Supplier supplier = Supplier.Travorama)
        {
            if (!ValidateSymbol(symbol))
                return;

            Supplier = supplier;
            Symbol = symbol.ToUpper();
            Rate = rate;
            RoundingOrder = GetRoundingOrder();
        }

        [JsonConstructor]
        public Currency(string symbol, decimal rate, decimal roundingOrder, Supplier supplier = Supplier.Travorama)
        {
            if (!ValidateSymbol(symbol))
                return;

            Supplier = supplier;
            Symbol = symbol.ToUpper();
            Rate = rate;
            RoundingOrder = roundingOrder;
        }

        public decimal GetRoundingOrder()
        {
            //for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            //{
            //        var redis = RedisService.GetInstance();
            //        var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            //        var redisKey = "currencyRoundingOrder:" + Symbol + ":" + Supplier;
            //        var roundingOrder = Convert.ToDecimal(redisDb.StringGet(redisKey));
            //        return roundingOrder;
            //}
            //return 0;
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var rates = CurrencyTableRepo.GetInstance().Find(conn, new CurrencyTableRecord
                {
                    Symbol = Symbol,
                }).ToList();
                if (rates.Count < 1)
                {
                    return 100000;
                }
                var rate = rates.Where(a => a.SupplierCd == SupplierCd.Mnemonic(Supplier)).First();
                return (decimal)rate.RoundingOrder;
            }
        }

        public decimal GetLatestRate()
        {
            //for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            //{
            //        var redis = RedisService.GetInstance();
            //        var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
            //        var redisKey = "currencyRate:" + Symbol + ":" + Supplier;
            //        var rate = Convert.ToDecimal(redisDb.StringGet(redisKey));
            //        return rate;
            //}
            //return 100000;
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var rates = CurrencyTableRepo.GetInstance().Find(conn, new CurrencyTableRecord
                {
                    Symbol = Symbol,
                }).ToList();
                if (rates.Count < 1)
                {
                    return 100000;
                }
                var rate = rates.Where(a => a.SupplierCd == SupplierCd.Mnemonic(Supplier)).First();          
                return (decimal)rate.Rate;
            }
        }

        public static Dictionary<string, Currency> GetAllCurrencies(Supplier supplier = Supplier.Travorama)
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
                for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
                {
                        foreach (var record in records)
                        {
                            var rateKey = "currencyRate:" + record.Symbol + ":" + SupplierCd.Mnemonic(record.SupplierCd);
                            var roundingOrderKey = "currencyRoundingOrder:" + record.Symbol + ":" +
                                                   SupplierCd.Mnemonic(record.SupplierCd);
                            redisDb.StringSet(rateKey, Convert.ToString(record.Rate.GetValueOrDefault()));
                            redisDb.StringSet(roundingOrderKey,
                                Convert.ToString(record.RoundingOrder.GetValueOrDefault()));
                        }
                        var suppliers = records.Select(r => SupplierCd.Mnemonic(r.SupplierCd)).Distinct().ToList();
                        foreach (var supp in suppliers)
                        {
                            var currencyListKey = "currencies:" + supp;
                            var currencyList =
                                records.Where(r => SupplierCd.Mnemonic(r.SupplierCd) == supp).Select(r => r.Symbol);
                            redisDb.StringSet(currencyListKey, currencyList.Serialize());
                        }
                        return;
                }
            }
        }

        public static void SetRate(string symbol, decimal rate, Supplier supplier = Supplier.Travorama)
        {
            if (!ValidateSymbol(symbol))
                return;
            symbol = symbol.ToUpper();
            using (var conn = DbService.GetInstance().GetOpenConnection())
                CurrencyTableRepo.GetInstance().Update(conn, new CurrencyTableRecord { Symbol = symbol, Rate = rate, SupplierCd = SupplierCd.Mnemonic(supplier), UpdateDate = DateTime.UtcNow});
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                    var redis = RedisService.GetInstance();
                    var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
                    var rateKey = "currencyRate:" + symbol + ":" + supplier;
                    redisDb.StringSet(rateKey, Convert.ToString(rate));
                    return;
            }
        }

        public static void SetRoundingOrder(string symbol, decimal roundingOrder, Supplier supplier = Supplier.Travorama)
        {
            if (!ValidateSymbol(symbol))
                return;
            symbol = symbol.ToUpper();
            using (var conn = DbService.GetInstance().GetOpenConnection())
                CurrencyTableRepo.GetInstance().Update(conn, new CurrencyTableRecord { Symbol = symbol, RoundingOrder = roundingOrder, SupplierCd = SupplierCd.Mnemonic(supplier) });
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                    var redis = RedisService.GetInstance();
                    var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
                    var roundingOrderKey = "currencyRoundingOrder:" + symbol;
                    redisDb.StringSet(roundingOrderKey, Convert.ToString(roundingOrder));
                    return;
            }
        }

        private static IEnumerable<string> GetCurrencyList(Supplier supplier = Supplier.Travorama)
        {
            for (var i = 0; i < ApConstant.RedisMaxRetry; i++)
            {
                    var redis = RedisService.GetInstance();
                    var redisDb = redis.GetDatabase(ApConstant.SearchResultCacheName);
                    var currencyListKey = "currencies:" + supplier;
                    var currencyList = ((string) redisDb.StringGet(currencyListKey)).Deserialize<IEnumerable<string>>();
                    return currencyList;
            }
            return null;
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

        private class GetCurrencyQuery : DbQueryBase<GetCurrencyQuery, CurrencyTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT Symbol, Rate, RoundingOrder, SupplierCd " +
                       "FROM Currency";
            }
        }
    }
}
