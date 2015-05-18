using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Payment.Constant
{
    public enum PaymentStatus 
    {
        Undefined = 0,
        Cancelled = 1,
        Pending = 2,
        BeingAuthorized = 3,
        Accepted = 4,
        Denied = 5,
        Error = 6
    }
}
