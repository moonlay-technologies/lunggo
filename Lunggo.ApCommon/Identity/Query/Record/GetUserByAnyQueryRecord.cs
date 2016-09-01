using System;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Identity.Query.Record
{
    public class GetUserByAnyQueryRecord : QueryRecord
    {
        public string Email { get; set; }
        
        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string CountryCallCd { get; set; }
        
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }
        
        public bool TwoFactorEnabled { get; set; }
        
        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }
        
        public int AccessFailedCount { get; set; }

        public virtual string Id { get; set; }

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
    }
}
