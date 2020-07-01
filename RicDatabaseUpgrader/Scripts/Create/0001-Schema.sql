/****** Object:  Table [dbo].[BookedDetails]    Script Date: 17/05/2020 11:19:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookedDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Country] [nvarchar](100) NULL,
	[LanguagesSpoken] [nvarchar](100) NULL,
	[Email] [nvarchar](50) NULL,
	[Contact] [nvarchar](15) NULL,
	[LeaveMessage] [nvarchar](1000) NULL,
 CONSTRAINT [PK_BookedDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BookedPersons]    Script Date: 17/05/2020 11:19:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookedPersons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Ages] [int] NOT NULL,
	[BookedDetailId] [int] NOT NULL,
 CONSTRAINT [PK_BookedPersons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

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
	[Month] [int] NOT NULL,
	[Year] [int] NOT NULL,
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
	[RentTransactionId] [int] NULL,
	[NextDueDate] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
	[PreviousDueDate] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
 CONSTRAINT [PK_Renters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

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
 CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

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

ALTER TABLE [dbo].[BookedPersons]  WITH CHECK ADD  CONSTRAINT [FK_BookedPersons_BookedDetails_BookedDetailId] FOREIGN KEY([BookedDetailId])
REFERENCES [dbo].[BookedDetails] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BookedPersons] CHECK CONSTRAINT [FK_BookedPersons_BookedDetails_BookedDetailId]
GO
ALTER TABLE [dbo].[BookedPersons]  WITH CHECK ADD  CONSTRAINT [ForeignKey_LookupTypeItems_BookedPersons] FOREIGN KEY([Ages])
REFERENCES [dbo].[LookupTypeItems] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BookedPersons] CHECK CONSTRAINT [ForeignKey_LookupTypeItems_BookedPersons]
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
ALTER TABLE [dbo].[Renters]  WITH CHECK ADD  CONSTRAINT [ForeignKey_Renter_RentTransaction] FOREIGN KEY([RentTransactionId])
REFERENCES [dbo].[RentTransactions] ([Id])
GO
ALTER TABLE [dbo].[Renters] CHECK CONSTRAINT [ForeignKey_Renter_RentTransaction]
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
	[RentTransactionId] [int] NULL,
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