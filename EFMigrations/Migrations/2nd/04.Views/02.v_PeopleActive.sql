IF object_id('[dbo].[v_PeopleActive]') IS NULL
	EXEC ('CREATE VIEW [dbo].[v_PeopleActive] AS SELECT 1 AS Id')
GO

ALTER VIEW [dbo].[v_PeopleActive]
AS
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
		   --Below three columns added from second migration deployment
		  ,[Email]
		  ,[PrimaryPhone]
		  ,[SecondaryPhone]
	  FROM [dbo].[v_People]
	  WHERE IsActive = 1
GO
