
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AccountProducts')
BEGIN
	CREATE TABLE [dbo].[AccountProducts] (
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](50) NOT NULL,
		[Price] [Money] NOT NULL,
		[OnlinePrice] [Money] NOT NULL,
		[IsWebPurchasable] [bit] NOT NULL DEFAULT(0),
		[AccountId] [INT] NOT NULL
	 CONSTRAINT [PK_AccountProducts] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[AccountProducts]  WITH CHECK ADD  CONSTRAINT [FK_AccountProducts_Accounts] FOREIGN KEY([AccountId])
	REFERENCES [dbo].[Accounts] ([Id])

	ALTER TABLE [dbo].[AccountProducts] CHECK CONSTRAINT [FK_AccountProducts_Accounts]
END
GO

