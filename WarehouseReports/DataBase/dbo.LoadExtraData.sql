CREATE PROCEDURE [dbo].[LoadExtraData]
	@ExcelExtraData TypeExtraDataExcelTable READONLY
AS
	DECLARE @xDate			DATETIME2(0),
			@ZoneShipper	INT,
			@QtyUnloadedLPN	INT,
			@QtyOrders		INT,
			@AvgQtyPcs		INT

	DECLARE TableCursor CURSOR FOR SELECT xDate, ZoneShipper, QtyUnloadedLPN, QtyOrders, AvgQtyPcs FROM @ExcelExtraData

	OPEN TableCursor
	FETCH NEXT FROM TableCursor INTO @xDate, @ZoneShipper, @QtyUnloadedLPN, @QtyOrders, @AvgQtyPcs

	WHILE @@FETCH_STATUS = 0
		BEGIN
			INSERT INTO ExtraData(xDate, ZoneShipper, QtyUnloadedLPN, QtyOrders, AvgQtyPcs)
			VALUES (@xDate, @ZoneShipper, @QtyUnloadedLPN, @QtyOrders, @AvgQtyPcs)

			FETCH NEXT FROM TableCursor INTO @xDate, @ZoneShipper, @QtyUnloadedLPN, @QtyOrders, @AvgQtyPcs
		END
	CLOSE TableCursor
	DEALLOCATE TableCursor

	DELETE FROM ExtraData WHERE Id IN (SELECT MIN(Id)
									  FROM ExtraData
									  GROUP BY xDate, ZoneShipper, QtyUnloadedLPN, QtyOrders, AvgQtyPcs
									  HAVING COUNT(*) > 1)