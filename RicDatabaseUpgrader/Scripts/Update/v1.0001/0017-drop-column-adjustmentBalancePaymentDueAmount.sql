IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AuditRentTransactions' AND COLUMN_NAME = 'adjustmentBalancePaymentDueAmount')
BEGIN
	ALTER TABLE AuditRentTransactions DROP CONSTRAINT DF__AuditRent__Adjus__5A4F643B

	ALTER TABLE AuditRentTransactions DROP COLUMN adjustmentBalancePaymentDueAmount
END
GO