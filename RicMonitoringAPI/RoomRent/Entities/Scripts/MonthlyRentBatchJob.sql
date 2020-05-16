USE [RicMonitoring]
GO
/****** Object:  StoredProcedure [dbo].[RentTransactionBatchFile]    Script Date: 16/05/2020 1:54:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[RentTransactionBatchFile]
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