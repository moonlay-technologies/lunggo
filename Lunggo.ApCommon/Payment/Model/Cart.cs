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
        public List<string> RsvNoList;
        public decimal TotalPrice;
        public string CartId;
        public Contact Contact { get; set; }
        public HttpStatusCode StatusCode;
    }
}
