CREATE PROCEDURE [dbo].[LoadExtraData]
	@ExcelExtraData TypeExtraDataExcelTable READONLY
AS
	DECLARE @XDate			DATETIME2(0),
			@ZoneShipper	INT,
			@QtyUnloadedLPN	INT,
			@QtyOrders		INT,
			@AvgQtyPcs		INT,

			@TimeZoneOffset	INT

	SET @TimeZoneOffset = DATEPART(TZoffset, SYSDATETIMEOFFSET()) - 180

	DECLARE TableCursor CURSOR FOR SELECT XDate, ZoneShipper, QtyUnloadedLPN, QtyOrders, AvgQtyPcs FROM @ExcelExtraData

	OPEN TableCursor
	FETCH NEXT FROM TableCursor INTO @XDate, @ZoneShipper, @QtyUnloadedLPN, @QtyOrders, @AvgQtyPcs

	WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @XDate = CONVERT(DATETIME2(0), SWITCHOFFSET(@XDate, @TimeZoneOffset))

			INSERT INTO ExtraData(XDate, ZoneShipper, QtyUnloadedLPN, QtyOrders, AvgQtyPcs)
			VALUES (@XDate, @ZoneShipper, @QtyUnloadedLPN, @QtyOrders, @AvgQtyPcs)

			FETCH NEXT FROM TableCursor INTO @XDate, @ZoneShipper, @QtyUnloadedLPN, @QtyOrders, @AvgQtyPcs
		END
	CLOSE TableCursor
	DEALLOCATE TableCursor

	DELETE FROM ExtraData WHERE Id IN (SELECT MIN(Id)
									  FROM ExtraData
									  GROUP BY XDate, ZoneShipper, QtyUnloadedLPN, QtyOrders, AvgQtyPcs
									  HAVING COUNT(*) > 1)