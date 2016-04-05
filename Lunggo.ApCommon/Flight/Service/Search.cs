﻿using System;
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
            var gottenSupplierCount = input.Progress / (100 / Suppliers.Count);
            var searchedSupplierIds = GetSearchedSupplierIndicesFromCache(input.SearchId);
            var requestedSupplierIds = searchedSupplierIds.Skip(gottenSupplierCount).ToList();
            var unsearchedSupplierIds = Suppliers.Keys.Except(searchedSupplierIds);

            foreach (var unsearchedSupplierId in unsearchedSupplierIds)
            {
                var isSearching = GetSearchingStatusInCache(input.SearchId, unsearchedSupplierId);
                if (!isSearching)
                {
                    var queue = QueueService.GetInstance().GetQueueByReference("FlightCrawl" + unsearchedSupplierId);
                    queue.AddMessage(new CloudQueueMessage(input.SearchId));
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

                if (searchedItinLists.Any())
                {
                    for (var itinListIdx = 0; itinListIdx < searchedItinLists.Count; itinListIdx++)
                    {
                        var searchedItinList = searchedItinLists[itinListIdx];
                        AddPriceMargin(searchedItinList);
                        SaveFlightRequestPrices(input.SearchId, searchedItinList, itinListIdx);
                    }
                }

                var seachedItinListsForDisplay =
                    searchedItinLists.Skip(ParseTripType(input.SearchId) != TripType.OneWay ? 1 : 0).Select(lists => lists.Select(ConvertToItineraryForDisplay).ToList()).ToArray();

                var combos = searchedSuppliers.SelectMany(sup => GetCombosFromCache(input.SearchId, sup)).ToList();

                SetComboFare(seachedItinListsForDisplay, searchedItinLists[0], combos);

                var expiry = searchedSuppliers.Select(supplier => GetSearchedItinerariesExpiry(input.SearchId, supplier)).Min();
                output.IsSuccess = true;
                output.SearchId = input.SearchId;
                output.ExpiryTime = expiry;
                output.ItineraryLists = seachedItinListsForDisplay;
                output.Combos = combos.Any() ? combos.Select(ConvertToComboForDisplay).ToList() : null;
                output.Progress = CalculateProgress(input.Progress, searchedSupplierIds.Count, Suppliers.Count);
            }

            return output;
        }

        public void CommenceSearchFlight(string searchId, int supplierIndex)
        {
            var conditions = DecodeSearchConditions(searchId);
            SearchFlightInternal(conditions, supplierIndex);
        }

        private void SetComboFare(List<FlightItineraryForDisplay>[] itinLists, List<FlightItinerary> bundledItins, List<Combo> combos)
        {
            foreach (var combo in combos)
            {
                combo.Fare = bundledItins.Single(itin => itin.RegisterNumber == combo.BundledRegister).LocalPrice;
                var comboFare = combo.Fare / combo.Registers.Length;
                for (var i = 0; i < combo.Registers.Length; i++)
                {
                    var reg = combo.Registers[i];
                    var identicalItin = itinLists[i].Single(itin => itin.RegisterNumber == reg);
                    if (identicalItin.ComboFare == null ||
                        (identicalItin.ComboFare != null && identicalItin.ComboFare > comboFare))
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
