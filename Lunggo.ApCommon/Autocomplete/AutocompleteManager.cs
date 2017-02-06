using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Service;
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
            if (string.IsNullOrWhiteSpace(prefix))
                return FlightService.GetInstance().AirportDict.Keys;

            if (prefix.ToLower() == "popular")
                return PopularAirports();

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

        public IEnumerable<string> GetHotelIdsAutocomplete(string prefix)
        {
            var hotelIndex = TrieIndexService.GetInstance().HotelAutocompleteIndex;
            var splittedString = prefix.Split(' ');
            var hotelIds = new List<string>();
            hotelIds.AddRange(hotelIndex.GetAllSuggestionIds(splittedString[0]).Select(id => id.ToString()));
            var i = 1;
            while (i < splittedString.Count())
            {
                hotelIds = hotelIds.Intersect(hotelIndex.GetAllSuggestionIds(splittedString[i]).Select(id => id.ToString())).ToList();
                i++;
            }
            var distincthotelIds = hotelIds.Distinct();
            return distincthotelIds;
        }

        private static IEnumerable<long> PopularAirports()
        {
            return new List<long>
            {
                //JAWA
                3474, //JKT
                7168, //SUB
                7099, //SRG
                3508, //JOG
                7025, //SOC
                612, //BDO
                4825, //MLG

                //BALI NUSA TENGGARA
                1890, //DPS
                4359, //LOP
                3871, //KOE,
                4114, //LBJ

                //SUMATRA
                3855, //KNO
                990, //BTH
                5875, //PDG
                6025, //PLM
                6009, //PKU
                7519, //TKG

                //KALIMANTAN
                911, //BPN
                607, //BDJ
                6073, //PNK
                7660, //TRK

                //INDONESIA TIMUR

                7906, //UPG
                4621, //MDC
                270, //AMQ
                734, //BIK
                1800, //DJJ

                //ASIA TENGGARA
                6882, //SIN
                1833, //DMK
                2866, //HDY
                2942, //HKT
                2821, //HAN
                6830, //SGN
                4001, //KUL
                3446, //JHB
                9227, //ZWR
                3629, //KCH
                5901, //PEN
                4879, //MNL
                6070, //PNH

                //ASIA TIMUR
                5898, //PEK
                2938, //HKG
                4682, //MFM
                3764, //KIX
                3142, //ICN
                7632, //TPE
                7771, //TYO

                //ASIA BARAT DAN SELATAN
                433, //AUH
                1959, //DXB
                1870, //DOH
                888, //BOM
                1741, //DEL

                //AUSTRALIA
                4656, //MLB
                5904, //PER

                //AS
                411, //ATL
                5685, //ORD
                1752, //DFW
                4102, //LAX
                3433, //JFK

                //EROPA
                272, //AMS
                407, //ATH
                7758, //TXL
                4358, //LON
                4754, //MIL
                5814, //PAR
                6560 //ROM
            };
        }
    }
}