﻿CREATE PROCEDURE [dbo].[LoadTasks]
	@ExcelTasks TaskExcelTable READONLY
AS
	DECLARE @SystemTaskType_id	INT,
			@ZoneShipper		INT,
			@ZoneConsignee		INT,
			@UserTaskType		NVARCHAR(8),
			@Employee			NVARCHAR(MAX),
			@LoadTime			DATETIME2(0),

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

	DECLARE TableCursor CURSOR FOR SELECT SystemTaskType_id, ZoneShipper, ZoneConsignee, UserTaskType, Employee, LoadTime FROM @ExcelTasks

	OPEN TableCursor
	FETCH NEXT FROM TableCursor INTO @SystemTaskType_id, @ZoneShipper, @ZoneConsignee, @UserTaskType, @Employee, @LoadTime

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

			INSERT INTO TaskData(SystemTaskType_id, ZoneShipper, ZoneConsignee, UserTaskType, Norm, Employee_id,
			TaskDate,
			YearNum,
			MonthNum,
			WeekNum,
			DayNum,
			WeekdayNum,
			HourNum,
			TaskDateOnShifts,
			YearNumOnShifts,
			MonthNumOnShifts,
			WeekNumOnShifts,
			DayNumOnShifts,
			WeekdayNumOnShifts,
			GangNum)
			VALUES (@SystemTaskType_id, @ZoneShipper, @ZoneConsignee, @UserTaskType, @Norm, @Employee_id,
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
			@GangNum)

			FETCH NEXT FROM TableCursor INTO @SystemTaskType_id, @ZoneShipper, @ZoneConsignee, @UserTaskType, @Employee, @LoadTime
		END
	CLOSE TableCursor
	DEALLOCATE TableCursor