using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
	public static class ExtendExceptions
	{
		public static T FindInnerException<T>(this Exception ex) where T : Exception
		{
			if (!ex.GetType().Equals(typeof(T)))
			{
				Exception inner = ex.InnerException;
				if (inner == null)
				{
					return null;
				}
				else
				{
					if (inner.GetType().Equals(typeof(T)))
					{
						return (T)inner;
					}
					else
					{
						return inner.FindInnerException<T>();
					}
				}
			}
			else
			{
				return (T)ex;
			}
		}

		public static string GetNestedExceptions(this Exception ex)
		{
			StringBuilder str = new StringBuilder();
			str.AppendFormat("{0}", ex.Message).AppendLine();
			if (ex.InnerException != null)
			{
				str.Append(ex.InnerException.GetNestedExceptions());
			}
			return str.ToString();
		}
	}
}
