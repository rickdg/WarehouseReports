CREATE TYPE [dbo].[TaskExcelTable] AS TABLE(
	SystemTaskType_id	INT				NULL,
	ZoneShipper			INT				NULL,
	ZoneConsignee		INT				NULL,
	UserTaskType		nvarchar(8)		NULL,
	Employee			nvarchar(50)	NULL,
	LoadTime			datetime2(0)	NULL,
	QtyTasks			INT				NULL
)