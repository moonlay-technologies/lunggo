using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class ActivityReservationIdSequence : SequenceBase
    {
        private static readonly ActivityReservationIdSequence Instance = new ActivityReservationIdSequence();
        private readonly SequenceProperties _properties;

        private ActivityReservationIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightItineraryIdSequence",
                InitialValue = 20000
            };
            Init(_properties);
        }

        public static ActivityReservationIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
