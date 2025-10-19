DECLARE @TableName NVARCHAR(128), @SchemaName NVARCHAR(128), @SQL NVARCHAR(MAX);

DECLARE TableCursor CURSOR FOR
SELECT s.name AS SchemaName, t.name AS TableName
FROM sys.tables t
JOIN sys.schemas s ON t.schema_id = s.schema_id
JOIN sys.indexes i ON t.object_id = i.object_id
WHERE i.is_primary_key = 1 AND t.temporal_type = 0;

OPEN TableCursor;
FETCH NEXT FROM TableCursor INTO @SchemaName, @TableName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @SQL = '
    ALTER TABLE [' + @SchemaName + '].[' + @TableName + ']
    ADD 
        ValidFrom DATETIME2 GENERATED ALWAYS AS ROW START HIDDEN NOT NULL DEFAULT SYSUTCDATETIME(),
        ValidTo DATETIME2 GENERATED ALWAYS AS ROW END HIDDEN NOT NULL DEFAULT CONVERT(DATETIME2, ''9999-12-31 23:59:59.9999999''),
        PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo);

    ALTER TABLE [' + @SchemaName + '].[' + @TableName + ']
    SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [' + @SchemaName + '].[' + @TableName + '_History]));';

    PRINT 'Enabling temporal table for [' + @SchemaName + '].[' + @TableName + ']';
    EXEC sp_executesql @SQL;

    FETCH NEXT FROM TableCursor INTO @SchemaName, @TableName;
END

CLOSE TableCursor;
DEALLOCATE TableCursor;
