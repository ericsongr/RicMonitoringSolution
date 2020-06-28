IF NOT EXISTS(SELECT 1 FROM SYS.indexes WHERE NAME = 'IDX_RentArrears_IsProcessed')
BEGIN
	CREATE INDEX IDX_RentArrears_IsProcessed ON RentArrears(IsProcessed)
END
GO

IF NOT EXISTS(SELECT 1 FROM SYS.indexes WHERE NAME = 'IDX_rentTransactions_IsProcessed')
BEGIN
	CREATE INDEX IDX_rentTransactions_IsProcessed ON renttransactions(IsProcessed)
END
GO