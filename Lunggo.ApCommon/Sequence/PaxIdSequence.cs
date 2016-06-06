using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class PaxIdSequence : SequenceBase
    {
        private static readonly PaxIdSequence Instance = new PaxIdSequence();
        private readonly SequenceProperties _properties;

        private PaxIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "PaxIdSequence",
                InitialValue = 747
            };
            Init(_properties);
        }

        public static PaxIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
