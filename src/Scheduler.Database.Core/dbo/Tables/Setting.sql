CREATE TABLE [dbo].[Setting] (
    [InstanceId] UNIQUEIDENTIFIER NOT NULL,
    [Key]        NVARCHAR (150)   NOT NULL,
    [Value]      NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([InstanceId] ASC, [Key] ASC),
    CONSTRAINT [FK_Setting_Instance] FOREIGN KEY ([InstanceId]) REFERENCES [dbo].[Instance] ([Id])
);



