using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class HotelReservationIdSequence : SequenceBase
    {
        private static readonly HotelReservationIdSequence Instance = new HotelReservationIdSequence();
        private readonly SequenceProperties _properties;

        private HotelReservationIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightItineraryIdSequence",
                InitialValue = 20000
            };
            Init(_properties);
        }

        public static HotelReservationIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
