using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class TransferConfirmationReportIdSequence : SequenceBase
    {
        private static readonly TransferConfirmationReportIdSequence Instance = new TransferConfirmationReportIdSequence();
        private readonly SequenceProperties _properties;

        private TransferConfirmationReportIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "TransferConfirmationReportIdSequence",
                InitialValue = 747
            };
            Init(_properties);
        }

        public static TransferConfirmationReportIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
