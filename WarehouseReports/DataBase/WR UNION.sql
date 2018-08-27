SELECT SystemTaskType_id, ZoneShipper, RowShipper, ZoneConsignee, UserTaskType, Employee, MIN(LoadTime) AS LoadTime, COUNT(*) AS QtyTasks
FROM (	SELECT 2 AS SystemTaskType_id,
				IIF([Складское подразделение] IS NULL, 0, [Складское подразделение]) AS ZoneShipper,
				NULL AS RowShipper,
				[Склад-получ#] AS ZoneConsignee,
				'W' & ZoneShipper & 'C' & [Склад-получ#] AS UserTaskType,
				[Работник] AS Employee,
				MIN([Время загрузки]) AS LoadTime
		FROM [{table}]
		WHERE [Тип задачи системы] = 'Размещение' {Placement}
		GROUP BY [Складское подразделение], [Склад-получ#], [Работник], [НЗ содержимого], [Номерной знак отправителя]
		
		UNION ALL
		
		SELECT 3 AS SystemTaskType_id,
				[Складское подразделение] AS ZoneShipper,
				NULL AS RowShipper,
				[Склад-получ#] AS ZoneConsignee,
				[Тип задачи пользователя] AS UserTaskType,
				[Работник] AS Employee,
				MIN([Время загрузки]) AS LoadTime
		FROM [{table}]
		WHERE [Тип задачи системы] = 'Пополнение' AND [Тип задачи пользователя] IS NOT NULL {Resupply}
		GROUP BY [Складское подразделение], [Склад-получ#], [Тип задачи пользователя], [Работник], [Загруженный НЗ]
		
		UNION ALL
		
		SELECT 4 AS SystemTaskType_id,
				[Складское подразделение] AS ZoneShipper,
				NULL AS RowShipper,
				[Склад-получ#] AS ZoneConsignee,
				IIF([Тип задачи пользователя] IS NULL, 'M' & [Складское подразделение] & 'C' & [Склад-получ#], [Тип задачи пользователя]) AS UserTaskType,
				[Работник] AS Employee,
				MIN([Время загрузки]) AS LoadTime
		FROM [{table}]
		WHERE [Тип задачи системы] IN ('Перенос заказа на перемещение', 'Пополнение', 'Размещение') AND [Складское подразделение] IS NOT NULL {Movement}
		GROUP BY [Складское подразделение], [Склад-получ#], [Тип задачи пользователя], [Работник], [НЗ содержимого], [Номерной знак отправителя], [Загруженный НЗ]
		
		UNION ALL
		
		SELECT 5 AS SystemTaskType_id,
				[Складское подразделение] AS ZoneShipper,
				IIF([Складское подразделение] = 520, LEFT([Складское место], INSTR([Складское место], '.') - 1), NULL) AS RowShipper,
				[Склад-получ#] AS ZoneConsignee,
				[Тип задачи пользователя] AS UserTaskType,
				[Работник] AS Employee,
				MIN([Время загрузки]) AS LoadTime
		FROM [{table}]
		WHERE [Тип задачи системы] = 'Отбор' AND [План/задача] = 'Независимая задача' AND [Тип задачи пользователя] IS NOT NULL
		GROUP BY [Заголовок источника], [Номер строки], [Позиция], [Складское подразделение], [Складское место], [Склад-получ#], [СМ-получатель], [Тип задачи пользователя], [Работник], [Назначенное время]
		
		UNION ALL
		
		SELECT 5 AS SystemTaskType_id,
				[Складское подразделение] AS ZoneShipper,
				IIF([Складское подразделение] = 520, LEFT([Складское место], INSTR([Складское место], '.') - 1), NULL) AS RowShipper,
				[Склад-получ#] AS ZoneConsignee,
				[Тип задачи пользователя] AS UserTaskType,
				[Работник] AS Employee,
				MIN([Время загрузки]) AS LoadTime
		FROM [{table}]
		WHERE [Тип задачи системы] = 'Отбор' AND [План/задача] = 'Дочерняя задача' AND [Тип задачи пользователя] IS NOT NULL
		GROUP BY [Позиция], [Складское подразделение], [Складское место], [Склад-получ#], [СМ-получатель], [Тип задачи пользователя], [Работник], [Назначенное время]
		
		UNION ALL
		
		SELECT 7 AS SystemTaskType_id,
		       Move.ZoneShipper,
		       NULL AS RowShipper,
		       Move.ZoneConsignee,
		       'C900' AS UserTaskType,
		       Move.Employee,
		       MIN(Move.LoadTime)
		FROM ( SELECT [СМ-получатель] AS AddressConsignee, [Загруженный НЗ] AS LoadedLPN, [Выгруженный НЗ] AS UnloadedLPN
		       FROM [{table}]
		       WHERE [Тип задачи системы] = 'Отбор' AND [Тип задачи пользователя] IS NOT NULL) Pick,
		     ( SELECT [Складское подразделение] AS ZoneShipper, [Складское место] AS AddressShipper, [Склад-получ#] AS ZoneConsignee, [Работник] AS Employee, [Назначенное время] AS LoadTime, [НЗ содержимого] AS ContentLPN
		       FROM [{table}]
		       WHERE [Тип задачи системы] = 'Перемещение для промежуточного хранения' AND [Складское место] <> [СМ-получатель] AND [НЗ содержимого] IS NOT NULL) Move
		WHERE Pick.UnloadedLPN = Move.ContentLPN AND Pick.AddressConsignee = Move.AddressShipper
		GROUP BY Pick.LoadedLPN, Move.ZoneShipper, Move.ZoneConsignee, Move.Employee
		
		UNION ALL
		
		SELECT 8 AS SystemTaskType_id,
				[Складское подразделение] AS ZoneShipper,
				NULL AS RowShipper,
				[Склад-получ#] AS ZoneConsignee,
				[Тип задачи пользователя] AS UserTaskType,
				[Работник] AS Employee,
				MIN([Время загрузки]) AS LoadTime
		FROM [{table}]
		WHERE [Тип задачи системы] = 'Перенос заказа на перемещение' AND [Тип задачи пользователя] IS NOT NULL
		GROUP BY [Складское подразделение], [Склад-получ#], [Тип задачи пользователя], [Работник], [Загруженный НЗ]) G
GROUP BY SystemTaskType_id, ZoneShipper, RowShipper, ZoneConsignee, UserTaskType, Employee, FORMAT(LoadTime, 'Short Date'), HOUR(LoadTime)