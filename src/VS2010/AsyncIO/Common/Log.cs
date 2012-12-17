using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Common
{
	public static class Log
	{
		public static void Write(string message)
		{
			Console.WriteLine("Thread: {0} {1} | {2}", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId, message);
		}

		public static void Write(string formatStringMessage, params object[] args)
		{
			Console.WriteLine("Thread: {0} {1} | {2}", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId, string.Format(formatStringMessage, args));
		}
	}
}
