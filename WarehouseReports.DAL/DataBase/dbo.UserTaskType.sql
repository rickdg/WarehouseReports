CREATE TABLE [dbo].[UserTaskType] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (8) NOT NULL,
    [Norm] FLOAT (53)   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

