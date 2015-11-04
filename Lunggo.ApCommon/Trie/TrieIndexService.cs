using Lunggo.ApCommon.Dictionary;

namespace Lunggo.ApCommon.Trie
{
    internal class TrieIndexService
    {
        private static readonly TrieIndexService Instance = new TrieIndexService();
        private bool _isInitialized;
        internal TrieNode AirlineIndex = new TrieNode();
        internal TrieNode AirportIndex = new TrieNode();
        internal TrieNode HotelLocationIndex = new TrieNode();

        private TrieIndexService()
        {
            
        }

        internal static TrieIndexService GetInstance()
        {
            return Instance;
        }

        internal void Init()
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
                AirportIndex.InsertWordsBySentence(airport.Value.Country, airport.Key);
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
