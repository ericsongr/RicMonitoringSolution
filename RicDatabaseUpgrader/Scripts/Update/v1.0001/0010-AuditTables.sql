IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AuditRenters')
BEGIN
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
		[AuditRenterId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[AuditRenters]  WITH CHECK ADD  CONSTRAINT [ForeignKey_AuditRenters_Renters] FOREIGN KEY([Id])
	REFERENCES [dbo].[Renters] ([Id])
	ALTER TABLE [dbo].[AuditRenters] CHECK CONSTRAINT [ForeignKey_AuditRenters_Renters]

	ALTER TABLE [dbo].[AuditRenters]  WITH CHECK ADD  CONSTRAINT [ForeignKey_AuditRenters_Room] FOREIGN KEY([RoomId])
	REFERENCES [dbo].[Rooms] ([Id])
	ON DELETE CASCADE
	ALTER TABLE [dbo].[AuditRenters] CHECK CONSTRAINT [ForeignKey_AuditRenters_Room]

END
GO


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AuditRooms')
BEGIN
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

ALTER TABLE [dbo].[AuditRooms]  WITH CHECK ADD  CONSTRAINT [FK_AuditRooms_Rooms_RoomId] FOREIGN KEY([Id])
REFERENCES [dbo].[Rooms] ([Id])
ON DELETE CASCADE

END
GO


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AuditRentTransactions')
BEGIN
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
		[AdjustmentBalancePaymentDueAmount] [decimal](18, 2) NOT NULL DEFAULT ((0.0)),
		[AuditDateTime] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
		[Username] [nvarchar](50) NOT NULL,
		[AuditAction] [nvarchar](20) NOT NULL,
	 CONSTRAINT [PK_AuditRentTransactions] PRIMARY KEY CLUSTERED 
	(
		[AuditRentTransactionId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE [dbo].[AuditRentTransactions]  WITH CHECK ADD  CONSTRAINT [FK_AuditRentTransactions_Room_RoomId] FOREIGN KEY([RoomId])
	REFERENCES [dbo].[Rooms] ([Id])
	ALTER TABLE [dbo].[AuditRentTransactions] CHECK CONSTRAINT [FK_AuditRentTransactions_Room_RoomId]

	ALTER TABLE [dbo].[AuditRentTransactions]  WITH CHECK ADD  CONSTRAINT [FK_AuditRentTransactions_Renters_RenterId] FOREIGN KEY([RenterId])
	REFERENCES [dbo].[Renters] ([Id])
	ALTER TABLE [dbo].[AuditRentTransactions] CHECK CONSTRAINT [FK_AuditRentTransactions_Renters_RenterId]

	ALTER TABLE [dbo].[AuditRentTransactions]  WITH CHECK ADD  CONSTRAINT [FK_AuditRentTransactions_RentTransactions_Id] FOREIGN KEY([Id])
	REFERENCES [dbo].[RentTransactions] ([Id])
	ALTER TABLE [dbo].[AuditRentTransactions] CHECK CONSTRAINT [FK_AuditRentTransactions_RentTransactions_Id]
END
GO

IF EXISTS(SELECT 1 FROM SYS.foreign_keys WHERE NAME = 'ForeignKey_RentTransaction_Renter_RenterId')
BEGIN
	ALTER TABLE RentTransactions DROP CONSTRAINT ForeignKey_RentTransaction_Renter_RenterId
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.foreign_keys WHERE NAME = 'FK_Transactions_Room_RoomId')
BEGIN
	ALTER TABLE [dbo].[RentTransactions]  WITH CHECK ADD  CONSTRAINT [FK_RentTransactions_Room_RoomId] FOREIGN KEY([RoomId])
	REFERENCES [dbo].[Rooms] ([Id])

	ALTER TABLE [dbo].[RentTransactions] CHECK CONSTRAINT [FK_RentTransactions_Room_RoomId]
END
GO