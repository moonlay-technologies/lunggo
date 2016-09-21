using System.Globalization;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class MarginIdSequence : SequenceBase
    {
        private static readonly MarginIdSequence Instance = new MarginIdSequence();
        private readonly SequenceProperties _properties;

        private MarginIdSequence()
        {
            _properties = new SequenceProperties
            {
                Name = "MarginIdSequence",
                InitialValue = 20000
            };
            Init(_properties);
        }

        public static MarginIdSequence GetInstance()
        {
            return Instance;
        }

        public override long GetNext()
        {
            return GetNextNumber(_properties);
        }

        public long GetNext(ProductType productType)
        {
            var number = GetNextNumber(_properties);
            var numberString = (int) productType + number.ToString(CultureInfo.InvariantCulture);
            return long.Parse(numberString);
        }
    }
}
