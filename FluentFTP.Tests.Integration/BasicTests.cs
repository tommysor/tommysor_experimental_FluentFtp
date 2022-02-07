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
		public async Task UploadListAndDeleteFile()
		{
			await _ftpClient.ConnectAsync();
			using var file = FileUtil.GetSimpleTextFile();
			var fileName = "BasicTests_UploadFile.txt";
			var uploadStatus = await _ftpClient.UploadAsync(file, fileName);
			Assert.Equal(FtpStatus.Success, uploadStatus);

			await Task.Delay(TimeSpan.FromSeconds(1));

			var list = await _ftpClient.GetListingAsync();
			
			// todo: Why do I get either none, or "test.txt" (leftover from IntegrationTests). Not {fileName}?
			//var listLength = list.Length;
			//var firstOrDefault = list.FirstOrDefault();		
			//Assert.True(false, $"listLength: '{listLength}'. Name: '{firstOrDefault?.Name}'. FullName: '{firstOrDefault?.FullName}'");
			
			var listItem = list.FirstOrDefault(x => x.Name == fileName);
			//Assert.Single(list, x => x.Name == fileName);
			Assert.NotNull(listItem);

			await Task.Delay(TimeSpan.FromSeconds(1));

			await _ftpClient.DeleteFileAsync(listItem.FullName);

			await Task.Delay(TimeSpan.FromSeconds(1));

			var list2 = await _ftpClient.GetListingAsync();
			//Assert.Empty(list2);
			Assert.DoesNotContain(list2, x => x.Name == fileName);
		}
	}
}
