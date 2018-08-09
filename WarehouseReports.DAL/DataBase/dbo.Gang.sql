CREATE TABLE [dbo].[Gang] (
    [Id]          INT      IDENTITY (1, 1) NOT NULL,
    [Number]      INT      NOT NULL,
    [StartTime]   TIME (0) NOT NULL,
    [EndTime]     TIME (0) NOT NULL,
    [PreviousDay] BIT      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

