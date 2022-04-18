using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ObjectExtensions
{
    public static class ExtendLinqToXml
    {
		public static bool HasAttribute(this XElement element, string name)
		{
			bool result = false;
			IEnumerable<XAttribute> attributes = element.Attributes(name);
			result = (attributes.Count() > 0);
			return result;
		}

		public static bool HasElement(this XElement element, string name)
		{
			bool result = false;
			IEnumerable<XElement> elements = element.Elements(name);
			result = (elements.Count() > 0);
			return result;
		}

		public static T GetValue<T>(this XElement root, string name, T defaultValue)
		{
			T value = defaultValue;
			string strValue = (string)root.Elements(name).FirstOrDefault() ?? defaultValue.ToString();

			if (value is string)
			{
				return ((T)(object)strValue);
			}
			else
			{
				var tryParse = typeof (T).GetMethod("TryParse", new [] {typeof(string), typeof(T).MakeByRefType()});
				if (tryParse == null)
				{
					throw new InvalidOperationException();
				}
				var parameters = new object[] {strValue, value};
				if ((bool)tryParse.Invoke(null, parameters))
				{
					value = (T)parameters[1];
				}
			}
			return value;
		}

		public static T GetAttribute<T>(this XElement root, string name, T defaultValue)
		{
			T value = defaultValue;
			string strValue = (string)root.Attributes(name).FirstOrDefault() ?? defaultValue.ToString();
			if (value is string)
			{
				value = (T)(object)strValue;
			}
			else
			{
				var tryParse = typeof (T).GetMethod("TryParse", new [] {typeof(string), typeof(T).MakeByRefType()});
				if (tryParse == null)
				{
					throw new InvalidOperationException();
				}
				var parameters = new object[] {strValue, value};
				if ((bool)tryParse.Invoke(null, parameters))
				{
					value = (T)parameters[1];
				}
			}
			return value;
		}
    }
}
