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
            var splittedData = SplitByLength(data, 30000);
            DivideData(hotel,splittedData);
            var tableClient = TableStorageService.GetInstance();
            var table = tableClient.GetTableByReference("hoteldetail");
            var insertOp = TableOperation.InsertOrReplace(hotel);
            table.Execute(insertOp);
        }

        public void SaveTruncatedHotelDetailToTableStorage(HotelDetailsBase hotelDetail, int hotelCd)
        {
            HotelDetailWrapper hotel = new HotelDetailWrapper("hotel", hotelCd.ToString());
            var data = hotelDetail.Serialize();
            var splittedData = SplitByLength(data, 30000);
            DivideData(hotel, splittedData);
            var tableClient = TableStorageService.GetInstance();
            var table = tableClient.GetTableByReference("hoteldetailtrunc");
            var insertOp = TableOperation.InsertOrReplace(hotel);
            table.Execute(insertOp);
        }

        public void SaveHotelAmenitiesAndAccomodationTypeToTableStorage(HotelDetailsBase hotelDetail, int hotelCd)
        {
            HotelDetailWrapper hotel = new HotelDetailWrapper("hotel", hotelCd.ToString());
            var data = hotelDetail.Serialize();
            var splittedData = SplitByLength(data, 30000);
            DivideData(hotel, splittedData);
            var tableClient = TableStorageService.GetInstance();
            var table = tableClient.GetTableByReference("amenitiesacc");
            var insertOp = TableOperation.InsertOrReplace(hotel);
            table.Execute(insertOp);
        }
        public void DivideData(HotelDetailWrapper hotel, IEnumerable<string> splittedData)
        {
            try
            {
                var separatedData = splittedData.ToList();
                hotel.Data1 = separatedData[0];
                hotel.Data2 = separatedData[1];
                hotel.Data3 = separatedData[2];
                hotel.Data4 = separatedData[3];
                hotel.Data5 = separatedData[4];
                hotel.Data6 = separatedData[5];
                hotel.Data7 = separatedData[6];
                hotel.Data8 = separatedData[7];
                hotel.Data9 = separatedData[8];
                hotel.Data10 = separatedData[9];
                hotel.Data11 = separatedData[10];
                hotel.Data12 = separatedData[11];
                hotel.Data13 = separatedData[12];
                hotel.Data14 = separatedData[13];
                hotel.Data15 = separatedData[14];
                hotel.Data16 = separatedData[15];
                hotel.Data17 = separatedData[16];
                hotel.Data18 = separatedData[17];
                hotel.Data19 = separatedData[18];
                hotel.Data20 = separatedData[19];
            }
            catch (Exception e)
            {
                
            }
        }

        public static IEnumerable<string> SplitByLength(string str, int maxLength)
        {
            int index = 0;
            while (index + maxLength < str.Length)
            {
                yield return str.Substring(index, maxLength);
                index += maxLength;
            }

            yield return str.Substring(index);
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
            var concatedResult = ConcateData(resultWrapper);
            HotelDetailsBase result = concatedResult.Deserialize<HotelDetailsBase>();
            return result;
        }

        public HotelDetailsBase GetTruncatedHotelDetailFromTableStorage(int hotelCd)
        {
            var partitionKey = "hotel";
            var rowKey = hotelCd.ToString();
            var tableClient = TableStorageService.GetInstance();
            CloudTable table = tableClient.GetTableByReference("hoteldetailtrunc");

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<HotelDetailWrapper>(partitionKey, rowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);
            HotelDetailWrapper resultWrapper = (HotelDetailWrapper)retrievedResult.Result;
            var concatedResult = ConcateData(resultWrapper);
            HotelDetailsBase result = concatedResult.Deserialize<HotelDetailsBase>();
            return result;
        }

        public HotelDetailsBase GetHotelAmenitiesAndAccomodationTypeFromTableStorage(int hotelCd)
        {
            var partitionKey = "hotel";
            var rowKey = hotelCd.ToString();
            var tableClient = TableStorageService.GetInstance();
            CloudTable table = tableClient.GetTableByReference("amenitiesacc");

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<HotelDetailWrapper>(partitionKey, rowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);
            HotelDetailWrapper resultWrapper = (HotelDetailWrapper)retrievedResult.Result;
            var concatedResult = ConcateData(resultWrapper);
            HotelDetailsBase result = concatedResult.Deserialize<HotelDetailsBase>();
            return result;
        }
        public HotelDetailsBase GetHotelDetailFromTableStorage()
        {
            var tableClient = TableStorageService.GetInstance();
            CloudTable table = tableClient.GetTableByReference("hoteldetail");
            var entities = table.ExecuteQuery(new TableQuery<HotelDetailWrapper>()).ToList();
            return new HotelDetailsBase();
        }

        public string ConcateData(HotelDetailWrapper hotel)
        {
            var data = new StringBuilder();
            try
            {
                data.Append(hotel.Data1);
                data.Append(hotel.Data2);
                data.Append(hotel.Data3);
                data.Append(hotel.Data4);
                data.Append(hotel.Data5);
                data.Append(hotel.Data6);
                data.Append(hotel.Data7);
                data.Append(hotel.Data8);
                data.Append(hotel.Data9);
                data.Append(hotel.Data10);
                data.Append(hotel.Data11);
                data.Append(hotel.Data12);
                data.Append(hotel.Data13);
                data.Append(hotel.Data14);
                data.Append(hotel.Data15);
                data.Append(hotel.Data16);
                data.Append(hotel.Data17);
                data.Append(hotel.Data18);
                data.Append(hotel.Data19);
                data.Append(hotel.Data20);
            }
            catch
            {
                
            }

            return data.ToString();

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
