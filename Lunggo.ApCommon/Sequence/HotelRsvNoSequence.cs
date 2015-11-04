using System;
using Lunggo.ApCommon.Constant;
using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class HotelRsvNoSequence : SequenceBase
    {
        private static readonly HotelRsvNoSequence Instance = new HotelRsvNoSequence();
        private readonly SequenceProperties _properties;


        private HotelRsvNoSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "HotelRsvNoSequence",
                InitialValue = 6532679
            };
            Init(_properties);
        }

        public static HotelRsvNoSequence GetInstance()
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
            var nextId = encodedDate + nextRawId;
            return nextId;
        }

        public string GetNextHotelRsvNo()
        {
            var id = GetNext();
            var rsvNo = RsvNoIdentifier.Hotel + id;
            return rsvNo;
        }
    }
}
