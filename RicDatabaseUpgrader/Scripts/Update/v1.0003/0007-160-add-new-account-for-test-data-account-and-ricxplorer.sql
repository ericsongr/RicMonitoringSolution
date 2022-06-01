
IF NOT EXISTS(SELECT 1 FROM Accounts WHERE Name = 'Ricxplorer')
BEGIN
	SET IDENTITY_INSERT [dbo].[Accounts] ON 
	INSERT [dbo].[Accounts] 
		([Id], [Name], [Timezone], [IsActive], [Street], [SubUrb], 
		[State], [PostalCode], [Email], [PhoneNumber], [WebsiteUrl], 
		[FacebookUrl], [AddressLine1], [City], [DialingCode], [BusinessOwnerName], 
		[BusinessOwnerPhoneNumber], [BusinessOwnerEmail], [GeoCoordinates], 
		[CompanyFeeFailedPaymentCount], [PaymentIssueSuspensionDate], [IsSelected]) 
	VALUES (2, N'Ricxplorer', N'E. Australia Standard Time', 1, N'Port Barton, San Vicente, Palawan', N'.', N'.', N'1715', N'ericsongr@yahoo.com', N'09297233031', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1)
	SET IDENTITY_INSERT [dbo].[Accounts] OFF
END
GO

IF NOT EXISTS(SELECT 1 FROM Accounts WHERE Name = 'Test Resort')
BEGIN
	SET IDENTITY_INSERT [dbo].[Accounts] ON 
	INSERT [dbo].[Accounts] 
		([Id], [Name], [Timezone], [IsActive], [Street], [SubUrb], 
		[State], [PostalCode], [Email], [PhoneNumber], [WebsiteUrl], 
		[FacebookUrl], [AddressLine1], [City], [DialingCode], [BusinessOwnerName], 
		[BusinessOwnerPhoneNumber], [BusinessOwnerEmail], [GeoCoordinates], 
		[CompanyFeeFailedPaymentCount], [PaymentIssueSuspensionDate], [IsSelected]) 
	VALUES (3, N'Test Resort', N'E. Australia Standard Time', 1, N'Test', N'.', N'.', N'1715', N'ericsongr@yahoo.com', N'09297233031', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1)
	SET IDENTITY_INSERT [dbo].[Accounts] OFF
END
GO

