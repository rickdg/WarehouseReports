CREATE TYPE [dbo].[TypeExtraDataExcelTable] AS TABLE(
	xDate			DATETIME2(0)	NULL,
    ZoneShipper		INT				NULL,
    QtyUnloadedLPN	INT				NULL,
    QtyOrders		INT				NULL,
    AvgQtyPcs		INT				NULL
)