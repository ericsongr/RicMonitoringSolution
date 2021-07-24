IF NOT EXISTS(SELECT 1 FROM Settings WHERE [Key] = 'OneSignalAuthKey')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ( N'OneSignalAuthKey', N'YWEwNDNlOGQtZjVhMS00NGNhLWI5ZmEtOTI4ZDcxNjA5MDhj', N'One Signal Auth Key')
END
GO
IF NOT EXISTS(SELECT 1 FROM Settings WHERE [Key] = 'OneSignalAppId')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ( N'OneSignalAppId', N'da240190-93b6-4c23-813e-637e472bca16', N'One Signal Application Id')
END
GO