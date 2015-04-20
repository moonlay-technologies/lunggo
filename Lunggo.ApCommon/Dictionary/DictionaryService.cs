using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc.Routing;

namespace Lunggo.ApCommon.Dictionary
{
    public class DictionaryService 
    {
        private static readonly DictionaryService Instance = new DictionaryService();
        private bool _isInitialized;
        public Dictionary<long, AirlineDict> AirlineDict;

         private DictionaryService()
        {
            
        }

        public static DictionaryService GetInstance()
        {
            return Instance;
        }

        public void Init(string airlineFilePath)
        {
            if (!_isInitialized)
            {
                AirlineDict = PopulateAirlineDict(airlineFilePath);
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("DictionaryService is already initialized");
            }
        }

        public string GetAirlineName(string code)
        {
            var searchedDict = AirlineDict.Single(dict => dict.Value.Code == code);
            return searchedDict.Value.Name;
        }

        public string GetAirlineCode(string name)
        {
            var searchedDict = AirlineDict.Single(dict => dict.Value.Name == name);
            return searchedDict.Value.Code;
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
    }

    public class AirlineDict
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
