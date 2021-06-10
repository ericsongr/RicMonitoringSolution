
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Accounts')
BEGIN
	
	SET ANSI_NULLS ON
	
	SET QUOTED_IDENTIFIER ON
	
	CREATE TABLE [dbo].[Accounts](
		[Id] [int] IDENTITY(1,1) NOT NULL,
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
	 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO


