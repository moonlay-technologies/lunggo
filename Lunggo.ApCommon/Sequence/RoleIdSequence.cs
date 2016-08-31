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
                InitialValue = 20000
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
