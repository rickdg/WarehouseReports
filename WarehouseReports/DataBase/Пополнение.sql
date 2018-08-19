SELECT 3 AS SystemTaskType_id, IIF([Складское подразделение] IS NULL, 0, [Складское подразделение]) AS ZoneShipper, [Склад-получ#] AS ZoneConsignee, [Тип задачи пользователя] AS UserTaskType, [Работник] AS Employee, MIN([Время загрузки]) AS LoadTime
FROM [{Table}]
WHERE [Тип задачи системы] = 'Пополнение' AND [Работник] IS NOT NULL
GROUP BY [Складское подразделение], [Склад-получ#], [Работник], [НЗ содержимого], [Номерной знак отправителя]