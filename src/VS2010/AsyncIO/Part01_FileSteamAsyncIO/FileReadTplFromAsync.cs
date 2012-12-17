using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace Part01_FileSteamAsyncIO
{
	public static class FileReadTplFromAsync
	{		
		public static void Run()
		{
			Log.Write("File read Task Parallel Library : FromAsync");
			int testFileSize = 5000;
			int bufferSize = 4096;
			var waitHandle = new AutoResetEvent(false);

			var fileName = TempFileCreator.CreateRandomFile(testFileSize);

			FileStream asyncStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Inheritable, bufferSize);
			
			var buffer = new byte[bufferSize];
			var task = Task<Int32>.Factory.FromAsync(asyncStream.BeginRead, asyncStream.EndRead, buffer, 0, bufferSize,
				new FileReadTplState(buffer, asyncStream, waitHandle));

			Action<Task<Int32>> cont = null;
				
			cont = (t) =>
			{
				var state = (FileReadTplState)t.AsyncState;
				Log.Write("Continuation bytes read: {0}", t.Result);

				if (t.Result > 0)
				{
					state.FileContents.Write(state.Buffer, 0, t.Result);
					task = Task<Int32>.Factory.FromAsync(state.FileStream.BeginRead, state.FileStream.EndRead, state.Buffer, 0, bufferSize, state);

					task.ContinueWith(cont);
				}
				else
				{
					state.FileStream.Dispose();
					File.Delete(state.FileStream.Name);
					state.CompletedWaitHandle.Set();
				}
			};

			task.ContinueWith(cont);

			Log.Write("Task started");

			waitHandle.WaitOne();
		}

		class FileReadTplState
		{
			public FileReadTplState(byte[] buffer, FileStream sourceStream, AutoResetEvent waitHandle)
			{
				Buffer = buffer;
				FileStream = sourceStream;
				FileContents = new MemoryStream();
				CompletedWaitHandle = waitHandle;
			}

			public byte[] Buffer { get; private set; }
			public FileStream FileStream { get; private set; }
			public MemoryStream FileContents { get; internal set; }
			public AutoResetEvent CompletedWaitHandle { get; internal set; }
		}
	}
}
