using System;
using System.Diagnostics;

namespace Shared.Data
{
	public static class Global
	{
		public static bool IsConsoleApp { get; set; }
		static Global()
		{
			Global.IsConsoleApp = false;
		}

		public static void WriteLine(string text)
		{
			if (Global.IsConsoleApp) { Console.WriteLine(text); }
			Debug.WriteLine(text);
		}
	}
}
