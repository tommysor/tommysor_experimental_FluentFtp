using DotNet.Testcontainers.Containers.Modules;
using FluentFTP.Tests.Integration;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FluentFTP.Tests.System
{
    public class IntegrationTests : IDisposable
    {
		private readonly FtpClient _ftpClient;
		private readonly TestcontainersContainer _ftpContainer;

		public IntegrationTests()
		{
			_ftpContainer = FtpUtil.GetFtpContainer();
			_ftpContainer.StartAsync().Wait();
			_ftpClient = FtpUtil.GetFtpClient();
			FtpUtil.CleanFtp(_ftpClient);
		}

		public void Dispose()
		{
			if (_ftpClient != null)
				_ftpClient.Dispose();
			if (_ftpContainer != null)
				_ftpContainer.DisposeAsync().AsTask().Wait();
			GC.SuppressFinalize(this);
		}

		[Theory]
		[InlineData(FtpDataType.Binary)]
		[InlineData(FtpDataType.ASCII)]
		public void UploadFile(FtpDataType ftpDataType)
		{
			_ftpClient.Connect();
			using var file = FileUtil.GetSimpleTextFile();
			var filePath = "test.txt";
			_ftpClient.UploadDataType = ftpDataType;

			var uploadStatus = _ftpClient.Upload(file, filePath);
			Assert.Equal(FtpStatus.Success, uploadStatus);
		}

		[Theory]
		[InlineData(FtpDataType.Binary)]
		[InlineData(FtpDataType.ASCII)]
		public async Task UploadFileAsync(FtpDataType ftpDataType)
		{
			await _ftpClient.ConnectAsync();
			using var file = FileUtil.GetSimpleTextFile();
			var filePath = "test.txt";
			_ftpClient.UploadDataType = ftpDataType;

			var uploadStatus = await _ftpClient.UploadAsync(file, filePath);
			Assert.Equal(FtpStatus.Success, uploadStatus);
		}

		[Theory]
		[InlineData(FtpDataType.Binary)]
		[InlineData(FtpDataType.ASCII)]
		public void DownloadFile(FtpDataType ftpDataType)
		{
			_ftpClient.Connect();
			using var originalFile = FileUtil.GetSimpleTextFile();
			var filePath = "test.txt";
			_ftpClient.DownloadDataType = ftpDataType;
			// Not setting any explicit UploadDataType.
			_ftpClient.Upload(originalFile, filePath);

			using var downloadedFile = new MemoryStream();
			var downloadResult = _ftpClient.Download(downloadedFile, filePath);
			Assert.True(downloadResult, "downloadResult");

			using var originalFileReader = new StreamReader(originalFile);
			using var downloadedFileReader = new StreamReader(downloadedFile);
			var originalText = originalFileReader.ReadToEnd();
			var downloadedText = downloadedFileReader.ReadToEnd();
			Assert.Equal(originalText, downloadedText);
		}

		[Theory]
		[InlineData(FtpDataType.Binary)]
		[InlineData(FtpDataType.ASCII)]
		public async Task DownloadFileAsync(FtpDataType ftpDataType)
		{
			await _ftpClient.ConnectAsync();
			using var originalFile = FileUtil.GetSimpleTextFile();
			var filePath = "test.txt";
			_ftpClient.DownloadDataType = ftpDataType;
			// Not setting any explicit UploadDataType.
			await _ftpClient.UploadAsync(originalFile, filePath);

			using var downloadedFile = new MemoryStream();
			var downloadResult = await _ftpClient.DownloadAsync(downloadedFile, filePath);
			Assert.True(downloadResult, "downloadResult");

			using var originalFileReader = new StreamReader(originalFile);
			using var downloadedFileReader = new StreamReader(downloadedFile);
			var originalText = originalFileReader.ReadToEnd();
			var downloadedText = downloadedFileReader.ReadToEnd();
			Assert.Equal(originalText, downloadedText);
		}
	}
}
