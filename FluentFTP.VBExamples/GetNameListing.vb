﻿Imports System
Imports System.Net
Imports System.Threading
Imports System.Threading.Tasks
Imports FluentFTP

Namespace Examples
	Friend Module GetNameListingExample
		Sub GetNameListing()
			Using conn = New FtpClient("127.0.0.1", "ftptest", "ftptest")
				conn.Connect()

				For Each s In conn.GetNameListing()
					Dim isDirectory = conn.DirectoryExists(s)
					Dim modify = conn.GetModifiedTime(s)
					Dim size = If(isDirectory, 0, conn.GetFileSize(s))
				Next
			End Using
		End Sub

		Async Function GetNameListingAsync() As Task
			Dim token = New CancellationToken()

			Using conn = New FtpClient("127.0.0.1", "ftptest", "ftptest")
				Await conn.ConnectAsync(token)

				For Each s In Await conn.GetNameListingAsync(token)
					Dim isDirectory = Await conn.DirectoryExistsAsync(s, token)
					Dim modify = Await conn.GetModifiedTimeAsync(s, token)
					Dim size = If(isDirectory, 0, Await conn.GetFileSizeAsync(s, -1, token))
				Next
			End Using
		End Function
	End Module
End Namespace
