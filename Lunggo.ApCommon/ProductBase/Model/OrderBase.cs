namespace Lunggo.ApCommon.ProductBase.Model
{
    public abstract class OrderBase
    {
        public Price Price { get; private set; }

        protected OrderBase()
        {
            Price = new Price();
        }
    }
}
