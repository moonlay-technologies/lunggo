using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommon.Campaign.Model
{
    public class BinMethodDiscount
    {
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public string DisplayName { get; set; }
        public bool IsAvailable { get; set; }
        public bool ReplaceMargin { get; set; }
    }
}
