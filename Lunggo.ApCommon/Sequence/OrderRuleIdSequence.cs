using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class OrderRuleIdSequence : SequenceBase
    {
        private static readonly OrderRuleIdSequence Instance = new OrderRuleIdSequence();
        private readonly SequenceProperties _properties;

        private OrderRuleIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "OrderRuleIdSequence",
                InitialValue = 1
            };
            Init(_properties);
        }

        public static OrderRuleIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
