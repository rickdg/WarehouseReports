SELECT 1 AS SystemTaskType_id,
		NULL AS ZoneShipper,
		0 AS ZoneConsignee,
		'A000' AS UserTaskType,
		[Получатель] AS Employee,
		MIN([Дата]) AS LoadTime
FROM [{Table}]
WHERE [Тип транзакции] = 'Получить' AND [Номерной знак переноса] IS NOT NULL
GROUP BY [Получатель], [Номерной знак переноса]