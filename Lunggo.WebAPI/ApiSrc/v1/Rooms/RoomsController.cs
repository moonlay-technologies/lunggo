using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Lunggo.ApCommon.Model;
using Lunggo.WebAPI.ApiSrc.v1.Rooms.Object;

namespace Lunggo.WebAPI.ApiSrc.v1.Rooms
{
    public class RoomsController : ApiController
    {
        [EnableCors(origins: "http://localhost", headers: "*", methods: "*")]
        [Route("api/v1/rooms")]
        public RoomSearchApiResponse GetRooms(HttpRequestMessage httpRequest, [FromUri] RoomSearchApiRequest request)
        {
            var checkedRequest = CreateRoomSearchApiRequestIfNull(request);
            var packageList = GetDummyData();
            var roomPackageExcerpts = packageList as IList<RoomPackageExcerpt> ?? packageList.ToList();
            var apiResponse = new RoomSearchApiResponse
            {
                TotalPackageCount = roomPackageExcerpts.Count,
                PackageList = roomPackageExcerpts,
                InitialRequest = checkedRequest
            };
            return apiResponse;
        }

        private RoomSearchApiRequest CreateRoomSearchApiRequestIfNull(RoomSearchApiRequest request)
        {
            return request ?? new RoomSearchApiRequest();
        }

        private IEnumerable<RoomPackageExcerpt> GetDummyData()
        {
            var list = new List<RoomPackageExcerpt>
            {
                new RoomPackageExcerpt
                {
                    FinalPackagePrice = new Price
                    {
                        Currency = "IDR",
                        Value = 500000
                    },
                  PackageId = 1,
                  RoomList = new List<RoomExcerpt>
                  {
                    new RoomExcerpt
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
                new RoomPackageExcerpt
                {
                    FinalPackagePrice = new Price
                    {
                        Currency = "IDR",
                        Value = 2000000
                    },
                  PackageId = 2,
                  RoomList = new List<RoomExcerpt>
                  {
                    new RoomExcerpt
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
                    new RoomExcerpt
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
                new RoomPackageExcerpt
                {
                    FinalPackagePrice = new Price
                    {
                        Currency = "IDR",
                        Value = 4500000
                    },
                  PackageId = 3,
                  RoomList = new List<RoomExcerpt>
                  {
                    new RoomExcerpt
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
                    new RoomExcerpt
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
                    new RoomExcerpt
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

    

    public class RoomPackageExcerpt
    {
        public long PackageId { get; set; }
        public Price FinalPackagePrice { get; set; }
        public IEnumerable<RoomExcerpt> RoomList { get; set; } 
    }

    public class RoomExcerpt
    {
        public String RoomName { get; set; }
        public long RoomId { get; set; }
        public int AdultCount { get; set; }
        public int ChildrenCount { get; set; }
        public String RoomDescription { get; set; }
        public Price FinalRoomPrice { get; set; }
    }
}
