
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME ='Tools')
BEGIN
	CREATE TABLE [dbo].[Tools] (
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [varchar](100) NOT NULL,
		[Description] [varchar](500) NOT NULL,
		[Images] [varchar](1000) NOT NULL,
		[PowerTool] BIT NOT NULL DEFAULT(0),
		[CreatedDateTimeUtc] datetime,
		[CreatedBy] [varchar](50) NOT NULL,
		[IsDeleted] [BIT] NOT NULL DEFAULT(0),
		[DeletedDateTimeUtc] datetime NULL,
		[DeletedBy] [varchar](50) NULL,
		CONSTRAINT [PK_Tools] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME ='ToolsInventory')
BEGIN
	CREATE TABLE [dbo].[ToolsInventory] (
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ToolId] [int] NOT NULL,
		[InventoryDateTimeUtc] datetime,
		[Status] [varchar](20) NOT NULL,
		[Action] [varchar](20) NOT NULL,
		[CreatedBy] VARCHAR(100),
		[CreatedDateTimeUtc] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
		[IsDeleted] [BIT] NOT NULL DEFAULT(0),
		[DeletedDateTimeUtc] datetime NULL,
		[DeletedBy] [varchar](50) NULL,
		CONSTRAINT [PK_ToolsInventory] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[ToolsInventory]  WITH CHECK ADD  CONSTRAINT [FK_ToolsInventory_ToolsInventory_Tools] FOREIGN KEY([ToolId])
	REFERENCES [dbo].[Tools] ([Id])
	ON DELETE CASCADE

	ALTER TABLE [dbo].[ToolsInventory] CHECK CONSTRAINT [FK_ToolsInventory_ToolsInventory_Tools]
	END
GO
