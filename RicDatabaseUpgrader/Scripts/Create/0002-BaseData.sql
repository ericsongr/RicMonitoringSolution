
--LookupTypes
SET IDENTITY_INSERT LookupTypes ON
INSERT INTO LookupTypes(Id, Name) VALUES(1, 'Ages')
SET IDENTITY_INSERT LookupTypes OFF

--LookupTypeItems
SET IDENTITY_INSERT LookupTypeItems ON
INSERT INTO LookupTypeItems(Id, Description, IsActive, LookupTypeId) VALUES(1, 'Adult', 1, 1)
INSERT INTO LookupTypeItems(Id, Description, IsActive, LookupTypeId) VALUES(2, 'Children', 1, 1)
INSERT INTO LookupTypeItems(Id, Description, IsActive, LookupTypeId) VALUES(3, 'Infant', 1, 1)
SET IDENTITY_INSERT LookupTypeItems OFF

--LookupTypeItems
SET IDENTITY_INSERT Settings ON
INSERT INTO Settings(Id, [Key], Value, FriendlyName) VALUES(1, 'TenantGracePeriod', '10', 'Tenant''s Grace Period')
SET IDENTITY_INSERT Settings OFF