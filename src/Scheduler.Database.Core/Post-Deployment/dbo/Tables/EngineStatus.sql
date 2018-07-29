SET NOCOUNT ON

SET IDENTITY_INSERT [dbo].[EngineStatus] ON

MERGE INTO [dbo].[EngineStatus] AS Target
USING (VALUES
  (1,N'StandBy')
 ,(2,N'Normal')
 ,(3,N'Paused')
 ,(4,N'Terminated')
) AS Source ([Id],[Name])
ON (Target.[Id] = Source.[Id])
WHEN MATCHED AND (
	NULLIF(Source.[Name], Target.[Name]) IS NOT NULL OR NULLIF(Target.[Name], Source.[Name]) IS NOT NULL) THEN
 UPDATE SET
  [Name] = Source.[Name]
WHEN NOT MATCHED BY TARGET THEN
 INSERT([Id],[Name])
 VALUES(Source.[Id],Source.[Name])
WHEN NOT MATCHED BY SOURCE THEN 
 DELETE
;
GO
DECLARE @mergeError int
 , @mergeCount int
SELECT @mergeError = @@ERROR, @mergeCount = @@ROWCOUNT
IF @mergeError != 0
 BEGIN
 PRINT 'ERROR OCCURRED IN MERGE FOR [dbo].[EngineStatus]. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100)); -- SQL should always return zero rows affected
 END
ELSE
 BEGIN
 PRINT '[dbo].[EngineStatus] rows affected by MERGE: ' + CAST(@mergeCount AS VARCHAR(100));
 END
GO

SET IDENTITY_INSERT [dbo].[EngineStatus] OFF
GO
SET NOCOUNT OFF
GO