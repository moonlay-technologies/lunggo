using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsQuery.ExtensionMethods.Internal;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Config;
using Lunggo.Framework.Context;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public SearchFlightOutput SearchFlight(SearchFlightInput input)
        {
            var minuteTimeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));
            var timeout = DateTime.UtcNow.AddMinutes(minuteTimeout);
            var gottenSupplierCount = input.Progress / (100 / Suppliers.Count);
            var searchedSupplierIds = GetSearchedSupplierIndicesFromCache(input.SearchId);
            var requestedSupplierIds = searchedSupplierIds.Skip(gottenSupplierCount).ToList();
            var unsearchedSupplierIds = Suppliers.Keys.Except(searchedSupplierIds);

            var searchTimeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "searchTimeout"));

            var isSearching = GetSearchingStatusInCache(input.SearchId);
            if (!isSearching)
            {
                var priceCalendarQueue = QueueService.GetInstance().GetQueueByReference("FlightPriceCalendar");
                priceCalendarQueue.AddMessage(new CloudQueueMessage(input.SearchId), initialVisibilityDelay: new TimeSpan(0, 2, 0));
            }

            foreach (var unsearchedSupplierId in unsearchedSupplierIds)
            {
                var isSupplierSearching = GetSearchingStatusInCache(input.SearchId, unsearchedSupplierId);
                if (!isSupplierSearching)
                {
                    var queue = QueueService.GetInstance().GetQueueByReference("FlightCrawl" + unsearchedSupplierId);
                    queue.AddMessage(new CloudQueueMessage(input.SearchId + '|' + timeout), timeToLive: new TimeSpan(0, 0, searchTimeout));

                    var timeoutQueue = QueueService.GetInstance().GetQueueByReference("FlightCrawlTimeout" + unsearchedSupplierId);
                    timeoutQueue.AddMessage(new CloudQueueMessage(input.SearchId + '|' + timeout), initialVisibilityDelay: new TimeSpan(0, 0, searchTimeout));
                }
            }

            var output = new SearchFlightOutput
            {
                SearchId = input.SearchId
            };

            if (requestedSupplierIds.IsNullOrEmpty())
            {
                output.IsSuccess = true;
                output.ItineraryLists = new List<FlightItineraryForDisplay>[0];
                output.Progress = CalculateProgress(input.Progress, searchedSupplierIds.Count, Suppliers.Count);
            }
            else
            {
                var searchedSupplierItins = GetSearchedSupplierItineraries(input.SearchId, requestedSupplierIds);
                var searchedSuppliers = searchedSupplierItins.Keys.ToList();
                //var searchedItinLists = searchedSupplierItins.SelectMany(dict => dict.Value).ToList();
                var searchedItinLists = new List<List<FlightItinerary>>();
                for (var i = 0; i < searchedSupplierItins.First().Value.Count; i++)
                    searchedItinLists.Add(new List<FlightItinerary>());
                foreach (var searchedItins in searchedSupplierItins.Values)
                    for (var i = 0; i < searchedItins.Count; i++)
                        searchedItinLists[i].AddRange(searchedItins[i]);

                var currencies = GetCurrencyStatesFromCache(input.SearchId);
                var localCurrency = currencies[OnlineContext.GetActiveCurrencyCode()];
                searchedItinLists.ForEach(list => list.ForEach(itin => itin.Price.CalculateFinalAndLocal(localCurrency)));
                searchedItinLists.ForEach(list => list.ForEach(RoundFinalAndLocalPrice));

                var seachedItinListsForDisplay =
                    searchedItinLists.Skip(ParseTripType(input.SearchId) != TripType.OneWay ? 1 : 0).Select(lists => lists.Select(ConvertToItineraryForDisplay).ToList()).ToArray();

                //var combos = searchedSuppliers.SelectMany(sup => GetCombosFromCache(input.SearchId, sup)).ToList();
                //SetComboFare(seachedItinListsForDisplay, searchedItinLists[0], combos, localCurrency);

                var expiry = searchedSuppliers.Select(supplier => GetSearchedItinerariesExpiry(input.SearchId, supplier)).Min();
                output.IsSuccess = true;
                output.SearchId = input.SearchId;
                output.ExpiryTime = expiry;
                output.ItineraryLists = seachedItinListsForDisplay;
                //output.Combos = combos.Any() ? combos.Select(ConvertToComboForDisplay).ToList() : null;
                output.Progress = CalculateProgress(input.Progress, searchedSupplierIds.Count, Suppliers.Count);

                //if (output.Progress == 100)
                //{
                //    var tripType = ParseTripType(input.SearchId);
                //    if (tripType == TripType.OneWay)
                //    {
                //        SetLowestPriceToCache(searchedItinLists[0]);
                //    }
                //    else if (tripType == TripType.RoundTrip)
                //    {
                //        foreach (var itinList in searchedItinLists.Skip(1))
                //        {
                //            SetLowestPriceToCache(itinList);
                //        }
                //    }
                //}
            }

            return output;
        }

        public void CommenceSearchFlight(string searchId, int supplierIndex, DateTime searchTimeOut)
        {
            var conditions = DecodeSearchConditions(searchId);
            SearchFlightInternal(conditions, supplierIndex, searchTimeOut);
        }

        private void SearchFlightInternal(SearchFlightConditions conditions, int supplierIndex, DateTime searchflightTimeout)
        {
            var supplier = Suppliers[supplierIndex];

            var conditionsList = new List<SearchFlightConditions> { conditions };
            if (conditions.Trips.Count > 1)
                conditionsList.AddRange(conditions.Trips.Select(trip => new SearchFlightConditions
                {
                    AdultCount = conditions.AdultCount,
                    ChildCount = conditions.ChildCount,
                    InfantCount = conditions.InfantCount,
                    CabinClass = conditions.CabinClass,
                    AirlinePreferences = conditions.AirlinePreferences,
                    AirlineExcludes = conditions.AirlineExcludes,
                    Trips = new List<FlightTrip> { trip }
                }));

            var searchId = EncodeSearchConditions(conditions);
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));

            var searchTimeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "searchTimeout"));
            var searchCancellationSource = new CancellationTokenSource();
            var searchCancellation = searchCancellationSource.Token;

            var searchTask = Task.Run(() =>
            {
                Parallel.ForEach(conditionsList, partialConditions =>
                {
                    var result = supplier.SearchFlight(partialConditions);
                    result.Itineraries = result.Itineraries ?? new List<FlightItinerary>();
                    if (result.IsSuccess)
                        foreach (var itin in result.Itineraries)
                        {
                            itin.SearchId = searchId;
                        }

                    SaveSearchedPartialItinerariesToBufferCache(result.Itineraries, searchId, supplierIndex,
                        conditionsList.IndexOf(partialConditions));
                });
            }, searchCancellation);

            if (Task.WhenAny(searchTask, Task.Delay(new TimeSpan(0, 0, searchTimeout), searchCancellation)).Result == searchTask)
            {
                searchTask.Wait(searchCancellation);
            }
            else
            {
                searchCancellationSource.Cancel();
                Parallel.ForEach(conditionsList,
                    partialConditions =>
                        SaveSearchedPartialItinerariesToBufferCache(new List<FlightItinerary>(), searchId, supplierIndex, conditionsList.IndexOf(partialConditions)));
            }

            var itinLists = GetSearchedPartialItinerariesFromBufferCache(searchId, supplierIndex);
            var tripType = ParseTripType(searchId);
            foreach (var itinList in itinLists)
            {
                var sequenceNo = 0;
                foreach (var itin in itinList)
                {
                    itin.RegisterNumber = (supplierIndex * SupplierIndexCap) + sequenceNo++;
                    itin.RequestedTripType = tripType;
                }
            }
            if (conditions.Trips.Count > 1)
            {
                var combos = GenerateCombo(itinLists);
                SaveCombosToCache(combos, searchId, supplierIndex);
            }
            if (itinLists.Any())
            {
                foreach (var itinList in itinLists)
                {
                    AddPriceMargin(itinList);
                }
            }
            SaveSearchedItinerariesToCache(itinLists, searchId, timeout, supplierIndex, searchflightTimeout);
            SaveCurrencyStatesToCache(searchId, Currency.GetAllCurrencies(), timeout);
            SaveSearchedSupplierIndexToCache(searchId, supplierIndex, timeout);
            InvalidateSearchingStatusInCache(searchId, supplierIndex);
        }

        public void SetSearchAsEmptyIfNotSet(string searchIdWithTimeout, int supplierIndex)
        {
            var searchId = searchIdWithTimeout.Split('|')[0];
            var searchFlightTimeOut = DateTime.Parse(searchIdWithTimeout.Split('|')[1]);
            var searchExpiry = GetSearchedItinerariesExpiry(searchId, supplierIndex);
            if (searchExpiry == null)
            {
                var cacheTimeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));
                var emptyLists = new List<List<FlightItinerary>> { new List<FlightItinerary>() };

                var conditions = DecodeSearchConditions(searchId);
                var conditionsList = new List<SearchFlightConditions> { conditions };
                if (conditions.Trips.Count > 1)
                    conditionsList.ForEach(c => emptyLists.Add(new List<FlightItinerary>()));

                SaveSearchedItinerariesToCache(emptyLists, searchId, cacheTimeout, supplierIndex, searchFlightTimeOut);
                SaveCurrencyStatesToCache(searchId, Currency.GetAllCurrencies(), cacheTimeout);
                SaveSearchedSupplierIndexToCache(searchId, supplierIndex, cacheTimeout);
                InvalidateSearchingStatusInCache(searchId, supplierIndex);
            }
        }

        private void SetComboFare(List<FlightItineraryForDisplay>[] itinLists, List<FlightItinerary> bundledItins, List<Combo> combos, Currency localCurrency)
        {
            foreach (var combo in combos)
            {
                var bundledItin = bundledItins.Single(itin => itin.RegisterNumber == combo.BundledRegister);
                bundledItin.Price.CalculateFinalAndLocal(localCurrency);
                RoundFinalAndLocalPrice(bundledItin);
                var comboFare = bundledItin.Price.Local / combo.Registers.Length;
                for (var i = 0; i < combo.Registers.Length; i++)
                {
                    var reg = combo.Registers[i];
                    var identicalItin = itinLists[i].SingleOrDefault(itin => itin.RegisterNumber == reg);
                    if (identicalItin != null &&
                        (identicalItin.ComboFare == null ||
                         (identicalItin.ComboFare != null && identicalItin.ComboFare > comboFare)))
                        identicalItin.ComboFare = comboFare;
                }
            }
        }

        private static int CalculateProgress(int progress, int searchedSupplierCount, int totalSupplierCount)
        {
            var currentProgress = searchedSupplierCount * 100 / totalSupplierCount;
            if (progress < currentProgress)
                return currentProgress;
            else
            {
                var nextProgress = (searchedSupplierCount + 1) * 100 / totalSupplierCount;
                if (nextProgress > 100)
                    return currentProgress;
                progress += new Random().Next(0, 3);
                if (progress >= nextProgress)
                    progress = nextProgress - 1;
                return progress;
            }
        }

        private List<Combo> GenerateCombo(List<List<FlightItinerary>> itinLists)
        {
            var bundledItins = itinLists[0];
            var combos = new List<Combo>();
            foreach (var bundledItin in bundledItins)
            {
                var combo = new Combo();
                combo.Registers = new int[itinLists.Count - 1];
                for (var i = 0; i < bundledItin.Trips.Count; i++)
                {
                    var trip = bundledItin.Trips[i];
                    var identicalItin = itinLists[i + 1].SingleOrDefault(itin => itin.Trips[0].Identical(trip));
                    combo.Registers[i] = identicalItin != null ? identicalItin.RegisterNumber : -1;
                }
                if (combo.Registers.Contains(-1))
                    continue;
                combo.BundledRegister = bundledItin.RegisterNumber;
                combo.Fare = bundledItin.Price.FinalIdr;
                combos.Add(combo);
            }
            return combos;

            //var depItins = itinLists[1];
            //var retItins = itinLists[2];
            //var identicalDepItins =
            //    bundledItins.Select(itin => itin.Trips[0])
            //    .Select(trip =>
            //    {
            //        var identicalTrip = itinLists[1].SingleOrDefault(itin => itin.Trips[0].Identical(trip));
            //        return identicalTrip != null ? depItins.IndexOf(identicalTrip) : -1;
            //    }).ToList();
            //var identicalRetItins =
            //    bundledItins.Select(itin => itin.Trips[1])
            //    .Select(trip =>
            //    {
            //        var identicalTrip = itinLists[2].SingleOrDefault(itin => itin.Trips[0].Identical(trip));
            //        return identicalTrip != null ? retItins.IndexOf(identicalTrip) : -1;
            //    }).ToList();

            //for (var itinIdx = 0; itinIdx < bundledItins.Count; itinIdx++)
            //{
            //    var bundledItin = bundledItins[itinIdx];

            //    if (identicalDepItins[itinIdx] == -1 || identicalRetItins[itinIdx] == -1)
            //        continue;

            //    var depItin = itinLists[1][identicalDepItins[itinIdx]];
            //    var retItin = itinLists[2][identicalRetItins[itinIdx]];

            //    if (depItin.SupplierPrice + retItin.SupplierPrice < bundledItin.SupplierPrice)
            //        continue;

            //    InitializeComboSet(depItin);
            //    InitializeComboSet(retItin);
            //    depItin.ComboPairRegisters.Add(retItins[identicalRetItins[itinIdx]].RegisterNumber);
            //    retItin.ComboPairRegisters.Add(depItins[identicalDepItins[itinIdx]].RegisterNumber);
            //    depItin.ComboBundledRegisters.Add(bundledItin.RegisterNumber);
            //    retItin.ComboBundledRegisters.Add(bundledItin.RegisterNumber);
            //}
        }
    }
}