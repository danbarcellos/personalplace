DECLARE @sql NVARCHAR(MAX), @cr CHAR(2);
SET @sql = N'';
SET @cr = CHAR(13) + CHAR(10);
SELECT @sql = @sql + CHAR(13) + CHAR(10) + 'EXEC sp_rename '''
      + REPLACE(c.name, '''', '''''') + ''', ''PK_'
      + REPLACE(OBJECT_NAME(c.parent_object_id), '''', '') + ''', ''OBJECT'';'
FROM sys.key_constraints AS c
WHERE c.type = 'PK'
AND c.name <> 'PK_' + REPLACE(OBJECT_NAME(c.parent_object_id), '''', '')
AND OBJECTPROPERTY(c.parent_object_id, 'IsMsShipped') = 0;
SELECT @sql = @sql + @cr + N'EXEC sp_rename '''
       + CASE is_unique_constraint WHEN 0 THEN QUOTENAME(REPLACE(OBJECT_NAME(i.[object_id]), '''', '''''')) + '.' ELSE '' END
       + QUOTENAME(REPLACE(i.name, '''', '''''')) + ''', '''
       + CASE is_unique_constraint WHEN 1 THEN 'UQ_' ELSE 'IX_'
       + CASE is_unique WHEN 1 THEN 'U_'  ELSE '' END
       END + CASE has_filter WHEN 1 THEN 'F_'  ELSE '' END
       + REPLACE(OBJECT_NAME(i.[object_id]), '''', '')
       + '_' + STUFF((SELECT '_' + REPLACE(c.name, '''', '')
                        FROM sys.columns AS c
                             INNER JOIN sys.index_columns AS ic
                                ON ic.column_id = c.column_id
                                AND ic.[object_id] = c.[object_id]
                       WHERE ic.[object_id] = i.[object_id]
                             AND ic.index_id = i.index_id
                             AND is_included_column = 0
				       ORDER BY ic.index_column_id FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, '')
		+''', ''OBJECT'';'
        FROM sys.indexes AS i
        WHERE index_id > 0
		      AND is_primary_key = 0
			  AND type IN (1,2)
			  AND is_unique_constraint = 1
        AND OBJECTPROPERTY(i.[object_id], 'IsMsShipped') = 0;
EXEC sp_executesql @sql;