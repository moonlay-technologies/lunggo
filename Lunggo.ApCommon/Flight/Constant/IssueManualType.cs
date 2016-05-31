using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Constant
{
    public enum IssueManualType
    {
        ErrorSystem = 0,
        WorkingHourCase = 1,
        SundayOrNotWorkingHourCase = 2,
        FridayNotWorkingHourCase = 3,
        SaturdayCase = 4,
    }
}
