using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class PaymentIdSequence : SequenceBase
    {
        private static readonly PaymentIdSequence Instance = new PaymentIdSequence();
        private readonly SequenceProperties _properties;

        private PaymentIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "PaymentIdSequence",
                InitialValue = 1000
            };
            Init(_properties);
        }

        public static PaymentIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
