using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using CsQuery.ExtensionMethods.Internal;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;

using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Redis;
using Lunggo.Framework.TableStorage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public SearchFlightOutput SearchFlight(SearchFlightInput input)
        {
            var output = new SearchFlightOutput();
            var searchId = EncodeConditions(input.Conditions);

            if (input.RequestedSupplierIds.IsNullOrEmpty())
            {
                output.IsSuccess = true;
                output.TotalSupplier = Suppliers.Count;
                output.Itineraries = new List<FlightItineraryForDisplay>();
                output.SearchedSuppliers = new List<int>();
                output.SearchId = searchId;
            }
            else
            {
                var searchedItins = GetSearchedSupplierItineraries(searchId, input.RequestedSupplierIds);
                var searchedSuppliers = searchedItins.Keys.ToList();
                var missingSuppliers = input.RequestedSupplierIds.Except(searchedSuppliers).ToList();
                foreach (var missingSupplier in missingSuppliers)
                {
                    var isSearching = GetSearchingStatusInCache(searchId, missingSupplier);
                    if (!isSearching)
                    {
                        var queue = QueueService.GetInstance().GetQueueByReference("FlightCrawl" + missingSupplier);
                        queue.AddMessage(new CloudQueueMessage(searchId));
                    }
                }

                var itinsForDisplay =
                    searchedItins.SelectMany(dict => dict.Value).Select(ConvertToItineraryForDisplay).ToList();
                itinsForDisplay.ForEach(itin => itin.SearchId = output.SearchId);

                var expiry = searchedSuppliers.Select(supplier => GetSearchedItinerariesExpiry(searchId, supplier)).Min();

                output.IsSuccess = true;
                output.SearchId = searchId;
                output.ExpiryTime = expiry;
                output.Itineraries = itinsForDisplay;
                output.SearchedSuppliers = searchedSuppliers;
                output.TotalSupplier = Suppliers.Count;
            }

            return output;
        }

        public void CommenceSearchFlight(string searchId, int supplierIndex)
        {
            var conditions = DecodeConditions(searchId);
            SearchFlightInternal(conditions, supplierIndex);
        }
    }
}
