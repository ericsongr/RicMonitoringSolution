IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'AspNetUsers' AND COLUMN_NAME = 'IsIncomingDueDatePushNotification')
BEGIN
	ALTER TABLE AspNetUsers ADD IsIncomingDueDatePushNotification BIT NOT NULL DEFAULT(0)
END
GO

IF NOT EXISTS(SELECT 1 FROM Settings WHERE [key] = 'EnableIncomingDueDateAlertPushNotification')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('EnableIncomingDueDateAlertPushNotification', 'False', 'Enable Incoming Due Date Alert Push Notification')
END
GO