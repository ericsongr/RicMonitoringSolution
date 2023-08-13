IF NOT EXISTS(select 1 from LookupTypes WHERE Name = 'Cost Categories')
BEGIN
	INSERT INTO LookupTypes(Name) VALUES('Cost Categories')
END
GO