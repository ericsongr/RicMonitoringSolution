
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AccountBillingItems')
BEGIN
	CREATE TABLE [dbo].[AccountBillingItems](
		[AccountBillingItemId] [bigint] IDENTITY(1,1) NOT NULL,
		[AccountId] [int] NOT NULL,
		[BillingReason] [int] NOT NULL,
		[BillingAmount] [money] NOT NULL,
		[BillingReference] [varchar](100) NULL,
		[CreatedUtcDateTime] [datetime] NOT NULL,
		[ProcessedUtcDateTime] [datetime] NULL,
		[MessageId] [varchar](50) NULL,
		[PaymentType] [int] NULL,
	 CONSTRAINT [PK_AccountBillingItems] PRIMARY KEY CLUSTERED 
	(
		[AccountBillingItemId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_AccountBillingItems_Accounts')
BEGIN
	ALTER TABLE [dbo].[AccountBillingItems]  WITH CHECK ADD  CONSTRAINT [FK_AccountBillingItems_Accounts] FOREIGN KEY([AccountId])
	REFERENCES [dbo].[Accounts] ([Id])

	ALTER TABLE [dbo].[AccountBillingItems] CHECK CONSTRAINT [FK_AccountBillingItems_Accounts]
END
GO

IF NOT EXISTS(SELECT 1 FROM Settings WHERE [Key] = 'SMSFee')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ( N'SMSFee', N'0.09', N'SMS fee')
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME= 'Renters' AND COLUMN_NAME = 'Mobile')
BEGIN
	ALTER TABLE Renters ADD Mobile VARCHAR(50) NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME= 'Renters' AND COLUMN_NAME = 'MobileRenterBeforeDueDateEnable')
BEGIN
	ALTER TABLE Renters ADD MobileRenterBeforeDueDateEnable BIT NOT NULL CONSTRAINT DF_Renters_MobileRenterBeforeDueDateEnable DEFAULT(0) 
END
GO