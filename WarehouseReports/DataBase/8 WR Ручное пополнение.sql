SELECT 8 AS SystemTaskType_id,
		[Складское подразделение] AS ZoneShipper,
		NULL AS RowShipper,
		[Склад-получ#] AS ZoneConsignee,
		[Тип задачи пользователя] AS UserTaskType,
		[Работник] AS Employee,
		MIN([Время загрузки]) AS LoadTime
FROM [{table}]
WHERE [Тип задачи системы] = 'Перенос заказа на перемещение' AND [Тип задачи пользователя] IS NOT NULL
GROUP BY [Складское подразделение], [Склад-получ#], [Тип задачи пользователя], [Работник], [Загруженный НЗ]