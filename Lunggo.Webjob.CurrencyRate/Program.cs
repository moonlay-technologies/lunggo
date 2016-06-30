using System;
using System.Collections.Generic;
using Lunggo.ApCommon;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Service;
using System.Globalization;
using Lunggo.ApCommon.Travolutionary.WebService.Hotel;


namespace Lunggo.Webjob.CurrencyRate
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            Init();
            FlightService.GetInstance().CurrencyGetterInternal("MYR", Supplier.LionAir); //dapatlah objek currency
            //masukin ke databse

            //alternatif ke2
            //1 . dalam crawlingan, sekalian masukin ke databse
            //2. udah

        }
        

        //private static List<KeyValuePair<string, string>> ToPairList(List<string> list)
        //{
        //    List<KeyValuePair<string, string>> pairList = new List<KeyValuePair<string,string>>();
        //    for (int i = 0; i < list.Count; i += 2)
        //    {
        //        pairList.Add(new KeyValuePair<string, string>(list[i + 1], list[i]));
        //    }
        //    return pairList;
        //}

        //private static List<KeyValuePair<string, string>> CompareList(List<KeyValuePair<string, string>> lcrawl, List<KeyValuePair<string, string>> lredis) 
        //{
        //    List<KeyValuePair<string, string>> diff = new List<KeyValuePair<string, string>>();
        //    var format = "dd/MM/yyyy HH:mm:ss";
        //    CultureInfo provider = new CultureInfo("id-ID");
        //    lredis.Sort(CompareValue);
        //    for (var i = lcrawl.Count - 1; i >= 0; i--)
        //    {
        //        if (DateTime.ParseExact(lcrawl[i].Value, format, provider) > DateTime.ParseExact(lredis[lredis.Count - 1].Value, format, provider))
        //        {
        //            diff.Add(new KeyValuePair<string, string>(lcrawl[i].Key, lcrawl[i].Value));
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //    return diff;
        //}

        //private static void PrintData(List<KeyValuePair<string, string>> list)
        //{
        //    int count = 1;
        //    if (list != null && list.Count != 0) 
        //    {
        //        foreach (var pair in list)
        //        {
        //            Console.WriteLine(count + ") Key : " + pair.Key + ", Value : " + pair.Value);
        //            count++;
        //        }
        //    }
        //}

        //static int CompareValue(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        //{
        //    return a.Value.CompareTo(b.Value);
        //}
    }
}