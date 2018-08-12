SELECT 7 AS SystemTaskType_id, Move.ZoneShipper, Move.ZoneConsignee, 'C900' AS UserTaskType, Move.Employee, MIN(Move.LoadTime)
FROM (  SELECT F9 AS AddressConsignee, F16 AS LoadedLPN, F17 AS UnloadedLPN
        FROM [{Table}]) Pick,
     (  SELECT F6 AS ZoneShipper, F7 AS AddressShipper, F8 AS ZoneConsignee, F11 AS Employee, F12 AS LoadTime, F14 AS ContentLPN
        FROM [{Table}]
        WHERE F2 = 'Перемещение для промежуточного хранения' AND F7 <> F9 AND F14 IS NOT NULL) Move
WHERE Pick.UnloadedLPN = Move.ContentLPN AND Pick.AddressConsignee = Move.AddressShipper
GROUP BY Pick.LoadedLPN, Move.ZoneShipper, Move.ZoneConsignee, Move.Employee