using Lunggo.ApCommon.Hotel.Model;
using Lunggo.CustomerWeb.Areas.UW100.Models;
using System.Collections.Generic;

namespace Lunggo.CustomerWeb.Areas.UW200.Models
{
    public class UW200HotelDetailViewModel : HotelDetailBase
    {
        public List<UW200PlacesNearHotelViewModel> InterestingPlacesNearby { get; set; }
        public UW100SearchParamViewModel SearchParam { get; set; }
    }
}