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

		public IntegrationTests()
		{
			_ftpClient = new FtpClient("localhost", 21, "testUser", "testPass");
			//var cleanScript = Environment.GetEnvironmentVariable("cleanScript");
			//var escapedArgs = cleanScript.Replace("\"", "\\\"");
			//var proc = new Process
			//{
			//	StartInfo = new ProcessStartInfo
			//	{
			//		FileName = "bash",
			//		Arguments = $"-c \"{escapedArgs}\"",
			//		RedirectStandardOutput = true,
			//		RedirectStandardError = true,
			//		UseShellExecute = false,
			//		CreateNoWindow = true
			//	},
			//	EnableRaisingEvents = true
			//};
			//proc.Start();
			//var output = proc.StandardError.ReadToEnd();
			//throw new InvalidOperationException(output);

			_ftpClient.Connect();
			var list = _ftpClient.GetListing();
			foreach (var item in list)
			{
				_ftpClient.DeleteFile(item.FullName);
			}
			
			_ftpClient.Disconnect();
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

		//[Fact]
		//public void DummyDirectory()
		//{
		//	_ftpClient.Connect();
		//	var directory = _ftpClient.GetWorkingDirectory();
		//	Assert.False(true, directory);
		//}

		//[Fact]
		//public void DummyListing()
		//{
		//	_ftpClient.Connect();

		//	using var file = FileUtil.GetSimpleTextFile();
		//	//var filePath = "test.txt";
		//	var dirPath = GetDirPath();
		//	var filePath = Path.Combine(dirPath, "test.txt");
			
		//	_ftpClient.CreateDirectory(dirPath);

		//	//var uploadStatus = _ftpClient.Upload(file, filePath);
		//	//Assert.Equal(FtpStatus.Success, uploadStatus);

		//	var listing = _ftpClient.GetListing(null, FtpListOption.Recursive);
		//	Assert.True(listing.Length > 0, "No entries in listing");
		//	Assert.True(false, listing[0].FullName);
		//}

		//[Theory]
		//[InlineData(FtpDataType.Binary)]
		//[InlineData(FtpDataType.ASCII)]
		//public void UploadFile(FtpDataType ftpDataType)
		//{
		//	_ftpClient.Connect();
		//	using var file = FileUtil.GetSimpleTextFile();
		//	var filePath = GetPath("test.txt");
		//	var dirPath = new FileInfo(filePath).Directory.FullName;
		//	_ftpClient.CreateDirectory(dirPath);
		//	_ftpClient.UploadDataType = ftpDataType;

		//	var uploadStatus = _ftpClient.Upload(file, filePath);
		//	Assert.Equal(FtpStatus.Success, uploadStatus);

		//	//var hash = _ftpClient.GetChecksum(filePath);
		//	//Assert.True(hash.IsValid, "hash.IsValid");
		//	//var isVerified = hash.Verify(file);
		//	//Assert.True(isVerified, "hash.Verify");
		//}

		[Theory]
		[InlineData(FtpDataType.Binary)]
		[InlineData(FtpDataType.ASCII)]
		public async Task UploadFileAsync(FtpDataType ftpDataType)
		{
			await _ftpClient.ConnectAsync();
			using var file = FileUtil.GetSimpleTextFile();
			//var filePath = GetPath("test.txt");
			var filePath = ftpDataType + "_test.txt";
			_ftpClient.UploadDataType = ftpDataType;

			var uploadStatus = await _ftpClient.UploadAsync(file, filePath);
			Assert.Equal(FtpStatus.Success, uploadStatus);

			//var hash = await _ftpClient.GetChecksumAsync(filePath);
			//Assert.True(hash.IsValid, "hash.IsValid");
			//var isVerified = hash.Verify(file);
			//Assert.True(isVerified, "hash.Verify");
		}

		//[Theory]
		//[InlineData(FtpDataType.Binary)]
		//[InlineData(FtpDataType.ASCII)]
		//public void DownloadFile(FtpDataType ftpDataType)
		//{
		//	_ftpClient.Connect();
		//	using var originalFile = FileUtil.GetSimpleTextFile();
		//	var filePath = GetPath("test.txt");
		//	_ftpClient.DownloadDataType = ftpDataType;
		//	// Not setting any explicit UploadDataType.
		//	_ftpClient.Upload(originalFile, filePath);

		//	using var downloadedFile = new MemoryStream();
		//	var downloadResult = _ftpClient.Download(downloadedFile, filePath);
		//	Assert.True(downloadResult, "downloadResult");

		//	using var originalFileReader = new StreamReader(originalFile);
		//	using var downloadedFileReader = new StreamReader(downloadedFile);
		//	var originalText = originalFileReader.ReadToEnd();
		//	var downloadedText = downloadedFileReader.ReadToEnd();
		//	Assert.Equal(originalText, downloadedText);
		//}

		//[Theory]
		//[InlineData(FtpDataType.Binary)]
		//[InlineData(FtpDataType.ASCII)]
		//public async Task DownloadFileAsync(FtpDataType ftpDataType)
		//{
		//	await _ftpClient.ConnectAsync();
		//	using var originalFile = FileUtil.GetSimpleTextFile();
		//	var filePath = GetPath("test.txt");
		//	_ftpClient.DownloadDataType = ftpDataType;
		//	// Not setting any explicit UploadDataType.
		//	await _ftpClient.UploadAsync(originalFile, filePath);

		//	using var downloadedFile = new MemoryStream();
		//	var downloadResult = await _ftpClient.DownloadAsync(downloadedFile, filePath);
		//	Assert.True(downloadResult, "downloadResult");

		//	using var originalFileReader = new StreamReader(originalFile);
		//	using var downloadedFileReader = new StreamReader(downloadedFile);
		//	var originalText = originalFileReader.ReadToEnd();
		//	var downloadedText = downloadedFileReader.ReadToEnd();
		//	Assert.Equal(originalText, downloadedText);
		//}
	}
}
