CREATE PROCEDURE [dbo].[LoadTasks]
	@ExcelTasks TaskExcelTable READONLY
AS
	DECLARE @SystemTaskType_id	INT,
			@ZoneShipper		INT,
			@ZoneConsignee		INT,
			@UserTaskType		NVARCHAR(8),
			@Employee			NVARCHAR(MAX),
			@LoadTime			DATETIME2(7),

			@Tmp_id				INT,
			@Employee_id		INT,
			@WeekdayNum			INT,
			@Time				TIME(7),
			@GangNum			INT,
			@GangName			NVARCHAR(MAX),
			@Norm				FLOAT(53)

	SET DATEFIRST 1

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

			SET @WeekdayNum = DATEPART(weekday, @LoadTime)

			SET @Time = CONVERT(TIME(0), @LoadTime)
			SELECT @GangNum = Number, @GangName = [Name] FROM Gang WHERE @Time BETWEEN StartTime AND EndTime

			SET @Norm = (SELECT Norm FROM UserTaskType WHERE [Name] = @UserTaskType)
			IF @Norm IS NULL
				SET @Norm = 0

			INSERT INTO TaskData(SystemTaskType_id, ZoneShipper, ZoneConsignee, UserTaskType, Employee_id, LoadTime, Norm,
			YearNum,
			MonthNum,
			[MonthName],
			WeekNum,
			DayNum,
			WeekdayNum,
			WeekdayName,
			HourNum,
			GangNum, GangName)
			VALUES (@SystemTaskType_id, @ZoneShipper, @ZoneConsignee, @UserTaskType, @Employee_id, @LoadTime, @Norm,
			YEAR(@LoadTime),
			MONTH(@LoadTime),
			FORMAT(@LoadTime, 'MMMM', 'ru-RU'),
			DATEPART(week, @LoadTime),
			DAY(@LoadTime),
			@WeekdayNum,
			(SELECT [Name] FROM WeekdayName Where Id = @WeekdayNum),
			DATEPART(hour, @LoadTime),
			@GangNum, @GangName)

			FETCH NEXT FROM TableCursor INTO @SystemTaskType_id, @ZoneShipper, @ZoneConsignee, @UserTaskType, @Employee, @LoadTime
		END
	CLOSE TableCursor
	DEALLOCATE TableCursor