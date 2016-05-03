using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.ApCommon.ProductBase.Model;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightRsvRule : RsvRuleBase<FlightItineraryRule>
    {
        protected override ProductType Type
        {
            get { return ProductType.Flight; }
        }
    }
}
