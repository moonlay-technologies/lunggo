using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class FlightEticketIdSequence : SequenceBase
    {
        private static readonly FlightEticketIdSequence Instance = new FlightEticketIdSequence();
        private readonly SequenceProperties _properties;

        private FlightEticketIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightEticketIdSequence",
                InitialValue = 747
            };
            Init(_properties);
        }

        public static FlightEticketIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
