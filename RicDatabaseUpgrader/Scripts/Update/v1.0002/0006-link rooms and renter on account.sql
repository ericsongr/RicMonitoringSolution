
IF NOT EXISTS(select 1 from accounts where Name = 'Casa Filipina Estate')
BEGIN
	INSERT [dbo].[Accounts] ([Name], [Timezone], [IsActive], [Street], [SubUrb], [State], [PostalCode], [Email], [PhoneNumber], [WebsiteUrl], [FacebookUrl], [AddressLine1], [City], [DialingCode], [BusinessOwnerName], [BusinessOwnerPhoneNumber], [BusinessOwnerEmail], [GeoCoordinates], [CompanyFeeFailedPaymentCount], [PaymentIssueSuspensionDate]) VALUES (N'Casa Filipina Estate', N'E. Australia Standard Time', 1, N'L1 B17 Onyx St Casa Filipina, Raymond Ville, Brgy San Antonio, Paranaque City', N'.', N'.', N'1715', N'ericsongr@yahoo.com', N'09297233031', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
END
GO

--ROOMS
IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rooms' AND COLUMN_nAME = 'AccountId')
BEGIN
	ALTER TABLE Rooms ADD AccountId INT NULL

	ALTER TABLE [dbo].[Rooms]  WITH CHECK ADD  CONSTRAINT [FK_Rooms_Accounts_Id] FOREIGN KEY(AccountId)
	REFERENCES [dbo].[Accounts] ([Id])
	ON DELETE CASCADE
END
GO

IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Rooms' AND COLUMN_nAME = 'AccountId')
BEGIN
	DECLARE @accountId INT
	
	SELECT TOP 1 @accountId = ID FROM Accounts ORDER BY id DESC

	UPDATE Rooms SET AccountId = @accountId

	ALTER TABLE Rooms ALTER COLUMN AccountId INT NOT NULL 
END
GO