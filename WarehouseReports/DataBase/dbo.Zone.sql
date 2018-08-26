CREATE TABLE [dbo].[Zone] (
    [Id]          INT        IDENTITY (1, 1) NOT NULL,
    [ZoneNum]     INT        NOT NULL,
    [MainGroup]   INT        NOT NULL,
    [CustomGroup] INT        NOT NULL,
    [UpDown]      BIT        NOT NULL,
    [PickingNorm] FLOAT (53) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

