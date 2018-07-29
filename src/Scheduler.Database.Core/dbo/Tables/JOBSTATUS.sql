CREATE TABLE [dbo].[JobStatus] (
    [Id]         TINYINT        IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_JobStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

