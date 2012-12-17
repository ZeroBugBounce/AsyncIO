using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
	public class TempFileCreator
	{
		public static string CreateRandomFile(int size)
		{			
			var fileName = Path.GetTempFileName();
			var rng = new Random();

			byte[] buffer = new byte[size];

			rng.NextBytes(buffer);

			using(FileStream fs = new FileStream(fileName, FileMode.Create))
			{
				fs.Write(buffer, 0, buffer.Length);
			}

			return fileName;
		}

		public static string[] CreateRandomFiles(int size)
		{
			string[] fileNames = new string[size];
			for(int index = 0; index < size; index++)
			{
				fileNames[index] = CreateRandomFile(size);
			}

			return fileNames;
		}
	}
}
