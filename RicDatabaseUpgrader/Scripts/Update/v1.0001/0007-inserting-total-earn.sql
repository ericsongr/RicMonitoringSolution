IF NOT EXISTS(SELECT 1 FROM SYS.indexes WHERE Name = 'IDX_Renters_Name')
BEGIN
	CREATE INDEX IDX_Renters_Name ON Renters(Name)
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.indexes WHERE Name = 'IDX_Renters_IsEndRent')
BEGIN
	CREATE INDEX IDX_Renters_IsEndRent ON Renters(IsEndRent)
END
GO

IF NOT EXISTS(SELECT 1 FROM Renters WHERE Name = 'EarningsFromTheStartToTimeStartUsingTheWebapp')
BEGIN
	INSERT INTO [dbo].[Renters]
			   ([Name]
			   ,[AdvanceMonths]
			   ,[AdvancePaidDate]
			   ,[StartDate]
			   ,[NoOfPersons]
			   ,[RoomId]
			   ,[DateEndRent]
			   ,[IsEndRent]
			   ,[MonthsUsed]
			   ,[BalanceAmount]
			   ,[BalancePaidDate]
			   ,[TotalPaidAmount]
			   ,[DueDay]
			   ,[NextDueDate]
			   ,[PreviousDueDate]
			   ,[RentTransactionId])
		 VALUES
			   ('EarningsFromTheStartToTimeStartUsingTheWebapp'
			   ,0
			   ,DATEADD(MONTH, -1, GETDATE())
			   ,DATEADD(MONTH, -1, GETDATE())
			   ,0
			   ,1
			   ,DATEADD(MONTH, -1, GETDATE())
			   ,1
			   ,0
			   ,0
			   ,DATEADD(MONTH, -1, GETDATE())
			   ,9
			   ,1
			   ,DATEADD(MONTH, -1, GETDATE())
			   ,DATEADD(MONTH, -1, GETDATE())
			   ,NULL)
END
GO

IF NOT EXISTS(SELECT 1 FROM RentTransactions WHERE Note = 'Earnings from start until system start using')
BEGIN
	DECLARE @RenterId INT,
		    @RentTransactionId INT

	select @RenterId = Id from Renters where Name = 'EarningsFromTheStartToTimeStartUsingTheWebapp'

	INSERT INTO [dbo].[RentTransactions]
			   ([PaidDate]
			   ,[PaidAmount]
			   ,[Balance]
			   ,[BalanceDateToBePaid]
			   ,[IsDepositUsed]
			   ,[Note]
			   ,[RoomId]
			   ,[RenterId]
			   ,[DueDate]
			   ,[Period]
			   ,[TransactionType]
			   ,[IsSystemProcessed]
			   ,[SystemDateTimeProcessed]
			   ,[TotalAmountDue]
			   ,[IsProcessed]
			   ,[AdjustmentBalancePaymentDueAmount])
		 VALUES
			   (DATEADD(MONTH, -1, GETDATE())
			   ,853490
			   ,0
			   ,null
			   ,0
			   ,'Earnings from start until system start using'
			   ,1
			   ,@RenterId
			   ,DATEADD(MONTH, -1, GETDATE())
			   ,''
			   ,3
			   ,1
			   ,DATEADD(MONTH, -1, GETDATE())
			   ,0
			   ,1
			   ,0)

		SET @RentTransactionId = @@IDENTITY

		--INSERT TRANSACTION DETAIL
		INSERT RentTransactionDetails(TransactionId, Amount, RentArrearId)
			VALUES(@RentTransactionId, 853490, NULL)
END
GO