SELECT 5 AS SystemTaskType_id,
		[Складское подразделение] AS ZoneShipper,
		NULL AS RowShipper,
		[Склад-получ#] AS ZoneConsignee,
		IIF([Тип задачи пользователя] IS NULL, 'M' & [Складское подразделение] & 'C' & [Склад-получ#], [Тип задачи пользователя]) AS UserTaskType,
		[Работник] AS Employee,
		MIN([Время загрузки]) AS LoadTime
FROM [{table}]
WHERE [Тип задачи системы] IN ('Перенос заказа на перемещение', 'Пополнение', 'Размещение') AND [Складское подразделение] IS NOT NULL
GROUP BY [Складское подразделение], [Склад-получ#], [Тип задачи пользователя], [Работник], [НЗ содержимого], [Номерной знак отправителя], [Загруженный НЗ]