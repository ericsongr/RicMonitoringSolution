
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RenterCommunicationHistory')
BEGIN
	CREATE TABLE [dbo].[RenterCommunicationHistory](
		[Id] [bigint] IDENTITY(1,1) NOT NULL,
		[RenterId] [int] NOT NULL,
		[CommunicationUtcDateTime] [datetime] NULL,
		[CommunicationType] [int] NULL,
		[CommunicationText] [nvarchar](max) NULL,
		[CommunicationSentTo] [nvarchar](100) NULL,
		[RequestedBy] [nvarchar](25) NULL,
		[IsSuccessfullySent] [bit] NULL,
		[BatchID] [nvarchar](100) NULL,
		[MessageId] [nvarchar](500) NULL,
		[HasRead] [bit] NOT NULL,
		[AttachmentFileName] [nvarchar](500) NULL,
		[ContentType] [nvarchar](50) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = 'FK_RenterCommunicationHistory_Accounts_RenterId')
	BEGIN

	ALTER TABLE [dbo].[RenterCommunicationHistory]  WITH CHECK ADD  CONSTRAINT [FK_RenterCommunicationHistory_Accounts_RenterId] FOREIGN KEY(RenterId)
	REFERENCES [dbo].[Renters] ([Id])
	ON DELETE CASCADE
	
	END
GO
