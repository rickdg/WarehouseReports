CREATE PROCEDURE [dbo].[LoadWorkHour]
	@ExcelWorkHour WorkHourExcelTable READONLY
AS
	DECLARE @Employee		NVARCHAR(50),
			@WorkDate		DATETIME2(0),
			@QHours			FLOAT(53),

			@Tmp_id			INT,
			@Employee_id	INT

	DECLARE TableCursor CURSOR FOR SELECT Employee, WorkDate, QHours FROM @ExcelWorkHour

	OPEN TableCursor
	FETCH NEXT FROM TableCursor INTO @Employee, @WorkDate, @QHours

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

			INSERT INTO WorkHour(Employee_id, WorkDate, QHours)
			VALUES (@Employee_id, @WorkDate, @QHours)

			FETCH NEXT FROM TableCursor INTO @Employee, @WorkDate, @QHours
		END
	CLOSE TableCursor
	DEALLOCATE TableCursor

	DELETE FROM WorkHour WHERE Id IN (SELECT MIN(Id)
									  FROM WorkHour
									  GROUP BY Employee_id, WorkDate
									  HAVING COUNT(*) > 1)