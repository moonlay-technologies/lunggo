using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.ProductBase.Model;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightMarginRule
    {
        internal Margin Margin { get; set; }
        internal FlightItineraryRule Rule { get; set; }

        internal FlightMarginRule(Margin margin, FlightItineraryRule rule)
        {
            Margin = margin;
            Rule = rule;
        }
    }
}
