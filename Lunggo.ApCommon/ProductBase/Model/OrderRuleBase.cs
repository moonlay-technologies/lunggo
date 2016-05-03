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
    public abstract class OrderRuleBase
    {
        private static Type[] _orderRules = ListOrderRules();

        private static Type[] ListOrderRules()
        {
            return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
             from assemblyType in domainAssembly.GetTypes()
             where typeof(OrderRuleBase).IsAssignableFrom(assemblyType)
             select assemblyType).OrderBy(type => type.GetProperty("Type")).ToArray();
        }

        protected abstract ProductType Type { get; }
        public long RuleId { get; internal set; }
        public int ConstraintCount { get; set; }
        public int Priority { get; set; }

        protected OrderRuleBase()
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
