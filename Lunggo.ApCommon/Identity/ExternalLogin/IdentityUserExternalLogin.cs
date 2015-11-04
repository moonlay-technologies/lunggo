namespace Lunggo.ApCommon.Identity.ExternalLogin
{
    public class IdentityUserExternalLogin<TKey>
    {
        /// <summary>
        /// The login provider for the login (i.e. facebook, google)
        /// 
        /// </summary>
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Key representing the login for the provider
        /// 
        /// </summary>
        public virtual string ProviderKey { get; set; }

        /// <summary>
        /// User Id for the user who owns this login
        /// 
        /// </summary>
        public virtual TKey UserId { get; set; }
    }

}
