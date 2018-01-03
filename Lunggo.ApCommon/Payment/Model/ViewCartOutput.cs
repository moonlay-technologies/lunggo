using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Activity.Model;
using System.Net;

namespace Lunggo.ApCommon.Payment.Model
{
    public class ViewCartOutput 
    {
        public List<string> RsvNoList;
        public decimal TotalPrice;
        public string CartId;
        public HttpStatusCode StatusCode;
    }
}
