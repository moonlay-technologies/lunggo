using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Identity.Auth
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string ClientId { get; set; }
        public DateTime IssueTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public string ProtectedTicket { get; set; }
    }
}
