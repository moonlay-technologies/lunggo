using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class AppointmentRequestIdSequence : SequenceBase
    {
        private static readonly AppointmentRequestIdSequence Instance = new AppointmentRequestIdSequence();
        private readonly SequenceProperties _properties;

        private AppointmentRequestIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "AppointmentRequestSequence",
                InitialValue = 20000
            };
            Init(_properties);
        }

        public static AppointmentRequestIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }
    }
}
