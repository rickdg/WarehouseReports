CREATE PROCEDURE [dbo].[DeleteTasks]
	@StartDate	DATETIME2(0),
	@EndDate	DATETIME2(0)
AS
	DELETE FROM TaskData
	WHERE XDate BETWEEN @StartDate AND @EndDate