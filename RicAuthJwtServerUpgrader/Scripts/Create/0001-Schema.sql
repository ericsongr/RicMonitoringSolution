
CREATE TABLE [dbo].[AspNetUserLoginTokens](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[TokenValue] [nvarchar](max) NOT NULL,
	[CreatedDateUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_AspNetUserLoginTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetUserLoginTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLoginTokens_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[AspNetUserLoginTokens] CHECK CONSTRAINT [FK_AspNetUserLoginTokens_AspNetUsers]
GO

CREATE TABLE [dbo].[RefreshTokens](
	[RefreshTokenId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[IssuedUtc] [datetime] NOT NULL,
	[ExpiresUtc] [datetime] NOT NULL,
	[Token] [nvarchar](max) NOT NULL,
	[DeviceId] [nvarchar](1000) NULL,
 CONSTRAINT [PK_RefreshToken] PRIMARY KEY CLUSTERED 
(
	[RefreshTokenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[RefreshTokens]  WITH CHECK ADD  CONSTRAINT [FK_RefreshTokens_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[RefreshTokens] CHECK CONSTRAINT [FK_RefreshTokens_AspNetUsers]
GO

CREATE TABLE [dbo].[RegisteredDevices](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AspNetUsersId] [nvarchar](450) NOT NULL,
	[DeviceId] [nvarchar](1000) NOT NULL,
	[Platform] [nvarchar](100) NULL,
	[LastAccessOnUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_RegisteredDevices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RegisteredDevices]  WITH CHECK ADD  CONSTRAINT [FK_RegisteredDevices_AspNetUsers] FOREIGN KEY([AspNetUsersId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[RegisteredDevices] CHECK CONSTRAINT [FK_RegisteredDevices_AspNetUsers]
GO