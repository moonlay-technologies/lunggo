using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class InsertRatingAndReviewInput
    {
        public string UserId { get; set; }
        public string RsvNo { get; set; }
        public string Review { get; set; }
        public int Rating { get; set; }
    }
}
