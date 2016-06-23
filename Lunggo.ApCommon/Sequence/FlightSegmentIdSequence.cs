﻿using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class FlightSegmentIdSequence : SequenceBase
    {
        private static readonly FlightSegmentIdSequence Instance = new FlightSegmentIdSequence();
        private readonly SequenceProperties _properties;

        private FlightSegmentIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "FlightSegmentIdSequence",
                InitialValue = 7470
            };
            Init(_properties);
        }

        public static FlightSegmentIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
