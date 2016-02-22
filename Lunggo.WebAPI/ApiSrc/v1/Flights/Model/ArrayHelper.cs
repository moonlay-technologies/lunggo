using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    class ListConverter<T> : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    var elements = (value as string).Split(',');

                    if (typeof (T) == typeof (int))
                        return elements.Select(int.Parse).ToList();
                    if (typeof(T) == typeof(long))
                        return elements.Select(long.Parse).ToList();
                    if (typeof(T) == typeof(decimal))
                        return elements.Select(decimal.Parse).ToList();
                    if (typeof(T) == typeof(float))
                        return elements.Select(float.Parse).ToList();
                    if (typeof(T) == typeof(double))
                        return elements.Select(double.Parse).ToList();
                }
                catch
                {
                    return null;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}