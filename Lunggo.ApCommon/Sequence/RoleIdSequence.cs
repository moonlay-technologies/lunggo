using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class RoleIdSequence : SequenceBase
    {
        private static readonly RoleIdSequence Instance = new RoleIdSequence();
        private readonly SequenceProperties _properties;


        private RoleIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "RoleIdSequence",
                InitialValue = 1
            };
            Init(_properties);
        }

        public static RoleIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
