SELECT 3 AS SystemTaskType_id,
		[Складское подразделение] AS ZoneShipper,
		[Склад-получ#] AS ZoneConsignee,
		[Тип задачи пользователя] AS UserTaskType,
		[Работник] AS Employee,
		MIN([Время загрузки]) AS LoadTime
FROM [{Table}]
WHERE [Тип задачи системы] = 'Пополнение' AND [Тип задачи пользователя] IS NOT NULL
GROUP BY [Складское подразделение], [Склад-получ#], [Тип задачи пользователя], [Работник], [Загруженный НЗ]