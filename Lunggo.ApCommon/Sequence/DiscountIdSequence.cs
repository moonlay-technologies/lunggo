using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class DiscountIdSequence : SequenceBase
    {
        private static readonly DiscountIdSequence Instance = new DiscountIdSequence();
        private readonly SequenceProperties _properties;

        private DiscountIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "DiscountIdSequence",
                InitialValue = 20000
            };
            Init(_properties);
        }

        public static DiscountIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
