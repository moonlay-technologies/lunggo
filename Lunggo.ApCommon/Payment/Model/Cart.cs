using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Activity.Model;
using System.Net;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Payment.Model
{
    public class Cart 
    {
        public List<string> RsvNoList { get; set; }
        public decimal TotalPrice { get; set; }
        public string Id { get; set; }
        public Contact Contact { get; set; }
        public ActivityPackageReservation Pax { get; set; }
    }
}
