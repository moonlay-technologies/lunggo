using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    /**
     * 
     * This class is intended as example how to build class for sequence
     * 
     * */

    public class SampleSequence : SequenceBase
    {
        private static readonly SampleSequence Instance = new SampleSequence();
        private readonly SequenceProperties _properties;


        private SampleSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "SampleSequence",
                InitialValue = 1
            };
            Init(_properties);
        }

        public static SampleSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
