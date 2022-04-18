﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
	public static class ExtendChars
	{
		public static bool IsAsciiNumeric(this char ch)
		{
			int chInt = (int)ch;
			return (chInt >= 48 && chInt <= 57);
		}

		public static bool IsAsciiAlpha(this char ch)
		{
			int chInt = (int)ch;
			return ((chInt >= 65 && chInt <= 90) || (chInt >=97 && chInt <=122));
		}

		public static bool IsAsciiNonPrintable(this char ch)
		{
			int chInt = (int)ch;
			return (chInt >= 0 && chInt <= 31 || chInt == 127);
		}

		public static bool IsAsciiOther(this char ch)
		{
			return (!ch.IsAsciiNumeric() && !ch.IsAsciiAlpha() && !ch.IsAsciiNonPrintable());
		}
	}
}
