using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CsQuery.ExtensionMethods.Internal;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.Framework.Config;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public SearchFlightOutput SearchFlight(SearchFlightInput input)
        {
            var output = new SearchFlightOutput
            {
                SearchId = input.SearchId
            };

            if (input.RequestedSupplierIds.IsNullOrEmpty())
            {
                output.IsSuccess = true;
                output.TotalSupplier = Suppliers.Count;
                output.Itineraries = new List<FlightItineraryForDisplay>();
                output.SearchedSuppliers = new List<int>();
            }
            else
            {
                var searchedSupplierItins = GetSearchedSupplierItineraries(input.SearchId, input.RequestedSupplierIds);
                var searchedSuppliers = searchedSupplierItins.Keys.ToList();
                var missingSuppliers = input.RequestedSupplierIds.Except(searchedSuppliers).ToList();
                foreach (var missingSupplier in missingSuppliers)
                {
                    var isSearching = GetSearchingStatusInCache(input.SearchId, missingSupplier);
                    if (!isSearching)
                    {
                        var queue = QueueService.GetInstance().GetQueueByReference("FlightCrawl" + missingSupplier);
                        queue.AddMessage(new CloudQueueMessage(input.SearchId));
                    }
                }

                var tripType = ParseTripType(input.SearchId);
                var searchedItinLists = searchedSupplierItins.SelectMany(dict => dict.Value).ToList();
                searchedItinLists.SelectMany(list => list).ToList().ForEach(itin => itin.RequestedTripType = tripType);
                var depItins = new List<FlightItineraryForDisplay>();
                List<FlightItineraryForDisplay> retItins = null;

                if (searchedItinLists.Any())
                {
                    if (tripType == TripType.RoundTrip)
                    {

                        for (var itinListIdx = 0; itinListIdx < searchedItinLists.Count; itinListIdx++)
                        {
                            var searchedItinList = searchedItinLists[itinListIdx];
                            AddPriceMargin(searchedItinList);
                            SaveFlightRequestPrices(input.SearchId, searchedItinList, itinListIdx);
                        }
                        SetComboFare(searchedItinLists);

                        depItins = searchedItinLists[1].Select(ConvertToItineraryForDisplay).ToList();
                        retItins = searchedItinLists[2].Select(ConvertToItineraryForDisplay).ToList();
                        depItins.ForEach(itin => itin.SearchId = output.SearchId);
                        retItins.ForEach(itin => itin.SearchId = output.SearchId);
                    }
                    else
                    {
                        var searchedItinList = searchedItinLists[0];
                        AddPriceMargin(searchedItinList);
                        SaveFlightRequestPrices(input.SearchId, searchedItinList);

                        depItins = searchedItinLists[0].Select(ConvertToItineraryForDisplay).ToList();
                        depItins.ForEach(itin => itin.SearchId = output.SearchId);
                    }
                }

                var expiry = searchedSuppliers.Select(supplier => GetSearchedItinerariesExpiry(input.SearchId, supplier)).Min();
                output.IsSuccess = true;
                output.SearchId = input.SearchId;
                output.ExpiryTime = expiry;
                output.Itineraries = depItins;
                output.ReturnItineraries = retItins;
                output.SearchedSuppliers = searchedSuppliers;
                output.TotalSupplier = Suppliers.Count;
            }

            return output;
        }

        public void CommenceSearchFlight(string searchId, int supplierIndex)
        {
            var conditions = DecodeSearchConditions(searchId);
            SearchFlightInternal(conditions, supplierIndex);
        }

        private void SearchNormalFares(SearchFlightConditions conditions, int supplierIndex)
        {
            var supplier = Suppliers[supplierIndex.ToString(CultureInfo.InvariantCulture)];
            var searchId = EncodeSearchConditions(conditions);
            var timeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "SearchResultCacheTimeout"));

            var result = supplier.SearchFlight(conditions);
            result.Itineraries = result.Itineraries ?? new List<FlightItinerary>();
            if (result.IsSuccess)
                foreach (var itin in result.Itineraries)
                {
                    itin.FareId = IdUtil.ConstructIntegratedId(itin.FareId, supplier.SupplierName, itin.FareType);
                }
            SaveSearchedItinerariesToCache(result.Itineraries, searchId, timeout, supplierIndex);
            InvalidateSearchingStatusInCache(searchId, supplierIndex);
        }

        private void SearchComboFares(SearchFlightConditions conditions, int supplierIndex)
        {
            var supplier = Suppliers[supplierIndex.ToString(CultureInfo.InvariantCulture)];

            var conditionsList = new List<SearchFlightConditions> { conditions };
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

            Parallel.ForEach(conditionsList, partialConditions =>
            {
                var result = supplier.SearchFlight(partialConditions);
                result.Itineraries = result.Itineraries ?? new List<FlightItinerary>();
                if (result.IsSuccess)
                    foreach (var itin in result.Itineraries)
                    {
                        itin.FareId = IdUtil.ConstructIntegratedId(itin.FareId, supplier.SupplierName, itin.FareType);
                    }
                SaveSearchedPartialItinerariesToBufferCache(result.Itineraries, searchId, timeout, supplierIndex, conditionsList.IndexOf(partialConditions));
            });

            var itinLists = GetSearchedPartialItinerariesFromBufferCache(searchId, supplierIndex);
            GenerateReturnCombo(itinLists);
            SaveSearchedItinerariesToCache(itinLists[1], itinLists[2], itinLists[0], searchId, timeout, supplierIndex);
            InvalidateSearchingStatusInCache(searchId, supplierIndex);
        }

        private void GenerateReturnCombo(List<List<FlightItinerary>> itinLists)
        {
            var bundledItins = itinLists[0];
            var depItins = itinLists[1];
            var retItins = itinLists[2];
            var identicalDepItins =
                bundledItins.Select(itin => itin.Trips[0])
                .Select(trip =>
                {
                    var identicalTrip = itinLists[1].SingleOrDefault(itin => itin.Trips[0].Identical(trip));
                    return identicalTrip != null ? depItins.IndexOf(identicalTrip) : -1;
                }).ToList();
            var identicalRetItins =
                bundledItins.Select(itin => itin.Trips[1])
                .Select(trip =>
                {
                    var identicalTrip = itinLists[2].SingleOrDefault(itin => itin.Trips[0].Identical(trip));
                    return identicalTrip != null ? retItins.IndexOf(identicalTrip) : -1;
                }).ToList();

            for (var itinIdx = 0; itinIdx < bundledItins.Count; itinIdx++)
            {
                var bundledItin = bundledItins[itinIdx];
                
                if (identicalDepItins[itinIdx] == -1 || identicalRetItins[itinIdx] == -1)
                    continue;

                var depItin = itinLists[1][identicalDepItins[itinIdx]];
                var retItin = itinLists[2][identicalRetItins[itinIdx]];

                if (depItin.SupplierPrice + retItin.SupplierPrice < bundledItin.SupplierPrice)
                    continue;

                InitializeComboSet(depItin);
                InitializeComboSet(retItin);
                depItin.ComboPairRegisters.Add(retItins[identicalRetItins[itinIdx]].RegisterNumber);
                retItin.ComboPairRegisters.Add(depItins[identicalDepItins[itinIdx]].RegisterNumber);
                depItin.ComboBundledRegisters.Add(bundledItin.RegisterNumber);
                retItin.ComboBundledRegisters.Add(bundledItin.RegisterNumber);
            }
        }

        private void SetComboFare(List<List<FlightItinerary>> searchedItinLists)
        {
            var bundledItin = searchedItinLists[0];
            for (var itinListIdx = 1; itinListIdx < searchedItinLists.Count; itinListIdx++)
            {
                var itins = searchedItinLists[itinListIdx];
                foreach (var itin in itins.Where(itin => itin.ComboPairRegisters != null))
                {
                    for (var comboIdx = 0; comboIdx < itin.ComboBundledRegisters.Count; comboIdx++)
                    {
                        itin.TotalComboFares.Add(bundledItin[comboIdx].LocalPrice);
                    }
                }
            }
        }

        private static void InitializeComboSet(FlightItinerary itin)
        {
            if (itin.ComboPairRegisters == null)
            {
                itin.ComboPairRegisters = new List<int>();
                itin.ComboBundledRegisters = new List<int>();
                itin.TotalComboFares = new List<decimal>();
            }
        }
    }
}
