CREATE TABLE [dbo].[Balances] (
    [Id] uniqueidentifier primary key not null,
    [CorrelationId] uniqueidentifier not null,
    [Balance] int not null,
    [CreateTime] datetimeoffset default sysdatetimeoffset() not null,
    [UpdateTime] datetimeoffset default sysdatetimeoffset() not null
    );

CREATE INDEX IX_Balances_CorrelationId
    ON Balances (CorrelationId)
    GO

CREATE TRIGGER T_Balances_UpdateTime_Automatic
    ON Balances
    AFTER UPDATE AS
        UPDATE [Balances]
        SET [UpdateTime] = sysdatetimeoffset()
        WHERE Id in (SELECT DISTINCT Id FROM Inserted)
    GO

CREATE TABLE [dbo].[Balances_H]
    [Action] nvarchar(10),
    [ActionTime] datetimeoffset,
    [Id] uniqueidentifier not null,
    [CorrelationId] uniqueidentifier not null,
    [Balance] int not null,
    [CreateTime] datetimeoffset default sysdatetimeoffset() not null,
    [UpdateTime] datetimeoffset default sysdatetimeoffset() not null
    );

CREATE UNIQUE INDEX IX_Balances_H ON Balances_H (Id, ActionTime)
    GO

CREATE TRIGGER T_Insert_Balances
    ON Balances_H
    FOR INSERT AS 
    BEGIN
        SET NOCOUNT ON 
        INSERT INTO Balances_H SELECT 'Insert', getDate(), * FROM Inserted
    END 
    GO

CREATE TRIGGER T_Update_Balances
    ON Balances_H
    FOR UPDATE AS
    BEGIN
        SET NOCOUNT ON 
        INSERT INTO Balances_H SELECT 'Update', getDate(), * FROM Inserted
    END
    GO

CREATE TRIGGER T_Delete_Balances
    ON Balances_H
    FOR DELETE AS
    BEGIN
        SET NOCOUNT ON 
        INSERT INTO Balances_H SELECT 'Delete', getDate(), * FROM Inserted
    END
    GO