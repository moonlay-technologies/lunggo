using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lunggo.Framework.Util
{
    public class ReflectionUtil
    {
        public static IEnumerable<String> GetPropertyNameList(Object source)
        {
            BindingFlags defaultBindingFlags = BindingFlags.Instance | BindingFlags.Public;
            return GetPropertyNameList(source, defaultBindingFlags);
        }

        public static IEnumerable<String> GetPropertyNameList(Type type)
        {
            BindingFlags defaultBindingFlags = BindingFlags.Instance | BindingFlags.Public;
            return GetPropertyNameList(type, defaultBindingFlags);
        }

        public static IEnumerable<String> GetPropertyNameList(Type type,BindingFlags bindingFlags)
        {
            List<String> propertyNameList = new List<String>();
            FillPropertyNameList(propertyNameList, type, bindingFlags);
            return propertyNameList;
        }

        public static IEnumerable<String> GetPropertyNameList(Object source,BindingFlags bindingFlags)
        {
            List<String> propertyNameList = new List<String>();
            FillPropertyNameList(propertyNameList, source, bindingFlags);
            return propertyNameList;
        }

        private static void FillPropertyNameList(List<String> propertyNameList, Object source,BindingFlags bindingFlags)
        {
            FillPropertyNameList(propertyNameList, source.GetType(), bindingFlags);
        }

        private static void FillPropertyNameList(List<String> propertyNameList, Type type, BindingFlags bindingFlags)
        {
            foreach (var property in type.GetProperties(bindingFlags))
            {
                propertyNameList.Add(property.Name);
            }
        }
    }
}