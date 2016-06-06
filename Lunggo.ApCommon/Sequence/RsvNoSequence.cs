using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.Framework.Sequence;

namespace Lunggo.ApCommon.Sequence
{
    public class RsvNoSequence : SequenceBase
    {
        private static readonly RsvNoSequence Instance = new RsvNoSequence();
        private readonly SequenceProperties[] _properties;
        private readonly string[] _productTypes = Enum.GetNames(typeof(ProductType));

        private RsvNoSequence()
        {
            _properties = _productTypes.Select(type =>
            {
                var prop = new SequenceProperties
                {
                    Name = type + "RsvNoSequence",
                    InitialValue = 6532679
                };
                Init(prop);
                return prop;
            }).ToArray();
        }

        public static RsvNoSequence GetInstance()
        {
            return Instance;
        }

        [Obsolete]
        public override long GetNext()
        {
            throw new NotSupportedException();
        }

        public string GetNext(ProductType productType)
        {
            var currentYear = DateTime.UtcNow.Year;
            var relativeYear = currentYear - 2015;
            var currentDay = DateTime.UtcNow.DayOfYear;
            var encodedDate = (27 * (currentDay - 1) + relativeYear);
            var nextRawId = GetNextNumber(_properties[(int)productType]) % 10000000L;
            var nextId = encodedDate * 10000000L + nextRawId;
            var rsvNo = (int)productType + nextId.ToString("00000000000");
            return rsvNo;
        }
    }
}
