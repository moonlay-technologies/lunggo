using System;
using System.IO;
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
        private long Id { get; set; }

        public decimal Supplier { get; set; }
        public Currency SupplierCurrency { get; set; }
        public decimal OriginalIdr { get; set; }
        public UsedMargin Margin { get; set; }
        public decimal Rounding { get; set; }
        public decimal MarginNominal { get; set; }
        public decimal FinalIdr { get; set; }
        public decimal Local { get; set; }
        public Currency LocalCurrency { get; set; }

        public static Price operator +(Price price1, Price price2)
        {
            if (price1 == null || price2 == null)
                throw new ArgumentNullException();

            if (price1.SupplierCurrency.Symbol != price2.SupplierCurrency.Symbol ||
                price1.SupplierCurrency.Supplier != price2.SupplierCurrency.Supplier ||
                price1.SupplierCurrency.Rate != price2.SupplierCurrency.Rate ||
                price1.SupplierCurrency.RoundingOrder != price2.SupplierCurrency.RoundingOrder ||
                price1.LocalCurrency.Symbol != price2.LocalCurrency.Symbol ||
                price1.LocalCurrency.Supplier != price2.LocalCurrency.Supplier ||
                price1.LocalCurrency.Rate != price2.LocalCurrency.Rate ||
                price1.LocalCurrency.RoundingOrder != price2.LocalCurrency.RoundingOrder)
                throw new InvalidDataException(
                    "Supplier currency or local currency from both Price objects do not match.");

            return new Price
            {
                Supplier = price1.Supplier + price2.Supplier,
                SupplierCurrency = price1.SupplierCurrency,
                OriginalIdr = price1.OriginalIdr + price2.OriginalIdr,
                Margin = price1.Margin,
                MarginNominal = price1.MarginNominal + price2.MarginNominal,
                Rounding = price1.Rounding + price2.Rounding,
                FinalIdr = price1.FinalIdr + price2.FinalIdr,
                Local = price1.Local + price2.Local,
                LocalCurrency = price1.LocalCurrency
            };
        }

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

        internal void UpdateToDb()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                PriceTableRepo.GetInstance().Update(conn, new PriceTableRecord
                {
                    Id = Id,
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
                    UpdateBy = "LunggoSystem",
                    UpdateDate = DateTime.UtcNow,
                    UpdatePgId = "0"
                });
                Margin.UpdateToDb(Id);
            }
        }

        internal static Price GetFromDb(long priceId)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var record = GetPriceQuery.GetInstance().Execute(conn, new { Id = priceId }).SingleOrDefault();

                if (record == null)
                    return null;

                return new Price
                {
                    Id = priceId,
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

        private class GetPriceQuery : DbQueryBase<GetPriceQuery, PriceTableRecord>
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

        public void SetMargin(UsedMargin margin)
        {
            Margin = margin;
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

        internal decimal CalculateOriginalPrice()
        {
            if (OriginalIdr >= FinalIdr)
            {
                var originalLocal = OriginalIdr/LocalCurrency.Rate;
                var roundedOriginal = Math.Round(originalLocal/LocalCurrency.RoundingOrder)*LocalCurrency.RoundingOrder;
                return roundedOriginal;
            }
            else
            {
                var percentage = 1.05M + (FinalIdr % 11M) * 0.01M;
                var original = FinalIdr*percentage;
                var originalLocal = original / LocalCurrency.Rate;
                var roundedOriginal = Math.Round(originalLocal / LocalCurrency.RoundingOrder) * LocalCurrency.RoundingOrder;
                return roundedOriginal;
            }
        }
    }
}
