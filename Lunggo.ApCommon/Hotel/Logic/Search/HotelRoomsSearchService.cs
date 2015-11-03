using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.ApCommon.Travolutionary;

namespace Lunggo.ApCommon.Hotel.Logic.Search
{
    public class HotelRoomsSearchService
    {
        public static HotelRoomsSearchServiceResponse GetRooms(HotelRoomsSearchServiceRequest request)
        {
            IEnumerable<RoomPackage> roomPackages = null;
            
            roomPackages = GetRoomsInternal(request);
            
            
                
            var response = new HotelRoomsSearchServiceResponse
            {
                RoomPackages = roomPackages
            };
            return response;
        }

        public static IEnumerable<RoomPackage> GetRoomsInternal(HotelRoomsSearchServiceRequest request)
        {
            var response = TravolutionaryHotelService.GetHotelRooms(request);
            if (TravolutionaryHotelService.IsErrorTravolutionaryResponse(response))
            {
                //TODO please throw specialized custom exception class
                throw new Exception("Error in retrieving Hotel Rooms");
            }
            var roomPackages = ToRoomPackages(response.RoomPackages);
            return roomPackages;
        }

        private static IEnumerable<RoomPackage> ToRoomPackages(IEnumerable<RawRoomPackage>  rawRoomPackages)
        {
            return rawRoomPackages.Select(p => new RoomPackage
            {
                FinalPackagePrice = HotelPriceUtil.CountPrice(p.PackagePrice),
                PackageId = p.PackageId,
                RoomList = p.RoomList.Select(r => new Room
                {
                    AdultCount = r.AdultCount,
                    ChildrenCount = r.ChildrenCount,
                    RoomDescription = r.RoomDescription,
                    RoomId = r.RoomId,
                    RoomName = r.RoomName,
                    FinalRoomPrice = HotelPriceUtil.CountPrice(r.RoomPrice)
                })
            });
        }

        private static IEnumerable<RoomPackage> GetDummyRoomPackages()
        {
            return null;
            /*var list = new List<RoomPackage>
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

            return list;*/
        }
    }
}
