
--LookupTypes
SET IDENTITY_INSERT LookupTypes ON
INSERT INTO LookupTypes(Id, Name) VALUES(1, 'Ages')
SET IDENTITY_INSERT LookupTypes OFF

--LookupTypeItems
SET IDENTITY_INSERT LookupTypeItems ON
INSERT INTO LookupTypeItems(Id, Description, IsActive, LookupTypeId) VALUES(1, 'Adult', 1, 1)
INSERT INTO LookupTypeItems(Id, Description, IsActive, LookupTypeId) VALUES(2, 'Children', 1, 1)
INSERT INTO LookupTypeItems(Id, Description, IsActive, LookupTypeId) VALUES(3, 'Infant', 1, 1)
SET IDENTITY_INSERT LookupTypeItems OFF

--LookupTypeItems
SET IDENTITY_INSERT Settings ON
INSERT INTO Settings(Id, [Key], Value, FriendlyName) VALUES(1, 'TenantGracePeriod', '10', 'Tenant''s Grace Period')
SET IDENTITY_INSERT Settings OFF

INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName])VALUES ('AppDomain', 'beta.clubfit.net.au', 'Application Domain')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AmazonS3BucketName', 'clubfitassets', 'AmazonS3 Bucket Name')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('EnableAmazonS3', 'True', 'Enable Amazon S3')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AmazonS3AccessKey', 'AKIAINLQSISTD3SORLVA', 'AmazonS3 Access Key')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AmazonS3SecretKey', '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', 'AmazonS3 Secret Key')
