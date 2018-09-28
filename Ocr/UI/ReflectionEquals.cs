using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ocr.UI
{
    static class ReflectionEquals
    {
        public static bool DeepEquals<T>(this T self, T to)
        {
            Type type = self.GetType();
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                {
                    if (selfValue != null && toValue != null && selfValue.GetType().Assembly == assembly)
                    {
                        if (!selfValue.DeepEquals(toValue))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
