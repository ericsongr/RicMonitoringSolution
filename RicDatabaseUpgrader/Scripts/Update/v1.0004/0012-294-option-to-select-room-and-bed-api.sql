IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BookingTypes' AND COLUMN_NAME = 'LinkRooms')
BEGIN
	ALTER TABLE BookingTypes ADD LinkRooms VARCHAR(20)
END
GO	

UPDATE BookingTypes SET LinkRooms = '7,8' WHERE Id = 1
GO	
UPDATE BookingTypes SET LinkRooms = '9,10' WHERE Id = 2
GO	
UPDATE BookingTypes SET LinkRooms = '11' WHERE Id = 3
GO	

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GuestBookingDetails' AND COLUMN_NAME = 'RoomOrBedId')
BEGIN
	ALTER TABLE GuestBookingDetails ADD RoomOrBedId INT NULL

	ALTER TABLE [dbo].[GuestBookingDetails]  WITH CHECK ADD  CONSTRAINT [FK_GuestBookingDetails_LookupTypeItems_RoomOrBedId] FOREIGN KEY([RoomOrBedId])
	REFERENCES [dbo].[LookupTypeItems] ([Id])

	ALTER TABLE [dbo].[GuestBookingDetails] CHECK CONSTRAINT [FK_GuestBookingDetails_LookupTypeItems_RoomOrBedId]

END
GO