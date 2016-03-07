using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Lunggo.CloudApp.CaptchaReader.Models
{
    public static class Account
    {
        public static List<string> AccountList = new List<string>();
        public static ConcurrentDictionary<int, Tuple<DateTime, int>> DictAccountOn = new ConcurrentDictionary<int, Tuple<DateTime, int>>();
        static Account()
        {
            //AccountList.Add("trv.agent.satu");
            AccountList.Add("trv.agent.dua");
            AccountList.Add("trv.agent.tiga");
            //AccountList.Add("trv.agent.empat");
            AccountList.Add("trv.agent.lima");
            AccountList.Add("trv.agent.enam");
            AccountList.Add("trv.agent.tujuh");
            AccountList.Add("trv.agent.delapan");
            AccountList.Add("trv.agent.sembilan");
            AccountList.Add("trv.agent.sepuluh");
            AccountList.Add("trv.agent.sebelas");
            AccountList.Add("trv.agent.duabelas");
            AccountList.Add("trv.agent.tigabelas");
            AccountList.Add("trv.agent.empatbelas");
            AccountList.Add("trv.agent.limabelas");
            AccountList.Add("trv.agent.enambelas");
            AccountList.Add("trv.agent.tujuhbelas");
            AccountList.Add("trv.agent.delapanbelas");
            AccountList.Add("trv.agent.sembilanbelas");
            AccountList.Add("trv.agent.duapuluh");
            AccountList.Add("trv.agent.duasatu");
            AccountList.Add("trv.agent.duadua");
            AccountList.Add("trv.agent.duatiga");
            AccountList.Add("trv.agent.duaempat");
            AccountList.Add("trv.agent.dualima");
            AccountList.Add("trv.agent.duaenam");
            AccountList.Add("trv.agent.duatujuh");
            AccountList.Add("trv.agent.duadelapan");
            AccountList.Add("trv.agent.duasembilan");
        }
        public static string GetUserId()
        {
            bool isKeyGiven = false;
            int ind = 0;
            string acc = "";
            DateTime timeReqKey = DateTime.UtcNow;

            while (!isKeyGiven && DateTime.UtcNow < timeReqKey.AddSeconds(2))
            {
                acc = AccountList.ElementAt(ind);
                var value = DictAccountOn.GetOrAdd(ind, new Tuple<DateTime, int>(timeReqKey, Thread.CurrentThread.ManagedThreadId));
                isKeyGiven = ((value.Item1 == timeReqKey && value.Item2 == Thread.CurrentThread.ManagedThreadId) ||
                              timeReqKey > value.Item1.AddMinutes(10));
                if (timeReqKey > value.Item1.AddMinutes(10))
                {
                    DictAccountOn[ind] = new Tuple<DateTime, int>(timeReqKey, Thread.CurrentThread.ManagedThreadId);
                }
                if (ind == AccountList.Count-1)
                {
                    ind = 0;
                }
                else
                {
                    ind++;
                }
            }

            if (DateTime.UtcNow >= timeReqKey.AddSeconds(2))
            {
                return "";
            }
            return acc;
        }

        public static bool LogOutAck(string userId)
        {
            int index = AccountList.IndexOf(userId);
            Tuple<DateTime, int> timeStamp;
            bool ret = DictAccountOn.TryRemove(index, out timeStamp);
            return ret;
        }
    }
}