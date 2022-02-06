using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FluentFTP.Tests.Integration
{
	/// <summary>
	/// Test what is needed for cleanup operations in other test classes.
	/// </summary>
	public class BasicTests : IDisposable
	{
		private readonly FtpClient _ftpClient;

		public BasicTests()
		{
			_ftpClient = FtpUtil.GetFtpClient();
		}

		public void Dispose()
		{
			if (_ftpClient != null)
				_ftpClient.Dispose();
			GC.SuppressFinalize(this);
		}

		[Fact]
		public void Connect()
		{
			_ftpClient.Connect();
			Assert.True(_ftpClient.IsConnected, "IsConnected");
			Assert.True(_ftpClient.IsAuthenticated, "IsAuthenticated");
		}

		[Fact]
		public async Task ConnectAsync()
		{
			await _ftpClient.ConnectAsync();
			Assert.True(_ftpClient.IsConnected, "IsConnected");
			Assert.True(_ftpClient.IsAuthenticated, "IsAuthenticated");
		}

		[Fact]
		public void Disconnect()
		{
			_ftpClient.Connect();
			_ftpClient.Disconnect();
			Assert.False(_ftpClient.IsConnected, "IsConnected");
		}

		[Fact]
		public async Task DisconnectAsync()
		{
			await _ftpClient.ConnectAsync();
			await _ftpClient.DisconnectAsync();
			Assert.False(_ftpClient.IsConnected, "IsConnected");
		}

		[Fact]
		public void UploadListAndDeleteFile()
		{
			_ftpClient.Connect();
			using var file = FileUtil.GetSimpleTextFile();
			var fileName = "BasicTests_UploadFile.txt";
			var uploadStatus = _ftpClient.Upload(file, fileName);
			Assert.Equal(FtpStatus.Success, uploadStatus);

			var list = _ftpClient.GetListing();
			Assert.Single(list, x => x.Name == fileName);

			throw new InvalidOperationException(list[0].FullName);

			_ftpClient.DeleteFile(list[0].FullName);
			var list2 = _ftpClient.GetListing();
			Assert.Empty(list2);
		}
	}
}
