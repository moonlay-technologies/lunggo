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
        public void Init(String hotelLocationFilePath)
        {
            if (!_isInitialized)
            {
                AirlineDict = PopulateAirlineDict();
                AirportDict = PopulateAirportDict();
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
            var airportAutocomplete = airportIndex.GetAllSuggestionIds(prefix).Select(id => AirportDict[id]);
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

        private static Dictionary<long, AirportDict> PopulateAirportDict()
        {
            return new Dictionary<long, AirportDict>
            {
                {1, new AirportDict { Code = "BTH", Name = "Hang Nadim Int'l", City = "Batam", Region = "Sumatera"}},
                {2, new AirportDict { Code = "BTJ", Name = "Sultan Iskandar Muda Int'l", City = "Banda Aceh", Region = "Sumatera"}},
                {3, new AirportDict { Code = "KNO", Name = "Kuala Namu Int'l", City = "Deli Serdang", Region = "Sumatera"}},
                {4, new AirportDict { Code = "SIX", Name = "Dr. Ferdinand Lumban Tobing", City = "Sibolga", Region = "Sumatera"}},
                {5, new AirportDict { Code = "SGT", Name = "Silangit Int'l", City = "Siborong-borong", Region = "Sumatera"}},
                {6, new AirportDict { Code = "LSW", Name = "Malikus Saleh", City = "Lhokseumawe", Region = "Sumatera"}},
                {7, new AirportDict { Code = "RGT", Name = "Japura", City = "Rengat", Region = "Sumatera"}},
                {8, new AirportDict { Code = "MEQ", Name = "Cut Nyak Dhien", City = "Nagan Raya", Region = "Sumatera"}},
                {9, new AirportDict { Code = "PDG", Name = "Minangkabau Int'l", City = "Kota Padang", Region = "Sumatera"}},
                {10, new AirportDict { Code = "PKU", Name = "Sultan Syarif Kasim II Int'l", City = "Pekanbaru", Region = "Sumatera"}},
                {11, new AirportDict { Code = "PLM", Name = "Sultan Mahmud Badaruddin II Int'l", City = "Palembang", Region = "Sumatera"}},
                {12, new AirportDict { Code = "TNJ", Name = "Raja Haji Fisabilillah Int'l", City = "Tanjungpinang", Region = "Sumatera"}},
                {13, new AirportDict { Code = "BDO", Name = "Husein Sastranegara Int'l", City = "Bandung", Region = "Jawa"}},
                {14, new AirportDict { Code = "CGK", Name = "Soekarno-Hatta Int'l", City = "Jakarta", Region = "Jawa"}},
                {15, new AirportDict { Code = "JOG", Name = "Adi Sucipto Int'l", City = "Yogyakarta", Region = "Jawa"}},
                {16, new AirportDict { Code = "SOC", Name = "Adisumarmo Int'l", City = "Solo", Region = "Jawa"}},
                {17, new AirportDict { Code = "SRG", Name = "Achmad Yani Int'l", City = "Semarang", Region = "Jawa"}},
                {18, new AirportDict { Code = "SUB", Name = "Juanda Int'l", City = "Surabaya", Region = "Jawa"}},
                {19, new AirportDict { Code = "MSI", Name = "Valia Rahma Int'l", City = "Masalembo", Region = "Jawa"}},
                {20, new AirportDict { Code = "JBB", Name = "Notohadinegoro", City = "Jember", Region = "Jawa"}},
                {21, new AirportDict { Code = "BWX", Name = "Blimbingsari", City = "Banyuwangi", Region = "Jawa"}},
                {22, new AirportDict { Code = "DPS", Name = "Ngurah Rai Int'l", City = "Denpasar", Region = "Bali"}},
                {23, new AirportDict { Code = "LOP", Name = "Lombok Int'l", City = "Lombok Tengah", Region = "Nusa Tenggara"}},
                {24, new AirportDict { Code = "MOF", Name = "Wai Oti", City = "Maumere", Region = "Nusa Tenggara"}},
                {25, new AirportDict { Code = "TMC", Name = "Tambolaka", City = "Waikabubak", Region = "Nusa Tenggara"}},
                {26, new AirportDict { Code = "LKA", Name = "Gewayantana", City = "Larantuka", Region = "Nusa Tenggara"}},
                {27, new AirportDict { Code = "SWQ", Name = "Sultan Muhammad Kaharuddin III", City = "Sumbawa Besar", Region = "Nusa Tenggara"}},
                {28, new AirportDict { Code = "MLK", Name = "Melalan", City = "Sendawar", Region = "Kalimantan"}},
                {29, new AirportDict { Code = "PKY", Name = "Tjilik Riwut", City = "Palangka Raya", Region = "Kalimantan"}},
                {30, new AirportDict { Code = "TRK", Name = "Juwata Int'l", City = "Tarakan", Region = "Kalimantan"}},
                {31, new AirportDict { Code = "SRI", Name = "Temindung", City = "Samarinda", Region = "Kalimantan"}},
                {32, new AirportDict { Code = "BEJ", Name = "Kalimarau Int'l", City = "Berau", Region = "Kalimantan"}},
                {33, new AirportDict { Code = "BPN", Name = "Sultan Aji Muhammad Sulaiman", City = "Balikpapan", Region = "Kalimantan"}},
                {34, new AirportDict { Code = "NNX", Name = "Warukin", City = "Tabalong", Region = "Kalimantan"}},
                {35, new AirportDict { Code = "BDJ", Name = "Syamsuddin Noor Int'l", City = "Banjarmasin", Region = "Kalimantan"}},
                {36, new AirportDict { Code = "MTW", Name = "Beringin", City = "Muara Teweh", Region = "Kalimantan"}},
                {37, new AirportDict { Code = "MDC", Name = "Sam Ratulangi Int'l", City = "Manado", Region = "Sulawesi"}},
                {38, new AirportDict { Code = "UPG", Name = "Sultan Hasanuddin Int'l", City = "Makassar", Region = "Sulawesi"}},
                {39, new AirportDict { Code = "KDI", Name = "Haluoleo Int'l", City = "Kendari", Region = "Sulawesi"}},
                {40, new AirportDict { Code = "LUW", Name = "Syukuran Aminuddin Amir", City = "Luwuk", Region = "Sulawesi"}},
                {41, new AirportDict { Code = "GTO", Name = "Jalaluddin", City = "Gorontalo", Region = "Sulawesi"}},
                {42, new AirportDict { Code = "WKB", Name = "Matahora", City = "Wangi-wangi", Region = "Sulawesi"}},
                {43, new AirportDict { Code = "TMI", Name = "Maranggo", City = "Pulau Tomia", Region = "Sulawesi"}},
                {44, new AirportDict { Code = "NBX", Name = "Yos Sudarso Int'l", City = "Nabire", Region = "Papua"}},
                {45, new AirportDict { Code = "BIK", Name = "Frans Kaisiepo", City = "Biak", Region = "Papua"}},
                {46, new AirportDict { Code = "ORG", Name = "Iskak Int'l", City = "Oksibil", Region = "Papua"}},
                {47, new AirportDict { Code = "TMH", Name = "Tanah Merah", City = "Tanah Merah", Region = "Papua"}},
                                
                {48, new AirportDict { Code = "ICN", Name = "Incheon Int'l", City = "Seoul", Region = "Korea Selatan"}},
                {49, new AirportDict { Code = "SIN", Name = "Changi Int'l", City = "Singapura", Region = "Singapura"}},
                {50, new AirportDict { Code = "KTM", Name = "Tribhuvan Int'l", City = "Kathmandu", Region = "Nepal"}},
                {51, new AirportDict { Code = "KUL", Name = "KLIA", City = "Kuala Lumpur", Region = "Malaysia"}}
            };
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
        public string Region { get; set; }
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