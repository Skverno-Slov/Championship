Create TRIGGER TRG_VendingMachine_LastMaintenanceDate
    ON [dbo].VendingMachine
    FOR DELETE, INSERT, UPDATE
    AS
    BEGIN
    SET NOCOUNT ON
    IF EXISTS(
        SELECT 1
        FROM inserted
        WHERE LastMaintenanceDate > GETDATE())
        BEGIN
            PRINT N'Дата последней проверке не может быть позже текуей даты'
            ROLLBACK TRANSACTION;
        END
    END
