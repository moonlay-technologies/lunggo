using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class HotelBookingIdSequence : SequenceBase
    {
        private static readonly HotelBookingIdSequence Instance = new HotelBookingIdSequence();
        private readonly SequenceProperties _properties;

        private HotelBookingIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "HotelBookingIdSequence",
                InitialValue = 20000
            };
            Init(_properties);
        }

        public static HotelBookingIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties)*23456789;
        }
    }
}
