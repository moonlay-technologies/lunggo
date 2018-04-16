using Lunggo.Framework.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Sequence
{
    public class TrxNoSequence : SequenceBase
    {
        private static readonly TrxNoSequence Instance = new TrxNoSequence();
        private readonly SequenceProperties _properties;


        private TrxNoSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "TrxNoSequence",
                InitialValue = 0000000000
            };
            Init(_properties);
        }

        public static TrxNoSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
