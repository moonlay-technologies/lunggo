using System;
using Lunggo.ApCommon.Constant;
using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class ActivityRsvNoSequence : SequenceBase
    {
        private static readonly ActivityRsvNoSequence Instance = new ActivityRsvNoSequence();
        private readonly SequenceProperties _properties;


        private ActivityRsvNoSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "ActivityRsvNoSequence",
                InitialValue = 6532679
            };
            Init(_properties);
        }

        public static ActivityRsvNoSequence GetInstance()
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

        public string GetNextActivityRsvNo()
        {
            var id = GetNext();
            var rsvNo = RsvNoIdentifier.Activity + id;
            return rsvNo;
        }
    }
}
