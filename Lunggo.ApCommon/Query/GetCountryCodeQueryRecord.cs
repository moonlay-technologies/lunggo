using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Query
{
    public class GetCountryCodeQueryRecord : QueryRecord
    {
        public long CountryCode { get; set; }
    }
}
