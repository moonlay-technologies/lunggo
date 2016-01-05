using System;
using Lunggo.ApCommon.Constant;
using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class FlightRsvNoSequence : SequenceBase
    {
        private static readonly FlightRsvNoSequence Instance = new FlightRsvNoSequence();
        private readonly SequenceProperties _properties;


        private FlightRsvNoSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightRsvNoSequence",
                InitialValue = 6532679
            };
            Init(_properties);
        }

        public static FlightRsvNoSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            var currentYear = DateTime.UtcNow.Year;
            var relativeYear = currentYear - 2015;
            var currentDay = DateTime.UtcNow.DayOfYear;
            var encodedDate = (27*(currentDay-1)+relativeYear);
            var nextRawId = GetNextNumber(_properties)%10000000L;
            var nextId = encodedDate*10000000L + nextRawId;
            return nextId;
        }

        public string GetNextFlightRsvNo()
        {
            var id = GetNext();
            var rsvNo = RsvNoIdentifier.Flight + id.ToString("00000000000");
            return rsvNo;
        }
    }
}
