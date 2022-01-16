
IF NOT EXISTS(SELECT 1 FROM Settings WHERE [key] = 'EnableDueDateAlertPushNotification')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('EnableDueDateAlertPushNotification', 'False', 'Enable Due Date Alert Push Notification')
END
GO