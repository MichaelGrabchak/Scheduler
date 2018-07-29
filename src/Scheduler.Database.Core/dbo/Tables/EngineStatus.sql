CREATE TABLE [dbo].[EngineStatus] (
    [Id]         SMALLINT       IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_EngineStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

