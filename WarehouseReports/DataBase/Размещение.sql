SELECT 2 AS SystemTaskType_id, IIF(F6 IS NULL, 0, F6) AS ZoneShipper, F8 AS ZoneConsignee, 'W'&ZoneShipper&'C'&F8 AS UserTaskType, F11 AS Employee, MIN(F13) AS LoadTime
FROM [{Table}]
WHERE F2 = 'Размещение' AND F11 IS NOT NULL
GROUP BY F6, F8, F11, F14, F15