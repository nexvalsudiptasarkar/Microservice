using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ObjectExtensions
{
    public static class Extensions
    {
        public static bool CompareEquals<T>(this T objectFromCompare, T objectToCompare, bool includeNonPublic = false)
        {
            bool result = (objectFromCompare == null && objectToCompare == null);
            if (!result)
            {
                Type fromType = objectFromCompare.GetType();
                if (fromType.IsPrimitive)
                {
                    result = objectFromCompare.Equals(objectToCompare);
                }
                else if (fromType.FullName.Contains("System.String"))
                {
                    result = ((objectFromCompare as string) == (objectToCompare as string));
                }
                else if (fromType.FullName.Contains("DateTime"))
                {
                    result = DateTime.Parse(objectFromCompare.ToString()).Ticks == DateTime.Parse(objectToCompare.ToString()).Ticks;
                }
                else if (fromType.FullName.Contains("System.Text.StringBuilder"))
                {
                    result = ((objectFromCompare as StringBuilder).ToString() == (objectToCompare as StringBuilder).ToString());
                }

                else if (fromType.IsGenericType || fromType.IsArray)
                {
                    string propName = (fromType.IsGenericType) ? "Count" : "Length";
                    string methName = (fromType.IsGenericType) ? "get_Item" : "Get";
                    PropertyInfo propInfo = fromType.GetProperty(propName);
                    MethodInfo methInfo = fromType.GetMethod(methName);
                    if (propInfo != null && methInfo != null)
                    {
                        int fromCount = (int)propInfo.GetValue(objectFromCompare, null);
                        int toCount = (int)propInfo.GetValue(objectToCompare, null);
                        result = (fromCount == toCount);
                        if (result && fromCount > 0)
                        {
                            for (int index = 0; index < fromCount; index++)
                            {
                                object fromItem = methInfo.Invoke(objectFromCompare, new object[] { index });
                                object toItem = methInfo.Invoke(objectToCompare, new object[] { index });
                                result = CompareEquals(fromItem, toItem);
                                if (!result)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else { }
                }
                else
                {
                    BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                    if (includeNonPublic)
                    {
                        flags |= BindingFlags.NonPublic;
                    }
                    PropertyInfo[] props = typeof(T).GetProperties(flags);
                    foreach (PropertyInfo prop in props)
                    {
                        Type type = fromType.GetProperty(prop.Name).GetValue(objectToCompare, null).GetType();
                        //Type type = objectFromCompare.GetType().GetProperty(prop.Name).GetValue(objectToCompare,null).GetType();
                        object dataFromCompare = objectFromCompare.GetType().GetProperty(prop.Name).GetValue(objectFromCompare, null);
                        object dataToCompare = objectToCompare.GetType().GetProperty(prop.Name).GetValue(objectToCompare, null);
                        result = CompareEquals(Convert.ChangeType(dataFromCompare, type), Convert.ChangeType(dataToCompare, type), includeNonPublic);
                        if (!result)
                        {
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}
