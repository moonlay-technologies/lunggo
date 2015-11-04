using System;
using Base36Encoder;
using Lunggo.ApCommon.Constant;
using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class MemberIdCardSequence : SequenceBase
    {
        private static readonly MemberIdCardSequence Instance = new MemberIdCardSequence();
        private readonly SequenceProperties _properties;


        private MemberIdCardSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "MemberIdCardSequence",
                InitialValue = 1
            };
            Init(_properties);
        }

        public static MemberIdCardSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }

        public string GetFlightReservationId(EnumReservationType.ReservationType type)
        {
            object enumType = Convert.ChangeType(type, type.GetTypeCode());
            long Id = GetNext();
            string prefixReservationID = "F" + enumType.ToString();
            string ReservationID = string.Format("{0}{1}", prefixReservationID, Base36.Encode(Id));
            return ReservationID;
        }

    }
}
