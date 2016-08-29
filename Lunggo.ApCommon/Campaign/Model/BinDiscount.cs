using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommon.Campaign.Model
{
    public class BinDiscount
    {
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public string DisplayName { get; set; }
    }
}
