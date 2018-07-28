SET NOCOUNT ON

MERGE INTO [dbo].[SCHEDULERINSTANCESETTING] AS Target
USING (VALUES
  (N'5AAF9FD7-5BC6-4C49-A7BD-182966A34D4C',N'Engine:IsImmediateStartEnabled',N'false')
 ,(N'5AAF9FD7-5BC6-4C49-A7BD-182966A34D4C',N'Engine:IsJobsDirectoryTrackingEnabled',N'true')
 ,(N'5AAF9FD7-5BC6-4C49-A7BD-182966A34D4C',N'Engine:JobsDirectory',NULL)
 ,(N'98C1A030-039D-4EE3-9CCA-640B5F47848A',N'Engine:IsImmediateStartEnabled',N'true')
 ,(N'98C1A030-039D-4EE3-9CCA-640B5F47848A',N'Engine:IsJobsDirectoryTrackingEnabled',N'true')
 ,(N'98C1A030-039D-4EE3-9CCA-640B5F47848A',N'Engine:JobsDirectory',N'E:\Development\Sources\Scheduler\src\Jobs')
) AS Source ([INSTANCEID],[KEY],[VALUE])
ON (Target.[INSTANCEID] = Source.[INSTANCEID] AND Target.[KEY] = Source.[KEY])
WHEN MATCHED AND (
	NULLIF(Source.[VALUE], Target.[VALUE]) IS NOT NULL OR NULLIF(Target.[VALUE], Source.[VALUE]) IS NOT NULL) THEN
 UPDATE SET
  [VALUE] = Source.[VALUE]
WHEN NOT MATCHED BY TARGET THEN
 INSERT([INSTANCEID],[KEY],[VALUE])
 VALUES(Source.[INSTANCEID],Source.[KEY],Source.[VALUE])
WHEN NOT MATCHED BY SOURCE THEN 
 DELETE
;
GO
DECLARE @mergeError int
 , @mergeCount int
SELECT @mergeError = @@ERROR, @mergeCount = @@ROWCOUNT
IF @mergeError != 0
 BEGIN
 PRINT 'ERROR OCCURRED IN MERGE FOR [dbo].[SCHEDULERINSTANCESETTING]. Rows affected: ' + CAST(@mergeCount AS VARCHAR(100)); -- SQL should always return zero rows affected
 END
ELSE
 BEGIN
 PRINT '[dbo].[SCHEDULERINSTANCESETTING] rows affected by MERGE: ' + CAST(@mergeCount AS VARCHAR(100));
 END
GO

SET NOCOUNT OFF
GO