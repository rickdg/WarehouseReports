CREATE TYPE [dbo].[TypeTasksExcelTable] AS TABLE(
	SystemTaskType_id	INT				NULL,
	ZoneShipper			INT				NULL,
	RowShipper			NVARCHAR(8)		NULL,
	ZoneConsignee		INT				NULL,
	UserTaskType		NVARCHAR(8)		NULL,
	Employee			NVARCHAR(50)	NULL,
	LoadTime			DATETIME2(0)	NULL,
	QtyTasks			INT				NULL
)