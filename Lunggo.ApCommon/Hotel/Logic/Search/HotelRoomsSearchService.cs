using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.ApCommon.Model;

namespace Lunggo.ApCommon.Hotel.Logic.Search
{
    public class HotelRoomsSearchService
    {
        public static HotelRoomsSearchServiceResponse GetRooms(HotelRoomsSearchServiceRequest request)
        {
            var roomPackages = GetDummyRoomPackages();
            var response = new HotelRoomsSearchServiceResponse
            {
                RoomPackages = roomPackages
            };
            return response;
        }

        private static IEnumerable<RoomPackage> GetDummyRoomPackages()
        {
            var list = new List<RoomPackage>
            {
                new RoomPackage
                {
                    FinalPackagePrice = new Price
                    {
                        Currency = "IDR",
                        Value = 500000
                    },
                  PackageId = 1,
                  RoomList = new List<Room>
                  {
                    new Room
                    {
                        AdultCount = 2,
                        ChildrenCount = 0,
                        FinalRoomPrice = new Price
                        {
                            Currency = "IDR",
                            Value = 500000
                        },
                        RoomDescription = "Room Only",
                        RoomId = 1,
                        RoomName = "Superior Double Deluxe"
                    }
                  }
                },
                new RoomPackage
                {
                    FinalPackagePrice = new Price
                    {
                        Currency = "IDR",
                        Value = 2000000
                    },
                  PackageId = 2,
                  RoomList = new List<Room>
                  {
                    new Room
                    {
                        AdultCount = 2,
                        ChildrenCount = 0,
                        FinalRoomPrice = new Price
                        {
                            Currency = "IDR",
                            Value = 1000000
                        },
                        RoomDescription = "Continental Breakfast",
                        RoomId = 2,
                        RoomName = "Superior Twin Deluxe"
                    },
                    new Room
                    {
                        AdultCount = 2,
                        ChildrenCount = 0,
                        FinalRoomPrice = new Price
                        {
                            Currency = "IDR",
                            Value = 1000000
                        },
                        RoomDescription = "Continental Breakfast",
                        RoomId = 3,
                        RoomName = "Superior Twin Deluxe"
                    }
                  }
                },
                new RoomPackage
                {
                    FinalPackagePrice = new Price
                    {
                        Currency = "IDR",
                        Value = 4500000
                    },
                  PackageId = 3,
                  RoomList = new List<Room>
                  {
                    new Room
                    {
                        AdultCount = 2,
                        ChildrenCount = 0,
                        FinalRoomPrice = new Price
                        {
                            Currency = "IDR",
                            Value = 1500000
                        },
                        RoomDescription = "Continental Breakfast",
                        RoomId = 4,
                        RoomName = "Suite Room"
                    },
                    new Room
                    {
                        AdultCount = 2,
                        ChildrenCount = 0,
                        FinalRoomPrice = new Price
                        {
                            Currency = "IDR",
                            Value = 1500000
                        },
                        RoomDescription = "Continental Breakfast",
                        RoomId = 5,
                        RoomName = "Suite Room"
                    },
                    new Room
                    {
                        AdultCount = 2,
                        ChildrenCount = 0,
                        FinalRoomPrice = new Price
                        {
                            Currency = "IDR",
                            Value = 1500000
                        },
                        RoomDescription = "Continental Breakfast",
                        RoomId = 6,
                        RoomName = "Suite Room"
                    }
                  }
                }
            };

            return list;
        }
    }
}
