IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CheckListForCheckInOutGuests')
BEGIN
	CREATE TABLE [dbo].[CheckListForCheckInOutGuests](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[LookupId] [int] NOT NULL,
		[LookupTypeItemId] [int] NOT NULL,
		[IsIncluded] [bit] NOT NULL,
	 CONSTRAINT [PK_CheckListForCheckInOutGuests] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[CheckListForCheckInOutGuests]  WITH CHECK ADD  CONSTRAINT [FK_LookupType_CheckListForCheckInOutGuest_LookupId] FOREIGN KEY([LookupId])
	REFERENCES [dbo].[LookupTypes] ([Id])
	
	ALTER TABLE [dbo].[CheckListForCheckInOutGuests] CHECK CONSTRAINT [FK_LookupType_CheckListForCheckInOutGuest_LookupId]
	
	ALTER TABLE [dbo].[CheckListForCheckInOutGuests]  WITH CHECK ADD  CONSTRAINT [FK_LookupTypeItem_CheckListForCheckInOutGuest_LookupTypeItemId] FOREIGN KEY([LookupTypeItemId])
	REFERENCES [dbo].[LookupTypeItems] ([Id])

	ALTER TABLE [dbo].[CheckListForCheckInOutGuests] CHECK CONSTRAINT [FK_LookupTypeItem_CheckListForCheckInOutGuest_LookupTypeItemId]
END
GO