DECLARE @statusId INT = 0
BEGIN
	IF NOT EXISTS(SELECT 1 from LookupTypes WHERE Name = 'Inventory Tool Status')
	BEGIN
		INSERT INTO LookupTypes(Name) VALUES('Inventory Tool Status')
	
		SELECT @statusId = Id from LookupTypes WHERE Name = 'Inventory Tool Status'

		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('Working', 1, @statusId)
		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('Still Working', 1, @statusId)
		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('Not Working', 1, @statusId)
	END

	IF NOT EXISTS(SELECT 1 from LookupTypes WHERE Name = 'Inventory Tool Action')
	BEGIN
		INSERT INTO LookupTypes(Name) VALUES('Inventory Tool Action')
	
		SELECT @statusId = Id from LookupTypes WHERE Name = 'Inventory Tool Action'

		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('Newly Added', 1, @statusId)
		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('Run Testing', 1, @statusId)
		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('Repair', 1, @statusId)
	END
END
GO