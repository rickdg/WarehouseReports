CREATE TABLE [dbo].[ExtraData] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [XDate]          DATETIME2 (0) NOT NULL,
    [ZoneShipper]    INT           NULL,
    [QtyUnloadedLPN] INT           NOT NULL,
    [QtyOrders]      INT           NOT NULL,
    [AvgQtyPcs]      INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

