CREATE TYPE [dbo].[TypeWorkHourExcelTable] AS TABLE(
	Employee	NVARCHAR(50)	NULL,
	WorkDate	DATETIME2(0)	NULL,
	QtyHours	FLOAT(53)		NULL
)