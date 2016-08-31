using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class FlightBookingIdSequence : SequenceBase
    {
        private static readonly FlightBookingIdSequence Instance = new FlightBookingIdSequence();
        private readonly SequenceProperties _properties;

        private FlightBookingIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightBookingIdSequence",
                InitialValue = 20000
            };
            Init(_properties);
        }

        public static FlightBookingIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
