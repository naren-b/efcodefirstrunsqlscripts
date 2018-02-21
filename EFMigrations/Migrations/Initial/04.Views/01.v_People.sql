IF object_id('[dbo].[v_People]') IS NULL
	EXEC ('CREATE VIEW [dbo].[v_People] AS SELECT 1 AS Id')
GO

ALTER VIEW [dbo].[v_People]
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
	  FROM [dbo].[People]
GO
