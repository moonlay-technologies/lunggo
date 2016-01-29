﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Trie;

namespace Lunggo.ApCommon.Autocomplete
{
    public class AutocompleteManager
    {
        private static readonly AutocompleteManager Instance = new AutocompleteManager();
        private bool _isInitialized;

        private AutocompleteManager()
        {
            
        }
        public static AutocompleteManager GetInstance()
        {
            return Instance;
        }
        public void Init()
        {
            if (!_isInitialized)
            {
                TrieIndexService.GetInstance().Init();
                _isInitialized = true;
            }
        }

        public IEnumerable<long> GetAirportIdsAutocomplete(string prefix)
        {
            var airportIndex = TrieIndexService.GetInstance().AirportIndex;
            var splittedString = prefix.Split(' ');
            var airportIds = new List<long>();
            airportIds.AddRange(airportIndex.GetAllSuggestionIds(splittedString[0]));
            var i = 1;
            while (i < splittedString.Count())
            {
                airportIds = airportIds.Intersect(airportIndex.GetAllSuggestionIds(splittedString[i])).ToList();
                i++;
            }
            var distinctAirportIds = airportIds.Distinct();
            return distinctAirportIds;
        }

        public IEnumerable<long> GetAirlineIdsAutocomplete(string prefix)
        {
            var airlineIndex = TrieIndexService.GetInstance().AirlineIndex;
            var splittedString = prefix.Split(' ');
            var airlineIds = new List<long>();
            airlineIds.AddRange(airlineIndex.GetAllSuggestionIds(splittedString[0]));
            var i = 1;
            while (i < splittedString.Count())
            {
                airlineIds = airlineIds.Intersect(airlineIndex.GetAllSuggestionIds(splittedString[i])).ToList();
                i++;
            }
            var distinctairlineIds = airlineIds.Distinct();
            return distinctairlineIds;
        }

        public IEnumerable<long> GetHotelLocationIdsAutocomplete(string prefix)
        {
            var hotelLocationIndex = TrieIndexService.GetInstance().HotelLocationIndex;
            var splittedString = prefix.Split(' ');
            var hotelLocationIds = new List<long>();
            hotelLocationIds.AddRange(hotelLocationIndex.GetAllSuggestionIds(splittedString[0]));
            var i = 1;
            while (i < splittedString.Count())
            {
                hotelLocationIds = hotelLocationIds.Intersect(hotelLocationIndex.GetAllSuggestionIds(splittedString[i])).ToList();
                i++;
            }
            var distincthotelLocationIds = hotelLocationIds.Distinct();
            return distincthotelLocationIds;
        }
    }
}