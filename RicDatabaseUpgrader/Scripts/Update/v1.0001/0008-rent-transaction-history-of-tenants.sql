IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='RentArrears' AND COLUMN_NAME = 'ManualEntryDateTimeLocal')
BEGIN
	ALTER TABLE RentArrears ADD ManualEntryDateTimeLocal DATETIME NULL
END
GO

IF EXISTS(SELECT 1 FROM RentArrears WHERE Note = 'Unpaid rent: (3,500) Mar 1 - Mar 30, (5,500) Apr 1 - Apr 30' and ManualEntryDateTimeLocal IS NULL)
BEGIN
	UPDATE RentArrears
		SET ManualEntryDateTimeLocal = '2020-06-24 09:48:31.723'
			WHERE Note = 'Unpaid rent: (3,500) Mar 1 - Mar 30, (5,500) Apr 1 - Apr 30' and ManualEntryDateTimeLocal IS NULL
END
GO