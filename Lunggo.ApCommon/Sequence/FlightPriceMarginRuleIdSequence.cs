using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class FlightPriceMarginRuleIdSequence : SequenceBase
    {
        private static readonly FlightPriceMarginRuleIdSequence Instance = new FlightPriceMarginRuleIdSequence();
        private readonly SequenceProperties _properties;

        private FlightPriceMarginRuleIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightPriceMarginRuleIdSequence",
                InitialValue = 747
            };
            Init(_properties);
        }

        public static FlightPriceMarginRuleIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
