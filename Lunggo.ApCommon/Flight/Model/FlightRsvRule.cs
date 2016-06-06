using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;

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
