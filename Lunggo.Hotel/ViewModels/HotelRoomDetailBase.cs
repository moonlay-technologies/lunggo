using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Hotel.ViewModels
{
    public class HotelRoomDetailBase
    {
        public string RoomName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public double Discount { get; set; }
        public List<string> ListPhotoUrl { get; set; }
    }
}
