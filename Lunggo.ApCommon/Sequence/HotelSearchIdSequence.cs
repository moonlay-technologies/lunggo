using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class HotelSearchIdSequence : SequenceBase
    {
        private static readonly HotelSearchIdSequence Instance = new HotelSearchIdSequence();
        private readonly SequenceProperties _properties;

        private HotelSearchIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "HotelSearchIdSequence",
                InitialValue = 747
            };
            Init(_properties);
        }

        public static HotelSearchIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
