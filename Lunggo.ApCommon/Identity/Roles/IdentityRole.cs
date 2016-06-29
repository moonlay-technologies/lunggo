using Microsoft.AspNet.Identity;

namespace Lunggo.ApCommon.Identity.Roles
{
    public class IdentityRole<TKey> : IRole<TKey>
    {
        public TKey Id { get; set; }

        public string Name { get; set; }
    }
}
