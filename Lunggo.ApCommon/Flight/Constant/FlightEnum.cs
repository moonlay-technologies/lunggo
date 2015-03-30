using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Constant
{
    public enum CabinType
    {
        Economy = 0,
        Business = 1,
        First = 2
    }

    public enum PassengerType
    {
        Adult = 0,
        Child = 1,
        Infant = 2
    }

    public enum Gender
    {
        Male = 0,
        Female = 1
    }

    public enum Title
    {
        Mister = 0,
        Mistress = 1,
        Miss = 2
    }

    public enum BookingStatus
    {
        Confirmed = 0,
        Pending = 1
    }
}
