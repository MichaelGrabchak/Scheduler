SET NOCOUNT ON

MERGE INTO [dbo].[Instance] AS Target
USING (VALUES
  (N'00000000-0000-0000-0000-000000000000')
) AS Source ([Id])
ON (Target.[Id] = Source.[Id])
WHEN NOT MATCHED BY TARGET THEN
 INSERT([Id])
 VALUES(Source.[Id])
WHEN NOT MATCHED BY SOURCE THEN 
 DELETE
;
GO
DECLARE @mergeError int
 , @mergeCount int
SELECT @mergeError = @@ERROR, @mergeCount = @@ROWCOUNT
IF @mergeError != 0
 BEGIN
 PRINT 'ERROR OCCURRED IN MERGE FOR [dbo].[Instance]. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100)); -- SQL should always return zero rows affected
 END
ELSE
 BEGIN
 PRINT '[dbo].[Instance] rows affected by MERGE: ' + CAST(@mergeCount AS VARCHAR(100));
 END
GO

SET NOCOUNT OFF
GO