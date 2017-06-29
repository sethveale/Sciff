using System;
using System.Linq;
using System.Reflection;

namespace Sciff.Logic.LambdaReflection.Members
{
    /// <summary>
    ///     Wraps up ways to reflect on Properties and Fields for a type
    /// </summary>
    public static class Values<T>
    {
        /// <summary>
        ///     Finds the member matching a name and type
        /// </summary>
        /// <exception cref="MissingMemberException" />
        public static Tuple<PropertyInfo, FieldInfo> AsMember<TValue>(string name)
        {
            var result = AsMember(name);
            var (property, field) = result;

            if (property != null && property.PropertyType != typeof(TValue) ||
                field != null && field.FieldType != typeof(TValue))
                throw new MissingMemberException(typeof(TValue).Name, name);

            return result;
        }

        /// <summary>
        ///     Finds the member matching a name, regardless of type.
        /// </summary>
        /// <exception cref="MissingMemberException" />
        public static Tuple<PropertyInfo, FieldInfo> AsMember(string name)
        {
            var result = typeof(T).GetMember(name)
                .Select(m => Tuple.Create(m as PropertyInfo, m as FieldInfo))
                .FirstOrDefault(t =>
                {
                    var (property, field) = t;
                    return property != null || field != null;
                });

            if (result == null)
                throw new MissingMemberException(typeof(T).Name, name);

            return result;
        }
    }
}