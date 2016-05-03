using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.ProductBase.Constant
{
    public enum ProductType
    {
        Undefined = 0,
        Flight = 1,
        Hotel = 2,
        Activity = 3
    }

    internal class ProductTypeCd
    {
        internal static ProductType Parse(string rsvNo)
        {
            return (ProductType) int.Parse(rsvNo.Substring(0, 1));
        }
    }
}
