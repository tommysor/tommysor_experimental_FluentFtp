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
		private static readonly Random rnd = new Random();
		internal static Stream GetSimpleTextFile()
		{
			var file = new MemoryStream();
			var fileWriter = new StreamWriter(file, Encoding.UTF8);

			var stringBuilder = new StringBuilder();
			for (var i = 0; i < 10; i++)
			{
				var c = rnd.Next('a', 'z' + 1);
				stringBuilder.Append(c);
			}

			fileWriter.WriteLine(stringBuilder.ToString());
			file.Position = 0;
			return file;
		}
	}
}
