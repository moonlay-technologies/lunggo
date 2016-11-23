using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Extension;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.TableStorage;
using Microsoft.WindowsAzure.Storage.Table;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public void SaveHotelLocationInStorage(string destination, string zoneOrArea, int hotelCode)
        {
            var table = TableStorageService.GetInstance().GetTableByReference("hotelLocations");
            var retrieveOp = TableOperation.Retrieve<DynamicTableEntity>(destination, zoneOrArea);
            var retrievedRow = (DynamicTableEntity) table.Execute(retrieveOp).Result;
            var hotelCodes = retrievedRow == null ? null : retrievedRow.Properties["HotelCode"].StringValue;
            var updatedHotelCodes = string.IsNullOrEmpty(hotelCodes) ? hotelCode.ToString(CultureInfo.InvariantCulture) : string.Join(",", hotelCodes, hotelCode);
            var row = new DynamicTableEntity(destination, zoneOrArea, null, new Dictionary<string, EntityProperty>
            {
                {"HotelCode", new EntityProperty(updatedHotelCodes)}
            });
            TableStorageService.GetInstance().InsertOrReplaceEntityToTableStorage(row, "hotelLocations");
        }

        public List<int> GetHotelListByLocationFromStorage(string location)
        {
            var table = TableStorageService.GetInstance().GetTableByReference("hotelLocations");
            var splitLocation = location.Split('-');
            if (splitLocation.Length > 1)
            {
                var destination = splitLocation[0];
                var retrieveOp = TableOperation.Retrieve<DynamicTableEntity>(destination, location);
                var retrievedRow = (DynamicTableEntity)table.Execute(retrieveOp).Result;
                return retrievedRow == null ? 
                    new List<int>() : 
                    retrievedRow.Properties["HotelCode"].StringValue.Split(',').Select(int.Parse).ToList();
            }
            else
            {
                var query =
                    new TableQuery<DynamicTableEntity>()
                        .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, location))
                        .Select(new[] { "HotelCode" });
                var hotelCodes =
                    table.ExecuteQuery(query).Select(r => r["HotelCode"].StringValue.Split(',')).ToList();
                return hotelCodes.SelectMany(s => s).Select(int.Parse).ToList();
            }
        }

        public void SaveHotelDetailByLocation()
        {
            var location = GetAllLocations();
            foreach (var loc in location)
            {
                Console.WriteLine("location: " + loc);
                var hotelCds = GetHotelListByLocationFromStorage(loc);
                var hotelDetailDict = hotelCds.ToDictionary(hotelCd => hotelCd, GetHotelDetailFromTableStorage);

                BlobStorageService.GetInstance().WriteFileToBlob(new BlobWriteDto
                    {
                        FileBlobModel = new FileBlobModel
                        {
                            Container = "HotelDetailByLocation",
                            FileInfo = new FileInfo
                            {
                                ContentType = "",
                                FileData = hotelDetailDict.ToCacheObject(),
                                FileName = loc
                            }
                        },
                        SaveMethod = SaveMethod.Force
                    });
                Console.WriteLine("Done saving hotel details for location: " + loc);
            }
        }

        public Dictionary<int, HotelDetailsBase> GetHotelDetailByLocation(string location)
        {
            var blob = BlobStorageService.GetInstance().GetByteArrayByFileInContainer(location, "HotelDetailByLocation");
            var value = (RedisValue) blob;
            return value.DeconvertTo<Dictionary<int, HotelDetailsBase>>();
        }

        public void SaveHotelDetailToTableStorage(HotelDetailsBase hotelDetail, int hotelCd)
        {
            HotelDetailWrapper hotel = new HotelDetailWrapper("hotel", hotelCd.ToString());
            var data = hotelDetail.Serialize();
            var splittedData = SplitByLength(data, 30000);
            DivideData(hotel, splittedData);
            var tableClient = TableStorageService.GetInstance();
            var table = tableClient.GetTableByReference("hoteldetail");
            var insertOp = TableOperation.InsertOrReplace(hotel);
            table.Execute(insertOp);
        }

        public void SaveAutoCompleteToTableStorage()
        {
            //var autocomplete = new Autocomplete();
            var tableClient = TableStorageService.GetInstance();
            var table = tableClient.GetTableByReference("hotelautocomplete");
            var hotelCdTable = tableClient.GetTableByReference("hotelCodeTable");

            long index = 1;
            var newValue = new HotelAutoComplete("hotelAutoComplete", index.ToString());
            foreach (var country in Countries)
            {
                foreach (var destination in country.Destinations)
                {
                    newValue.Id = index;
                    newValue.Code = destination.Code;
                    newValue.Destination = destination.Name;
                    newValue.Country = country.Name;
                    newValue.Type = 1;
                    newValue.HotelCount = GetHotelListByLocationFromStorage(destination.Code).Count;
                    var insertOp = TableOperation.InsertOrReplace(newValue);
                    table.Execute(insertOp);
                    Console.WriteLine("Success inserting index: " + index + " for destination: " + destination.Name);
                    index++;

                    foreach (var zone in destination.Zones)
                    {
                        newValue = new HotelAutoComplete("hotelAutoComplete", index.ToString());
                        newValue.Id = index;
                        newValue.Code = zone.Code;
                        newValue.Zone = zone.Name;
                        newValue.Destination = destination.Name;
                        newValue.Country = country.Name;
                        newValue.Type = 2;
                        newValue.HotelCount = GetHotelListByLocationFromStorage(zone.Code).Count;
                        insertOp = TableOperation.InsertOrReplace(newValue);
                        table.Execute(insertOp);
                        Console.WriteLine("Success inserting index: " + index + " for zoneCode: " + zone.Code);
                        index++;
                    }
                }
            }

            for (var i = 1; i < 600001; i++)
            {
                try
                {
                    var hotelDetail = GetHotelDetailFromTableStorage(i);
                    newValue = new HotelAutoComplete("hotelAutoComplete", index.ToString());
                    newValue.Id = index;
                    newValue.Code = hotelDetail.HotelCode.ToString();
                    newValue.HotelName = hotelDetail.HotelName;
                    newValue.Destination = GetDestinationNameFromDict(hotelDetail.DestinationCode).Name;
                    newValue.Zone = GetZoneNameFromDict(hotelDetail.DestinationCode + "-" + hotelDetail.ZoneCode);
                    newValue.Country = GetCountryNameFromDict(hotelDetail.CountryCode).Name;
                    newValue.Type = 4;
                    var insertOp = TableOperation.InsertOrReplace(newValue);
                    var hotelCode = new HotelCodeExist("hotelCodeExist", hotelDetail.HotelCode.ToString());
                    var insertHotelCd = TableOperation.InsertOrReplace(hotelCode);
                    table.Execute(insertOp);
                    Console.WriteLine("Success inserting index: " + index + " for hotelCode: " + hotelDetail.HotelCode);
                    hotelCdTable.Execute(insertHotelCd);
                    Console.WriteLine("Success inserting hotelCd to Table: " + hotelDetail.HotelCode);
                    index++;
                }
                catch
                {
                    
                }
                
            }
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

        public void SaveRateCommentToTableStorage(HotelRateComment hotelRateComment)
        {
            //var rowKey = hotelRateComment.Incoming + "|" + hotelRateComment.HotelCode + "|" + hotelRateComment.Code;
            var partitionKey = hotelRateComment.Incoming + "|" + hotelRateComment.Code + "|" + hotelRateComment.RateCode;
            var rowKey = hotelRateComment.DateStart.Ticks.ToString("d19");
            HotelDetailWrapper rateCom = new HotelDetailWrapper(partitionKey, rowKey);
            var data = hotelRateComment.Serialize();
            var splittedData = SplitByLength(data, 30000);
            DivideData(rateCom, splittedData);
            var tableClient = TableStorageService.GetInstance();
            //var table = tableClient.GetTableByReference("ratecomments");
            var table = tableClient.GetTableByReference("ratecomments");
            var insertOp = TableOperation.InsertOrReplace(rateCom);
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
            HotelDetailWrapper resultWrapper = (HotelDetailWrapper)retrievedResult.Result;
            var concatedResult = ConcateData(resultWrapper);
            HotelDetailsBase result = concatedResult.Deserialize<HotelDetailsBase>();
            return result;
        }

        public HotelAutoComplete GetAutoCompleteFromTableStorage(long code)
        {
            var partitionKey = "hotelAutoComplete";
            var rowKey = code.ToString();
            var tableClient = TableStorageService.GetInstance();
            CloudTable table = tableClient.GetTableByReference("hotelautocomplete");

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<HotelAutoComplete>(partitionKey, rowKey);

            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);
            HotelAutoComplete resultWrapper = (HotelAutoComplete)retrievedResult.Result;
            //var concatedResult = ConcateData(resultWrapper);
            //HotelAutoComplete result = resultWrapper.Deserialize<HotelAutoComplete>();
            return resultWrapper;
        }

        public List<HotelAutoComplete> GetAutocompleteBatch()
        {
            var table = TableStorageService.GetInstance().GetTableByReference("hotelautocomplete");
            var query =
                new TableQuery<DynamicTableEntity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,
                        "hotelAutoComplete"));
            var autocompletes = table.ExecuteQuery(query).Select(r => new HotelAutoComplete
            {
                Type = r["Type"].Int32Value.GetValueOrDefault(),
                HotelName = r.Properties.ContainsKey("HotelName") ? r["HotelName"].StringValue : "",
                Code = r["Code"].StringValue ,
                Destination = r["Destination"].StringValue,
                Zone = r.Properties.ContainsKey("Zone") ? r["Zone"].StringValue : "",
                Id = r["Id"].Int64Value.GetValueOrDefault(),
                Country = r["Country"].StringValue,
                HotelCount = r.Properties.ContainsKey("HotelCount") ? r["HotelCount"].Int32Value.GetValueOrDefault() : 0
            }).ToList();
             
            return autocompletes;
        }

        public List<HotelRateComment> GetRateCommentFromTableStorage(string rateComment, DateTime startDateTime)
        {
            //var partitionKey = "rateComments";
            //var rowKey = incoming + "|" + hotelCode + "|" + code;
            var partitionKey = rateComment;
            var rowKey = startDateTime.Ticks.ToString("d19");
            var tableClient = TableStorageService.GetInstance();
            CloudTable table = tableClient.GetTableByReference("ratecomments");
            TableQuery<HotelDetailWrapper> rangeQuery = new TableQuery<HotelDetailWrapper>().Where(
            TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
            TableOperators.And,
            TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual, rowKey)));
            List<HotelRateComment> result = new List<HotelRateComment>();
            var resultQuery = table.ExecuteQuery(rangeQuery);
            if (resultQuery != null)
            {
                foreach (var rateCom in resultQuery)
                {
                    var concatedResult = ConcateData(rateCom);
                    var rate= concatedResult.Deserialize<HotelRateComment>();
                    result.Add(rate);
                }
                var sortedResult = result.Where(x => x.DateEnd > startDateTime).Select(x => new HotelRateComment
                {
                  Code = x.Code,
                  DateEnd = x.DateEnd,
                  DateStart = x.DateStart,
                  Incoming = x.Incoming,
                  HotelCode = x.HotelCode,
                  RateCode = x.RateCode,
                  Description = x.Description
                }).ToList();
                return sortedResult;
            }
            else
            {
                return new List<HotelRateComment>();
            }
            
            //// Create a retrieve operation that takes a customer entity.
            //TableOperation retrieveOperation = TableOperation.Retrieve<HotelDetailWrapper>(partitionKey, rowKey);

            //// Execute the retrieve operation.
            //TableResult retrievedResult = table.Execute(retrieveOperation);
            //HotelDetailWrapper resultWrapper = (HotelDetailWrapper)retrievedResult.Result;
            //var concatedResult = ConcateData(resultWrapper);
            //HotelRateComment result = concatedResult.Deserialize<HotelRateComment>();
            //return result;
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
