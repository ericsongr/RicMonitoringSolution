IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RentTransactionPayments')
BEGIN
	CREATE TABLE [dbo].[RentTransactionPayments](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Amount] [decimal](18, 2) NULL,
		[DatePaid] [datetime2](7) NULL,
		[PaymentTransactionType] [int] NOT NULL,
		[RentTransactionId] [int] NOT NULL
	 CONSTRAINT [PK_RentTransactionPayments] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[RentTransactionPayments]  WITH CHECK ADD  CONSTRAINT [FK_RentTransactionPayments_RentTransactions_RentTransactionId] FOREIGN KEY([RentTransactionId])
	REFERENCES [dbo].[RentTransactions] ([Id])
	ON DELETE CASCADE

	CREATE INDEX IDX_RentTransactionPayments_DatePaid ON RentTransactionPayments(DatePaid)
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AuditRentTransactionPayments')
BEGIN
	CREATE TABLE [dbo].[AuditRentTransactionPayments](
		[AuditRentTransactionPaymentId] [int] IDENTITY(1,1) NOT NULL,
		[Id] [int] NOT NULL,
		[Amount] [decimal](18, 2) NULL,
		[DatePaid] [datetime2](7) NULL,
		[PaymentTransactionType] [int] NOT NULL,
		[AuditDateTime] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
		[Username] [nvarchar](50) NOT NULL,
		[AuditAction] [nvarchar](20) NOT NULL,
		CONSTRAINT [PK_AuditRentTransactionPayments] PRIMARY KEY CLUSTERED 
	(
		[AuditRentTransactionPaymentId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[AuditRentTransactionPayments]  WITH CHECK ADD  CONSTRAINT [FK_AuditRentTransactionPayments_RentTransactions_Id] FOREIGN KEY(Id)
	REFERENCES [dbo].[RentTransactionPayments] ([Id])
	ON DELETE CASCADE

	CREATE INDEX IDX_AuditRentTransactionPayments_DatePaid ON AuditRentTransactionPayments(DatePaid)
END
GO