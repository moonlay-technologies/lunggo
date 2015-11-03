using Microsoft.AspNet.Identity;

namespace Lunggo.ApCommon.Identity.Role
{
    public class IdentityRole<TKey> : IRole<TKey>
    {
        public TKey Id { get; set; }

        public string Name { get; set; }
    }
}
