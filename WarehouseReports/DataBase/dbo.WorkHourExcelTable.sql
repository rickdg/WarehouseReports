CREATE TYPE [dbo].[WorkHourExcelTable] AS TABLE(
	@Employee	NVARCHAR(50),
	@WorkDate	DATETIME2(0),
	@QHours		FLOAT(53)
)