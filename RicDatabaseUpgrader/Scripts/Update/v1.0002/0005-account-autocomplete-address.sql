
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AuditAccounts')
BEGIN
	CREATE TABLE [dbo].[AuditAccounts](
		[AuditAccountId] [int] IDENTITY(1,1) NOT NULL,
		[Id] [int] NOT NULL,
		[Name] [nvarchar](50) NULL,
		[Timezone] [nvarchar](50) NULL,
		[IsActive] [bit] NOT NULL DEFAULT(1),
		[Street] [nvarchar](500) NULL,
		[SubUrb] [nvarchar](500) NULL,
		[State] [nvarchar](500) NULL,
		[PostalCode] [nvarchar](500) NULL,
		[Email] [nvarchar](500) NULL,
		[PhoneNumber] [nvarchar](100) NULL,
		[WebsiteUrl] [varchar](500) NULL,
		[FacebookUrl] [varchar](500) NULL,
		[AddressLine1] [varchar](200) NULL,
		[City] [varchar](200) NULL,
		[DialingCode] [nvarchar](5) NULL,
		[BusinessOwnerName] [varchar](100) NULL,
		[BusinessOwnerPhoneNumber] [varchar](100) NULL,
		[BusinessOwnerEmail] [varchar](100) NULL,
		[GeoCoordinates] [varchar](75) NULL,
		[CompanyFeeFailedPaymentCount] [int] NULL,
		[PaymentIssueSuspensionDate] [datetime] NULL,
		[AuditDateTime] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
		[Username] [nvarchar](50) NOT NULL,
		[AuditAction] [nvarchar](20) NOT NULL,
	 CONSTRAINT [PK_AuditAccounts] PRIMARY KEY CLUSTERED 
	(
		[AuditAccountId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[AuditAccounts]  WITH CHECK ADD  CONSTRAINT [FK_AuditAccounts_Accounts_Id] FOREIGN KEY(Id)
	REFERENCES [dbo].[Accounts] ([Id])
	ON DELETE CASCADE

END
GO


