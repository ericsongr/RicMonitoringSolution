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
	[AdjustmentBalancePaymentDueAmount] [decimal](18, 2) NOT NULL DEFAULT ((0.0)),
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
ALTER TABLE [dbo].[RentTransactions]  WITH CHECK ADD  CONSTRAINT [ForeignKey_RentTransaction_Renter_RenterId] FOREIGN KEY([RenterId])
REFERENCES [dbo].[Renters] ([Id])
GO
ALTER TABLE [dbo].[RentTransactions] CHECK CONSTRAINT [ForeignKey_RentTransaction_Renter_RenterId]
GO
CREATE INDEX IX_RentArrears_IsManualEntry ON RentArrears(IsManualEntry)
GO
/****** Object:  StoredProcedure [dbo].[RentTransactionBatchFile]    Script Date: 17/05/2020 11:19:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RentTransactionBatchFile]
	@CurrentDate	DATETIME
AS

--NOTES
--1. DEBUGGING PURPOSES CHECK IF NEXT DUE DATE ALREADY UPDATED
--2. SELECT 1 / 0 AS Error; --THIS LINE USE TO FORCE ERROR COMMENTED FOR FUTURE USE

IF NOT EXISTS(select 1 from MonthlyRentBatch WHERE CONVERT(VARCHAR(101), ProcessStartDateTime, 11) = CONVERT(VARCHAR(101), GETDATE(), 11))
BEGIN

	--DECLARE @CurrentDate	DATETIME

	DECLARE @TransactionTypeAsMonthlyRent	INT,
			@DateStart						DATETIME,
			@DateEnd						DATETIME,
			@Period							VARCHAR(100),
			@SystemDateTimeProcessed		DATETIME, 
			@LastDayOfTheMonth				INT,
			@TotalBalance					DECIMAL,
			@PreviousRentArrearId			INT,
			@PreviousRentTransactionId		INT,
			@PreviousTotalBalance			DECIMAL, 
			@TransactionId					INT,
			@TenantGracePeriod				INT

	DECLARE @RenterId		INT,
			@RoomId			INT,
			@DueDate		DATETIME,
			@PaidDate		DATETIME,
			@MonthlyRent	DECIMAL,
			@PaidAmount		DECIMAL,
			@Balance		DECIMAL,
			@TotalAmountDue DECIMAL,
			@IsDeposit		BIT,
			@Note			VARCHAR(100)

	BEGIN
		BEGIN TRY
			BEGIN TRANSACTION
				DECLARE @MonthlyRentBatchId INT

				SET @CurrentDate = GETDATE()

				--INSERT LOG BATCH DAILY RUN FOR A MONTH
				INSERT INTO MonthlyRentBatch VALUES(MONTH(@CurrentDate), YEAR(@CurrentDate), @CurrentDate, NULL)
				--FETCH @MonthlyRentBatchId FOR PROCESS END DATE  LATER UPDATE
				SET @MonthlyRentBatchId = @@IDENTITY

				SET @SystemDateTimeProcessed = @CurrentDate
				SET @Note = 'PROCESSED BY THE SYSTEM'
				SET @TransactionTypeAsMonthlyRent = 1 -- 1 for Monthly Rent

				SELECT @TenantGracePeriod = Value FROM Settings WHERE [Key] = 'TenantGracePeriod'

				--COMMENT FOR TESTING PURPOSES
				--SET @CurrentDate = DATEADD(MONTH, 1, DATEADD(DAY, @TenantGracePeriod, '2020-04-30'))

				--SET @CurrentDate = DATEADD(DAY, @TenantGracePeriod, '2020-04-30')

				DECLARE ProcessMonthlyRentTransactionsCursor CURSOR FOR
					SELECT 
						r.Id AS RenterId,
						rm.id RoomId, 
						rm.Price MonthlyRent,
						t.PaidAmount,
						t.Balance,
						t.IsDepositUsed,
						t.Id,
						r.NextDueDate
					FROM Rooms rm INNER JOIN Renters r ON rm.id = r.RoomId 
						LEFT JOIN RentTransactions t ON r.Id = t.RenterId AND CONVERT(VARCHAR(11), r.NextDueDate, 101) = CONVERT(VARCHAR(11), t.DueDate, 101)
					where IsEndRent = 0
					AND   IIF(t.IsProcessed IS NULL, 0, t.IsProcessed) = 0
					AND DATEADD(DAY, @TenantGracePeriod, r.NextDueDate) <= @CurrentDate


				OPEN ProcessMonthlyRentTransactionsCursor
				FETCH NEXT FROM ProcessMonthlyRentTransactionsCursor INTO @RenterId, @RoomId, @MonthlyRent, @PaidAmount, @Balance, @IsDeposit, @TransactionId, @DueDate
				WHILE @@FETCH_STATUS = 0
				BEGIN
					SET @IsDeposit = 0
					--GET THE TOTAL PREVIOUS UNPAID AMOUNT BILL
					SET @PreviousRentArrearId = 0
					SET @PreviousRentTransactionId = 0
					SET @PreviousTotalBalance = 0

					SELECT 
						@PreviousRentArrearId  = Id,
						@PreviousRentTransactionId = RentTransactionId,
						@PreviousTotalBalance = UnpaidAmount
					FROM RentArrears
					WHERE RenterId = @RenterId
					AND IsProcessed = 0
	
					IF @PaidAmount IS NULL
					BEGIN
		
						--DUE DATE TIME
						--SET @DueDate = CAST((CONVERT(VARCHAR(2), @Month) + '/' + CONVERT(VARCHAR(2), @DueDay) + '/' + CONVERT(VARCHAR(4), @Year)) AS DATETIME)
						SET @DateStart = DATEADD(DAY, 1, @DueDate)
						SET @DateEnd = DATEADD(DAY, -1, DATEADD(MONTH, 1, @DateStart))
						SET @Period = FORMAT(@DateStart, 'dd-MMM') + ' to ' + FORMAT(@DateEnd, 'dd-MMM-yyyy')
		
						SET @TotalAmountDue = @MonthlyRent + IIF(@PreviousTotalBalance IS NULL, 0, @PreviousTotalBalance) --MonthlyRent + Balance
		
						--IF THERE'S REMAINING DEPOSIT DEDUCT INSTEAD OF ADDING TO ARREAR
						IF EXISTS(SELECT 1 FROM Renters WHERE Id = @RenterId AND MonthsUsed < AdvanceMonths)
						BEGIN
							SET @IsDeposit = 1
							--UPDATE MonthsUsed FIELD
							UPDATE Renters SET MonthsUsed = MonthsUsed + 1 WHERE Id = @RenterId

							--FETCH ONLY THE EXISTING BALANCE
							SET @TotalBalance = IIF(@PreviousTotalBalance IS NULL, 0, @PreviousTotalBalance) 
							SET @PaidDate = @DueDate
						END 
						ELSE
						BEGIN
							--TOTAL BALANCE HERE
							SET @TotalBalance = @TotalAmountDue;
							SET @PaidDate = NULL
						END
		

						--PROCESSED TRANSACTION BY SYSTEM IF CURRENT MONTH DUE STILL UNPAID
						INSERT INTO RentTransactions(
							RoomId,
							RenterId,
							Balance,
							Note,
							IsDepositUsed,
							DueDate,
							PaidDate,
							Period,
							TransactionType,
							IsSystemProcessed,
							TotalAmountDue,
							SystemDateTimeProcessed,
							IsProcessed)
						VALUES(
							@RoomId,
							@RenterId,
							@TotalBalance,
							@Note,
							@IsDeposit,
							@DueDate,
							@PaidDate,
							@Period,
							@TransactionTypeAsMonthlyRent,
							1,
							@TotalAmountDue,
							@SystemDateTimeProcessed,
							1)
		
						SET @TransactionId = @@IDENTITY

						--START RentTransactionDetails
						--INSERT DATA ON TRANSACTION DETAIL
						--INSERT PREVIOUS SAVE ARREAR
						IF (@PreviousTotalBalance > 0)
						BEGIN
							INSERT INTO RentTransactionDetails(TransactionId, Amount, RentArrearId)
								VALUES(@TransactionId, @PreviousTotalBalance, @PreviousRentArrearId)
						END
						--INSERT NEW UNPAID RENT
						INSERT INTO RentTransactionDetails(TransactionId, Amount, RentArrearId)
								VALUES(@TransactionId, @MonthlyRent, NULL)
						--END RentTransactionDetails

						IF @TotalBalance > 0
						BEGIN
							--INSERT TOTAL ARREAR/UNPAID AMOUNT BILL
							INSERT INTO RentArrears(
								RenterId,
								RentTransactionId,
								UnpaidAmount,
								IsProcessed)
							VALUES
								(@RenterId,
									@TransactionId,
									@TotalBalance,
									0)
						END

						--SET THE PREVIOUS TOTAL BALANCE IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
						UPDATE RentArrears
							SET IsProcessed = 1
								WHERE RentTransactionId = @PreviousRentTransactionId	

						UPDATE RentTransactions
							SET IsProcessed = 1
								WHERE Id = @TransactionId
		
					END

					ELSE IF @Balance > 0
					BEGIN
						--INSERT TOTAL ARREAR/UNPAID AMOUNT BILL
						INSERT INTO RentArrears(
							RenterId,
							RentTransactionId,
							UnpaidAmount,
							IsProcessed)
						VALUES
							(@RenterId,
								@TransactionId,
								@Balance,
								0)
		
						--SET THE PREVIOUS TOTAL BALANCE IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
						UPDATE RentArrears
							SET IsProcessed = 1
								WHERE RentTransactionId = @PreviousRentTransactionId	
		
						--UPDATE THE @SystemDateTimeProcessed
						UPDATE RentTransactions
							SET SystemDateTimeProcessed = @SystemDateTimeProcessed,
								IsProcessed = 1
							WHERE Id = @TransactionId
					END

					ELSE IF @IsDeposit = 1
					BEGIN
		
						IF @PreviousTotalBalance > 0
						BEGIN
							--INSERT TOTAL ARREAR/UNPAID AMOUNT BILL
							INSERT INTO RentArrears(
								RenterId,
								RentTransactionId,
								UnpaidAmount,
								IsProcessed)
							VALUES
								(@RenterId,
								@TransactionId,
								@PreviousTotalBalance,
								0)
						END

						--SET THE PREVIOUS TOTAL BALANCE IN ARREARS TABLE AS PROCESSED SO IT WON'T COUNT THE NEXT TIME IT BILL
						UPDATE RentArrears
							SET IsProcessed = 1
								WHERE RentTransactionId = @PreviousRentTransactionId	

						--UPDATE THE @SystemDateTimeProcessed
						UPDATE RentTransactions
							SET SystemDateTimeProcessed = @SystemDateTimeProcessed,
								IsProcessed = 1
							WHERE Id = @TransactionId
					END

					--UPDATE PREVIOUS AND NEXT DUE DATE
					UPDATE Renters 
						SET PreviousDueDate = NextDueDate,
							NextDueDate = DATEADD(MONTH,1, NextDueDate)
								WHERE Id = @RenterId

					FETCH NEXT FROM ProcessMonthlyRentTransactionsCursor INTO @RenterId, @RoomId, @MonthlyRent, @PaidAmount, @Balance, @IsDeposit, @TransactionId, @DueDate
				END
				CLOSE ProcessMonthlyRentTransactionsCursor
				DEALLOCATE ProcessMonthlyRentTransactionsCursor

				--UPDATE ProcesssEndDateTime SO THAT BATCH DATE FOR TODAY RUN ONCE
				UPDATE MonthlyRentBatch SET ProcesssEndDateTime = GETDATE() WHERE Id = @MonthlyRentBatchId
				--SELECT 1 / 0 AS Error;
			COMMIT TRANSACTION
		END TRY
		BEGIN CATCH
			BEGIN
				ROLLBACK TRANSACTION
				INSERT dbErrorLogs VALUES(
					'Failed daily batch process', 
					ERROR_LINE(),
					ERROR_MESSAGE(),
					ERROR_PROCEDURE(),
					ERROR_NUMBER(),
					ERROR_SEVERITY(),
					ERROR_STATE(),
					GETDATE())
			END
		END CATCH
	END
END
GO
