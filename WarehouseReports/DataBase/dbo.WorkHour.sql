CREATE TABLE [dbo].[WorkHour] (
    [Id]          INT           NOT NULL,
    [Employee_id] INT           NOT NULL,
    [WorkDate]    DATETIME2 (0) NOT NULL,
    [Hours]       INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

