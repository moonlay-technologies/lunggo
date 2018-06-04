using Lunggo.Framework.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Sequence
{
    public class TrxIdSequence : SequenceBase
    {
        private static readonly TrxIdSequence Instance = new TrxIdSequence();
        private readonly SequenceProperties _properties;


        private TrxIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "TrxIdSequence",
                InitialValue = 46829
            };
            Init(_properties);
        }

        public static TrxIdSequence GetInstance()
        {
            return Instance;
        }

        [Obsolete]
        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }

        public string GetNextTrxId()
        {
            var number = GetNextNumber(_properties);
            return "TRX" + number;
        }
    }
}
