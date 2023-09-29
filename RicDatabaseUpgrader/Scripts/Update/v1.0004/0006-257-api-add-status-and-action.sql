IF EXISTS(select 1 from INFORMATION_SCHEMA.columns where table_name = 'ToolsInventory' AND COLUMN_NAME = 'Status' AND DATA_TYPE = 'varchar')
BEGIN
	ALTER TABLE ToolsInventory ALTER COLUMN Status INT NOT NULL
	
	ALTER TABLE [dbo].[ToolsInventory]  WITH CHECK ADD  CONSTRAINT [FK_ToolsInventory_LookupTypeItem_Status] FOREIGN KEY([Status])
	REFERENCES [dbo].[LookupTypeItems] ([Id])

	ALTER TABLE [dbo].[ToolsInventory] CHECK CONSTRAINT [FK_ToolsInventory_LookupTypeItem_Status]

END
GO

IF EXISTS(select 1 from INFORMATION_SCHEMA.columns where table_name = 'ToolsInventory' AND COLUMN_NAME = 'Action' AND DATA_TYPE = 'varchar')
BEGIN
	ALTER TABLE ToolsInventory ALTER COLUMN Action INT NOT NULL

	ALTER TABLE [dbo].[ToolsInventory]  WITH CHECK ADD  CONSTRAINT [FK_ToolsInventory_LookupTypeItem_Action] FOREIGN KEY([Action])
	REFERENCES [dbo].[LookupTypeItems] ([Id])

	ALTER TABLE [dbo].[ToolsInventory] CHECK CONSTRAINT [FK_ToolsInventory_LookupTypeItem_Action]
END
GO