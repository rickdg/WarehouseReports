SELECT 2 AS SystemTaskType_id,
		IIF([Складское подразделение] IS NULL, 0, [Складское подразделение]) AS ZoneShipper,
		NULL AS RowShipper,
		[Склад-получ#] AS ZoneConsignee,
		'W' & ZoneShipper & 'C' & [Склад-получ#] AS UserTaskType,
		[Работник] AS Employee,
		MIN([Время загрузки]) AS LoadTime
FROM [{table}]
WHERE [Тип задачи системы] = 'Размещение'
GROUP BY [Складское подразделение], [Склад-получ#], [Работник], [НЗ содержимого], [Номерной знак отправителя]