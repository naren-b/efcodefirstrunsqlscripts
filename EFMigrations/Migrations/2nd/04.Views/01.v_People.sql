IF object_id('[dbo].[v_People]') IS NULL
	EXEC ('CREATE VIEW [dbo].[v_People] AS SELECT 1 AS Id')
GO

ALTER VIEW [dbo].[v_People]
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
	  FROM [dbo].[People]
GO
