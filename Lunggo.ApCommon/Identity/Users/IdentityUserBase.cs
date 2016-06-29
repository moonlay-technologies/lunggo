using Microsoft.AspNet.Identity;

namespace Lunggo.ApCommon.Identity.Users
{
    public class IdentityUserBase<TKey> : IUser<TKey>
    {
        public virtual TKey Id { get; set; }

        public string UserName { get; set; }
    }
}
