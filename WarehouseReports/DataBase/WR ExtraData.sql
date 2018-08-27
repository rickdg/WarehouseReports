SELECT	CDate(Pcs.XDate) AS XDate, Pcs.ZoneShipper, UnloadedLPN.QtyUnloadedLPN, Orders.QtyOrders, Pcs.AvgQtyPcs
FROM (	SELECT XDate, ZoneShipper, COUNT(*) AS QtyUnloadedLPN
		FROM (	SELECT FORMAT([Время загрузки], 'Short Date') AS XDate, [Складское подразделение] AS ZoneShipper
				FROM [{table}]
				WHERE [Тип задачи системы] = 'Отбор'
				GROUP BY FORMAT([Время загрузки], 'Short Date'), [Складское подразделение], [Выгруженный НЗ]) G
		GROUP BY XDate, ZoneShipper) UnloadedLPN,

	(	SELECT XDate, ZoneShipper, COUNT(*) AS QtyOrders
		FROM (	SELECT FORMAT([Время загрузки], 'Short Date') AS XDate, [Складское подразделение] AS ZoneShipper
				FROM [{table}]
				WHERE [Тип задачи системы] = 'Отбор'
				GROUP BY FORMAT([Время загрузки], 'Short Date'), [Складское подразделение], [Заголовок источника]) G
		GROUP BY XDate, ZoneShipper) Orders,

	(	SELECT XDate, ZoneShipper, ROUND(AVG(QtyPcs), 0) AS AvgQtyPcs
		FROM (	SELECT FORMAT([Время загрузки], 'Short Date') AS XDate, [Складское подразделение] AS ZoneShipper, SUM(Количество) AS QtyPcs
				FROM [{table}]
				WHERE [Тип задачи системы] = 'Отбор'
				GROUP BY FORMAT([Время загрузки], 'Short Date'), [Складское подразделение], [Заголовок источника], [Номер строки]) G
		GROUP BY XDate, ZoneShipper) Pcs
WHERE UnloadedLPN.XDate = Orders.XDate AND UnloadedLPN.ZoneShipper = Orders.ZoneShipper AND UnloadedLPN.XDate = Pcs.XDate AND UnloadedLPN.ZoneShipper = Pcs.ZoneShipper