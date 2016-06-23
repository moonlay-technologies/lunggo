using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class FlightItineraryCacheIdSequence : SequenceBase
    {
        private static readonly FlightItineraryCacheIdSequence Instance = new FlightItineraryCacheIdSequence();
        private readonly SequenceProperties _properties;

        private FlightItineraryCacheIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightItineraryCacheIdSequence",
                InitialValue = 7470
            };
            Init(_properties);
        }

        public static FlightItineraryCacheIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
