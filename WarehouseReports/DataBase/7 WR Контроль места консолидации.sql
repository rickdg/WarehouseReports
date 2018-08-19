SELECT 7 AS SystemTaskType_id,
        Move.ZoneShipper,
        Move.ZoneConsignee,
        'C900' AS UserTaskType,
        Move.Employee,
        MIN(Move.LoadTime)
FROM (  SELECT [СМ-получатель] AS AddressConsignee, [Загруженный НЗ] AS LoadedLPN, [Выгруженный НЗ] AS UnloadedLPN
        FROM [{Table}]
        WHERE [Тип задачи системы] = 'Отбор') Pick,
     (  SELECT [Складское подразделение] AS ZoneShipper, [Складское место] AS AddressShipper, [Склад-получ#] AS ZoneConsignee, [Работник] AS Employee, [Назначенное время] AS LoadTime, [НЗ содержимого] AS ContentLPN
        FROM [{Table}]
        WHERE [Тип задачи системы] = 'Перемещение для промежуточного хранения' AND [Складское место] <> [СМ-получатель] AND [НЗ содержимого] IS NOT NULL) Move
WHERE Pick.UnloadedLPN = Move.ContentLPN AND Pick.AddressConsignee = Move.AddressShipper
GROUP BY Pick.LoadedLPN, Move.ZoneShipper, Move.ZoneConsignee, Move.Employee