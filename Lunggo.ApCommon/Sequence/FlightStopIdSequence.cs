using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class FlightStopIdSequence : SequenceBase
    {
        private static readonly FlightStopIdSequence Instance = new FlightStopIdSequence();
        private readonly SequenceProperties _properties;

        private FlightStopIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightStopIdSequence",
                InitialValue = 7470
            };
            Init(_properties);
        }

        public static FlightStopIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
