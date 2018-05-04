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