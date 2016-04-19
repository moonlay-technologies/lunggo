namespace Lunggo.ApCommon.ProductBase.Model
{
    public class Price
    {
        public decimal Supplier { get; set; }
        public Currency SupplierCurrency { get; set; }
        public decimal OriginalIdr { get; set; }
        public Margin Margin { get; set; }
        public decimal MarginNominal { get; set; }
        public decimal FinalIdr { get; set; }
        public decimal Local { get; set; }
        public Currency LocalCurrency { get; set; }
    }
}
