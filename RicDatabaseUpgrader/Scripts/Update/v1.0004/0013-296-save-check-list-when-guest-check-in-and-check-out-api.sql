IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GuestBookingDetails' AND COLUMN_NAME = 'CheckedListCheckInDateTime')
BEGIN
	ALTER TABLE GuestBookingDetails ADD CheckedListCheckInDateTime DATETIME NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GuestBookingDetails' AND COLUMN_NAME = 'CheckedListCheckInBy')
BEGIN
	ALTER TABLE GuestBookingDetails ADD CheckedListCheckInBy varchar(100)
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GuestBookingDetails' AND COLUMN_NAME = 'CheckedListCheckOutDateTime')
BEGIN
	ALTER TABLE GuestBookingDetails ADD CheckedListCheckOutDateTime DATETIME NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GuestBookingDetails' AND COLUMN_NAME = 'CheckedListCheckOutBy')
BEGIN
	ALTER TABLE GuestBookingDetails ADD CheckedListCheckOutBy varchar(100)
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'GuestCheckLists')
BEGIN
	CREATE TABLE [dbo].[GuestCheckLists](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[GuestBookingDetailId] [int] NOT NULL,
		[CheckListId] [int] NOT NULL,
		[IsChecked] [bit] NOT NULL,
		[Notes] [varchar](1000) NULL,
	 CONSTRAINT [PK_GuestCheckLists] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[GuestCheckLists]  WITH CHECK ADD  CONSTRAINT [FK_GuestCheckLists_LookupTypeItems_GuestBookingDetailId] FOREIGN KEY([GuestBookingDetailId]) REFERENCES [dbo].[GuestBookingDetails] ([Id])
	
	ALTER TABLE [dbo].[GuestCheckLists] CHECK CONSTRAINT [FK_GuestCheckLists_LookupTypeItems_GuestBookingDetailId]
	
	ALTER TABLE [dbo].[GuestCheckLists]  WITH CHECK ADD  CONSTRAINT [FK_GuestCheckLists_LookupTypeItems_CheckListId] FOREIGN KEY([CheckListId])	REFERENCES [dbo].[LookupTypeItems] ([Id])
	
	ALTER TABLE [dbo].[GuestCheckLists] CHECK CONSTRAINT [FK_GuestCheckLists_LookupTypeItems_CheckListId]
END	
GO