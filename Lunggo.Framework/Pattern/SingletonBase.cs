using System;
using System.Reflection;

namespace Lunggo.Framework.Pattern
{
    public abstract class SingletonBase<T> where T: SingletonBase<T>
    {
        private static readonly T Instance = CreateInstance();

        private static T CreateInstance()
        {
            var constructor = typeof (T).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                Type.EmptyTypes,
                null
            );

            return (T) constructor.Invoke(null);
        }

        public static T  GetInstance()
        {
            return Instance;
        }
    }
}
