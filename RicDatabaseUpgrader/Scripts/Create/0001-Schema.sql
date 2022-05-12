SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
		[IsSelected] BIT NOT NULL DEFAULT(0),
	 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
		[IsSelected] BIT NOT NULL DEFAULT(0),
	 CONSTRAINT [PK_AuditAccounts] PRIMARY KEY CLUSTERED 
	(
		[AuditAccountId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[AuditAccounts]  WITH CHECK ADD  CONSTRAINT [FK_AuditAccounts_Accounts_Id] FOREIGN KEY(Id)
	REFERENCES [dbo].[Accounts] ([Id])
	ON DELETE CASCADE
GO

/****** Object:  Table [dbo].[DbErrorLogs]    Script Date: 17/05/2020 11:19:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DbErrorLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProcessMessage] [nvarchar](max) NULL,
	[Line] [int] NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Procedure] [nvarchar](max) NULL,
	[Number] [int] NOT NULL,
	[Severity] [int] NOT NULL,
	[State] [int] NOT NULL,
	[DateTimeCreated] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
 CONSTRAINT [PK_DbErrorLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LookupTypeItems]    Script Date: 17/05/2020 11:19:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupTypeItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[LookupTypeId] [int] NOT NULL,
 CONSTRAINT [PK_LookupTypeItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LookupTypes]    Script Date: 17/05/2020 11:19:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LookupTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_LookupTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MonthlyRentBatch]    Script Date: 17/05/2020 11:19:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonthlyRentBatch](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProcessStartDateTime] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
	[ProcesssEndDateTime] [datetime2](7) NULL,
 CONSTRAINT [PK_MonthlyRentBatch] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RentArrears]    Script Date: 17/05/2020 11:19:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentArrears](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RenterId] [int] NOT NULL,
	[RentTransactionId] [int] NULL,
	[UnpaidAmount] [decimal](18, 2) NOT NULL,
	[IsProcessed] [bit] NOT NULL,
	[Note] NVARCHAR(2000),
	[IsManualEntry] BIT NOT NULL DEFAULT(0),
	[ManualEntryDateTimeLocal] DATETIME NULL,
	[ProcessedDateTimeUtc] DATETIME NULL,
 CONSTRAINT [PK_RentArrears] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Renters]    Script Date: 17/05/2020 11:19:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Renters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Email] NVARCHAR(50) NULL,
	[AdvanceMonths] [int] NOT NULL,
	[AdvancePaidDate] [datetime2](7) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[NoOfPersons] [int] NOT NULL,
	[RoomId] [int] NOT NULL,
	[DateEndRent] [datetime2](7) NULL,
	[IsEndRent] [bit] NOT NULL DEFAULT ((0)),
	[MonthsUsed] [int] NOT NULL DEFAULT ((0)),
	[BalanceAmount] [decimal](18, 2) NULL,
	[BalancePaidDate] [datetime2](7) NULL,
	[TotalPaidAmount] [decimal](18, 2) NOT NULL DEFAULT ((0.0)),
	[DueDay] [int] NOT NULL DEFAULT ((0)),
	[NextDueDate] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
	[PreviousDueDate] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
	[EmailRenterBeforeDueDateEnable] BIT NOT NULL DEFAULT(0),
	[AccountId] INT NOT NULL, 
 CONSTRAINT [PK_Renters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Renters]  WITH CHECK ADD  CONSTRAINT [FK_Renters_Accounts_Id] FOREIGN KEY(AccountId)
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE

GO
/****** Object:  Table [dbo].[RentTransactionDetails]    Script Date: 17/05/2020 11:19:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentTransactionDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[RentArrearId] [int] NULL,
 CONSTRAINT [PK_RentTransactionDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RentTransactions]    Script Date: 17/05/2020 11:19:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentTransactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaidDate] [datetime2](7) NULL,
	[PaidAmount] [decimal](18, 2) NULL,
	[Balance] [decimal](18, 2) NULL,
	[BalanceDateToBePaid] [datetime2](7) NULL,
	[IsDepositUsed] [bit] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[RoomId] [int] NOT NULL,
	[RenterId] [int] NOT NULL,
	[DueDate] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
	[Period] [nvarchar](max) NULL,
	[TransactionType] [int] NOT NULL DEFAULT ((0)),
	[IsSystemProcessed] [bit] NOT NULL DEFAULT ((0)),
	[SystemDateTimeProcessed] [datetime2](7) NULL,
	[TotalAmountDue] [decimal](18, 2) NOT NULL DEFAULT ((0.0)),
	[IsProcessed] [bit] NOT NULL DEFAULT ((0)),
	[ExcessPaidAmount] [decimal](18,2) NOT NULL DEFAULT(0),
 CONSTRAINT [PK_RentTransactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Rooms]    Script Date: 17/05/2020 11:19:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rooms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Frequency] [nvarchar](max) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[AccountId] INT NOT NULL,
 CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Rooms]  WITH CHECK ADD  CONSTRAINT [FK_Rooms_Accounts_Id] FOREIGN KEY(AccountId)
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE

GO
/****** Object:  Table [dbo].[Settings]    Script Date: 17/05/2020 11:19:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](max) NULL,
	[Value] [nvarchar](max) NULL,
	[FriendlyName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditRooms](
	[AuditRoomId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Frequency] [nvarchar](max) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[AuditDateTime] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
	[Username] [nvarchar](50) NOT NULL,
	[AuditAction] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_AuditRooms] PRIMARY KEY CLUSTERED 
(
	[AuditRoomId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AuditRooms]  WITH CHECK ADD  CONSTRAINT [FK_AuditRooms_Rooms_RoomId] FOREIGN KEY([Id])
REFERENCES [dbo].[Rooms] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LookupTypeItems]  WITH CHECK ADD  CONSTRAINT [ForeignKey_LookupTypeItems_LookupTypes] FOREIGN KEY([LookupTypeId])
REFERENCES [dbo].[LookupTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LookupTypeItems] CHECK CONSTRAINT [ForeignKey_LookupTypeItems_LookupTypes]
GO
ALTER TABLE [dbo].[RentArrears]  WITH CHECK ADD  CONSTRAINT [ForeignKey_RentArrears_Renter_RenterId] FOREIGN KEY([RenterId])
REFERENCES [dbo].[Renters] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RentArrears] CHECK CONSTRAINT [ForeignKey_RentArrears_Renter_RenterId]
GO
ALTER TABLE [dbo].[RentArrears]  WITH CHECK ADD  CONSTRAINT [ForeignKey_RentArrears_RentTransaction_RentTransactionId] FOREIGN KEY([RentTransactionId])
REFERENCES [dbo].[RentTransactions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RentArrears] CHECK CONSTRAINT [ForeignKey_RentArrears_RentTransaction_RentTransactionId]
GO
ALTER TABLE [dbo].[Renters]  WITH CHECK ADD  CONSTRAINT [ForeignKey_Renter_Room] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Rooms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Renters] CHECK CONSTRAINT [ForeignKey_Renter_Room]
GO
ALTER TABLE [dbo].[RentTransactionDetails]  WITH CHECK ADD  CONSTRAINT [ForeignKey_RentTransactionDetails_RentArrear_RentArrearId] FOREIGN KEY([RentArrearId])
REFERENCES [dbo].[RentArrears] ([Id])
GO
ALTER TABLE [dbo].[RentTransactionDetails] CHECK CONSTRAINT [ForeignKey_RentTransactionDetails_RentArrear_RentArrearId]
GO
ALTER TABLE [dbo].[RentTransactionDetails]  WITH CHECK ADD  CONSTRAINT [ForeignKey_RentTransactionDetails_RentTransaction_TransactionId] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[RentTransactions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RentTransactionDetails] CHECK CONSTRAINT [ForeignKey_RentTransactionDetails_RentTransaction_TransactionId]
GO
ALTER TABLE [dbo].[RentTransactions]  WITH CHECK ADD  CONSTRAINT [FK_RentTransactions_Renters_RenterId] FOREIGN KEY([RenterId])
REFERENCES [dbo].[Renters] ([Id])
GO
ALTER TABLE [dbo].[RentTransactions] CHECK CONSTRAINT [FK_RentTransactions_Renters_RenterId]
GO
ALTER TABLE [dbo].[RentTransactions]  WITH CHECK ADD  CONSTRAINT [FK_RentTransactions_Room_RoomId] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Rooms] ([Id])
GO
ALTER TABLE [dbo].[RentTransactions] CHECK CONSTRAINT [FK_RentTransactions_Room_RoomId]
CREATE INDEX IX_RentArrears_IsManualEntry ON RentArrears(IsManualEntry)
GO
CREATE INDEX IDX_Renters_Name ON Renters(Name)
GO
CREATE INDEX IDX_Renters_IsEndRent ON Renters(IsEndRent)
GO
/****** Object:  Table [dbo].[AuditRenters] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditRenters](
	[AuditRenterId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[AdvanceMonths] [int] NOT NULL,
	[AdvancePaidDate] [datetime2](7) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[NoOfPersons] [int] NOT NULL,
	[RoomId] [int] NOT NULL,
	[DateEndRent] [datetime2](7) NULL,
	[IsEndRent] [bit] NOT NULL DEFAULT ((0)),
	[MonthsUsed] [int] NOT NULL DEFAULT ((0)),
	[BalanceAmount] [decimal](18, 2) NULL,
	[BalancePaidDate] [datetime2](7) NULL,
	[TotalPaidAmount] [decimal](18, 2) NOT NULL DEFAULT ((0.0)),
	[DueDay] [int] NOT NULL DEFAULT ((0)),
	[NextDueDate] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
	[PreviousDueDate] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
	[AuditDateTime] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
	[Username] [nvarchar](50) NOT NULL,
	[AuditAction] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_AuditRenters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AuditRenters]  WITH CHECK ADD  CONSTRAINT [ForeignKey_AuditRenters_Renters] FOREIGN KEY([Id])
REFERENCES [dbo].[Renters] ([Id])
GO
ALTER TABLE [dbo].[AuditRenters] CHECK CONSTRAINT [ForeignKey_AuditRenters_Renters]
GO

ALTER TABLE [dbo].[AuditRenters]  WITH CHECK ADD  CONSTRAINT [ForeignKey_AuditRenters_Room] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Rooms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuditRenters] CHECK CONSTRAINT [ForeignKey_AuditRenters_Room]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditRentTransactions](
	[AuditRentTransactionId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[PaidDate] [datetime2](7) NULL,
	[PaidAmount] [decimal](18, 2) NULL,
	[Balance] [decimal](18, 2) NULL,
	[BalanceDateToBePaid] [datetime2](7) NULL,
	[IsDepositUsed] [bit] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[RoomId] [int] NOT NULL,
	[RenterId] [int] NOT NULL,
	[DueDate] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
	[Period] [nvarchar](max) NULL,
	[TransactionType] [int] NOT NULL DEFAULT ((0)),
	[IsSystemProcessed] [bit] NOT NULL DEFAULT ((0)),
	[SystemDateTimeProcessed] [datetime2](7) NULL,
	[TotalAmountDue] [decimal](18, 2) NOT NULL DEFAULT ((0.0)),
	[IsProcessed] [bit] NOT NULL DEFAULT ((0)),
	[ExcessPaidAmount] [decimal](18,2) NOT NULL DEFAULT(0),
	[AuditDateTime] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
	[Username] [nvarchar](50) NOT NULL,
	[AuditAction] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_AuditRentTransactions] PRIMARY KEY CLUSTERED 
(
	[AuditRentTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AuditRentTransactions]  WITH CHECK ADD  CONSTRAINT [FK_AuditRentTransactions_Room_RoomId] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Rooms] ([Id])
GO
ALTER TABLE [dbo].[AuditRentTransactions] CHECK CONSTRAINT [FK_AuditRentTransactions_Room_RoomId]

ALTER TABLE [dbo].[AuditRentTransactions]  WITH CHECK ADD  CONSTRAINT [FK_AuditRentTransactions_Renters_RenterId] FOREIGN KEY([RenterId])
REFERENCES [dbo].[Renters] ([Id])
GO
ALTER TABLE [dbo].[AuditRentTransactions] CHECK CONSTRAINT [FK_AuditRentTransactions_Renters_RenterId]

ALTER TABLE [dbo].[AuditRentTransactions]  WITH CHECK ADD  CONSTRAINT [ForeignKey_AuditRentTransactions_RentTransactions_Id] FOREIGN KEY([Id])
REFERENCES [dbo].[RentTransactions] ([Id])
GO
ALTER TABLE [dbo].[AuditRentTransactions] CHECK CONSTRAINT [ForeignKey_AuditRentTransactions_RentTransactions_Id]
GO

/****** Object:  Table [dbo].[RentTransactions]    ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentTransactionPayments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Amount] [decimal](18, 2) NULL,
	[DatePaid] [datetime2](7) NULL,
	[PaymentTransactionType] [int] NOT NULL,
	[RentTransactionId] [int] NOT NULL,
	[IsDeleted] [BIT] NOT NULL DEFAULT(0)
 CONSTRAINT [PK_RentTransactionPayments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RentTransactionPayments]  WITH CHECK ADD  CONSTRAINT [FK_RentTransactionPayments_RentTransactions_RentTransactionId] FOREIGN KEY([RentTransactionId])
REFERENCES [dbo].[RentTransactions] ([Id])
ON DELETE CASCADE
GO

CREATE INDEX IDX_RentTransactionPayments_DatePaid ON RentTransactionPayments(DatePaid)
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditRentTransactionPayments](
	[AuditRentTransactionPaymentId] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[Amount] [decimal](18, 2) NULL,
	[DatePaid] [datetime2](7) NULL,
	[PaymentTransactionType] [int] NOT NULL,
	[IsDeleted] [BIT] NOT NULL DEFAULT(0),
	[AuditDateTime] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
	[Username] [nvarchar](50) NOT NULL,
	[AuditAction] [nvarchar](20) NOT NULL,
	CONSTRAINT [PK_AuditRentTransactionPayments] PRIMARY KEY CLUSTERED 
(
	[AuditRentTransactionPaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AuditRentTransactionPayments]  WITH CHECK ADD  CONSTRAINT [FK_AuditRentTransactionPayments_RentTransactions_Id] FOREIGN KEY(Id)
REFERENCES [dbo].[RentTransactionPayments] ([Id])
ON DELETE CASCADE
GO

CREATE INDEX IDX_AuditRentTransactionPayments_DatePaid ON AuditRentTransactionPayments(DatePaid)
GO

CREATE INDEX IDX_RentArrears_IsProcessed ON RentArrears(IsProcessed)
GO
CREATE INDEX IDX_rentTransactions_IsProcessed ON renttransactions(IsProcessed)
GO

CREATE TABLE [dbo].[MobileAppLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NULL,
	[LogInfo] [nvarchar](max) NULL,
	[UtcCreatedDateTime] [datetime2](7) NOT NULL DEFAULT (GETUTCDATE()),
 CONSTRAINT [PK_MobileAppLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MemberCommunicationHistory]    Script Date: 25/10/2017 1:03:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO

ALTER TABLE [dbo].[RenterCommunicationHistory]  WITH CHECK ADD  CONSTRAINT [FK_RenterCommunicationHistory_Accounts_RenterId] FOREIGN KEY(RenterId)
	REFERENCES [dbo].[Renters] ([Id])
	ON DELETE CASCADE
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
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

ALTER TABLE [dbo].[AccountBillingItems]  WITH CHECK ADD  CONSTRAINT [FK_AccountBillingItems_Accounts] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])

ALTER TABLE [dbo].[AccountBillingItems] CHECK CONSTRAINT [FK_AccountBillingItems_Accounts]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Image] [nvarchar](100) NULL,
	[Price] [money] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UtcDateTimeCreated] [DateTime] NOT NULL,
	[UtcDateTimeUpdated] [DateTime] NULL,
	NoOfPersons INT NOT NULL DEFAULT(0),
	NoOfPersonsMax INT NOT NULL DEFAULT(0),
	BookingUrl NVARCHAR(100) NULL,
 CONSTRAINT [PK_BookingTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingTypeInclusions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookingTypeId] [int] NOT NULL,
	[InclusionId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UtcDateTimeCreated] [DateTime] NOT NULL,
	[UtcDateTimeUpdated] [DateTime] NULL,
 CONSTRAINT [PK_BookingTypeInclusions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BookingTypeInclusions]  WITH CHECK ADD  CONSTRAINT [ForeignKey_BookingType_BookingTypeInclusions_BookingTypeId] FOREIGN KEY([BookingTypeId])
REFERENCES [dbo].[BookingTypes] ([Id])
GO
ALTER TABLE [dbo].[BookingTypeInclusions] CHECK CONSTRAINT [ForeignKey_BookingType_BookingTypeInclusions_BookingTypeId]
GO

ALTER TABLE [dbo].[BookingTypeInclusions]  WITH CHECK ADD  CONSTRAINT [ForeignKey_LookupTypeItem_BookingTypeInclusions_InclusionId] FOREIGN KEY([BookingTypeId])
REFERENCES [dbo].[LookupTypeItemS] ([Id])
GO
ALTER TABLE [dbo].[BookingTypeInclusions] CHECK CONSTRAINT [ForeignKey_LookupTypeItem_BookingTypeInclusions_InclusionId]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookingTypeImages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ImageName] [nvarchar](100) NOT NULL,
	[IsShow] [bit] NOT NULL,
	[BookingTypeId] [int] NOT NULL,
 CONSTRAINT [PK_BookingTypeImages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[BookingTypeImages]  WITH CHECK ADD  CONSTRAINT [ForeignKey_BookingType_BookingTypeImages_BookingTypeId] FOREIGN KEY([BookingTypeId])
REFERENCES [dbo].[BookingTypes] ([Id])
GO
ALTER TABLE [dbo].[BookingTypeImages] CHECK CONSTRAINT [ForeignKey_BookingType_BookingTypeImages_BookingTypeId]
GO

CREATE TABLE [dbo].[GuestBookingDetails](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ArrivalDateLocal] DateTime,
		[DepartureDateLocal] Datetime,
		[Country] [nvarchar](100) NOT NULL,
		[LanguagesSpoken] [nvarchar](100) NOT NULL,
		[Email] [nvarchar](50) NOT NULL,
		[Contact] [nvarchar](15) NOT NULL,
		[ContactPerson] [nvarchar](100) NULL,
		[LeaveMessage] [nvarchar](1000) NULL,
		[CreatedDateTimeUtc] DateTime,
	 CONSTRAINT [PK_GuestBookingDetails] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO

CREATE TABLE [dbo].[GuestBookings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Gender] [varchar](10) NOT NULL,
	[Birthday] Datetime,
	[Ages] [int] NOT NULL,
	[GuestBookingDetailId] [int] NOT NULL,
	CONSTRAINT [PK_GuestBookings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[GuestBookings]  WITH CHECK ADD  CONSTRAINT [FK_GuestBookings_GuestBookingDetails_GuestBookingDetailId] FOREIGN KEY([GuestBookingDetailId])
REFERENCES [dbo].[GuestBookingDetails] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[GuestBookings] CHECK CONSTRAINT [FK_GuestBookings_GuestBookingDetails_GuestBookingDetailId]
GO

ALTER TABLE [dbo].[GuestBookings]  WITH CHECK ADD  CONSTRAINT [ForeignKey_LookupTypeItems_GuestBookings] FOREIGN KEY([Ages])
REFERENCES [dbo].[LookupTypeItems] ([Id])
ON DELETE CASCADE
GO
	
ALTER TABLE [dbo].[GuestBookings] CHECK CONSTRAINT [ForeignKey_LookupTypeItems_GuestBookings]
GO
