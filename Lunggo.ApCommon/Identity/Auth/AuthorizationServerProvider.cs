using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Identity.AuthStore;

using Lunggo.ApCommon.Identity.Users;
using Lunggo.ApCommon.Identity.UserStore;
using Lunggo.Framework.Encoder;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Lunggo.ApCommon.Identity.Auth
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            string clientId;
            string clientSecret;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.Validated();
                context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            var client = new DapperAuthStore().FindClient(context.ClientId);

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == ApplicationType.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != clientSecret.Sha512Encode())
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.IsActive)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set("as:deviceId", context.Parameters.Get("device_id") ?? "");

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var user = await new DapperUserStore<User>().FindByNameAsync(context.UserName);

            if (user == null)
            {
                context.SetError("not_registered", "The user is not registered.");
                return;
            }

            var userManager = new UserManager<User>(new DapperUserStore<User>());
            var isPasswordOk = await userManager.CheckPasswordAsync(user, context.Password);
            //if (!user.EmailConfirmed)
            //{
            //    context.SetError("not_active", "User is not yet activated.");
            //    return;
            //}

            if (!isPasswordOk)
            {
                context.SetError("invalid_grant", "Wrong username and password combination.");
                return;
            }
            
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Authentication, "password"));
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("Client ID", context.ClientId ?? ""));
            identity.AddClaim(new Claim("Device ID", context.OwinContext.Get<string>("as:deviceId") ?? ""));

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { 
                        "as:client_id", context.ClientId ?? ""
                    },
                    { 
                        "as:device_id", context.OwinContext.Get<string>("as:deviceId") ?? ""
                    },
                    { 
                        "userName", context.UserName
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            if (context.Identity.Name != "anonymous")
            {
                context.Properties.ExpiresUtc = context.Properties.IssuedUtc.GetValueOrDefault().AddHours(4);
                context.Properties.Dictionary[".expires"] = context.Properties.ExpiresUtc.Value.ToString("r");
                context.OwinContext.Set("as:clientRefreshTokenLifeTime", TimeSpan.FromDays(180).TotalMinutes);
            }
            else
            {
                context.Properties.ExpiresUtc = context.Properties.IssuedUtc.GetValueOrDefault().AddMonths(6);
                context.Properties.Dictionary[".expires"] = context.Properties.ExpiresUtc.Value.ToString("r");
                context.OwinContext.Set("as:clientRefreshTokenLifeTime", TimeSpan.FromDays(365).TotalMinutes);
            }

            context.Issue(context.Identity, context.Properties);
            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"] ?? "";
            var currentClient = context.ClientId ?? "";

            var originalDevice = context.Ticket.Properties.Dictionary["as:device_id"] ?? "";
            var currentDevice = context.OwinContext.Get<string>("as:deviceId") ?? "";

            if (originalClient != currentClient || originalDevice != currentDevice)
            {
                context.SetError("invalid_grant", "Refresh token is issued to a different client.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Authentication, "client_credentials"));
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, "anonymous"));
            oAuthIdentity.AddClaim(new Claim("Client ID", context.ClientId ?? ""));
            oAuthIdentity.AddClaim(new Claim("Device ID", context.OwinContext.Get<string>("as:deviceId") ?? ""));

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { 
                        "as:client_id", context.ClientId ?? ""
                    },
                    { 
                        "as:device_id", context.OwinContext.Get<string>("as:deviceId") ?? ""
                    }
                });

            var ticket = new AuthenticationTicket(oAuthIdentity, props);
            context.Validated(ticket);
            return Task.FromResult<object>(null);
        }
    }
}
