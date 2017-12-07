using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class AccountNoSequence : SequenceBase
    {
        private static readonly AccountNoSequence Instance = new AccountNoSequence();
        private readonly SequenceProperties _properties;

        private AccountNoSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "AccountNoSequence",
                InitialValue = 462578045
            };
            Init(_properties);
        }

        public static AccountNoSequence GetInstance()
        {
            return Instance;
        }


        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
