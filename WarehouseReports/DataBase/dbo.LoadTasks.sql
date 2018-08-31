CREATE PROCEDURE [dbo].[LoadTasks]
	@ExcelTasks TypeTasksExcelTable READONLY
AS
	DECLARE @SystemTaskType_id	INT,
			@ZoneShipper		INT,
			@RowShipper			NVARCHAR(8),
			@ZoneConsignee		INT,
			@UserTaskType		NVARCHAR(8),
			@Employee			NVARCHAR(50),
			@LoadTime			DATETIME2(0),
			@QtyTasks			INT,

			@Tmp_id				INT,
			@Employee_id		INT,
			@Time				TIME(0),
			@GangNum			INT,
			@IsPreviousDay		BIT,
			@Norm				FLOAT(53),
			@PreviousDay		DATETIME2(0),
			@TimeZoneOffset		INT

	SET DATEFIRST 1

	SET @TimeZoneOffset = DATEPART(TZoffset, SYSDATETIMEOFFSET()) - 180

	DECLARE TableCursor CURSOR FOR SELECT SystemTaskType_id, ZoneShipper, RowShipper, ZoneConsignee, UserTaskType, Employee, LoadTime, QtyTasks FROM @ExcelTasks

	OPEN TableCursor
	FETCH NEXT FROM TableCursor INTO @SystemTaskType_id, @ZoneShipper, @RowShipper, @ZoneConsignee, @UserTaskType, @Employee, @LoadTime, @QtyTasks

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

			SET @LoadTime = CONVERT(DATETIME2(0), SWITCHOFFSET(@LoadTime, @TimeZoneOffset))
			SET @Time = CONVERT(TIME(0), @LoadTime)
			SELECT @GangNum = Number, @IsPreviousDay = PreviousDay FROM Gang WHERE @Time BETWEEN StartTime AND EndTime

			SET @Norm = (SELECT Norm FROM UserTaskType WHERE [Name] = @UserTaskType)
			IF @Norm IS NULL
				SET @Norm = 0

			IF @IsPreviousDay = 1
				SET @PreviousDay =	DATEADD(DAY, -1, @LoadTime)
			ELSE
				SET @PreviousDay =	@LoadTime

			INSERT INTO TaskData(SystemTaskType_id, ZoneShipper, RowShipper, ZoneConsignee, UserTaskType, Norm, Employee_id,
			XDate,
			YearNum,
			MonthNum,
			WeekNum,
			DayNum,
			WeekdayNum,
			HourNum,
			XDateOnShifts,
			YearNumOnShifts,
			MonthNumOnShifts,
			WeekNumOnShifts,
			DayNumOnShifts,
			WeekdayNumOnShifts,
			GangNum,
			QtyTasks)
			VALUES (@SystemTaskType_id, @ZoneShipper, @RowShipper, @ZoneConsignee, @UserTaskType, @Norm, @Employee_id,
			CONVERT(date, @LoadTime),
			YEAR(@LoadTime),
			MONTH(@LoadTime),
			DATEPART(WEEK, @LoadTime),
			DAY(@LoadTime),
			DATEPART(WEEKDAY, @LoadTime),
			DATEPART(HOUR, @LoadTime),
			CONVERT(date, @PreviousDay),
			YEAR(@PreviousDay),
			MONTH(@PreviousDay),
			DATEPART(WEEK, @PreviousDay),
			DAY(@PreviousDay),
			DATEPART(WEEKDAY, @PreviousDay),
			@GangNum,
			@QtyTasks)

			FETCH NEXT FROM TableCursor INTO @SystemTaskType_id, @ZoneShipper, @RowShipper, @ZoneConsignee, @UserTaskType, @Employee, @LoadTime, @QtyTasks
		END
	CLOSE TableCursor
	DEALLOCATE TableCursor

	DELETE FROM TaskData WHERE Id IN (SELECT MIN(QtyTasks)
									  FROM TaskData
									  GROUP BY SystemTaskType_id, ZoneShipper, RowShipper, ZoneConsignee, UserTaskType, Employee_id, XDate, HourNum
									  HAVING COUNT(*) > 1)