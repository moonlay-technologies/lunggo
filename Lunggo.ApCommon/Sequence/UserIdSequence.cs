using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class UserIdSequence : SequenceBase
    {
        private static readonly UserIdSequence Instance = new UserIdSequence();
        private readonly SequenceProperties _properties;


        private UserIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "UserIdSequence",
                InitialValue = 1
            };
            Init(_properties);
        }

        public static UserIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
