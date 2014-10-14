using Base36Encoder;
using Lunggo.ApCommon.Constant;
using Lunggo.Framework.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Sequence
{
    public class HotelReservationSequence : SequenceBase
    {
        private static readonly HotelReservationSequence Instance = new HotelReservationSequence();
        private readonly SequenceProperties _properties;


        private HotelReservationSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "HotelReservationSequence",
                InitialValue = 623697409
            };
            Init(_properties);
        }

        public static HotelReservationSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }

        public string GetHotelReservationId(EnumReservationType.ReservationType type)
        {
            object enumType = Convert.ChangeType(type, type.GetTypeCode());
            long Id = GetNext();
            string prefixReservationID = "H" + enumType.ToString();
            string ReservationID = string.Format("{0}{1}", prefixReservationID, Base36.Encode(Id));
            return ReservationID;
        }

    }
}
