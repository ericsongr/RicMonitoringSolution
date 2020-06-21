
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RentTransactions' AND COLUMN_NAME = 'ExcessPaidAmount')
BEGIN
	ALTER TABLE RentTransactions ADD ExcessPaidAmount Decimal(18,2) NOT NULL DEFAULT(0)
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AuditRentTransactions' AND COLUMN_NAME = 'ExcessPaidAmount')
BEGIN
	ALTER TABLE AuditRentTransactions ADD ExcessPaidAmount Decimal(18,2) NOT NULL DEFAULT(0)
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RentTransactionPayments' AND COLUMN_NAME = 'IsDeleted')
BEGIN
	ALTER TABLE RentTransactionPayments ADD IsDeleted BIT NOT NULL DEFAULT(0)
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AuditRentTransactionPayments' AND COLUMN_NAME = 'IsDeleted')
BEGIN
ALTER TABLE AuditRentTransactionPayments ADD IsDeleted BIT NOT NULL DEFAULT(0)
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RentTransactions' AND COLUMN_NAME = 'IsDepositUsed')
BEGIN
	insert into RentTransactionPayments
		(RentTransactionId, DatePaid, Amount, PaymentTransactionType)
	select id, PaidDate, 0, 2 
	from RentTransactions where IsDepositUsed = 1

	ALTER TABLE RentTransactions DROP COLUMN IsDepositUsed
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'RentTransactions' AND COLUMN_NAME = 'AdjustmentBalancePaymentDueAmount')
BEGIN
	insert into RentTransactionPayments
		(RentTransactionId, DatePaid, Amount, PaymentTransactionType)
	select id, DueDate, AdjustmentBalancePaymentDueAmount, 1 
	from RentTransactions where AdjustmentBalancePaymentDueAmount > 0

	ALTER TABLE RentTransactions DROP CONSTRAINT DF__RentTrans__Adjus__7908F585

	ALTER TABLE RentTransactions DROP COLUMN AdjustmentBalancePaymentDueAmount
END
GO

--INSERT OLD PAYMENT TRANSACTION
BEGIN
	insert into RentTransactionPayments
			(RentTransactionId, DatePaid, Amount, PaymentTransactionType)
	select id, PaidDate, PaidAmount, 1 from RentTransactions 
	where PaidAmount > 0 and 
	id not in (select RentTransactionid from RentTransactionPayments)
END