IF NOT EXISTS(SELECT 1 FROM Settings WHERE [key] = 'VatPH')
BEGIN
	INSERT INTO [dbo].[Settings] ([Key], [Value], [FriendlyName]) VALUES ('VatPH', '12%', 'Philipine VAT')
END
GO