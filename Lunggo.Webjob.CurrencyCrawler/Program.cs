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
            Init();
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }
            Console.WriteLine();
            Console.WriteLine("Retrieving Exchange Rate from Air Asia...");
           // FlightService.GetInstance().CurrencyGetterInternal(args[0], Supplier.AirAsia);
            Console.WriteLine("Done Retrieving from Air Asia.");
            Console.WriteLine();
            Console.WriteLine("Retrieving Exchange Rate from Garuda...");
            //FlightService.GetInstance().CurrencyGetterInternal(args[0], Supplier.Garuda);
            Console.WriteLine("Done Retrieving from Garuda.");
            Console.WriteLine();
            Console.WriteLine("Retrieving Exchange Rate from Lion Air...");
            FlightService.GetInstance().CurrencyGetterInternal("SGD", Supplier.LionAir);
            Console.WriteLine("Done Retrieving from Lion Air.");
            Console.WriteLine();
            Console.WriteLine("Process is Done.");
            //Thread.Sleep(3000);
        }
    }
}
