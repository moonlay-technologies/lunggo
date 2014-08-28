using System.Security.Claims;
using System.Threading.Tasks;
using Lunggo.ApCommon.Sequence;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lunggo.ApCommon.Identity.User
{
    public class CustomUser : UserBase<string>
    {
        public CustomUser() 
        {
            Id = UserIdSequence.GetInstance().GetNext().ToString();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<CustomUser,string> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}