using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Activity.Constant;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public bool ReleasePaxSlots(string rsvNo)
        {
            return ReleasePaxSlotsDb(rsvNo);
        }
    }
}
