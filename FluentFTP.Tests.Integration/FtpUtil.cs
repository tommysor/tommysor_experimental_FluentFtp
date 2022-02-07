using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules;
using DotNet.Testcontainers.Containers.WaitStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFTP.Tests.Integration
{
	internal class FtpUtil
	{
		internal static FtpClient GetFtpClient(int port)
		{
			var ftpClient = new FtpClient("localhost", port, "testUser", "testPass");
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

		private static Random random;
		internal static int GetRandomPort()
		{
			if (random is null)
				random = new Random();

			return random.Next(8000, 9000);
		}

		internal static TestcontainersContainer GetFtpContainer(int port)
		{
			var builder = new TestcontainersBuilder<TestcontainersContainer>()
				.WithImage("fauria/vsftpd")
				.WithName("vsftpd")
				.WithPortBinding(20, true)
				.WithPortBinding(port, 21)
				.WithEnvironment("FTP_USER", "testUser")
				.WithEnvironment("FTP_PASS", "testPass")
				.WithEnvironment("PASV_ADDRESS", "127.0.0.1")
				.WithEnvironment("PASV_MIN_PORT", "21100")
				.WithEnvironment("PASV_MAX_PORT", "21110")
				.WithMount("/home/runner/work/_temp/ftpDir", "/home/vsftpd")
				;
			for (var i = 21100; i <= 21110; i++)
			{
				builder.WithPortBinding(i, true);
			}
			
			
			builder.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(port));
			

			var container = builder.Build();
			
			return container;
		}
	}
}
