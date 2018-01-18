using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class WishlistAnalyticsInput : TableEntity
    {
        public string UserId { get; set; }
        public long ActivityId { get; set; }
        public DateTime WishlistTime { get; set; }
    }
}
