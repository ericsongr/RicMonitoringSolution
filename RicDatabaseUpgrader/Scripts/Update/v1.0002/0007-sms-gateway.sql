IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SmsGateway')
BEGIN
	CREATE TABLE [dbo].[SmsGateway](
		[SmsGatewayId] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
		[UserName] [nvarchar](500) NOT NULL,
		[Password] [nvarchar](500) NOT NULL,
		[IsActive] [bit] NOT NULL,
		[GatewayUrl] [nvarchar](1000) NULL,
		[DedicatedNumber] [varchar](20) NULL,
	 CONSTRAINT [PK_SmsGateway] PRIMARY KEY CLUSTERED 
	(
		[SmsGatewayId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM SmsGateway WHERE [Name] = 'Sms Global HTTP')
BEGIN
	INSERT INTO [dbo].[SmsGateway]([Name],[UserName],[Password],[IsActive],[GatewayUrl], [DedicatedNumber])
		VALUES('Sms Global HTTP','96ez1jde','dL3hk7Vc',1,'https://api.smsglobal.com/http-api.php?', null)
END
GO

IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'UseSystemDedicatedNumber')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('UseSystemDedicatedNumber', 'false', 'Use dedicated number')
END
GO

IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'SMSGatewaySenderId')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('SMSGatewaySenderId', '639297233031', 'SMS from number')
END
GO

IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'AppSMSRenterBeforeDueDateEnable')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AppSMSRenterBeforeDueDateEnable', 'True', 'Enable SMS Renter before due date')
END
GO

IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'AppSMSRenterNoOfDaysBeforeDueDate')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AppSMSRenterNoOfDaysBeforeDueDate', '2', 'SMS Renter # of days before due date')
END
GO

IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'AppSMSMessageRenterBeforeDueDate')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AppSMSMessageRenterBeforeDueDate', 'Hi {{Name}}, Friendly reminder of your rent due on {{DueDate}} for the period of {{Period}}.', 'SMS Body')
END
GO