using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class ActivityCustomDateInput
    {
        public long ActivityId { get; set; }
        public DateTime CustomDate { get; set; }
        public string CustomHour { get; set; }
    }
}
