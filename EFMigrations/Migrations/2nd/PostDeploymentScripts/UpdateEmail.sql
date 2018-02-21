BEGIN TRANSACTION
BEGIN TRY
	
	UPDATE dbo.people
	SET Email= [FirstName] + '.'+[LastName]+'@xyz.com'
  
COMMIT TRANSACTION

END TRY
BEGIN CATCH

    DECLARE @ErrorNumber   INT;
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorMessage  NVARCHAR(MAX);

    SELECT
          @ErrorNumber   = ERROR_NUMBER()
        , @ErrorSeverity = ERROR_SEVERITY()
        , @ErrorMessage  = ERROR_MESSAGE();

    IF (@@TRANCOUNT > 0)
        ROLLBACK TRANSACTION

    RAISERROR(@ErrorMessage, @ErrorSeverity, 1);
END CATCH