SELECT 6 AS SystemTaskType_id,
		900 AS ZoneShipper,
		NULL AS RowShipper,
		900 AS ZoneConsignee,
		'L900' AS UserTaskType,
		[Наименование сотрудника] AS Employee,
		[Дата] AS LoadTime,
		COUNT(*) AS QtyTasks
FROM [{table}]
GROUP BY [Наименование сотрудника], [Дата]