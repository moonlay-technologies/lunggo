﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc.Routing;
using System.Web.WebPages;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;
using Lunggo.Framework.Http;

namespace Lunggo.ApCommon.Dictionary
{
    public class DictionaryService 
    {
        private static readonly DictionaryService Instance = new DictionaryService();
        private bool _isInitialized;
        public Dictionary<long, AirlineDict> AirlineDict;
        public Dictionary<long, AirportDict> AirportDict;
        public Dictionary<long, HotelLocationDict> HotelLocationDict;
        public Dictionary<long, CountryDict> CountryDict;

        private const string AirlineFileName = @"Airline.csv";
        private const string AirportFileName = @"Airport.csv";
        private const string HotelLocationFileName = @"HotelLocation.csv";
        private const string CountryFileName = @"Country.csv";
        private const string AirlineLogoFileExtension = @".png";

        private const string AirlineLogoPath = @"/Assets/Images/Airlines/";
        private const string CountryFlagPath = @"/Assets/Images/Countries/";
        private static string _configPath;
        private static string _airlineFilePath;
        private static string _airportFilePath;
        private static string _hotelLocationFilePath;
        private static string _countryFilePath;
        

        private DictionaryService()
        {
            
        }

        public static DictionaryService GetInstance()
        {
            return Instance;
        }

        public void Init()
        {
            if (!_isInitialized)
            {
                _configPath = HttpContext.Current != null
                    ? HttpContext.Current.Server.MapPath(@"~/Config/")
                    : @"Config\";
                _airlineFilePath = Path.Combine(_configPath, AirlineFileName);
                _airportFilePath = Path.Combine(_configPath, AirportFileName);
                _hotelLocationFilePath = Path.Combine(_configPath, HotelLocationFileName);
                _countryFilePath = Path.Combine(_configPath, CountryFileName);
                AirlineDict = PopulateAirlineDict(_airlineFilePath);
                AirportDict = PopulateAirportDict(_airportFilePath);
                HotelLocationDict = PopulateHotelLocationDict(_hotelLocationFilePath);
                CountryDict = PopulateCountryDict(_countryFilePath);
                _isInitialized = true;
            }
        }

        public bool IsAirlineCodeExists(string code)
        {
            var valueList = AirlineDict.Select(dict => dict.Value.Code);
            return valueList.Contains(code);
        }

        public string GetAirlineCode(string name)
        {
            try
            {
                var valueList = AirlineDict.Select(dict => dict.Value);
                var searchedValue = valueList.Single(value => value.Name == name);
                return searchedValue.Code;
            }
            catch
            {
                return "";
            }
        }

        public string GetAirlineName(string code)
        {
            try
            {
                var valueList = AirlineDict.Select(dict => dict.Value);
                var searchedValue = valueList.Single(value => value.Code == code);
                return searchedValue.Name;
            }
            catch
            {
                return "";
            }
        }

        public string GetAirlineLogoUrl(string code)
        {
            return AirlineLogoPath + code + AirlineLogoFileExtension;
        }

        public bool IsAirportCodeExists(string code)
        {
            var valueList = AirportDict.Select(dict => dict.Value.Code);
            return valueList.Contains(code);
        }

        public string GetAirportCode(string name)
        {
            try
            {
                var valueList = AirportDict.Select(dict => dict.Value);
                var searchedValue = valueList.Single(value => value.Name == name);
                return searchedValue.Code;
            }
            catch
            {
                return "";
            }
        }

        public double GetAirportTimeZone(string code)
        {
            try
            {
                var valueList = AirportDict.Select(dict => dict.Value);
                var searchedValue = valueList.Single(value => value.Code == code);
                return searchedValue.TimeZone;
            }
            catch
            {
                return 99;
            }
        }


        public string GetAirportName(string code)
        {
            try
            {
                var valueList = AirportDict.Select(dict => dict.Value);
                var searchedValue = valueList.Single(value => value.Code == code);
                return searchedValue.Name;
            }
            catch
            {
                return "";
            }
        }

        public string GetAirportCityCode(string code)
        {
            try
            {
                var valueList = AirportDict.Select(dict => dict.Value);
                var searchedValue = valueList.Single(value => value.Code == code);
                return searchedValue.CityCode;
            }
            catch
            {
                return "";
            }
        }

        public string GetAirportCity(string code)
        {
            try
            {
                var valueList = AirportDict.Select(dict => dict.Value);
                var searchedValue = valueList.Single(value => value.Code == code);
                return searchedValue.City;
            }
            catch
            {
                return "";
            }
        }

        public string GetAirportCountryCode(string code)
        {
            try
            {
                var valueList = AirportDict.Select(dict => dict.Value);
                var searchedValue = valueList.Single(value => value.Code == code);
                return searchedValue.CountryCode;
            }
            catch
            {
                return "";
            }
        }

        public string GetAirportCountry(string code)
        {
            try
            {
                var valueList = AirportDict.Select(dict => dict.Value);
                var searchedValue = valueList.Single(value => value.Code == code);
                return searchedValue.Country;
            }
            catch
            {
                return "";
            }
        }

        public string GetCountryName(string code)
        {
            try
            {
                var valueList = CountryDict.Select(dict => dict.Value);
                var searchedValue = valueList.Single(value => value.Code == code);
                return searchedValue.Name;
            }
            catch
            {
                return "";
            }
        }

        public string GetCountryCallingCode(string code)
        {
            try
            {
                var valueList = CountryDict.Select(dict => dict.Value);
                var searchedValue = valueList.Single(value => value.Code == code);
                return searchedValue.CallingCode;
            }
            catch
            {
                return "";
            }
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
                        CityCode = splittedLine[3],
                        City = splittedLine[4],
                        CountryCode = splittedLine[5],
                        Country = splittedLine[6],
                        TimeZone = double.Parse(splittedLine[7])
                    });
                }
            }
            return result;
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
                        Priority = splittedLine[6].IsEmpty() ? (int?)null : int.Parse(splittedLine[6]),
                        Latitude = splittedLine[7].IsEmpty() ? (decimal?)null : decimal.Parse(splittedLine[7]),
                        Longitude = splittedLine[8].IsEmpty() ? (decimal?)null : decimal.Parse(splittedLine[8]),
                        IsRegion = splittedLine[9].IsEmpty() ? (bool?)null : bool.Parse(splittedLine[9]),
                        IsAirport = splittedLine[10].IsEmpty() ? (bool?)null : bool.Parse(splittedLine[10]),
                        IsActive = splittedLine[11].IsEmpty() ? (bool?)null : bool.Parse(splittedLine[11])
                    });
                }
            }
            return result;
        }

        private static Dictionary<long, CountryDict> PopulateCountryDict(String countryFilePath)
        {
            var result = new Dictionary<long, CountryDict>();
            using (var file = new StreamReader(countryFilePath))
            {
                var line = file.ReadLine();
                while (!file.EndOfStream)
                {
                    line = file.ReadLine();
                    var splittedLine = line.Split('|');
                    result.Add(long.Parse(splittedLine[0]), new CountryDict
                    {
                        Name = splittedLine[0],
                        Code = splittedLine[1],
                        CallingCode = splittedLine[2]
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
        public string CityCode { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }

        public double TimeZone { get; set; }
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

    public class CountryDict
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string CallingCode { get; set; }
    }
}
