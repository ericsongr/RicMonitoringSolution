IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GuestBookingDetails' AND COLUMN_NAME = 'BookingType')
BEGIN
	ALTER TABLE GuestBookingDetails ADD BookingType INT NOT NULL DEFAULT(0)
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GuestBookingDetails' AND COLUMN_NAME = 'AccountId')
BEGIN
	ALTER TABLE [dbo].[GuestBookingDetails] ADD AccountId INT NOT NULL

	ALTER TABLE [dbo].[GuestBookingDetails]  WITH CHECK ADD  CONSTRAINT [FK_GuestBookingDetail_Account_AccountId] FOREIGN KEY([AccountId])
	REFERENCES [dbo].[Accounts] ([Id])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[GuestBookingDetails] CHECK CONSTRAINT [FK_GuestBookingDetail_Account_AccountId]
END
GO