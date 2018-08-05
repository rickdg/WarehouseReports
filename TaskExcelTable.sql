CREATE TYPE [dbo].[TaskExcelTable] AS TABLE(
	SystemTaskType_id int NULL,
	ZoneShipper int NULL,
	ZoneConsignee int NULL,
	UserTaskType nvarchar(8) NULL,
	Employee nvarchar(max) NULL,
	LoadTime datetime2(7) NULL
)