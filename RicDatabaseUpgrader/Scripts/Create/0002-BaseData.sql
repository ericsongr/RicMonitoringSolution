
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

INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AppEmailRenterBeforeDueDateEnable', 'True', 'Enable Email Renter before due date')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AppEmailRenterNoOfDaysBeforeDueDate', '2', 'Renter # of days before due date')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AppEmailMessageRenterBeforeDueDate', 'Hi {FirstName}', 'Email Body')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('UseSystemDedicatedNumber', 'false', 'Use dedicated number')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('SMSGatewaySenderId', '639297233031', 'SMS from number')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AppSMSRenterBeforeDueDateEnable', 'True', 'Enable SMS Renter before due date')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AppSMSRenterNoOfDaysBeforeDueDate', '2', 'SMS Renter # of days before due date')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AppSMSMessageRenterBeforeDueDate', 'Hi {{Name}}, Friendly reminder of your rent due on {{DueDate}} for the period of {{Period}}.', 'SMS Body')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ( N'SMSFee', N'0.09', N'SMS fee')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ( N'OneSignalAuthKey', N'YWEwNDNlOGQtZjVhMS00NGNhLWI5ZmEtOTI4ZDcxNjA5MDhj', N'One Signal Auth Key')
INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ( N'OneSignalAppId', N'da240190-93b6-4c23-813e-637e472bca16', N'One Signal Application Id')

INSERT INTO [dbo].[SmsGateway]([Name],[UserName],[Password],[IsActive],[GatewayUrl]) VALUES('Sms Global HTTP','96ez1jde','dL3hk7Vc',1,'https://api.smsglobal.com/http-api.php?')

