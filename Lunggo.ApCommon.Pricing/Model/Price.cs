namespace Lunggo.ApCommon.Pricing.Model
{
    internal class Price
    {
        internal decimal Supplier { get; set; }
        internal string SupplierCurrency { get; set; }
        internal decimal SupplierRate { get; set; }
        internal decimal OriginalIdr { get; set; }
        internal Margin Margin { get; set; }
        internal decimal MarginNominal { get; set; }
        internal decimal FinalIdr { get; set; }
        internal decimal Local { get; set; }
        internal string LocalCurrency { get; set; }
        internal decimal LocalRate { get; set; }
    }
}
