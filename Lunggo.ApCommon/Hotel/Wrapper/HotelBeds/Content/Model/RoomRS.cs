using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class RoomRS
    {
        public int total { get; set; }
        public List<RoomApi> rooms { get; set; }
    }

    public class RoomApi
    {
        public string code { get; set; }
        public string type { get; set; }
        public string characteristic { get; set; }
        public int minPax { get; set; }
        public int maxPax { get; set; }
        public int maxAdults { get; set; }
        public int maxChildren { get; set; }
        public int minAdults { get; set; }
        public string description { get; set; }
        public string descriptionEng { get; set; }
        public string descriptionInd { get; set; }
        public Description typeDescription { get; set; }
        public string typeDescriptionEng { get; set; }
        public string typeDescriptionInd { get; set; }
        public Description characteristicDescription { get; set; }
        public string characteristicDescriptionEng { get; set; }
        public string characteristicDescriptionInd { get; set; }
    }
}
