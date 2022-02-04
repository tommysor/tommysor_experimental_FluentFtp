﻿Imports System
Imports System.Net
Imports System.Threading
Imports System.Threading.Tasks
Imports FluentFTP

Namespace Examples
	Friend Module DeleteFileExample
		Sub DeleteFile()
			Using conn = New FtpClient("127.0.0.1", "ftptest", "ftptest")
				conn.Connect()
				conn.DeleteFile("/full/or/relative/path/to/file")
			End Using
		End Sub

		Async Function DeleteDirectoryAsync() As Task
			Dim token = New CancellationToken()

			Using conn = New FtpClient("127.0.0.1", "ftptest", "ftptest")
				Await conn.ConnectAsync(token)
				Await conn.DeleteFileAsync("/full/or/relative/path/to/file")
			End Using
		End Function
	End Module
End Namespace
