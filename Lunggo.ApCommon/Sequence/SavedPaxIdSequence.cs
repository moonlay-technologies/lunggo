using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class SavedPaxIdSequence : SequenceBase
    {
        private static readonly SavedPaxIdSequence Instance = new SavedPaxIdSequence();
        private readonly SequenceProperties _properties;


        private SavedPaxIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "SavedPaxIdSequence",
                InitialValue = 1
            };
            Init(_properties);
        }

        public static SavedPaxIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
