CREATE TYPE [dbo].[TypeExtraDataExcelTable] AS TABLE(
	XDate			DATETIME2(0)	NULL,
    ZoneShipper		INT				NULL,
    QtyUnloadedLPN	INT				NULL,
    QtyOrders		INT				NULL,
    AvgQtyPcs		INT				NULL
)