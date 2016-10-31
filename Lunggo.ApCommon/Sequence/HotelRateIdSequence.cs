using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class HotelRateIdSequence : SequenceBase
    {
        private static readonly HotelRateIdSequence Instance = new HotelRateIdSequence();
        private readonly SequenceProperties _properties;

        private HotelRateIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightItineraryIdSequence",
                InitialValue = 20000
            };
            Init(_properties);
        }

        public static HotelRateIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
