IF object_id('[dbo].[v_PeopleActive]') IS NULL
	EXEC ('CREATE VIEW [dbo].[v_PeopleActive] AS SELECT 1 AS Id')
GO

ALTER VIEW [dbo].[v_PeopleActive]
AS

--This view created from initial migration
	SELECT [Id]
		  ,[FirstName]
		  ,[MiddleName]
		  ,[LastName]
		  ,[DateOfBirth]
		  ,[IsActive]
		  ,[CreatedBy]
		  ,[UpdatedBy]
		  ,[DateCreated]
		  ,[DateUpdated]
	  FROM [dbo].[v_People]
	  WHERE IsActive = 1
GO
