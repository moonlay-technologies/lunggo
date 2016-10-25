using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Query;
using Lunggo.Framework.Documents;
using Microsoft.Azure.Documents;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public void SaveTruncatedHotelDetail()
        {
            for (var i = 150001; i <= 600000; i++)
            {
                try
                {
                    var hotel = GetInstance().GetHotelDetailFromTableStorage(i);
                    Console.WriteLine("hotelCd: " + i);
                    var truncatedHotelDetail = new HotelDetailsBase
                    {
                        HotelName = hotel.HotelName,
                        StarRating = hotel.StarRating,
                        ImageUrl = new List<Image>
                        {
                            hotel.ImageUrl.Where(u => u.Order == 1).ToList()[0]
                        },
                        WifiAccess = hotel.Facilities.Any(f => (f.FacilityGroupCode == 60 && f.FacilityCode == 261)
                            || (f.FacilityGroupCode == 70 && f.FacilityCode == 550)),
                        IsRestaurantAvailable = hotel.Facilities.Any(f => (f.FacilityGroupCode == 71 && f.FacilityCode == 200)
                            || (f.FacilityGroupCode == 75 && f.FacilityCode == 840)
                            || (f.FacilityGroupCode == 75 && f.FacilityCode == 845)),
                        Latitude = hotel.Latitude,
                        Longitude = hotel.Longitude,
                        DestinationCode = hotel.DestinationCode,
                        ZoneCode = hotel.ZoneCode,
                        City = hotel.City,
                    };
                    SaveTruncatedHotelDetailToTableStorage(truncatedHotelDetail, hotel.HotelCode);
                    Console.WriteLine("Hotel detail truncated saved for: "+ i);
                }
                catch
                {
                    Console.WriteLine("Hotel with code: " + i + " not found");
                }
            }               
        }

        public void SaveHotelAmenities()
        {
            for (var i = 109600; i <= 600000; i++)
            {
                try
                {
                    var hotel = GetInstance().GetHotelDetailFromTableStorage(i);
                    Console.WriteLine("hotelCd: " + i);
                    var truncatedHotelDetail = new HotelDetailsBase
                    {
                        HotelName = hotel.HotelName,
                        HotelCode = hotel.HotelCode,
                        Facilities = hotel.Facilities,
                        AccomodationType = hotel.AccomodationType
                    };
                    SaveHotelAmenitiesAndAccomodationTypeToTableStorage(truncatedHotelDetail, hotel.HotelCode);
                    Console.WriteLine("Amenities saved for: " + i);
                }
                catch
                {
                    Console.WriteLine("Hotel with code: " + i + " not found");
                }

            }
        }
    }
}
