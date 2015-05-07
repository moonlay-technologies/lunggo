using System.Web.Mvc;
using Lunggo.ApCommon.Constant;
using Lunggo.Framework.Redis;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Currency
{
    public class CurrencyService
    {
        private static readonly CurrencyService Instance = new CurrencyService();

        private CurrencyService()
        {
            
        }

        public static CurrencyService GetInstance()
        {
            return Instance;
        }

        public void UpdateSupplierExchangeRate(Supplier supplier, decimal addedDeposit, decimal newRate)
        {
            var supplierName = GetSupplierName(supplier);
            var currentDeposit = GetSupplierDepositInCache(supplierName);
            var currentBalance = currentDeposit.Balance;
            var currentRate = currentDeposit.ExchangeRate;
            var newBalance = currentBalance + addedDeposit;
            var averageRate = ((currentBalance*currentRate) + (addedDeposit*newRate))/newBalance;
            var deposit = new Deposit
            {
                Balance = newBalance,
                ExchangeRate = averageRate
            };
            SetSupplierDepositInCache(supplierName, deposit);
        }

        public void SetSupplierExchangeRate(Supplier supplier, decimal balance, decimal rate)
        {
            var supplierName = GetSupplierName(supplier);
            var deposit = new Deposit
            {
                Balance = balance,
                ExchangeRate = rate
            };
            SetSupplierDepositInCache(supplierName, deposit);
        }

        public decimal GetSupplierExchangeRate(Supplier supplier)
        {
            var supplierName = GetSupplierName(supplier);
            var deposit = GetSupplierDepositInCache(supplierName);
            return deposit.ExchangeRate;
        }

        public static void SetCurrencyRate(string currency, decimal rate)
        {
            SetCurrencyRateInCache(currency, rate);
        }

        public static decimal GetCurrencyRate(string currency)
        {
            return GetCurrencyRateInCache(currency);
        }

        private static void SetSupplierDepositInCache(string supplier, Deposit deposit)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = supplier + "Rate";
            var cacheObject = Serialize(deposit);
            redisDb.StringSet(redisKey, cacheObject);
        }

        private static Deposit GetSupplierDepositInCache(string supplier)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = supplier + "Rate";
            var cacheObject = redisDb.StringGet(redisKey);
            var deposit = Deserialize<Deposit>(cacheObject);
            return deposit;
        }

        private static void SetCurrencyRateInCache(string currency, decimal rate)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = currency + "Rate";
            redisDb.StringSet(redisKey, (RedisValue)rate);
        }

        private static decimal GetCurrencyRateInCache(string currency)
        {
            var redisService = RedisService.GetInstance();
            var redisDb = redisService.GetDatabase(ApConstant.SearchResultCacheName);
            var redisKey = currency + "Rate";
            var rate = (decimal) redisDb.StringGet(redisKey);
            return rate;
        }        

        private static string GetSupplierName(Supplier supplier)
        {
            switch (supplier)
            {
                case Supplier.Mystifly:
                    return "Mystifly";
                case Supplier.HotelsPro:
                    return "HotelsPro";
                default:
                    return null;
            }
        }

        private static string Serialize<T>(T input)
        {
            return JsonConvert.SerializeObject(input);
        }

        private static T Deserialize<T>(string jsonInput)
        {
            return JsonConvert.DeserializeObject<T>(jsonInput);
        }
    }

    internal class Deposit
    {
        public decimal Balance { get; set; }
        public decimal ExchangeRate { get; set; }
    }

    public enum Supplier
    {
        Mystifly = 0,
        HotelsPro = 1
    }
}
