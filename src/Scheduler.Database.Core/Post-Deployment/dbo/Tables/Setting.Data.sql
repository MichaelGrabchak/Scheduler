SET NOCOUNT ON

MERGE INTO [dbo].[Setting] AS Target
USING (VALUES
  (N'00000000-0000-0000-0000-000000000000',N'Application:Name',N'Default Scheduler Management Console')
 ,(N'00000000-0000-0000-0000-000000000000',N'Engine:IsImmediateStartEnabled',N'true')
 ,(N'00000000-0000-0000-0000-000000000000',N'Engine:IsJobsDirectoryTrackingEnabled',N'true')
 ,(N'00000000-0000-0000-0000-000000000000',N'Engine:JobsDirectory',N'C:\Jobs')
) AS Source ([InstanceId],[Key],[Value])
ON (Target.[InstanceId] = Source.[InstanceId] AND Target.[Key] = Source.[Key])
WHEN MATCHED AND (
	NULLIF(Source.[Value], Target.[Value]) IS NOT NULL OR NULLIF(Target.[Value], Source.[Value]) IS NOT NULL) THEN
 UPDATE SET
  [Value] = Source.[Value]
WHEN NOT MATCHED BY TARGET THEN
 INSERT([InstanceId],[Key],[Value])
 VALUES(Source.[InstanceId],Source.[Key],Source.[Value])
WHEN NOT MATCHED BY SOURCE THEN 
 DELETE
;
GO
DECLARE @mergeError int
 , @mergeCount int
SELECT @mergeError = @@ERROR, @mergeCount = @@ROWCOUNT
IF @mergeError != 0
 BEGIN
 PRINT 'ERROR OCCURRED IN MERGE FOR [dbo].[Setting]. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100)); -- SQL should always return zero rows affected
 END
ELSE
 BEGIN
 PRINT '[dbo].[Setting] rows affected by MERGE: ' + CAST(@mergeCount AS VARCHAR(100));
 END
GO

SET NOCOUNT OFF
GO