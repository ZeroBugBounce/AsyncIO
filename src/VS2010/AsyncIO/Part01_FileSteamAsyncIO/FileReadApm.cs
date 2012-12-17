using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;

namespace Part01_FileSteamAsyncIO
{
	static class FileReadApm
	{
		public static void Run()
		{
			Log.Write("File read APM");
			int testFileSize = 5000;
			int bufferSize = 1024;

			var fileName = TempFileCreator.CreateRandomFile(testFileSize);

			FileStream asyncStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Inheritable, bufferSize, FileOptions.Asynchronous);
			var buffer = new byte[bufferSize];
			asyncStream.BeginRead(buffer, 0, bufferSize, EndRead, new FileReadApmState(buffer, asyncStream));
			Log.Write("BeginRead called");
		}

		static void EndRead(IAsyncResult result)
		{
			FileReadApmState state = result.AsyncState as FileReadApmState;
			int bytesRead = state.FileStream.EndRead(result);

			Log.Write("EndRead bytes read: {0}", bytesRead);
			if (bytesRead > 0)
			{
				state.FileContents.Write(state.Buffer, 0, bytesRead);
				state.FileStream.BeginRead(state.Buffer, 0, state.Buffer.Length, EndRead, state);
			}
			else
			{
				state.FileStream.Dispose();
			}
		}

		class FileReadApmState
		{
			public FileReadApmState(byte[] buffer, FileStream sourceStream)
			{
				Buffer = buffer;
				FileStream = sourceStream;
				FileContents = new MemoryStream();
			}

			public byte[] Buffer { get; private set; }
			public FileStream FileStream { get; private set; }
			public MemoryStream FileContents { get; internal set; }
		}
	}
}
