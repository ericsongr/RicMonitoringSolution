IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'GuestBookingDates')
BEGIN
	CREATE TABLE [dbo].[GuestBookingDates](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[DateBooked] DateTime,
		[GuestBookingDetailId] INT NOT NULL
	 CONSTRAINT [PK_GuestBookingDates] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[GuestBookingDates]  WITH CHECK ADD  CONSTRAINT [FK_GuestBookings_GuestBookingDates_GuestBookingDetailId] FOREIGN KEY([GuestBookingDetailId])
	REFERENCES [dbo].[GuestBookingDetails] ([Id])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[GuestBookingDates] CHECK CONSTRAINT [FK_GuestBookings_GuestBookingDates_GuestBookingDetailId]
END
GO
