using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.Framework.Extension;
using Lunggo.Framework.TableStorage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public void SaveHotelDetailToTableStorage(HotelDetailsBase hotelDetail, int hotelCd)
        {
            HotelDetailWrapper hotel = new HotelDetailWrapper("hotel",hotelCd.ToString());
            var data = hotelDetail.Serialize();
            hotel.Data1 = data.Substring(0, (data.Length / 2));
            hotel.Data2 = data.Substring(data.Length/2);
            var tableClient = TableStorageService.GetInstance();
            var table = tableClient.GetTableByReference("hoteldetail");
            var insertOp = TableOperation.InsertOrReplace(hotel);
            table.Execute(insertOp);
        }

        public HotelDetailsBase GetHotelDetailFromTableStorage(int hotelCd)
        {
            var partitionKey = "hotel";
            var rowKey = hotelCd.ToString();
            var tableClient = TableStorageService.GetInstance();
            CloudTable table = tableClient.GetTableByReference("hoteldetail");

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<HotelDetailWrapper>(partitionKey, rowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);
            HotelDetailWrapper resultWrapper = (HotelDetailWrapper) retrievedResult.Result;
            var concatedResult = resultWrapper.Data1 + resultWrapper.Data2;
            HotelDetailsBase result = concatedResult.Deserialize<HotelDetailsBase>();
            return result;
        }

        public void DeleteEntityFromTableStorage(int hotelCd)
        {
            var partitionKey = "hotel";
            var rowKey = hotelCd.ToString();
            var tableClient = TableStorageService.GetInstance();
            CloudTable table = tableClient.GetTableByReference("hoteldetail");

            TableOperation retrieveOperation = TableOperation.Retrieve<HotelDetailWrapper>(partitionKey, rowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);
            HotelDetailWrapper deleteEntity = (HotelDetailWrapper)retrievedResult.Result;
            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                table.Execute(deleteOperation);
            }
        }
    }
}
