using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class InsertActivityReviewInput
    {
        public string UserId { get; set; }
        public string RsvNo { get; set; }
        public string Review { get; set; }
        public long? ActivityId { get; set; }
        public DateTime Date { get; set; }
    }
}
