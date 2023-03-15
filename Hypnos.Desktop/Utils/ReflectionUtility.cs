using System.Collections.Generic;
using System.Reflection;
using Wholesale.Desktop.Attributes;

namespace Wholesale.Desktop.Utils
{
    public static class ReflectionUtility
    {
        public static List<string> GetHiddenNames<T>()
            where T : class
        {
            var names = new List<string>();

            foreach (var property in typeof(T).GetProperties())
            {
                if (null != property.GetCustomAttribute<HiddenColumn>())
                {
                    names.Add(property.Name);
                }
            }

            return names;
        }
    }
}
