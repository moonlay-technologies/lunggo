using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using Lunggo.ApCommon.Trie;

namespace Lunggo.ApCommon.Autocomplete
{
    public class AutocompleteManager
    {
        private static readonly AutocompleteManager Instance = new AutocompleteManager();
        private bool _isInitialized;
        public Dictionary<long, AirlineDict> AirlineDict;
        public Dictionary<long, AirportDict> AirportDict;
        public Dictionary<long, HotelLocationDict> HotelLocationDict;

        private AutocompleteManager()
        {
            
        }
        public static AutocompleteManager GetInstance()
        {
            return Instance;
        }
        public void Init(String airportFilePath, String hotelLocationFilePath)
        {
            if (!_isInitialized)
            {
                AirlineDict = PopulateAirlineDict();
                AirportDict = PopulateAirportDict(airportFilePath);
                HotelLocationDict = PopulateHotelLocationDict(hotelLocationFilePath);
                TrieIndex.GetInstance().Init();
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("Autocomplete Manager is already initialized");
            }
        }

        public IEnumerable<AirportDict> GetAirportAutocomplete(string prefix)
        {
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
            var airportAutocomplete = distinctAirportIds.Select(id => AirportDict[id]);
            return airportAutocomplete;
        }

        public IEnumerable<AirlineDict> GetAirlineAutocomplete(string prefix)
        {
            var airlineIndex = TrieIndex.GetInstance().AirlineIndex;
            var airlineAutocomplete = airlineIndex.GetAllSuggestionIds(prefix).Select(id => AirlineDict[id]);
            return airlineAutocomplete;
        }

        public IEnumerable<object> GetHotelLocationAutocomplete(string prefix)
        {
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
                    HotelLocationDict[id].LocationId,
                    HotelLocationDict[id].LocationName,
                    HotelLocationDict[id].RegionName,   
                    HotelLocationDict[id].CountryName,
                    HotelLocationDict[id].Priority,
                })
                .OrderBy(dict => dict.Priority);
            return hotelLocationAutocomplete;
        }

        private static Dictionary<long, HotelLocationDict> PopulateHotelLocationDict(String hotelLocationFilePath)
        {
            var result = new Dictionary<long, HotelLocationDict>();
            using (var file = new StreamReader(hotelLocationFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    result.Add(long.Parse(splittedLine[0]), new HotelLocationDict
                    {
                        LocationId = long.Parse(splittedLine[0]),
                        CountryCode = splittedLine[1],
                        CountryName = splittedLine[2],
                        RegionName = splittedLine[3],
                        LocationName = splittedLine[4],
                        State = splittedLine[5],
                        Priority = splittedLine[6].IsEmpty() ? (int?) null : int.Parse(splittedLine[6]),
                        Latitude = splittedLine[7].IsEmpty() ? (decimal?) null : decimal.Parse(splittedLine[7]),
                        Longitude = splittedLine[8].IsEmpty() ? (decimal?) null : decimal.Parse(splittedLine[8]),
                        IsRegion = splittedLine[9].IsEmpty() ? (bool?) null : bool.Parse(splittedLine[9]),
                        IsAirport = splittedLine[10].IsEmpty() ? (bool?) null : bool.Parse(splittedLine[10]),
                        IsActive = splittedLine[11].IsEmpty() ? (bool?) null : bool.Parse(splittedLine[11])
                    });
                }
            }
            return result;
        }

        private static Dictionary<long, AirportDict> PopulateAirportDict(String airportFilePath)
        {
            var result = new Dictionary<long, AirportDict>();
            using (var file = new StreamReader(airportFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    result.Add(long.Parse(splittedLine[0]), new AirportDict
                    {
                        Code = splittedLine[1],
                        Name = splittedLine[2],
                        City = splittedLine[3],
                        Country = splittedLine[4]
                    });
                }
            }
            return result;
        }

        private static Dictionary<long, AirlineDict> PopulateAirlineDict()
        {
            return new Dictionary<long, AirlineDict>
            {
                {1, new AirlineDict { Code = "QZ", Name = "AirAsia"}},
                {2, new AirlineDict { Code = "JT", Name = "Lion Air"}},
                {3, new AirlineDict { Code = "SJ", Name = "Sriwijaya Air"}},
                {4, new AirlineDict { Code = "QG", Name = "Citylink"}},
                {5, new AirlineDict { Code = "MZ", Name = "Merpati Nusantara"}},
                {6, new AirlineDict { Code = "MV", Name = "Aviastar"}},
                {7, new AirlineDict { Code = "ID", Name = "Batik Air"}},
                {8, new AirlineDict { Code = "TN", Name = "Trigana Air"}},
                {9, new AirlineDict { Code = "KD", Name = "KalStar Aviation"}},
                {10, new AirlineDict { Code = "FS", Name = "Airfast Indonesia"}},
                {11, new AirlineDict { Code = "IW", Name = "Wings Air"}},
                {12, new AirlineDict { Code = "XN", Name = "Express Air"}},
                {13, new AirlineDict { Code = "SY", Name = "Sky Aviation"}},
                {14, new AirlineDict { Code = "SI", Name = "Susi Air"}},
                {15, new AirlineDict { Code = "GA", Name = "Garuda Indonesia"}},
                {16, new AirlineDict { Code = "CA", Name = "Air China"}},
                {17, new AirlineDict { Code = "AK", Name = "AirAsia"}},
                {18, new AirlineDict { Code = "NH", Name = "All Nippon Airways"}},
                {19, new AirlineDict { Code = "CX", Name = "Cathay Pacific"}},
                {20, new AirlineDict { Code = "5J", Name = "Cebu Pacific"}},
                {21, new AirlineDict { Code = "CI", Name = "China Airlines"}},
                {22, new AirlineDict { Code = "CZ", Name = "China Southern"}},
                {23, new AirlineDict { Code = "EK", Name = "Emirates"}},
                {24, new AirlineDict { Code = "EY", Name = "Etihad Airways"}},
                {25, new AirlineDict { Code = "BR", Name = "EVA Air"}},
                {26, new AirlineDict { Code = "JL", Name = "Japan Airlines"}},
                {27, new AirlineDict { Code = "JQ", Name = "Jetstar Airways"}},
                {28, new AirlineDict { Code = "KL", Name = "KLM"}},
                {29, new AirlineDict { Code = "KE", Name = "Korean Air"}},
                {30, new AirlineDict { Code = "KU", Name = "Kuwait Airways"}},
                {31, new AirlineDict { Code = "MH", Name = "Malaysia Airlines"}},
                {32, new AirlineDict { Code = "MJ", Name = "Mihin Lanka"}},
                {33, new AirlineDict { Code = "PR", Name = "Philipphine Airlines"}},
                {34, new AirlineDict { Code = "QF", Name = "Qantas"}},
                {35, new AirlineDict { Code = "QR", Name = "Qatar Airways"}},
                {36, new AirlineDict { Code = "BI", Name = "Royal Brunei Airlines"}},
                {37, new AirlineDict { Code = "SV", Name = "Saudi Arabian Airlines"}},
                {38, new AirlineDict { Code = "3U", Name = "Sichuan Airlines"}},
                {39, new AirlineDict { Code = "SQ", Name = "Singapore Airlines"}},
                {40, new AirlineDict { Code = "TQ", Name = "Thai Airways"}},
                {41, new AirlineDict { Code = "TR", Name = "Tiger Airways"}},
                {42, new AirlineDict { Code = "TK", Name = "Turkish Airlines"}},
                {43, new AirlineDict { Code = "VF", Name = "Valuair"}},
                {44, new AirlineDict { Code = "VN", Name = "Vietnam Airlines"}},
                {45, new AirlineDict { Code = "IY", Name = "Yemenia"}},
            };
        }
    }

    public class AirlineDict
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class AirportDict
    {
        public string Code { get; set; }

        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
    public class HotelLocationDict
    {
        public long LocationId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string LocationName { get; set; }
        public string State { get; set; }
        public int? Priority { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public bool? IsRegion { get; set; }
        public bool? IsAirport { get; set; }
        public bool? IsActive { get; set; }
    }
}