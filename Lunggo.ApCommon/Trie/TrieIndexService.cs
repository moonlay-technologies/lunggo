using System;
using Lunggo.ApCommon.Autocomplete;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Model;

namespace Lunggo.ApCommon.Trie
{
    public class TrieIndexService
    {
        private static readonly TrieIndexService Instance = new TrieIndexService();
        private bool _isInitialized;
        public TrieNode AirlineIndex = new TrieNode();
        public TrieNode AirportIndex = new TrieNode();
        public TrieNode HotelLocationIndex = new TrieNode();

        private TrieIndexService()
        {
            
        }

        public static TrieIndexService GetInstance()
        {
            return Instance;
        }

        public void Init()
        {
            if (!_isInitialized)
            {
                InitAirlineIndex();
                InitAirportIndex();
                InitHotelLocationIndex();
                _isInitialized = true;
            }
        }

        private void InitAirlineIndex()
        {
            foreach (var airline in DictionaryService.GetInstance().AirlineDict)
            {
                AirlineIndex.InsertWord(airline.Value.Code, airline.Key);
                AirlineIndex.InsertWordsBySentence(airline.Value.Name, airline.Key);
            }
        }

        private void InitAirportIndex()
        {
            foreach (var airport in DictionaryService.GetInstance().AirportDict)
            {
                AirportIndex.InsertWord(airport.Value.Code, airport.Key);
                AirportIndex.InsertWordsBySentence(airport.Value.Name, airport.Key);
                AirportIndex.InsertWordsBySentence(airport.Value.City, airport.Key);
            }
        }

        private void InitHotelLocationIndex()
        {
            foreach (var hotelLocation in DictionaryService.GetInstance().HotelLocationDict)
            {
                HotelLocationIndex.InsertWordsBySentence(hotelLocation.Value.CountryName, hotelLocation.Key);
                HotelLocationIndex.InsertWordsBySentence(hotelLocation.Value.LocationName, hotelLocation.Key);
                HotelLocationIndex.InsertWordsBySentence(hotelLocation.Value.RegionName, hotelLocation.Key);
            }
        }
    }
}
