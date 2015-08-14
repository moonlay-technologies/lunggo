using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class VoucherSequence : SequenceBase
    {
        private static readonly VoucherSequence Instance = new VoucherSequence();
        private readonly SequenceProperties _properties;


        private VoucherSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "VoucherSequence",
                InitialValue = 1
            };
            Init(_properties);
        }

        public static VoucherSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
