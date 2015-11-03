using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class ActivityIDSequence : SequenceBase
    {
        private static readonly ActivityIDSequence Instance = new ActivityIDSequence();
        private readonly SequenceProperties _properties;

        private ActivityIDSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "ActivityIDSequence",
                InitialValue = 1
            };
            Init(_properties);
        }

        public static ActivityIDSequence GetInstance()
        {
            return Instance;
        }

         public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}