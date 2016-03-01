using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.AuthStore;
using Lunggo.ApCommon.Identity.User;
using Lunggo.ApCommon.Identity.UserStore;
using Lunggo.Framework.Encoder;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;

namespace Lunggo.ApCommon.Identity.Auth
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            var token = new RefreshToken()
            {
                Id = refreshTokenId.Sha512Encode(),
                ClientId = clientid,
                Subject = context.Ticket.Identity.Name,
                IssueTime = DateTime.UtcNow,
                ExpireTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssueTime;
            context.Ticket.Properties.ExpiresUtc = token.ExpireTime;

            token.ProtectedTicket = context.SerializeTicket();

            var result = await new DapperAuthStore().AddRefreshToken(token);

            if (result)
            {
                context.SetToken(refreshTokenId);
            }

        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = context.Token;

            var refreshToken = await new DapperAuthStore().FindRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                var result = await new DapperAuthStore().RemoveRefreshToken(hashedTokenId);
            }
        }
    }
}
