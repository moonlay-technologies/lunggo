using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.AuthStore;

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
            var clientId = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientId))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            var refreshTokenLifeTime = context.OwinContext.Get<double>("as:clientRefreshTokenLifeTime");

            var deviceId = context.OwinContext.Get<string>("as:deviceId");

            var token = new RefreshToken
            {
                Id = refreshTokenId.Sha512Encode(),
                ClientId = clientId,
                Subject = context.Ticket.Identity.Name,
                DeviceId = deviceId,
                IssueTime = DateTime.UtcNow,
                ExpireTime = DateTime.UtcNow.AddMinutes(refreshTokenLifeTime)
            };

            context.Ticket.Properties.IssuedUtc = token.IssueTime;
            context.Ticket.Properties.ExpiresUtc = token.ExpireTime;

            token.ProtectedTicket = context.SerializeTicket();

            bool result;

            if (context.Ticket.Identity.Name != "anonymous")
            {
                result = await new DapperAuthStore().AddOrReplaceRefreshToken(token, ignoreDevice: true);
            }
            else
            {
                if (deviceId != null)
                {
                    result = await new DapperAuthStore().AddOrReplaceRefreshToken(token, ignoreDevice: false);
                }
                else
                {
                    refreshTokenId = clientId.Base64Encode().Base64Encode().Base64Encode().Base64Encode();
                    token.Id = refreshTokenId.Sha512Encode();
                    result = await new DapperAuthStore().AddOrReplaceRefreshToken(token, ignoreDevice: true);
                }
            }

            if (result)
            {
                context.SetToken(refreshTokenId);
            }

        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = context.Token.Sha512Encode();

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
