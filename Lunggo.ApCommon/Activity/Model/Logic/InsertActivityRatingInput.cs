using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class InsertActivityRatingInput
    {
        public long? ActivityId { get; set; }
        public string UserId { get; set; }
        public string RsvNo { get; set; }
        public List<ActivityRatingAnswer> Answers { get; set; }
    }
}
