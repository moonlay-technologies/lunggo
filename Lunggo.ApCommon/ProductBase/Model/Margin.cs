namespace Lunggo.ApCommon.ProductBase.Model
{
    public class Margin
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Coefficient { get; set; }
        public decimal Constant { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public bool IsFlat { get; set; }
    }
}
