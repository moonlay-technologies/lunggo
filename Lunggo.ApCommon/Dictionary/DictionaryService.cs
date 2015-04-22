using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc.Routing;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Dictionary
{
    public class DictionaryService 
    {
        private static readonly DictionaryService Instance = new DictionaryService();
        private bool _isInitialized;
        public Dictionary<long, AirlineDict> AirlineDict;
        public Dictionary<long, AirportDict> AirportDict;
        public Dictionary<string, FlightFareItinerary> ItineraryDict;

         private DictionaryService()
        {
            
        }

        public static DictionaryService GetInstance()
        {
            return Instance;
        }

        public void Init(string airlineFilePath, string airportFilePath)
        {
            if (!_isInitialized)
            {
                AirlineDict = PopulateAirlineDict(airlineFilePath);
                AirportDict = PopulateAirportDict(airportFilePath);
                ItineraryDict = new Dictionary<string, FlightFareItinerary>();
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("DictionaryService is already initialized");
            }
        }

        public string GetAirlineCode(string name)
        {
            var valueList = AirlineDict.Select(dict => dict.Value);
            var searchedValue = valueList.Single(value => value.Name == name);
            return searchedValue.Code;
        }

        public string GetAirlineName(string code)
        {
            var valueList = AirlineDict.Select(dict => dict.Value);
            var searchedValue = valueList.Single(value => value.Code == code);
            return searchedValue.Name;
        }

        public string GetAirportCode(string name)
        {
            var valueList = AirportDict.Select(dict => dict.Value);
            var searchedValue = valueList.Single(value => value.Name == name);
            return searchedValue.Code;
        }

        public string GetAirportName(string code)
        {
            var valueList = AirportDict.Select(dict => dict.Value);
            var searchedValue = valueList.Single(value => value.Code == code);
            return searchedValue.Name;
        }

        public string GetAirportCity(string code)
        {
            var valueList = AirportDict.Select(dict => dict.Value);
            var searchedValue = valueList.Single(value => value.Code == code);
            return searchedValue.City;
        }

        public string GetAirportCountry(string code)
        {
            var valueList = AirportDict.Select(dict => dict.Value);
            var searchedValue = valueList.Single(value => value.Code == code);
            return searchedValue.Country;
        }

        private static Dictionary<long, AirlineDict> PopulateAirlineDict(String airlineFilePath)
        {
            var result = new Dictionary<long, AirlineDict>();
            using (var file = new StreamReader(airlineFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    result.Add(long.Parse(splittedLine[0]), new AirlineDict
                    {
                        Code = splittedLine[1],
                        Name = splittedLine[2],
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
}
