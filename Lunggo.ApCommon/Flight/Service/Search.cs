using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CsQuery.ExtensionMethods.Internal;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.Framework.Config;
using Lunggo.Framework.Queue;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public SearchFlightOutput SearchFlight(SearchFlightInput input)
        {
            var output = new SearchFlightOutput();

            if (input.RequestedSupplierIds.IsNullOrEmpty())
            {
                output.IsSuccess = true;
                output.TotalSupplier = Suppliers.Count;
                output.Itineraries = new List<FlightItineraryForDisplay>();
                output.SearchedSuppliers = new List<int>();
                output.SearchId = input.SearchId;
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

                var searchedItinLists = searchedSupplierItins.SelectMany(dict => dict.Value).ToList();
                var isReturn = ParseTripType(input.SearchId) == TripType.Return;
                var depItins = new List<FlightItineraryForDisplay>();
                List<FlightItineraryForDisplay> retItins = null;

                if (searchedItinLists.Any())
                {
                    if (isReturn)
                    {

                        for (var itinListIdx = 0; itinListIdx < searchedItinLists.Count; itinListIdx++)
                        {
                            var searchedItinList = searchedItinLists[itinListIdx];
                            searchedItinList.ForEach(itin => itin.AsReturn = true);
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
            var identicalDepItins =
                bundledItins.Select(itin => itin.Trips[0])
                .Select(trip =>
                {
                    var identicalTrip = itinLists[1].SingleOrDefault(itin => itin.Trips[0].Identical(trip));
                    return identicalTrip != null ? itinLists[1].IndexOf(identicalTrip) : -1;
                }).ToList();
            var identicalRetItins =
                bundledItins.Select(itin => itin.Trips[1])
                .Select(trip =>
                {
                    var identicalTrip = itinLists[2].SingleOrDefault(itin => itin.Trips[0].Identical(trip));
                    return identicalTrip != null ? itinLists[2].IndexOf(identicalTrip) : -1;
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
                depItin.ComboSet.PairRegisterNumber.Add(bundledItins[identicalRetItins[itinIdx]].RegisterNumber);
                retItin.ComboSet.PairRegisterNumber.Add(bundledItins[identicalDepItins[itinIdx]].RegisterNumber);
                depItin.ComboSet.BundledRegisterNumber.Add(bundledItin.RegisterNumber);
                retItin.ComboSet.BundledRegisterNumber.Add(bundledItin.RegisterNumber);
            }
        }

        private void SetComboFare(List<List<FlightItinerary>> searchedItinLists)
        {
            var bundledItin = searchedItinLists[0];
            for (var itinListIdx = 1; itinListIdx < searchedItinLists.Count; itinListIdx++)
            {
                var itins = searchedItinLists[itinListIdx];
                foreach (var itin in itins.Where(itin => itin.ComboSet != null))
                {
                    for (var comboIdx = 0; comboIdx < itin.ComboSet.BundledRegisterNumber.Count; comboIdx++)
                    {
                        itin.ComboSet.TotalComboFare.Add(bundledItin[comboIdx].LocalPrice);
                    }
                    itin.ComboSet.ComboFare = itin.ComboSet.TotalComboFare.Min() / 2M;
                }
            }
        }

        private void InitializeComboSet(FlightItinerary itin)
        {
            if (itin.ComboSet == null)
            {
                itin.ComboSet = new ComboSet
                {
                    PairRegisterNumber = new List<int>(),
                    BundledRegisterNumber = new List<int>(),
                    TotalComboFare = new List<decimal>()
                };
            }
        }
    }
}
