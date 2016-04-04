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

    }
}
