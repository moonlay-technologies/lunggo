using System;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Web;

namespace Lunggo.Framework.TestHelpers
{
    public static partial class TestHelper
    {
        public static HttpContext LoginGuest()
        {
            var context = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
            );

            context.User = new GenericPrincipal(
                new GenericIdentity("anonymous"),
                new string[0]
            );

            return context;
        }

        public static HttpContext LoginUser(string username)
        {
            var context = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
            );

            var identity = new GenericIdentity(username);

            context.User = new GenericPrincipal(
                identity,
                new string[0]
            );

            return context;
        }

        public static HttpContext NoLogin()
        {
            var context = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
            );

            context.User = new GenericPrincipal(
                new GenericIdentity(""),
                new string[0]
            );

            return context;
        }

        public static HttpContext LoginInvalidUser()
        {
            var context = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
            );

            context.User = new GenericPrincipal(
                new GenericIdentity(RandomString()),
                new string[0]
            );

            return context;
        }

        public static HttpContext LoginRandomUser()
        {
            var context = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
            );

            context.User = new GenericPrincipal(
                new GenericIdentity(RandomString()),
                new string[0]
            );

            return context;
        }
    }
}