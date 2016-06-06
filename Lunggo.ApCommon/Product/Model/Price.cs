using System;
using System.Linq;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Product.Model
{
    public class Price
    {
        public decimal Supplier { get; set; }
        public Currency SupplierCurrency { get; set; }
        public decimal OriginalIdr { get; set; }
        public UsedMargin Margin { get; set; }
        public decimal Rounding { get; set; }
        public decimal MarginNominal { get; set; }
        public decimal FinalIdr { get; set; }
        public decimal Local { get; set; }
        public Currency LocalCurrency { get; set; }

        internal long InsertToDb()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var priceId = PriceIdSequence.GetInstance().GetNext();

                PriceTableRepo.GetInstance().Insert(conn, new PriceTableRecord
                {
                    Id = priceId,
                    SupplierPrice = Supplier,
                    SupplierCurrencyCd = SupplierCurrency,
                    SupplierRate = SupplierCurrency.Rate,
                    OriginalPriceIdr = OriginalIdr,
                    MarginNominal = MarginNominal,
                    Rounding = Rounding,
                    FinalPriceIdr = FinalIdr,
                    LocalPrice = Local,
                    LocalCurrencyCd = LocalCurrency,
                    LocalRate = LocalCurrency.Rate,
                    InsertBy = "LunggoSystem",
                    InsertDate = DateTime.UtcNow,
                    InsertPgId = "0"
                });
                Margin.InsertToDb(priceId);
                return priceId;
            }
        }

        internal static Price GetFromDb(long priceId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetPriceQuery.GetInstance().Execute(conn, new { Id = priceId }).Single();
                return new Price
                {
                    Supplier = record.SupplierPrice.GetValueOrDefault(),
                    SupplierCurrency = new Currency(record.SupplierCurrencyCd, record.SupplierRate.GetValueOrDefault()),
                    OriginalIdr = record.OriginalPriceIdr.GetValueOrDefault(),
                    MarginNominal = record.MarginNominal.GetValueOrDefault(),
                    Rounding = record.Rounding.GetValueOrDefault(),
                    FinalIdr = record.FinalPriceIdr.GetValueOrDefault(),
                    Local = record.LocalPrice.GetValueOrDefault(),
                    LocalCurrency = new Currency(record.LocalCurrencyCd, record.LocalRate.GetValueOrDefault()),
                    Margin = UsedMargin.GetFromDb(priceId),  
                };
            }
        }

        private class GetPriceQuery : QueryBase<GetPriceQuery, PriceTableRecord>
        {
            protected override string GetQuery(dynamic condition = null)
            {
                return "SELECT SupplierPrice, SupplierCurrencyCd, SupplierRate, OriginalPriceIdr, MarginNominal, " +
                       "Rounding, FinalPriceIdr, LocalPrice, LocalCurrencyCd, LocalRate " +
                       "FROM Price " +
                       "WHERE Id = @Id";
            }
        }

        public void SetSupplier(decimal price, Currency currency)
        {
            Supplier = price;
            SupplierCurrency = currency;
            OriginalIdr = price * currency.Rate;
        }

        public void SetMargin(Margin margin)
        {
            Margin = new UsedMargin
            {
                Name = margin.Name,
                Description = margin.Description,
                Percentage = margin.Percentage,
                Constant = margin.Constant,
                Currency = margin.Currency,
                IsFlat = margin.IsFlat
            };
        }

        public void CalculateFinalAndLocal(Currency localCurrency)
        {
            if (Margin.IsFlat)
            {
                LocalCurrency = localCurrency;
                FinalIdr = Margin.Constant * Margin.Currency.Rate;
                Local = FinalIdr / LocalCurrency.Rate;
                MarginNominal = FinalIdr - OriginalIdr;
            }
            else
            {
                LocalCurrency = localCurrency;
                var percentageMarginIdr = OriginalIdr * Margin.Percentage / 100M;
                var constantMarginIdr = Margin.Constant * Margin.Currency.Rate;
                var semiFinalIdr = OriginalIdr + percentageMarginIdr + constantMarginIdr;
                var semiLocalIdr = semiFinalIdr / LocalCurrency.Rate;
                var localRounding = (LocalCurrency.RoundingOrder - (semiLocalIdr % LocalCurrency.RoundingOrder)) %
                                     LocalCurrency.RoundingOrder;
                Local = semiLocalIdr + localRounding;
                Rounding = localRounding * LocalCurrency.Rate;
                FinalIdr = Local * LocalCurrency.Rate;
                MarginNominal = FinalIdr - OriginalIdr;
            }
        }
    }
}
