using System.Reflection;

namespace Lunggo.Framework.TestHelpers
{
    public static partial class TestHelper
    {
        public static void InvokePrivate(this object classObject, string methodName, params object[] methodParams)
        {
            var dynMethod = classObject.GetType().GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            dynMethod.Invoke(classObject, methodParams);
        }

        public static T InvokePrivate<T>(this object classObject, string methodName, params object[] methodParams)
        {
            var dynMethod = classObject.GetType().GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            return (T) dynMethod.Invoke(classObject, methodParams);
        }
    }
}