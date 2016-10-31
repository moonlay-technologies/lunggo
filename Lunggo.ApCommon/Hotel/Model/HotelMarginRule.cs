using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelMarginRule
    {
        internal Margin Margin { get; set; }
        internal HotelRateRule Rule { get; set; }

        internal HotelMarginRule(Margin margin, HotelRateRule rule)
        {
            Margin = margin;
            Rule = rule;
        }
    }
}
