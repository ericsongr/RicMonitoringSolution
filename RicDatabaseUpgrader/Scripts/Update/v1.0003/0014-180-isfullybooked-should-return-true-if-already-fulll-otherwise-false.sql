
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AccountProducts' AND COLUMN_NAME = 'MaximumLevelQuantity')
BEGIN
	ALTER TABLE AccountProducts ADD MaximumLevelQuantity INT NOT NULL DEFAULT(0)
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AccountProducts' AND COLUMN_NAME = 'MinimumLevelQuantity')
BEGIN
	ALTER TABLE AccountProducts ADD MinimumLevelQuantity INT NOT NULL DEFAULT(0)
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AccountProducts' AND COLUMN_NAME = 'WarnLevelQuantity')
BEGIN
	ALTER TABLE AccountProducts ADD WarnLevelQuantity INT NOT NULL DEFAULT(0)
END
GO

IF NOT EXISTS(select 1 from sys.foreign_keys where name = 'FK_GuestBookingDetail_BookingType_Id')
BEGIN
	ALTER TABLE [dbo].[GuestBookingDetails]  WITH CHECK ADD  CONSTRAINT FK_GuestBookingDetail_BookingType_Id FOREIGN KEY(BookingType)
	REFERENCES [dbo].[BookingTypes] (Id)
	ON DELETE CASCADE

	ALTER TABLE [dbo].[GuestBookingDetails] CHECK CONSTRAINT FK_GuestBookingDetail_BookingType_Id
END
GO