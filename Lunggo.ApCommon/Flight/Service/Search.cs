using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
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

            var isCurrentlySearching = GetSearchingStatusInCache(searchId);
            var completeness = GetSearchingCompletenessInCache(searchId);
            if (!isCurrentlySearching && completeness == 0)
            {
                //var queue = QueueService.GetInstance().GetQueueByReference("FlightCrawl");
                //queue.AddMessage(new CloudQueueMessage(searchId));
                Task.Run(() => CommenceSearchFlight(searchId));
            }

            var searchedItins = new List<FlightItinerary>();

            if (completeness > input.Completeness) 
                searchedItins = GetSearchedItinerariesFromCache(searchId, input.Completeness);

            output.IsSuccess = true;
            output.Itineraries = searchedItins.Select(ConvertToItineraryForDisplay).ToList();
            output.SearchId = searchId;
            output.Itineraries.ForEach(itin => itin.SearchId = output.SearchId);
            output.ExpiryTime = GetSearchedItinerariesExpiry(searchId);
            output.Completeness = completeness;

            return output;
        }

        public void CommenceSearchFlight(string searchId)
        {
            var conditions = DecodeConditions(searchId);
            SearchFlightInternal(conditions);
        }
    }
}
