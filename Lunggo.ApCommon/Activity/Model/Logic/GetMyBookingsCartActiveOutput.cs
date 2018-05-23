using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class GetMyBookingsCartActiveOutput
    {
        public List<TrxList> MyBookings { get; set; }
        public bool MustUpdate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
