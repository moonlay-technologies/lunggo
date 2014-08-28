using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Query
{
    public class GetRoleByAnyQueryRecord : QueryRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
