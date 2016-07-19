using System;
using System.Threading;
using Lunggo.ApCommon.Flight.Service;
using Supplier = Lunggo.ApCommon.Flight.Constant.Supplier;

namespace Lunggo.Webjob.CurrencyCrawler
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initialising Currency Crawler Programme...");
            Console.WriteLine();
            Init();
            var currency = new string[]
            {
                //"SAR"
                "SGD", "MYR", "HKD", "CNY", "AUD", 
                "USD", "JPY", "KRW", "SAR", "THB", 
                "EUR", "GBP", "BND", "PHP", "INR", 
                "MOP", "NPR", "NZD", "LKR", "TWD",
                "AED"
            };
            foreach (var curr in currency)
            {
                Console.WriteLine("Retrieving Exchange Rate " + curr + " from Air Asia...");
                FlightService.GetInstance().CurrencyGetterInternal(curr, Supplier.AirAsia);
                Console.WriteLine("Done Retrieving from Air Asia.");
                Console.WriteLine();
                Console.WriteLine("Retrieving Exchange Rate " + curr + " from Garuda...");
                FlightService.GetInstance().CurrencyGetterInternal(curr, Supplier.Garuda);
                Console.WriteLine("Done Retrieving from Garuda.");
                Console.WriteLine();
                Console.WriteLine("Retrieving Exchange Rate " + curr + " from Lion Air...");
                FlightService.GetInstance().CurrencyGetterInternal(curr, Supplier.LionAir);
                Console.WriteLine("Done Retrieving from Lion Air.");
                Console.WriteLine();
            }
            
            Console.WriteLine("Process is Done.");
        }
    }
}
