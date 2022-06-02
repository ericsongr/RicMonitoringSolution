
IF NOT EXISTS(SELECT 1 FROM AccountProductCategory WHERE Name = 'Accommodation Booking Options')
BEGIN
	INSERT INTO AccountProductCategory(Name, IsActive, AccountId) VALUES('Accommodation Booking Options', 1, 1)
END
GO

IF NOT EXISTS(SELECT 1 FROM AccountProducts WHERE Name = 'Backpacker')
BEGIN
	
	DECLARE @AccountProductCategoryId int
		SELECT @AccountProductCategoryId = id FROM AccountProductCategory WHERE Name = 'Accommodation Booking Options'

	INSERT INTO AccountProducts(Name,
				Price,
				OnlinePrice,
				IsWebPurchasable,
				AccountProductCategoryId) VALUES('Backpacker', 550, 599, 1, @AccountProductCategoryId)

END
GO

IF NOT EXISTS(SELECT 1 FROM AccountProducts WHERE Name = 'Couple Backpackers')
BEGIN

	DECLARE @AccountProductCategoryId int
		SELECT @AccountProductCategoryId = id FROM AccountProductCategory WHERE Name = 'Accommodation Booking Options'

	INSERT INTO AccountProducts(Name,
				Price,
				OnlinePrice,
				IsWebPurchasable,
				AccountProductCategoryId) VALUES('Couple Backpackers', 1050, 1099, 1, @AccountProductCategoryId)

END
GO

IF NOT EXISTS(SELECT 1 FROM AccountProducts WHERE Name = 'Family Room')
BEGIN

	DECLARE @AccountProductCategoryId int
		SELECT @AccountProductCategoryId = id FROM AccountProductCategory WHERE Name = 'Accommodation Booking Options'
	
	INSERT INTO AccountProducts(Name,
				Price,
				OnlinePrice,
				IsWebPurchasable,
				AccountProductCategoryId) VALUES('Family Room', 2550, 2599, 1, @AccountProductCategoryId)

END
GO



	
