IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'AppDomain')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName])VALUES ('AppDomain', 'beta.clubfit.net.au', 'Application Domain')
END
GO
IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'AmazonS3BucketName')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AmazonS3BucketName', 'clubfitassets', 'AmazonS3 Bucket Name')
END
GO
IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'EnableAmazonS3')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('EnableAmazonS3', 'True', 'Enable Amazon S3')
END
GO
IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'AmazonS3AccessKey')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AmazonS3AccessKey', 'AKIAINLQSISTD3SORLVA', 'AmazonS3 Access Key')
END
GO
IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'AmazonS3SecretKey')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AmazonS3SecretKey', 'fwMn6nrXYCNfd5vETlyIwW4Q8/UFvScE4TNsq5qb', 'AmazonS3 Secret Key')
END
GO
