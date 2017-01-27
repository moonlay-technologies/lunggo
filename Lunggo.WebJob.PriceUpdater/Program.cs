using System;
using System.Linq;
using Lunggo.ApCommon.Hotel.Service;

namespace Lunggo.WebJob.PriceUpdater
{
    partial class Program
    {
        static void Main()
        {
            Init();
            var todaydate = DateTime.Now;
            var endofmonth = DateTime.DaysInMonth(todaydate.Year, todaydate.Month);
            var difference = endofmonth - todaydate.Day;
            var baliCode = HotelService.AutoCompletes.First(c => c.Code == "BAI").Id;
            var jktCode = HotelService.AutoCompletes.First(c => c.Code == "JAV").Id;
            var bdoCd = HotelService.AutoCompletes.First(c => c.Code == "BDO").Id;
            var jogCd = HotelService.AutoCompletes.First(c => c.Code == "JOG").Id;
            var subCd = HotelService.AutoCompletes.First(c => c.Code == "SUB").Id;
            var soloCd = HotelService.AutoCompletes.First(c => c.Code == "SOC").Id;
            var mdnCd = HotelService.AutoCompletes.First(c => c.Code == "MES").Id;
            var plmCd = HotelService.AutoCompletes.First(c => c.Code == "ID6").Id;
            var mlgCd = HotelService.AutoCompletes.First(c => c.Code == "MLG").Id;
            var bgrCd = HotelService.AutoCompletes.First(c => c.Code == "ID5").Id;
            var sinCd = HotelService.AutoCompletes.First(c => c.Code == "SIN").Id;
            var kulCd = HotelService.AutoCompletes.First(c => c.Code == "KUL").Id;
            var bgkCd = HotelService.AutoCompletes.First(c => c.Code == "BKK").Id;
            var hgkCd = HotelService.AutoCompletes.First(c => c.Code == "HKG").Id;
            for (var i = 0; i <= difference; i++)
            {
                //FlightPriceUpdater("JKT", "DPS", todaydate.AddDays(i));
                //FlightPriceUpdater("JKT", "JOG", todaydate.AddDays(i));
                //FlightPriceUpdater("JKT", "KUL", todaydate.AddDays(i));
                //FlightPriceUpdater("JKT", "SIN", todaydate.AddDays(i));
                //FlightPriceUpdater("JKT", "DPS", todaydate.AddDays(i));
                //FlightPriceUpdater("JKT", "HKG", todaydate.AddDays(i));
                //FlightPriceUpdater("JKT", "SUB", todaydate.AddDays(i));
                //FlightPriceUpdater("JKT", "SIN", todaydate.AddDays(i));
                //FlightPriceUpdater("DPS", "JKT", todaydate.AddDays(i));
                //HotelPriceUpdater(baliCode,todaydate.AddDays(i));
                //HotelPriceUpdater(jktCode, todaydate.AddDays(i));
                HotelPriceUpdater(bdoCd, todaydate.AddDays(i));
                //HotelPriceUpdater(soloCd, todaydate.AddDays(i));
                //HotelPriceUpdater(jogCd, todaydate.AddDays(i));
                //HotelPriceUpdater(subCd, todaydate.AddDays(i));
                //HotelPriceUpdater(mdnCd, todaydate.AddDays(i));
                //HotelPriceUpdater(plmCd, todaydate.AddDays(i));
                //HotelPriceUpdater(bgrCd, todaydate.AddDays(i));
                //HotelPriceUpdater(mlgCd, todaydate.AddDays(i));
                //HotelPriceUpdater(kulCd, todaydate.AddDays(i));
                //HotelPriceUpdater(sinCd, todaydate.AddDays(i));
                //HotelPriceUpdater(bgkCd, todaydate.AddDays(i));
                //HotelPriceUpdater(hgkCd, todaydate.AddDays(i));

            }
            
        }
    }
}
