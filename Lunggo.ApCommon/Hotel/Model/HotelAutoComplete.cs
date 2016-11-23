using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Service;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelAutoComplete : TableEntity
    {

        public HotelAutoComplete(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public HotelAutoComplete()
        {
            
        }

        public string HotelName { get; set; }
        public int Type { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public string Destination { get; set; }
        public string Zone { get; set; }
        public string Country { get; set; }
        public int HotelCount { get; set; }

    }

    


    public class HotelCodeExist : TableEntity
    {

        public HotelCodeExist(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public HotelCodeExist()
        {

        }

        public string HotelCode;
    }
}
