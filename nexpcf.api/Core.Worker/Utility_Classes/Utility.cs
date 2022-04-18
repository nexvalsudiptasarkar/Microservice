using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Core.Worker.Utility_Classes
{
    public sealed class Utility
    {
        private static readonly Utility instance;

        private static readonly string[] UriRfc3986CharsToEscape;

        public static Utility Instance
        {
            get
            {
                return Utility.instance;
            }
        }

        static Utility()
        {
            Utility.instance = new Utility();
            Utility.UriRfc3986CharsToEscape = new string[]
        {
            "!",
            "*",
            "'",
            "(",
            ")"
        };
        }

        private Utility()
        {
        }

        public static string GetCustomDescription(Enum en)
        {
            FieldInfo fieldInfo = en.GetType().GetField(en.ToString());
            DescriptionAttribute[] descriptionAttribute =
                  (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                  typeof(DescriptionAttribute), false);
            return (descriptionAttribute.Length > 0) ? descriptionAttribute[0].Description : null;
        }

        public static XmlDocument CreateXml(DataTable dt)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "Root", null);
            xmlDocument.AppendChild(xmlNode);
            if (dt != null)
            {
                XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Element, "Rows", null);
                xmlNode.AppendChild(xmlNode2);
                foreach (DataRow dataRow in dt.Rows)
                {
                    XmlNode xmlNode3 = xmlDocument.CreateNode(XmlNodeType.Element, "Row", null);
                    xmlNode2.AppendChild(xmlNode3);
                    foreach (DataColumn dataColumn in dt.Columns)
                    {
                        XmlNode xmlNode4 = xmlDocument.CreateNode(XmlNodeType.Element, dataColumn.ColumnName, null);
                        xmlNode4.InnerText = Convert.ToString(dataRow[dataColumn.ColumnName]);
                        xmlNode3.AppendChild(xmlNode4);
                    }
                }
            }
            return xmlDocument;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] array = properties;
            for (int i = 0; i < array.Length; i++)
            {
                PropertyInfo propertyInfo = array[i];
                dataTable.Columns.Add(propertyInfo.Name);
            }
            foreach (T current in items)
            {
                object[] array2 = new object[properties.Length];
                for (int j = 0; j < properties.Length; j++)
                {
                    array2[j] = properties[j].GetValue(current, null);
                }
                dataTable.Rows.Add(array2);
            }
            return dataTable;
        }

        public static object MagicallyCreateInstance(string classLibraryName, string className)
        {
            object result;
            try
            {
                Type type = null;
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                for (int i = 0; i < assemblies.Length; i++)
                {
                    Assembly assembly = assemblies[i];
                    if (assembly.FullName.Contains(classLibraryName))
                    {
                        type = assembly.GetTypes().FirstOrDefault((Type t) => t.Name == className);
                        break;
                    }
                }
                result = Activator.CreateInstance(type);
            }
            catch (Exception)
            {
                throw new Exception("Cannot find the matching class of specified UserControl. Please contact with Administrator/Developer.");
            }
            return result;
        }
    }
}
