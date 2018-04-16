using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class SearchAnalyticsInput : TableEntity
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
        public string SearchTime { get; set; }
    }
}
