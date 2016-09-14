using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class FlightTripIdSequence : SequenceBase
    {
        private static readonly FlightTripIdSequence Instance = new FlightTripIdSequence();
        private readonly SequenceProperties _properties;

        private FlightTripIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightTripIdSequence",
                InitialValue = 20000
            };
            Init(_properties);
        }

        public static FlightTripIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
