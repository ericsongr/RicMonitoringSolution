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
