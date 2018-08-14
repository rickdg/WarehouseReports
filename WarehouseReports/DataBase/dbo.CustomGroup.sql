CREATE TABLE [dbo].[CustomGroup] (
    [Id]          INT        IDENTITY (1, 1) NOT NULL,
    [Group]       INT        NOT NULL,
    [PickingNorm] FLOAT (53) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

