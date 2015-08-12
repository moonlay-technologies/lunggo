using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        private static readonly List<DiscountRule> DiscountRules = new List<DiscountRule>();
        private static readonly List<DiscountRule> DeletedDiscountRules = new List<DiscountRule>();

        internal void InitDiscountRules()
        {
            DiscountRules.Add(new DiscountRule
            {
                Name = "Default",
                Description = "This is the default Discount rules",
                ConstraintCount = 0,
                Coefficient = 0.07M,
                Constant = 0
            });
        }
    }
}
