
IF NOT EXISTS(SELECT 1 FROM Settings WHERE [Key] = 'PrimaryColorForApp')
BEGIN
	INSERT INTO Settings([Key],Value,FriendlyName) VALUES('PrimaryColorForApp','#ef4656','Primary Color For App')
	INSERT INTO Settings([Key],Value,FriendlyName) VALUES('SecondaryColorForApp','#ec4c0e','Secondary Color For App')
	INSERT INTO Settings([Key],Value,FriendlyName) VALUES('PrimaryTextColorForApp','#808080','Primary Text Color For App')
	INSERT INTO Settings([Key],Value,FriendlyName) VALUES('SecondaryTextColorForApp','#','Secondary Text Color For App')
	INSERT INTO Settings([Key],Value,FriendlyName) VALUES('ThirdColorForApp','#','Third Color For App')
	INSERT INTO Settings([Key],Value,FriendlyName) VALUES('FourthColorForApp','#008000','Fourth Color For App')
	INSERT INTO Settings([Key],Value,FriendlyName) VALUES('FifthColorForApp','#b300b3','Fifth Color For App')
	INSERT INTO Settings([Key],Value,FriendlyName) VALUES('PrimaryColorForMyAccount','','Primary colour')
	INSERT INTO Settings([Key],Value,FriendlyName) VALUES('PrimaryTextColorForMyAccount','','Primary text colour')
	INSERT INTO Settings([Key],Value,FriendlyName) VALUES('SecondaryColorForMyAccount','','Secondary colour')
	INSERT INTO Settings([Key],Value,FriendlyName) VALUES('SecondaryTextColorForMyAccount','','Secondary text colour')
END
GO