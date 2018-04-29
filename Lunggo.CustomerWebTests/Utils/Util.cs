using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lunggo.CustomerWebTests.Utils
{
    internal static class Util
    {
        [TestInitialize]
        internal static Dictionary<string, object> GetRouteValues(this RedirectToRouteResult result)
        {
            var dict = new Dictionary<string, object>();
            if (result == null) return dict;

            foreach (var routeValue in result.RouteValues)
            {
                dict.Add(routeValue.Key, routeValue.Value);
            }

            return dict;
        }

        [TestInitialize]
        internal static IEnumerable<TEnum> EnumerateEnum<TEnum>()
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("Type is not enum");

            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}
