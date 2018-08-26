CREATE TABLE [dbo].[TaskData] (
    [Id]                 INT           IDENTITY (1, 1) NOT NULL,
    [SystemTaskType_id]  INT           NOT NULL,
    [ZoneShipper]        INT           NULL,
    [RowShipper]         NVARCHAR (8)  NULL,
    [ZoneConsignee]      INT           NULL,
    [UserTaskType]       NVARCHAR (8)  NOT NULL,
    [Norm]               FLOAT (53)    NOT NULL,
    [Employee_id]        INT           NOT NULL,
    [XDate]              DATETIME2 (0) NOT NULL,
    [YearNum]            INT           NOT NULL,
    [MonthNum]           INT           NOT NULL,
    [WeekNum]            INT           NOT NULL,
    [DayNum]             INT           NOT NULL,
    [WeekdayNum]         INT           NOT NULL,
    [HourNum]            INT           NOT NULL,
    [XDateOnShifts]      DATETIME2 (0) NOT NULL,
    [YearNumOnShifts]    INT           NOT NULL,
    [MonthNumOnShifts]   INT           NOT NULL,
    [WeekNumOnShifts]    INT           NOT NULL,
    [DayNumOnShifts]     INT           NOT NULL,
    [WeekdayNumOnShifts] INT           NOT NULL,
    [GangNum]            INT           NOT NULL,
    [QtyTasks]           INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ZoneShipper_QtyTasks]
    ON [dbo].[TaskData]([SystemTaskType_id] ASC, [XDateOnShifts] ASC)
    INCLUDE([ZoneShipper], [QtyTasks]);

