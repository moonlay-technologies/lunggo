using System;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.ProductBase.Constant;

namespace Lunggo.ApCommon.ProductBase.Model
{
    public abstract class OrderBase<TOrderRule> where TOrderRule : OrderRuleBase
    {
        //public abstract ProductType Type { get; }
        public Price Price { get; set; }
        public DateTime? TimeLimit { get; set; }
        public TOrderRule Rule { get; set; }

        //public void SetPrice(decimal supplierPrice, string supplierCurrency)
        //{
        //    Price = Price.SetSupplier(supplierPrice, new Currency(supplierCurrency));

        //    Price.CalculateFinal();
        //}
    }
}
