using System;
using System.Linq;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Context;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.ProductBase.Model
{
    public sealed partial class Price
    {
        public decimal Supplier { get; set; }
        public Currency SupplierCurrency { get; set; }
        public decimal OriginalIdr { get; set; }
        public Margin Margin { get; set; }
        public decimal Rounding { get; set; }
        public decimal MarginNominal { get; set; }
        public decimal FinalIdr { get; set; }
        public decimal Local { get; set; }
        public Currency LocalCurrency { get; set; }

        internal void InsertToDb(long orderId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                PriceTableRepo.GetInstance().Insert(conn, new PriceTableRecord
                {
                    Id = PriceIdSequence.GetInstance().GetNext(),
                    OrderId = orderId,
                    MarginId = Margin.Id,
                    SupplierPrice = Supplier,
                    SupplierCurrencyCd = SupplierCurrency,
                    SupplierRate = SupplierCurrency.Rate,
                    OriginalPriceIdr = OriginalIdr,
                    MarginNominal = MarginNominal,
                    FinalPriceIdr = FinalIdr,
                    LocalPrice = Local,
                    LocalCurrencyCd = LocalCurrency,
                    LocalRate = LocalCurrency.Rate,
                    InsertBy = "LunggoSystem",
                    InsertDate = DateTime.UtcNow,
                    InsertPgId = "0"
                });
            }
        }

        internal static Price GetFromDb(long orderId, out long orderRuleId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetPriceQuery.GetInstance().Execute(conn, new { OrderId = orderId }).Single();
                return new Price
                {
                    Supplier = record.SupplierPrice.GetValueOrDefault(),
                    SupplierCurrency = new Currency(record.SupplierCurrencyCd, record.SupplierRate.GetValueOrDefault()),
                    OriginalIdr = record.OriginalPriceIdr.GetValueOrDefault(),
                    MarginNominal = record.MarginNominal.GetValueOrDefault(),
                    FinalIdr = record.FinalPriceIdr.GetValueOrDefault(),
                    Local = record.LocalPrice.GetValueOrDefault(),
                    LocalCurrency = new Currency(record.LocalCurrencyCd, record.LocalRate.GetValueOrDefault()),
                    Margin = Margin.GetFromDb(record.MarginId.GetValueOrDefault(), out orderRuleId)
                };
            }
        }

        private class GetPriceQuery : QueryBase<GetPriceQuery, PriceTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT MarginId, SupplierPrice, SupplierCurrencyCd, SupplierRate, OriginalPriceIdr, MarginNominal, " +
                       "FinalPriceIdr, LocalPrice, LocalCurrencyCd, LocalRate " +
                       "FROM Price " +
                       "WHERE OrderId = @OrderId";
            }
        }

        public void SetSupplier(decimal price, Currency currency)
        {
            Supplier = price;
            SupplierCurrency = currency;
            OriginalIdr = price * currency.Rate;
        }

        public void CalculateFinal(Margin margin)
        {
            Margin = margin;
            if (Margin.IsFlat)
            {
                LocalCurrency = new Currency(OnlineContext.GetActiveCurrencyCode());
                FinalIdr = Margin.Constant*Margin.Currency.Rate;
                Local = FinalIdr/LocalCurrency.Rate;
                MarginNominal = FinalIdr - OriginalIdr;
            }
            else
            {
                LocalCurrency = new Currency(OnlineContext.GetActiveCurrencyCode());
                var percentageMarginIdr = OriginalIdr*Margin.Percentage/100M;
                var constantMarginIdr = Margin.Constant*Margin.Currency.Rate;
                var semiFinalIdr = OriginalIdr + percentageMarginIdr + constantMarginIdr;
                var semiLocalIdr = semiFinalIdr/LocalCurrency.Rate;
                var localRounding = (LocalCurrency.RoundingOrder - (semiLocalIdr%LocalCurrency.RoundingOrder))%
                                     LocalCurrency.RoundingOrder;
                Local = semiLocalIdr + localRounding;
                Rounding = localRounding*LocalCurrency.Rate;
                FinalIdr = Local*LocalCurrency.Rate;
                MarginNominal = FinalIdr - OriginalIdr;
            }
        }
    }
}
