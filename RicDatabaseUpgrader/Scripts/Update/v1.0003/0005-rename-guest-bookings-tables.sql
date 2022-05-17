IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BookedPersons')
BEGIN
	drop table BookedPersons
END
GO


IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BookedDetails')
BEGIN
	drop table BookedDetails
END
GO


IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GuestBookingDetails')
BEGIN
	CREATE TABLE [dbo].[GuestBookingDetails](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ArrivalDateLocal] DateTime,
		[DepartureDateLocal] Datetime,
		[Country] [nvarchar](100) NOT NULL,
		[LanguagesSpoken] [nvarchar](100) NOT NULL,
		[Email] [nvarchar](50) NOT NULL,
		[Contact] [nvarchar](15) NOT NULL,
		[ContactPerson] [nvarchar](100) NULL,
		[LeaveMessage] [nvarchar](1000) NULL,
		[CreatedDateTimeUtc] DateTime,
	 CONSTRAINT [PK_GuestBookingDetails] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GuestBookings')
BEGIN
	CREATE TABLE [dbo].[GuestBookings](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[FirstName] [nvarchar](50) NOT NULL,
		[LastName] [nvarchar](50) NOT NULL,
		[Gender] [varchar](10) NOT NULL,
		[Birthday] Datetime,
		[Age] [int] NOT NULL DEFAULT(0),
		[Ages] [int] NOT NULL,
		[GuestBookingDetailId] [int] NOT NULL,
	 CONSTRAINT [PK_GuestBookings] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[GuestBookings]  WITH CHECK ADD  CONSTRAINT [FK_GuestBookings_GuestBookingDetails_GuestBookingDetailId] FOREIGN KEY([GuestBookingDetailId])
	REFERENCES [dbo].[GuestBookingDetails] ([Id])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[GuestBookings] CHECK CONSTRAINT [FK_GuestBookings_GuestBookingDetails_GuestBookingDetailId]

	ALTER TABLE [dbo].[GuestBookings]  WITH CHECK ADD  CONSTRAINT [ForeignKey_LookupTypeItems_GuestBookings] FOREIGN KEY([Ages])
	REFERENCES [dbo].[LookupTypeItems] ([Id])
	ON DELETE CASCADE
	
	ALTER TABLE [dbo].[GuestBookings] CHECK CONSTRAINT [ForeignKey_LookupTypeItems_GuestBookings]
	
END
GO