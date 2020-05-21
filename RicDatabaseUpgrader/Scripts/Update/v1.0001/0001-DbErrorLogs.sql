IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DbErrorLogs')
BEGIN
	CREATE TABLE [dbo].[DbErrorLogs](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[ProcessMessage] [nvarchar](max) NULL,
		[Line] [int] NOT NULL,
		[Message] [nvarchar](max) NULL,
		[Procedure] [nvarchar](max) NULL,
		[Number] [int] NOT NULL,
		[Severity] [int] NOT NULL,
		[State] [int] NOT NULL,
		[DateTimeCreated] [datetime2](7) NOT NULL DEFAULT (GETDATE()),
	 CONSTRAINT [PK_DbErrorLogs] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO