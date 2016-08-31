using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class PriceIdSequence : SequenceBase
    {
        private static readonly PriceIdSequence Instance = new PriceIdSequence();
        private readonly SequenceProperties _properties;

        private PriceIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "PriceIdSequence",
                InitialValue = 20000
            };
            Init(_properties);
        }

        public static PriceIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
