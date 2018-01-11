using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class GetActivityTicketDetailInput
    {
        public long ActivityId { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
    }
}
