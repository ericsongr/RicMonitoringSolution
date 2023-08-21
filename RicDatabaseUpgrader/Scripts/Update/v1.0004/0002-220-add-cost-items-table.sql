

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CostItems')
BEGIN
	CREATE TABLE [dbo].[CostItems](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](50) NOT NULL,
		[IsDeleted] [bit] not null default(0),
		CONSTRAINT [PK_CostItems] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TransactionCost')
BEGIN
	CREATE TABLE [dbo].[TransactionCost](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[CostItemId] [int] NOT NULL,
		[Note] [nvarchar](100) NOT NULL,
		[CostCategoryId] [int] NOT NULL,
		[IsDeleted] [bit] not null default(0),
		[Cost] MONEY NOT NULL DEFAULT(0),
		CONSTRAINT [PK_TransactionCost] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[TransactionCost]  WITH CHECK ADD  CONSTRAINT [ForeignKey_TransactionCost_CostItems] FOREIGN KEY([CostItemId])
	REFERENCES [dbo].[CostItems] ([Id])
	ON DELETE CASCADE
	
	ALTER TABLE [dbo].[TransactionCost] CHECK CONSTRAINT [ForeignKey_TransactionCost_CostItems]

	ALTER TABLE [dbo].[TransactionCost]  WITH CHECK ADD  CONSTRAINT [ForeignKey_TransactionCost_LookupTypeItems] FOREIGN KEY([CostCategoryId])
	REFERENCES [dbo].[LookupTypeItems] ([Id])
	ON DELETE CASCADE
	
	ALTER TABLE [dbo].[TransactionCost] CHECK CONSTRAINT [ForeignKey_TransactionCost_LookupTypeItems]
END
GO

