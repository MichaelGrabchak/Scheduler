using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Scheduler.Core.Attributes;

namespace Scheduler.Core.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Builds list of key-value pairs based on object properties
        /// </summary>
        /// <param name="obj">The object with properties</param>
        /// <returns>Returns the object, represented as key-value(string,string) pairs</returns>
        public static IList<KeyValuePair<string, string>> AsKeyValuePairs(this object obj)
        {
            var result = new List<KeyValuePair<string, string>>();

            if (obj != null)
            {
                var properties = obj.GetType().GetProperties();
                foreach (var propertyInfo in properties)
                {
                    var value = propertyInfo.GetValue(obj)?.ToString() ?? string.Empty;
                    if (propertyInfo.GetCustomAttribute(typeof(KeyAttribute), true) is KeyAttribute attribute)
                    {
                        result.Add(new KeyValuePair<string, string>(attribute.Name, value));
                    }
                    else
                    {
                        result.Add(new KeyValuePair<string, string>(propertyInfo.Name, value));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Fills the object based on list of key-value pairs
        /// </summary>
        /// <param name="obj">The target object which need to be filled out</param>
        /// <param name="keyValuePairs">The list of key-value pairs</param>
        /// <returns>Returns the object, updated with the values from passed key-value pairs</returns>
        public static object SetObject(this object obj, IList<KeyValuePair<string, string>> keyValuePairs)
        {
            if (keyValuePairs != null && keyValuePairs.Count > 0)
            {
                var properties = obj.GetType().GetProperties();
                foreach (var propertyInfo in properties)
                {
                    var attribute = propertyInfo.GetCustomAttribute(typeof(KeyAttribute), true) as KeyAttribute;
                    var propertyName = !string.IsNullOrEmpty(attribute?.Name)
                        ? attribute.Name
                        : propertyInfo.Name;

                    var keyValuePair = keyValuePairs.FirstOrDefault(kvp =>
                        kvp.Key.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));

                    if (!keyValuePair.Equals(default(KeyValuePair<string, string>)))
                    {
                        keyValuePair.Value.TryConvert(propertyInfo.PropertyType, out var value);
                        if (value != null)
                        {
                            propertyInfo.SetValue(obj, value);
                        }
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// Try to cast the object from one type to another
        /// </summary>
        /// <param name="inputValue">The source object</param>
        /// <param name="type">The target type</param>
        /// <param name="outputValue">The target object</param>
        /// <returns>Returns the result of conversion operation</returns>
        public static bool TryConvert(this object inputValue, Type type, out object outputValue)
        {
            outputValue = null;

            try
            {
                if (inputValue == null) return true;

                var convertType = Nullable.GetUnderlyingType(type) ?? type;
                if (inputValue is string stringValue)
                {
                    if (string.IsNullOrEmpty(stringValue)) return true;

                    if (convertType == typeof(DateTime))
                    {
                        var pattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                        var parseResult = DateTime.TryParseExact(stringValue, pattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeValue);
                        if (!parseResult) return false;

                        outputValue = dateTimeValue;
                        return true;
                    }

                    if (convertType.IsEnum)
                    {
                        outputValue = Enum.Parse(convertType, stringValue);
                        return true;
                    }
                }

                outputValue = Convert.ChangeType(inputValue, convertType);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Casts the object to specified type
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="value">The source object</param>
        /// <returns>Returns the object of specified type</returns>
        public static T To<T>(this object value)
        {
            // Get the type that was made nullable
            Type valueType = Nullable.GetUnderlyingType(typeof(T));

            if (valueType != null)
            {
                // Nullable type
                if (value == null)
                {
                    return default(T);
                }

                // Convert to the value type
                object result = Convert.ChangeType(value, valueType);

                // Cast the value type to the nullable type
                return (T)result;
            }

            // Not nullable
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
