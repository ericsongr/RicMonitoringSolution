
IF NOT EXISTS(SELECT 1 FROM LookupTypes WHERE Name = 'Booking Type Inclusions')
BEGIN
	INSERT INTO LookupTypes(Name) VALUES('Booking Type Inclusions')
END
GO

DECLARE @lookupId INT = 0

SELECT @lookupId = Id FROM LookupTypes WHERE Name = 'Booking Type Inclusions'

IF (@lookupId > 0)
BEGIN
	IF NOT EXISTS(SELECT 1 FROM LookupTypeItems WHERE Description = 'Fan Only' and LookupTypeId = @lookupId)
		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('Fan Only', 1, @lookupId)
	
	IF NOT EXISTS(SELECT 1 FROM LookupTypeItems WHERE Description = 'Blanket' and LookupTypeId = @lookupId)
		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('Blanket', 1, @lookupId)
	
	IF NOT EXISTS(SELECT 1 FROM LookupTypeItems WHERE Description = 'Pillow' and LookupTypeId = @lookupId)
		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('Pillow', 1, @lookupId)
	
	IF NOT EXISTS(SELECT 1 FROM LookupTypeItems WHERE Description = '2 Pillows' and LookupTypeId = @lookupId)
		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('2 Pillows', 1, @lookupId)
	
	IF NOT EXISTS(SELECT 1 FROM LookupTypeItems WHERE Description = '4 Pillows' and LookupTypeId = @lookupId)
		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('4 Pillows', 1, @lookupId)
	
	IF NOT EXISTS(SELECT 1 FROM LookupTypeItems WHERE Description = 'Shower Towel' and LookupTypeId = @lookupId)
	    INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('Shower Towel', 1, @lookupId)
	
	IF NOT EXISTS(SELECT 1 FROM LookupTypeItems WHERE Description = 'Single Bed Uratex foam with 4x36x75 inches size' and LookupTypeId = @lookupId)
		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('Single Bed Uratex foam with 4x36x75 inches size', 1, @lookupId)
	
	IF NOT EXISTS(SELECT 1 FROM LookupTypeItems WHERE Description = 'Full-Double Bed Uratex foam with 4x54x75 inches size' and LookupTypeId = @lookupId)
		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('Full-Double Bed Uratex foam with 4x54x75 inches size', 1, @lookupId)

	IF NOT EXISTS(SELECT 1 FROM LookupTypeItems WHERE Description = '2 set Full-Double Bed Uratex foam with 4x54x75 inches size' and LookupTypeId = @lookupId)
		INSERT INTO LookupTypeItems(Description, IsActive, LookupTypeId) VALUES('2 set Full-Double Bed Uratex foam with 4x54x75 inches size', 1, @lookupId)
END
GO
