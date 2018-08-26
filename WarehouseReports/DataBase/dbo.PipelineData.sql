CREATE TABLE [dbo].[PipelineData] (
    [Id]                             INT           IDENTITY (1, 1) NOT NULL,
    [XDate]                          DATETIME2 (0) NOT NULL,
    [VolumeCargo]                    FLOAT (53)    NOT NULL,
    [VolumeBox]                      FLOAT (53)    NOT NULL,
    [QtyBoxesNotPassedWeightControl] INT           NOT NULL,
    [QtyBoxesPassedWeightControl]    INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

