UPDATE Renters 
SET PreviousDueDate = '2020-04-18 00:00:00.0000000', 
	NextDueDate = '2020-05-18 00:00:00.0000000' 
where name = 'Cherry Velasco'
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Renters' AND COLUMN_NAME = 'RentTransactionId')
BEGIN
	DROP INDEX IX_Renters_RentTransactionId ON Renters

	ALTER TABLE Renters DROP CONSTRAINT ForeignKey_Renter_RentTransaction

	ALTER TABLE Renters DROP COLUMN RentTransactionId
END
GO


delete RentTransactions where DueDate < '2020-06-30 16:15:07.180'
go