using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.PaymentTest
{
    internal static class Helper
    {
        internal static void InvokePrivate(this object classObject, string methodName, params object[] methodParams)
        {
            var dynMethod = classObject.GetType().GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            dynMethod.Invoke(classObject, methodParams);
        }

        internal static T InvokePrivate<T>(this object classObject, string methodName, params object[] methodParams)
        {
            var dynMethod = classObject.GetType().GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            return (T) dynMethod.Invoke(classObject, methodParams);
        }
    }
}
