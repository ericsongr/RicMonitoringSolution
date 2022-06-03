IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BookingTypes' AND COLUMN_NAME = 'AccountProductId')
BEGIN
	ALTER TABLE BookingTypes ADD AccountProductId INT NOT NULL

	ALTER TABLE [dbo].[BookingTypes]  WITH CHECK ADD  CONSTRAINT [FK_AccountProduct_BookingTypes_AccountProductId] FOREIGN KEY([AccountProductId])
	REFERENCES [dbo].[AccountProducts] ([Id])

	ALTER TABLE [dbo].[BookingTypes] CHECK CONSTRAINT [FK_AccountProduct_BookingTypes_AccountProductId]
END
GO