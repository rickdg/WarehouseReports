SELECT 5 AS SystemTaskType_id, F6 AS ZoneShipper, F8 AS ZoneConsignee, F10 AS UserTaskType, F11 AS Employee, MIN(F13) AS LoadTime
FROM [{Table}]
WHERE F2 = 'Отбор' AND F1 = 'Независимая задача'
GROUP BY F3, F4, F5, F6, F7, F8, F9, F10, F11, F12

UNION ALL

SELECT 5 AS SystemTaskType_id, F6 AS ZoneShipper, F8 AS ZoneConsignee, F10 AS UserTaskType, F11 AS Employee, MIN(F13) AS LoadTime
FROM [{Table}]
WHERE F2 = 'Отбор' AND F1 = 'Дочерняя задача'
GROUP BY F5, F6, F7, F8, F9, F10, F11, F12