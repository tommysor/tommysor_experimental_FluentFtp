using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluentFTP.Tests.Integration
{
	internal class FileUtil
	{
		internal static Stream GetSimpleTextFile()
		{
			var file = new MemoryStream();
			var fileWriter = new StreamWriter(file, Encoding.UTF8);
			fileWriter.WriteLine("abc");
			file.Position = 0;
			return file;
		}
	}
}
