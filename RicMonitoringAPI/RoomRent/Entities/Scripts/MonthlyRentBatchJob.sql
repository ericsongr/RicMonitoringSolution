
--SELECT DATEADD(MONTH, 1, Cast('7/7/2011' as datetime))

--SELECT FORMAT(GETDATE(), 'dd-MMM-yyyy')

DECLARE @Month			INT,
		@Year			INT

SET @Month = 4
SET @Year = 2020

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
		@TransactionId					INT

DECLARE @RenterId		INT,
		@RoomId			INT,
		@DueDay			INT,
		@DueDate		DATETIME,
		@PaidDate		DATETIME,
		@MonthlyRent	DECIMAL,
		@PaidAmount		DECIMAL,
		@Balance		DECIMAL,
		@TotalAmountDue DECIMAL,
		@IsDeposit		BIT,
		@Note			VARCHAR(100)

--use to get the last day of the each month to avoid error once save to due date field
SET @LastDayOfTheMonth = DAY(EOMONTH(CONVERT(VARCHAR(10), @Year) + '-' + CONVERT(VARCHAR(10),@Month)+ '-1'))

SET @SystemDateTimeProcessed = GETDATE()
SET @Note = 'PROCESSED BY THE SYSTEM'
SET @TransactionTypeAsMonthlyRent = 1 -- 1 for Monthly Rent

DECLARE ProcessMonthlyRentTransactionsCursor CURSOR FOR
	SELECT 
		r.Id AS RenterId,
		r.DueDay,
		rm.id RoomId, 
		rm.Price MonthlyRent,
		t.PaidAmount,
		t.Balance,
		t.IsDepositUsed,
		t.Id
	FROM Rooms rm LEFT JOIN Renters r ON rm.id = r.RoomId 
		LEFT JOIN RentTransactions t ON r.Id = t.RenterId AND MONTH(t.DueDate) = @Month AND YEAR(t.DueDate) = @Year
	where IsEndRent = 0
	AND   SystemDateTimeProcessed IS NULL


OPEN ProcessMonthlyRentTransactionsCursor
FETCH NEXT FROM ProcessMonthlyRentTransactionsCursor INTO @RenterId, @DueDay, @RoomId, @MonthlyRent, @PaidAmount, @Balance, @IsDeposit, @TransactionId
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
		--CHECK IF DUE DAY OF THE RENTER NOT EXCEED THE LAST DAY OF THE MONTH IF SO USE LAST DAY INSTEAD
		IF @DueDay > @LastDayOfTheMonth
			SET @DueDay = @LastDayOfTheMonth

		--DUE DATE TIME
		SET @DueDate = CAST((CONVERT(VARCHAR(2), @Month) + '/' + CONVERT(VARCHAR(2), @DueDay) + '/' + CONVERT(VARCHAR(4), @Year)) AS DATETIME)
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
			SET @TotalBalance = @TotalBalance;
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
				WHERE RentTransactionId = @TransactionId
		
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

	FETCH NEXT FROM ProcessMonthlyRentTransactionsCursor INTO @RenterId, @DueDay, @RoomId, @MonthlyRent, @PaidAmount, @Balance, @IsDeposit, @TransactionId
END
CLOSE ProcessMonthlyRentTransactionsCursor
DEALLOCATE ProcessMonthlyRentTransactionsCursor