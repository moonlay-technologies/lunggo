using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class FlightItineraryIdSequence : SequenceBase
    {
        private static readonly FlightItineraryIdSequence Instance = new FlightItineraryIdSequence();
        private readonly SequenceProperties _properties;

        private FlightItineraryIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightItineraryIdSequence",
                InitialValue = 20000
            };
            Init(_properties);
        }

        public static FlightItineraryIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
