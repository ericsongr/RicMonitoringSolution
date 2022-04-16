IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BookingTypes')
BEGIN
	CREATE TABLE [dbo].[BookingTypes](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](100) NULL,
		[Image] [nvarchar](100) NULL,
		[Price] [money] NOT NULL,
		[IsActive] [bit] NOT NULL,
		[UtcDateTimeCreated] [DateTime] NOT NULL,
		[UtcDateTimeUpdated] [DateTime] NULL,
	 CONSTRAINT [PK_BookingTypes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BookingTypeInclusions')
BEGIN
	CREATE TABLE [dbo].[BookingTypeInclusions](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[BookingTypeId] [int] NOT NULL,
		[InclusionId] [int] NOT NULL,
		[IsActive] [bit] NOT NULL,
		[UtcDateTimeCreated] [DateTime] NOT NULL,
		[UtcDateTimeUpdated] [DateTime] NULL,
	 CONSTRAINT [PK_BookingTypeInclusions] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[BookingTypeInclusions]  WITH CHECK ADD  CONSTRAINT [ForeignKey_BookingType_BookingTypeInclusions_BookingTypeId] FOREIGN KEY([BookingTypeId])
	REFERENCES [dbo].[BookingTypes] ([Id])
	
	ALTER TABLE [dbo].[BookingTypeInclusions] CHECK CONSTRAINT [ForeignKey_BookingType_BookingTypeInclusions_BookingTypeId]
	
	ALTER TABLE [dbo].[BookingTypeInclusions]  WITH CHECK ADD  CONSTRAINT [ForeignKey_LookupTypeItem_BookingTypeInclusions_InclusionId] FOREIGN KEY([BookingTypeId])
	REFERENCES [dbo].[LookupTypeItemS] ([Id])
	
	ALTER TABLE [dbo].[BookingTypeInclusions] CHECK CONSTRAINT [ForeignKey_LookupTypeItem_BookingTypeInclusions_InclusionId]
	
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BookingTypeImages')
BEGIN
	CREATE TABLE [dbo].[BookingTypeImages](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ImageName] [nvarchar](100) NOT NULL,
		[IsShow] [bit] NOT NULL,
		[BookingTypeId] [int] NOT NULL,
	 CONSTRAINT [PK_BookingTypeImages] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[BookingTypeImages]  WITH CHECK ADD  CONSTRAINT [ForeignKey_BookingType_BookingTypeImages_BookingTypeId] FOREIGN KEY([BookingTypeId])
	REFERENCES [dbo].[BookingTypes] ([Id])

	ALTER TABLE [dbo].[BookingTypeImages] CHECK CONSTRAINT [ForeignKey_BookingType_BookingTypeImages_BookingTypeId]

END
GO
