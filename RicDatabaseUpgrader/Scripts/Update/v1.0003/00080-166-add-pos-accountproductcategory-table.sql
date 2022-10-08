

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AccountProductCategory')
BEGIN
	CREATE TABLE [dbo].[AccountProductCategory] (
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](50) NOT NULL,
		[IsActive] [bit] NOT NULL,
		[AccountId] [INT] NOT NULL
	 CONSTRAINT [PK_AccountProductCategory] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[AccountProductCategory]  WITH CHECK ADD  CONSTRAINT [FK_AccountProductCategory_Accounts] FOREIGN KEY([AccountId])
	REFERENCES [dbo].[Accounts] ([Id])

	ALTER TABLE [dbo].[AccountProductCategory] CHECK CONSTRAINT [FK_AccountProductCategory_Accounts]
END
GO
