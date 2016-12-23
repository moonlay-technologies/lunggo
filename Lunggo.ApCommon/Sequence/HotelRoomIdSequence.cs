using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class HotelRoomIdSequence : SequenceBase
    {
        private static readonly HotelRoomIdSequence Instance = new HotelRoomIdSequence();
        private readonly SequenceProperties _properties;

        private HotelRoomIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightItineraryIdSequence",
                InitialValue = 20000
            };
            Init(_properties);
        }

        public static HotelRoomIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
