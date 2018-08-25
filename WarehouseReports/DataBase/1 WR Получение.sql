SELECT SystemTaskType_id, ZoneShipper, ZoneConsignee, UserTaskType, Employee, MIN(LoadTime) AS LoadTime, COUNT(*) AS QtyTasks
FROM (	SELECT 1 AS SystemTaskType_id,
				NULL AS ZoneShipper,
				0 AS ZoneConsignee,
				'A000' AS UserTaskType,
				[Получатель] AS Employee,
				MIN([Дата]) AS LoadTime
		FROM [{table}]
		WHERE [Тип транзакции] = 'Получить' AND [Номерной знак переноса] IS NOT NULL
		GROUP BY [Получатель], [Номерной знак переноса]) G
GROUP BY SystemTaskType_id, ZoneShipper, ZoneConsignee, UserTaskType, Employee, FORMAT(LoadTime, 'Short Date'), HOUR(LoadTime)