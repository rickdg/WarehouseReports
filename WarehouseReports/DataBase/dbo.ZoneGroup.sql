﻿CREATE TABLE [dbo].[ZoneGroup] (
    [Id]          INT        IDENTITY (1, 1) NOT NULL,
    [Zone]        INT        NOT NULL,
    [GroupA]      INT        NOT NULL,
    [GroupB]      INT        NOT NULL,
    [UpDown]      BIT        NOT NULL,
    [PickingNorm] FLOAT (53) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

