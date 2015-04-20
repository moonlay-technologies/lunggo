using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class FlightPassengerIdSequence : SequenceBase
    {
        private static readonly FlightPassengerIdSequence Instance = new FlightPassengerIdSequence();
        private readonly SequenceProperties _properties;

        private FlightPassengerIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightPassengerIdSequence",
                InitialValue = 747
            };
            Init(_properties);
        }

        public static FlightPassengerIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
