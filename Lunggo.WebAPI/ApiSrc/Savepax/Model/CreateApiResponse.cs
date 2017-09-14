using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.WebAPI.ApiSrc.Savepax.Model
{
    public class CreateApiResponse
    {
        public int Id { get; set; }
        public PaxForDisplay PaxForDisplay { get; set; }
    }
}