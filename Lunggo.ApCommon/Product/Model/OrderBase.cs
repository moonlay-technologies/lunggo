using System;

namespace Lunggo.ApCommon.Product.Model
{
    public abstract class OrderBase
    {
        
        public Price Price { get; set; }
        public DateTime TimeLimit { get; set; }

        internal abstract decimal GetApparentOriginalPrice();
    }
}
