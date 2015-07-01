using System;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query.Record
{
    public class GetUserLoginInfoByAnyQueryRecord : QueryRecord
    {
        public String LoginProvider { get; set; }
        public String ProviderKey { get; set; }
    }
}
