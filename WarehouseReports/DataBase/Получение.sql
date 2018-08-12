SELECT 1 AS SystemTaskType_id, NULL AS ZoneShipper, 0 AS ZoneConsignee, 'A000' AS UserTaskType, F2 AS Employee, MIN(F4) AS LoadTime
FROM [{Table}]
WHERE F1 = 'Получить' AND F3 IS NOT NULL
GROUP BY F2, F3