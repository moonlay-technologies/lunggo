namespace Lunggo.ApCommon.Identity.Roles
{
    public class IdentityUserRole<TKey>
    {
        /// <summary>
        /// UserId for the user that is in the role
        /// 
        /// </summary>
        public virtual TKey UserId { get; set; }

        /// <summary>
        /// RoleId for the role
        /// 
        /// </summary>
        public virtual TKey RoleId { get; set; }
    }
}
