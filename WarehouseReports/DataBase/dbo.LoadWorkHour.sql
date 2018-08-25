CREATE PROCEDURE [dbo].[LoadWorkHour]
	@ExcelWorkHour TypeWorkHourExcelTable READONLY
AS
	DECLARE @Employee		NVARCHAR(50),
			@WorkDate		DATETIME2(0),
			@QtyHours		FLOAT(53),

			@Tmp_id			INT,
			@Employee_id	INT

	DECLARE TableCursor CURSOR FOR SELECT Employee, WorkDate, QtyHours FROM @ExcelWorkHour

	OPEN TableCursor
	FETCH NEXT FROM TableCursor INTO @Employee, @WorkDate, @QtyHours

	WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @Tmp_id = (SELECT Id FROM Employee WHERE [Name] = @Employee)
			IF @Tmp_id IS NULL
				BEGIN
					INSERT INTO Employee([Name]) VALUES (@Employee)
					SET @Employee_id = @@IDENTITY
				END
			ELSE
				SET @Employee_id = @Tmp_id

			INSERT INTO WorkHour(Employee_id, WorkDate, QtyHours)
			VALUES (@Employee_id, @WorkDate, @QtyHours)

			FETCH NEXT FROM TableCursor INTO @Employee, @WorkDate, @QtyHours
		END
	CLOSE TableCursor
	DEALLOCATE TableCursor

	DELETE FROM WorkHour WHERE Id IN (SELECT MIN(Id)
									  FROM WorkHour
									  GROUP BY Employee_id, WorkDate
									  HAVING COUNT(*) > 1)