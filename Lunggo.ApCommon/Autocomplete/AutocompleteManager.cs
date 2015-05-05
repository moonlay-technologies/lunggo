using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.WebPages;
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
                try
                {
                    DictionaryService.GetInstance().Init();
                }
                catch
                {
                    
                }
                TrieIndex.GetInstance().Init();
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("Autocomplete Manager is already initialized");
            }
        }

        public IEnumerable<UpdateSet> GetDictionaryLastUpdate()
        {
           yield return new UpdateSet
           {
               Type = "airport",
               UpdateTime = new DateTime(2015,1,1)
           };
        }

        public IEnumerable<AirportDict> GetAirportAutocomplete(string prefix)
        {
            var airportDict = DictionaryService.GetInstance().AirportDict;
            var airportIndex = TrieIndex.GetInstance().AirportIndex;
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
            var airportAutocomplete = distinctAirportIds.Select(id => airportDict[id]);
            return airportAutocomplete;
        }

        public IEnumerable<AirlineDict> GetAirlineAutocomplete(string prefix)
        {
            var airlineDict = DictionaryService.GetInstance().AirlineDict;
            var airlineIndex = TrieIndex.GetInstance().AirlineIndex;
            var airlineAutocomplete = airlineIndex.GetAllSuggestionIds(prefix).Select(id => airlineDict[id]);
            return airlineAutocomplete;
        }

        public IEnumerable<object> GetHotelLocationAutocomplete(string prefix)
        {
            var hotelLocationDict = DictionaryService.GetInstance().HotelLocationDict;
            var hotelLocationIndex = TrieIndex.GetInstance().HotelLocationIndex;
            var splittedString = prefix.Split(' ');
            var hotelLocationIds = new List<long>();
            hotelLocationIds.AddRange(hotelLocationIndex.GetAllSuggestionIds(splittedString[0]));
            var i = 1;
            while (i < splittedString.Count())
            {
                hotelLocationIds = hotelLocationIds.Intersect(hotelLocationIndex.GetAllSuggestionIds(splittedString[i])).ToList();
                i++;
            }
            var distinctHotelLocationIds = hotelLocationIds.Distinct();
            var hotelLocationAutocomplete = distinctHotelLocationIds
                .Select(id => new
                {
                    hotelLocationDict[id].LocationId,
                    hotelLocationDict[id].LocationName,
                    hotelLocationDict[id].RegionName,   
                    hotelLocationDict[id].CountryName,
                    hotelLocationDict[id].Priority,
                })
                .OrderBy(dict => dict.Priority);
            return hotelLocationAutocomplete;
        }
    }

    public class UpdateSet
    {
        public string Type { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}