using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelDetailWrapper :TableEntity
    {
        public HotelDetailWrapper(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public HotelDetailWrapper()
        {
            
        }
        public string Data1 { get; set; }
        public string Data2 { get; set; }
    }
}
