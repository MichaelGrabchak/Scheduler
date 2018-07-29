CREATE TABLE [dbo].[JobDetail] (
    [Id]             INT                IDENTITY (1, 1) NOT NULL,
    [JobName]        NVARCHAR (255)     NOT NULL,
    [JobGroup]       NVARCHAR (255)     NOT NULL,
    [JobDescription] NVARCHAR (MAX)     NULL,
    [JobSchedule]    NVARCHAR (100)     NULL,
    [JobLastRunTime] DATETIMEOFFSET (7) NULL,
    [JobNextRunTime] DATETIMEOFFSET (7) NULL,
    [StatusId]       TINYINT            NOT NULL,
    [InstanceId]     UNIQUEIDENTIFIER   NOT NULL,
    CONSTRAINT [PK_JobDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_JobDetail_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Instance] ([Id]),
    CONSTRAINT [FK_JobDetail_JobStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[JobStatus] ([Id])
);

