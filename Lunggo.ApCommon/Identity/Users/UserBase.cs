using System;

namespace Lunggo.ApCommon.Identity.Users
{
    public class UserBase<TKey> : IdentityUserBase<TKey>
    {
        /// <summary>
        /// Email
        /// 
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// True if the email is confirmed, default is false
        /// 
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// The salted/hashed form of the user password
        /// 
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// 
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// CountryCd for the user
        /// 
        /// </summary>
        public virtual string CountryCallCd { get; set; }

        /// <summary>
        /// PhoneNumber for the user
        /// 
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// True if the phone number is confirmed, default is false
        /// 
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// FirstName of the user
        /// 
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// LastName of the user
        /// 
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Address of the user
        /// 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Platform of the user
        /// 
        /// </summary>
        public string PlatformCd { get; set; }

        /// <summary>
        /// Is two factor enabled for the user
        /// 
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// 
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Is lockout enabled for this user
        /// 
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// Used to record failures for the purposes of lockout
        /// 
        /// </summary>
        public virtual int AccessFailedCount { get; set; }
    }
}
