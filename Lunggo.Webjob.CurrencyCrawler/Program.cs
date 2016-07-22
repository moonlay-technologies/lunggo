using System;
using Lunggo.ApCommon.Flight.Service;
using Supplier = Lunggo.ApCommon.Flight.Constant.Supplier;

namespace Lunggo.Webjob.CurrencyCrawler
{
    public partial class Program
    {
        static void Main()
        {
            Console.WriteLine("Initialising Currency Crawler Programme...");
            Console.WriteLine();
            Init();
            var currencyAirAsia = new []
            {
                "SGD", "MYR", "HKD", "CNY", "AUD", 
                "USD", "JPY", "KRW", "SAR", "THB", 
                "BND", "PHP", "INR", "MOP", 
                "NPR", "NZD", "LKR", "TWD"
            };

            var currencyGaruda = new[]
            {
                "SGD", "MYR", "HKD", "CNY", "AUD",
                "USD", "JPY", "KRW", "SAR", "THB",
                "GBP", "EUR"
            };

            var currencyLion = new[] {"MYR", "SGD"};

            foreach (var curr in currencyLion)
            {
                Console.WriteLine("Retrieving Exchange Rate " + curr + " from Lion Air...");
                FlightService.GetInstance().CurrencyGetterInternal(curr, Supplier.LionAir);
                Console.WriteLine("Done Retrieving from Lion Air.");
                Console.WriteLine();
            }

            foreach (var curr in currencyGaruda)
            {
                Console.WriteLine("Retrieving Exchange Rate " + curr + " from Garuda...");
                FlightService.GetInstance().CurrencyGetterInternal(curr, Supplier.Garuda);
                Console.WriteLine("Done Retrieving from Garuda.");
                Console.WriteLine();
            }

            foreach (var curr in currencyAirAsia)
            {
                Console.WriteLine("Retrieving Exchange Rate " + curr + " from Air Asia...");
                FlightService.GetInstance().CurrencyGetterInternal(curr, Supplier.AirAsia);
                Console.WriteLine("Done Retrieving from Air Asia.");
                Console.WriteLine();
            }
            
            Console.WriteLine("Process is Done.");
        }
    }
}
