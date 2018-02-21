-- DROP and CREATE the procedure will loose the security settings.
IF object_id('[dbo].[svf_FunctionName]') IS NULL
    EXEC ('CREATE function [dbo].[svf_FunctionName]() RETURNS DATETIME AS  BEGIN RETURN getdate() END')
GO

ALTER FUNCTION [dbo].[svf_FunctionName]()
	RETURNS DATETIME
	AS
	BEGIN
		DECLARE @Result DATETIME;
		RETURN @Result;
	END
GO
