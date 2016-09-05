using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using System.Globalization;


namespace Lunggo.Webjob.BankTransferChecking
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            Init();

            var env = ConfigManager.GetInstance().GetConfigValue("general", "environment");
            if (env != "production")
                return;

            FlightService flight = FlightService.GetInstance();
            flight.Init("");
            MandiriClientHandler mandiriClient = MandiriClientHandler.GetClientInstance();
            mandiriClient.Init();
            _bankAccountNumber = mandiriClient.getBankAccount();
            var client = mandiriClient.CreateCustomerClient();
            Console.WriteLine("---> Started");
            var redisKey = datenow.Date.ToString();
            if (mandiriClient.Login(client))
            {
                var getListFirst = flight.GetTransacInquiryFromCache(redisKey);
                if (getListFirst != null)
                {
                    Console.WriteLine("=== Redis is not Empty ===");
                    var crawlingList = ProcessCrawl(client, datenow);
                    var pairList = ToPairList(crawlingList);
                    Console.WriteLine("Total Today Transaction : " + pairList.Count);
                    var difference = CompareList(pairList, getListFirst);
                    TimeSpan timeLimit = flight.GetRedisExpiry(redisKey);
                    if (difference.Count != 0)
                    {
                        Console.WriteLine("-----------Different----------");
                        PaymentCheck(difference); //check Payment
                        flight.SaveTransacInquiryInCache(redisKey, difference, timeLimit);
                    }
                    else
                    {
                        Console.WriteLine("Crawling List is same with Redis List");
                    }
                }
                else
                {
                    Console.WriteLine("---Redis is still empty---");
                    Console.WriteLine("=== Do Crawling and Get data from Redis for Previous Day ===");
                    var prevRedisKey = prevDate.Date.ToString();
                    var getRedisData = flight.GetTransacInquiryFromCache(prevRedisKey);
                    var prevDataList = ProcessCrawl(client, prevDate);
                    if (getRedisData != null && prevDataList.Count != 0) 
                    {
                        var prevList = ToPairList(prevDataList);
                        var diff = CompareList(prevList, getRedisData);
                        if (diff.Count != 0)
                        {
                            PaymentCheck(diff);  //Check Payment
                            TimeSpan prevTimeLimit = flight.GetRedisExpiry(prevRedisKey);
                            Console.WriteLine("Time Limit : " + prevTimeLimit.ToString());
                            flight.SaveTransacInquiryInCache(prevRedisKey, diff, prevTimeLimit);
                        }
                        else
                        {
                            Console.WriteLine("Prev Same :: Crawling data and Redis data");
                        }

                    }
                    else
                    {
                        // If Crawling data not empty, save to redis
                        if (prevDataList.Count != 0)
                        {
                            TimeSpan setPrevTimeLimit = TimeSpan.FromHours(6);
                            var listDict = ToPairList(prevDataList);
                            Console.WriteLine("Total Previous Transaction : "+listDict.Count);

                            PaymentCheck(listDict); //Check Payment 
                            flight.SaveTransacInquiryInCache(prevRedisKey, listDict, setPrevTimeLimit);
                        }
                        else
                        {
                            Console.WriteLine("There is no previous transaction");
                        }
                    }

                    // Do first crawling for this day
                    Console.WriteLine("===Do Crawling and Getting data from Redis for This Day===");
                    if (mandiriClient.Login(client))
                    {
                        var todayList = ProcessCrawl(client, datenow);
                        if (todayList.Count != 0)
                        {
                            Console.WriteLine("Crawling is not empty");
                            var todayPairList = ToPairList(todayList);
                            Console.WriteLine("Total Today Transaction : " + todayPairList.Count);
                            PaymentCheck(todayPairList); //Check Payment
                            TimeSpan setTimeLimit = TimeSpan.FromHours(30);
                            flight.SaveTransacInquiryInCache(redisKey, todayPairList, setTimeLimit);
                        }
                        else
                        {
                            Console.WriteLine("There is no transaction today");
                        }
                    }
                    else 
                    {
                        Console.WriteLine("Login Failed for transaction today");
                    }
                }
            }
            else 
            {
                Console.WriteLine("Login Failed");
            }
        }
        

        private static List<KeyValuePair<string, string>> ToPairList(List<string> list)
        {
            List<KeyValuePair<string, string>> pairList = new List<KeyValuePair<string,string>>();
            for (int i = 0; i < list.Count; i += 2)
            {
                pairList.Add(new KeyValuePair<string, string>(list[i + 1], list[i]));
            }
            return pairList;
        }

        private static List<KeyValuePair<string, string>> CompareList(List<KeyValuePair<string, string>> lcrawl, List<KeyValuePair<string, string>> lredis) 
        {
            List<KeyValuePair<string, string>> diff = new List<KeyValuePair<string, string>>();
            var format = "dd/MM/yyyy HH:mm:ss";
            CultureInfo provider = new CultureInfo("id-ID");
            lredis.Sort(CompareValue);
            for (var i = lcrawl.Count - 1; i >= 0; i--)
            {
                if (DateTime.ParseExact(lcrawl[i].Value, format, provider) > DateTime.ParseExact(lredis[lredis.Count - 1].Value, format, provider))
                {
                    diff.Add(new KeyValuePair<string, string>(lcrawl[i].Key, lcrawl[i].Value));
                }
                else
                {
                    break;
                }
            }
            return diff;
        }

        private static void PrintData(List<KeyValuePair<string, string>> list)
        {
            int count = 1;
            if (list != null && list.Count != 0) 
            {
                foreach (var pair in list)
                {
                    Console.WriteLine(count + ") Key : " + pair.Key + ", Value : " + pair.Value);
                    count++;
                }
            }
        }

        static int CompareValue(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        {
            return a.Value.CompareTo(b.Value);
        }
    }
}