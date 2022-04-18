using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
	public static class ExtendEnumerators
	{
		public static bool TryParse<T>(this Enum theEnum, string value, out T returnValue)
		{
			bool result = false;
			returnValue = default(T);
			int ordinal;
			if (Int32.TryParse(value, out ordinal))
			{
				result = theEnum.TryParse(ordinal, out returnValue);
				//if (Enum.IsDefined(typeof(T), intEnumValue))
				//{
				//	returnValue = (T)(object)intEnumValue;
				//	result = true;
				//}
			}
			return result;
		}

		public static bool TryParse<T>(this Enum theEnum, int value, out T returnValue)
		{
			bool result = false;
			returnValue = default(T);
			if (Enum.IsDefined(typeof(T), value))
			{
				returnValue = (T)(object)value;
				result = true;
			}
			return result;
		}
	}
}
