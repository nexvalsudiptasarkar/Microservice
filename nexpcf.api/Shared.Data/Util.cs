using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;
using System.Xml;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace Shared.Data
{
    public static class Util
    {
        public static string CS(string env = "UAT")
        {
            try
            {
                string connStr = "server=35.197.140.185;port=3306;database=nexat_uat;uid=root;password=Nexval@12345;Pooling=True;Persist Security Info=False;Connect Timeout=300";
                if (env == "PROD")
                {
                    connStr = "server=35.244.34.128;port=3306;database=nexat;uid=root;password=#pass1234;Pooling=True;Persist Security Info=False;Connect Timeout=300";
                }

                return connStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T ChangeType<T>(object value)
        {
            var t = typeof(T);
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default(T);
                }
                t = Nullable.GetUnderlyingType(t);
            }
            return (T)Convert.ChangeType(value, t);
        }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }
                t = Nullable.GetUnderlyingType(t);
            }
            return Convert.ChangeType(value, t);
        }

        public static void CopyPropertiesFrom(this object self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            string fromClassName = parent.GetType().Name;
            string toClassName = self.GetType().Name;

            foreach (var fromProperty in fromProperties)
            {
                foreach (var toProperty in toProperties)
                {
                    if (toProperty.Name == String.Format("in_{0}", fromProperty.Name))
                    {
                        string? val = null;
                        try
                        {
                            if (fromProperty.GetValue(parent) != null)
                            {
                                val = Convert.ToString(fromProperty.GetValue(parent));
                            }
                            else
                            {
                                val = null;
                            }
                            toProperty.SetValue(self, ChangeType(val, toProperty.PropertyType));
                        }
                        catch
                        { }
                        break;
                    }
                   
                }
            }
        }

        public static MySqlParameter[] GetParameter(object obj)
        {
            string name = string.Empty;
            try
            {

                Type myType = obj.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
                MySqlParameter[] arrSqlParameter = new MySqlParameter[(props.Count)];
                int index = 0;
                foreach (PropertyInfo prop in props)
                {
                    Type propType = prop.GetType();

                    object propValue = prop.GetValue(obj, null);
                    // string name = prop.Name;

                    name = prop.Name;
                    if (prop.PropertyType == typeof(XmlDocument))
                    {
                        if (propValue != null)
                        {
                            arrSqlParameter[index] = new MySqlParameter("@" + name, MySqlDbType.Text);
                            //arrSqlParameter[index] = new MySqlParameter("@" + name, SqlDbType.Xml);
                            arrSqlParameter[index].Value = ((XmlDocument)propValue).InnerXml;
                        }
                        else
                        {
                            arrSqlParameter[index] = new MySqlParameter("@" + name, MySqlDbType.Text);
                            //arrSqlParameter[index] = new MySqlParameter("@" + name, SqlDbType.Xml);
                            arrSqlParameter[index].Value = null;
                        }

                    }

                    else if (prop.PropertyType != typeof(XmlDocument))
                    {
                        if (prop.GetType() == typeof(DateTime))
                        {
                            DateTime objDate = new DateTime();
                            arrSqlParameter[index] = ((DateTime)propValue != objDate) ? new MySqlParameter("@" + name, propValue) : new MySqlParameter("@" + name, null);
                        }
                        else
                        {
                            //arrSqlParameter[index] = new SqlParameter("@" + name, propValue);
                            bool isOutputParam = false;
                            foreach (Attribute a in prop.GetCustomAttributes(false))
                            {
                                //if (a is outputparam || a.TypeId.ToString() == "outputparam")
                                if (a is outputparam)
                                {
                                    outputparam os = (outputparam)a;
                                    if (os.ParamType == ParamType.Output)
                                    {
                                        isOutputParam = true;
                                    }
                                }
                            }
                            if (!isOutputParam)
                            {
                                arrSqlParameter[index] = new MySqlParameter("@" + name, propValue);
                            }
                            else
                            {
                                int size = default(int);

                                foreach (PropertyInfo propertyInfo in prop.GetType().GetProperties())
                                {
                                    if (propertyInfo.PropertyType == typeof(int))
                                        size = 4;
                                    else if (propertyInfo.PropertyType == typeof(string))
                                        size = 2048;
                                    else if (propertyInfo.PropertyType == typeof(long))
                                        size = 8;
                                    else
                                        size = 0;
                                }

                                var outputParam = new MySqlParameter("@" + name, propValue) { Direction = ParameterDirection.Output, Size = size > 0 ? size : 2048 };
                                arrSqlParameter[index] = outputParam;
                            }
                        }
                    }
                    index++;
                }

                return arrSqlParameter;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

    /*public static class extntionMethod
    {
        public static bool HasMethod(this object objectToCheck, string methodName)
        {
            var type = objectToCheck.GetType();
            return type.GetMethod(methodName) != null;
        }
    }*/

    public class DualReturnType
    {
        public DataTable dt { get; set; }
        public string sp { get; set; }
    }


}
