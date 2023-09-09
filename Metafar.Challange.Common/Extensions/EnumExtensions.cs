using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Metafar.Challange.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string? GetFriendlyName<TEnumType>(this TEnumType enumValue) where TEnumType : struct
        {
            return enumValue.GetAttribute<TEnumType, DisplayAttribute>()?.Name ?? enumValue.ToString();
        }

        private static TAttribute? GetAttribute<TEnumType, TAttribute>(this TEnumType enumValue) where TEnumType : struct
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .FirstOrDefault()?
                            .GetCustomAttribute<TAttribute>();
        }

        public static TEnumType ToEnum<TEnumType>(this string value)
        {
            var fields = typeof(TEnumType).GetFields();

            foreach (var field in fields)
            {
                if (field.FieldType != typeof(TEnumType)) continue;

                var enumValue = (TEnumType)field.GetValue(null);
                if (field.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase)) return enumValue;

                var display = field.GetCustomAttribute<DisplayAttribute>();
                if (display != null)
                {
                    if (display.Name?.Equals(value, StringComparison.InvariantCultureIgnoreCase) ?? false) return enumValue;
                    if (display.ShortName?.Equals(value, StringComparison.InvariantCultureIgnoreCase) ?? false) return enumValue;
                }
            }

            return default(TEnumType);
        }

        public static TEnumType ToEnum<TEnumType>(this int value) where TEnumType : struct
        {
            return System.Enum.IsDefined(typeof(TEnumType), value)
                   ? (TEnumType)System.Enum.ToObject(typeof(TEnumType), value)
                   : default(TEnumType);
        }

        public static int AsInt<TEnumType>(this TEnumType value) where TEnumType : struct, IConvertible
        {
            if (!typeof(TEnumType).IsEnum)
            {
                throw new ArgumentException("TEnumType must be an enumerated type");
            }

            return Convert.ToInt32(value);
        }

        public static IDictionary<int, string> GetEnumValues<TEnumType>(this TEnumType obj)
            where TEnumType : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof(TEnumType).IsEnum)
            {
                throw new ArgumentException("TEnumType must be an enumerated type");
            }

            return System.Enum.GetValues(typeof(TEnumType))
                .OfType<TEnumType>()
                .ToDictionary(x => x.AsInt(), x => x.GetFriendlyName());
        }
    }
}
