using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;


namespace Lunggo.Webjob.BankTransferChecking
{
    public partial class Program
    {

        static void Main(string[] args)
        {
            Init();
            FlightService flight = FlightService.GetInstance();
            flight.Init();
            _companyId = ConfigManager.GetInstance().GetConfigValue("mandiri", "webCompanyId");
            _userName = ConfigManager.GetInstance().GetConfigValue("mandiri", "webUserName");
            _password = ConfigManager.GetInstance().GetConfigValue("mandiri", "webPassword");
            
            var redisKey = datenow.Date.ToString();
            var getListFirst = flight.GetTransacInquiryFromCache(redisKey);
            Console.WriteLine("---> Started");

            //Check if data in redis exists or not
            if (getListFirst != null)
            {
                Debug.Print("===Redis is not Empty===");
                Console.WriteLine("=== Redis is not Empty ===");
                var crawlingList = ProcessCrawl(_day,_month,_year);

                var listToDict = ListToDictionary(crawlingList);
                var difference = CompareDictionary(listToDict, getListFirst);
                Console.WriteLine("Time Limit : " + flight.GetRedisExpiry(redisKey).ToString());
                if (difference.Count != 0)  
                {
                    Console.WriteLine("-----------Different----------");
                    TimeSpan timeLimit = flight.GetRedisExpiry(redisKey);
                    Console.WriteLine("Time to Live : " + timeLimit.ToString());
                    flight.SaveTransacInquiryInCache(redisKey, difference, timeLimit);
                    var getList = flight.GetTransacInquiryFromCache(redisKey);
                    
                    //CHECK PAYMENT
                    PaymentCheck(getList);
                    
                    //print the data from redis
                    Debug.Print("Printing data from Redis");
                    foreach (var pair in getList)
                    {
                        Debug.Print("{0}, {1}", pair.Key, pair.Value);
                    }
                }
                else 
                {
                    Console.WriteLine("Crawling List is same with data in redis");
                    //PaymentCheck(listToDict);
                }
                
            }
            else
            {
                // Doing crawling for previous day
                // Paymment checking for previous day
                // then, do crawling for this day and check payment for this day
                
                Debug.Print("---Redis is still empty---");
                Console.WriteLine("---Redis is still empty---");
                //Process Crawling for a previous day
                Console.WriteLine("=== Do Crawling and Get data from Redis for Previous Day ===");
                var prevRedisKey = prevDate.Date.ToString();
                var getRedisData = flight.GetTransacInquiryFromCache(prevRedisKey);
                var prevDataList = ProcessCrawl(_prevDay, _prevMonth, _prevYear);
                if (getRedisData != null && prevDataList.Count != 0)
                {
                    var prevListToDict = ListToDictionary(prevDataList);
                    var diff = CompareDictionary(prevListToDict,getRedisData);
                    if (diff.Count != 0)
                    {
                        TimeSpan prevTimeLimit = flight.GetRedisExpiry(prevRedisKey);
                        Console.WriteLine("Time Limit : " + prevTimeLimit.ToString());
                        flight.SaveTransacInquiryInCache(prevRedisKey, diff, prevTimeLimit);
                        var getPrevList = flight.GetTransacInquiryFromCache(prevRedisKey);
                        
                        //CHECK PAYMENT
                        PaymentCheck(getPrevList);

                        //print data from the redis
                        foreach (var pair in getPrevList)
                        {
                            Debug.Print("{0}, {1}", pair.Key, pair.Value);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Prev :: Crawling data and in Redis is same ");
                    }
                    
                }
                else 
                {
                    // If Crawling data not empty, save to redis
                    if (prevDataList.Count != 0)
                    {
                        TimeSpan setPrevTimeLimit = TimeSpan.FromHours(6); // ??? Time is not available yet,change this to 6 hours
                        var listDict = ListToDictionary(prevDataList);
                        flight.SaveTransacInquiryInCache(prevRedisKey, listDict, setPrevTimeLimit);
                        var getPrevSaveList = flight.GetTransacInquiryFromCache(prevRedisKey);

                        //CHECK PAYMENT
                        PaymentCheck(getPrevSaveList);
                    }
                    else
                    {
                        Console.WriteLine("There is no previous transaction");
                    }
                }        

                // Do first crawling for this day
                Console.WriteLine("===Do Crawling and Getting data from Redis for This Day===");
                var todayList = ProcessCrawl(_day,_month,_year);
                if (todayList.Count != 0)
                {
                    Console.WriteLine("Crawling is not empty");
                    var dictTodayList = ListToDictionary(todayList);
                    TimeSpan setTimeLimit = TimeSpan.FromHours(30); // change this to set timelimit at first, 30
                    flight.SaveTransacInquiryInCache(redisKey, dictTodayList, setTimeLimit);
                    var getSaveList = flight.GetTransacInquiryFromCache(redisKey);

                    
                    var unpaid = flight.GetUnpaidReservations();
                    foreach (var data in unpaid) 
                    {
                        Debug.Print("Price : " + data.Payment.FinalPrice +", No : "+ data.RsvNo );
                    }

                    //CHECK PAYMENT
                    PaymentCheck(getSaveList);

                    //print data from the redis
                    Console.WriteLine("Printing data from Redis");
                    foreach (var pair in getSaveList)
                    {
                        Debug.Print("{0}, {1}", pair.Key, pair.Value);
                        Console.WriteLine("{0}, {1}", pair.Key, pair.Value);
                    }
                }
                else 
                {
                    Console.WriteLine("There is no transaction today");
                }
                
            }
        }

        private static Dictionary<string, string> ListToDictionary(List<string> list)
        {
            Dictionary<string,string> dict = new Dictionary<string,string>();
            for (int i = 0; i < list.Count; i += 2)
            {
                dict.Add(list[i + 1], list[i]);
                Debug.Print("Key : "+list[i+1]+" Value : "+list[i]);
            }
            
            return dict;
        }

        private static Dictionary<string, string> CompareDictionary(Dictionary<string, string> dcrawl, Dictionary<string, string> dredis) 
        {
            var listDiff = dcrawl.Keys.Except(dredis.Keys);
            Dictionary<string, string> diff = new Dictionary<string, string>();
            if (listDiff != null) 
            {
                foreach (var pair in dcrawl)
                {
                    if (listDiff.Contains(pair.Key))
                    {
                        diff.Add(pair.Key, pair.Value);
                    }
                }
            }
            return diff;
        }
    }
}
