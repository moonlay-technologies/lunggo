using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon;
using Lunggo.ApCommon.Flight.Service;
using System.Diagnostics;

namespace Lunggo.WebJob.PriceIdentifierGenerator
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            Init();
            string availableId=null;
            FlightService flight = FlightService.GetInstance();
            flight.Init();
            var redisKey = "PriceIdentifier";
            var getIdentifier = flight.GetPriceIdentifierFromCache(redisKey);
            Dictionary<string, string> updateDict = new Dictionary<string, string>();
            if (getIdentifier != null) 
            {
                Debug.Print("Redis is NOT empty");
                foreach (var pair in getIdentifier) 
                {
                    if (pair.Value == "Unused") 
                    {
                        Debug.Print("Available Identifier: "+pair.Key);
                        availableId = pair.Key;
                        updateDict.Add(pair.Key,"Used");
                        TimeSpan timelimit = flight.GetRedisExpiry(redisKey);
                        Debug.Print(timelimit.ToString());
                        flight.SavePriceIdentifierInCache(redisKey, updateDict, timelimit);
                        break;
                    }
                }
                Debug.Print("Available String: "+availableId);
                var getUpdateIdentifier = flight.GetPriceIdentifierFromCache(redisKey);
                foreach (var pair in getUpdateIdentifier) 
                {
                    Debug.Print("Key : "+pair.Key+", Value : "+pair.Value);
                }

            }
            else
            {
                Debug.Print("Redis is empty");
                Dictionary<string, string> identifier = new Dictionary<string, string>();
                for (int i = 0; i < 10; i++)
                {
                    identifier.Add(i.ToString(), "Unused");
                }
                flight.SaveTransacInquiryInCache(redisKey, identifier, TimeSpan.FromHours(2));
                var savedIdentifier = flight.GetPriceIdentifierFromCache(redisKey);
                foreach (var pair in savedIdentifier)
                {
                    Debug.Print("Key : " + pair.Key + ", Value : " + pair.Value);
                }
            }
            
        }

        public string getAvailableId(string redisKey, Dictionary<string,string>dict) 
        {
            string availableId = null;
            Debug.Print("Redis is NOT empty");
            Dictionary<string, string> updateDict = new Dictionary<string, string>();
            foreach (var pair in dict)
            {
                if (pair.Value == "Unused")
                {
                    Debug.Print("Yang Available : " + pair.Key);
                    availableId = pair.Key;
                    updateDict.Add(pair.Key, "Used");
                    TimeSpan timelimit = FlightService.GetInstance().GetRedisExpiry(redisKey);
                    Debug.Print(timelimit.ToString());
                    FlightService.GetInstance().SavePriceIdentifierInCache(redisKey, updateDict, timelimit);
                    break;
                }
            }
            var getUpdateIdentifier = FlightService.GetInstance().GetPriceIdentifierFromCache(redisKey);
            foreach (var pair in getUpdateIdentifier)
            {
                Debug.Print("Key : " + pair.Key + ", Value : " + pair.Value);
            }
            return availableId;
        }
    }
}
