using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Hotels.Model
{
    public class HotelExcerpt : HotelDetailBase
    {
        public Price LowestPrice { get; set; }
    }
}