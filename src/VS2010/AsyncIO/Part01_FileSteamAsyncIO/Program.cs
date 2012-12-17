using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Part01_FileSteamAsyncIO
{
	class Program
	{
		static void Main(string[] args)
		{
			Thread.CurrentThread.Name = "Program.Main";

			FileReadApm.Run();
			FileReadTplFromAsync.Run();

			Console.ReadLine();
		}
	}
}
