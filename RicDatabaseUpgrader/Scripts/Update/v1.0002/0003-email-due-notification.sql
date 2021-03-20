IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'AppEmailRenterBeforeDueDateEnable')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AppEmailRenterBeforeDueDateEnable', 'True', 'Enable Email Renter before due date')
END
GO

IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'AppEmailRenterNoOfDaysBeforeDueDate')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AppEmailRenterNoOfDaysBeforeDueDate', '2', 'Email Renter # of days before due date')
END
GO

IF NOT EXISTS (SELECT * FROM Settings WHERE [Key] = 'AppEmailMessageRenterBeforeDueDate')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('AppEmailMessageRenterBeforeDueDate', 'Hi {{Name}}, Friendly reminder of your rent due on {{DueDate}} for the period of {{Period}}.', 'Email Body')
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Renters' AND COLUMN_NAME = 'Email')
BEGIN
	ALTER TABLE Renters ADD Email NVARCHAR(50) NULL
END
GO

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Renters' AND COLUMN_NAME = 'EmailRenterBeforeDueDateEnable')
BEGIN
	ALTER TABLE Renters ADD EmailRenterBeforeDueDateEnable BIT NOT NULL DEFAULT(0)
END
GO
