IF NOT EXISTS(SELECT 1 FROM Settings WHERE [key] = 'EnableIsBatchCompletedPushNotification')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('EnableIsBatchCompletedPushNotification', 'False', 'Enable Batch Process Completed Push Notification')
END
GO

IF NOT EXISTS(SELECT 1 FROM Settings WHERE [key] = 'EnableIncomingDueDateAlertPushNotification')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('EnableIncomingDueDateAlertPushNotification', 'False', 'Enable Incoming Due Date Alert Push Notification')
END
GO