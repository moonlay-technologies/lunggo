using System;
using Lunggo.ApCommon.Flight.Service;
using Supplier = Lunggo.ApCommon.Flight.Constant.Supplier;


namespace Lunggo.Webjob.CurrencyRate
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initialising Currency Rate Programme...");
            Init();
            Console.WriteLine("Retrieving Exchange Rate from Air Asia...");
            FlightService.GetInstance().CurrencyGetterInternal("SGD", Supplier.AirAsia); //dapatlah objek currency
            Console.WriteLine("Retrieving Exchange Rate from Garuda...");
            FlightService.GetInstance().CurrencyGetterInternal("SGD", Supplier.Garuda);
            Console.WriteLine("Retrieving Exchange Rate from LionAir...");
            FlightService.GetInstance().CurrencyGetterInternal("SGD", Supplier.LionAir);
            Console.WriteLine("Process is Done.");
        } 
    }
}