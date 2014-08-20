using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Hotel.ViewModels
{
    public class HotelBookDetail:OrderDetail
    {
    }
    public class Customer
    {
    }
    public class OrderDetail
    {
        public Customer CustomerDetail { get; set; }
    }
}
