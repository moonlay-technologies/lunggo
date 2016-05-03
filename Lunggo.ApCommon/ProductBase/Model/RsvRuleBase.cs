using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.ApCommon.Sequence;

namespace Lunggo.ApCommon.ProductBase.Model
{
    public abstract class RsvRuleBase<TOrderRule> where TOrderRule : OrderRuleBase
    {
        protected abstract ProductType Type { get; }
        public long RuleId { get; internal set; }
        public int ConstraintCount { get; set; }
        public int Priority { get; set; }
        public TOrderRule OrderRule { get; set; }

        protected RsvRuleBase()
        {
            ConstraintCount = 0;
            Priority = int.MaxValue;
        }

        protected void GenerateRuleId()
        {
            var plainId = OrderRuleIdSequence.GetInstance().GetNext();
            var stringId = ((int) Type).ToString(CultureInfo.InvariantCulture) +
                     plainId.ToString(CultureInfo.InvariantCulture);
            RuleId = long.Parse(stringId);
        }
    }
}
