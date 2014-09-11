using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base36Encoder;
using Lunggo.ApCommon.Constant;
using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class FlightReservationSequence : SequenceBase
    {
        private static readonly FlightReservationSequence Instance = new FlightReservationSequence();
        private readonly SequenceProperties _properties;


        private FlightReservationSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightReservationSequence",
                InitialValue = 623697409
            };
            Init(_properties);
        }

        public static FlightReservationSequence GetInstance()
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
