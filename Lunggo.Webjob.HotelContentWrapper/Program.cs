using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content;
using Lunggo.ApCommon.Util;
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

            //var hotelService = HotelService.GetInstance();
            var hotelService = HotelService.GetInstance();
            var reservation = hotelService.GetReservationForDisplay("217306558579");
            var mailData = new HotelBookingNotif
            {
                Token = GenerateTokenUtil.GenerateTokenByRsvNo("217306558579"),
                Reservation = reservation
            };
            var roomlist = "";
            for (var n = 0; n < mailData.Reservation.HotelDetail.Rooms.Count; n++)
                    {
                        if (n != mailData.Reservation.HotelDetail.Rooms.Count - 1)
                        {
                            roomlist += mailData.Reservation.HotelDetail.Rooms[n].Rates.SelectMany(r => r.Breakdowns).Sum(f => f.RateCount) + " "
                                        + mailData.Reservation.HotelDetail.Rooms[n].RoomName + ", ";

                        }
                        else
                        {
                            roomlist += mailData.Reservation.HotelDetail.Rooms[n].Rates.SelectMany(r => r.Breakdowns).Sum(f => f.RateCount) + " "
                                        + mailData.Reservation.HotelDetail.Rooms[n].RoomName;
                        }
                    }
            Console.WriteLine(roomlist);
            //hotelService.UpdateHotelContentAll();
            //Console.WriteLine("On Running");
            //var x = HotelService.FacilityGroups;
            //Console.ReadKey();
            ///*Try to get file name*/
            //hotelService.UpdateHotelImage();
            //var blobService = BlobStorageService.GetInstance();
            //var u = blobService.GetFileNameList("hotelimage", "standard/120819");
            stopwatch.Stop();
            Debug.Print("Done in : {0}", stopwatch.Elapsed);
            Console.WriteLine("Done in : {0}", stopwatch.Elapsed);
            //Console.ReadKey();
        }
    }
}
