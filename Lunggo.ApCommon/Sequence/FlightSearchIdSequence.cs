using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class FlightSearchIdSequence : SequenceBase
    {
        private static readonly FlightSearchIdSequence Instance = new FlightSearchIdSequence();
        private readonly SequenceProperties _properties;

        private FlightSearchIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightSearchIdSequence",
                InitialValue = 747
            };
            Init(_properties);
        }

        public static FlightSearchIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
