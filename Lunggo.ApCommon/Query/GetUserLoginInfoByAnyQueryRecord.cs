using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Query
{
    public class GetUserLoginInfoByAnyQueryRecord : QueryRecord
    {
        public String LoginProvider { get; set; }
        public String ProviderKey { get; set; }
    }
}
