using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Documents;
using Lunggo.Framework.TableStorage;

namespace Lunggo.Webjob.HotelContentWrapper
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Init();
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();

            var hotel = new HotelBedsService();
            var hotelService = HotelService.GetInstance();
            //hotelService.UpdateDestinationStorage();
            //hotelService.SaveHotelAutocompleteToBlob();
            //var y = hotelService._Autocompletes;
            //var z = hotelService.AutocompleteDummyList;
            /** Update CSV Storage **/
            //hotelService.UpdateCountryStorage();
            //hotelService.UpdateHotelBoardStorage();
            //hotelService.UpdateHotelChainStorage();
            //hotelService.UpdateHotelCategoryStorage();
            //hotelService.UpdateHotelFacilityGroupStorage();
            //hotelService.UpdateHotelFacilityStorage();
            //hotelService.UpdateHotelRoomStorage();
            //hotelService.UpdateHotelSegmentStorage();
            //hotelService.UpdateHotelAccomodationStorage();
            var x = HotelService.HotelBoards;

            
            //hotel.GetBoard(1, 1000);
            //var y = HotelBedsService.hotelBoardList;
            /**  END **/
            
            
            
            //Console.WriteLine("Update Hotel Detail");
            //hotel.GetHotelData(148001, 149000);
            //var service = HotelService.GetInstance();
            //Console.WriteLine("start!");
            //service.SaveAutoCompleteToTableStorage();

            //hotelService.UpdateHotelImage();

            ///*Try to get file name*/
            //var blobService = BlobStorageService.GetInstance();
            //blobService.GetFileNameList("hotelimage");

            //hotelService.UpdateLocation();
            //var x = hotelService.HotelDestinationAreaDict;
            //var y = hotelService.HotelDestinationZoneDict;
            //var z = hotelService.HotelDestinationDict;
            //hotelService.UpdateHotelListByLocationContent();
            //hotelService.SaveHotelDetailByLocation();

            stopwatch.Stop();
            Debug.Print("Done in : {0}", stopwatch.Elapsed);
            Console.WriteLine("Done in : {0}", stopwatch.Elapsed);
            //Console.ReadKey();
        }
    }
}
