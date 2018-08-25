CREATE TABLE [dbo].[WorkHour] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Employee_id] INT           NOT NULL,
    [WorkDate]    DATETIME2 (0) NOT NULL,
    [QtyHours]    INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

