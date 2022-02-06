using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFTP.Tests.Integration
{
	internal class FtpUtil
	{
		internal static FtpClient GetFtpClient()
		{
			var ftpClient = new FtpClient("localhost", 21, "testUser", "testPass");
			return ftpClient;
		}

		internal static void CleanFtp(FtpClient ftpClient)
		{
			ftpClient.Connect();
			var list = ftpClient.GetListing();
			foreach (var item in list)
			{
				ftpClient.DeleteFile(item.FullName);
			}

			ftpClient.Disconnect();
		}
	}
}
