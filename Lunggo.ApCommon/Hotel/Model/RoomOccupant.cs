using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class RoomOccupant
    {
        public int AdultCount { get; set; }
        public int[] ChildrenAges { get; set; }
    }
}
