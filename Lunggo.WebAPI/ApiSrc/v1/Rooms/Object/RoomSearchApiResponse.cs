using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Rooms.Object
{
    public class RoomSearchApiResponse
    {
        public int TotalPackageCount { get; set; }
        public IEnumerable<RoomPackageExcerpt> PackageList { get; set; }
        public RoomSearchApiRequest InitialRequest { get; set; }
    }
}