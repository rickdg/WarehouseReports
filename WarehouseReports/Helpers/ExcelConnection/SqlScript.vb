Imports WarehouseReports.Content
Imports WarehouseReports.Enums

Namespace ExcelConnection
    Module SqlScript

        Public Function GetScript(loadType As LoadType, table As String) As String
            Select Case loadType
                Case LoadType.Receipt
                    Return GetReceiptScript(table)
                Case LoadType.Placement
                    Return GetPlacementScript(table)
                Case LoadType.Resupply
                    Return GetResupplyScript(table)
                Case LoadType.ManualResupply
                    Return GetManualResupplyScript(table)
                Case LoadType.Movement
                    Return GetMovementScript(table)
                Case LoadType.Pick
                    Return GetPickScript(table)
                Case LoadType.Load
                    Return GetLoadScript(table)
                Case LoadType.Control
                    Return GetControlScript(table)
                Case LoadType.ExtraData
                    Return GetExtraDataScript(table)
                Case LoadType.UnionTasks
                    Return GetUnionScript(table)
            End Select
            Return ""
        End Function


        Public Function GetReceiptScript(table As String) As String
            Return $"SELECT 1 AS SystemTaskType_id, NULL AS ZoneShipper, NULL AS RowShipper, 0 AS ZoneConsignee, 'A000' AS UserTaskType, Employee, MIN(LoadTime) AS LoadTime, COUNT(*) AS QtyTasks
                    FROM (	SELECT [Получатель] AS Employee,
				                   MIN([Дата]) AS LoadTime
		                    FROM [{table}]
		                    WHERE [Тип транзакции] = 'Получить' AND [Номерной знак переноса] IS NOT NULL
		                    GROUP BY [Получатель], [Номерной знак переноса]) G
                    GROUP BY Employee, FORMAT(LoadTime, 'Short Date'), HOUR(LoadTime)"
        End Function


        Public Function GetPlacementScript(table As String) As String
            Dim Placement = GetCompiledExpression(My.Settings.FilePlacement)
            Return $"SELECT 2 AS SystemTaskType_id, ZoneShipper, NULL AS RowShipper, ZoneConsignee, UserTaskType, Employee, MIN(LoadTime) AS LoadTime, COUNT(*) AS QtyTasks
                    FROM (	SELECT IIF([Складское подразделение] IS NULL, 0, [Складское подразделение]) AS ZoneShipper,
				                   [Склад-получ#] AS ZoneConsignee,
				                   'W' & ZoneShipper & 'C' & [Склад-получ#] AS UserTaskType,
				                   [Работник] AS Employee,
				                   MIN([Время загрузки]) AS LoadTime
		                    FROM [{table}]
		                    WHERE [Тип задачи системы] = 'Размещение' {Placement}
		                    GROUP BY [Складское подразделение], [Склад-получ#], [Работник], [НЗ содержимого], [Номерной знак отправителя]) G
                    GROUP BY ZoneShipper, ZoneConsignee, UserTaskType, Employee, FORMAT(LoadTime, 'Short Date'), HOUR(LoadTime)"
        End Function


        Public Function GetResupplyScript(table As String) As String
            Dim Resupply = GetCompiledExpression(My.Settings.FileResupply)
            Return $"SELECT 3 AS SystemTaskType_id, ZoneShipper, NULL AS RowShipper, ZoneConsignee, UserTaskType, Employee, MIN(LoadTime) AS LoadTime, COUNT(*) AS QtyTasks
                    FROM (	SELECT [Складское подразделение] AS ZoneShipper,
				                   [Склад-получ#] AS ZoneConsignee,
				                   [Тип задачи пользователя] AS UserTaskType,
				                   [Работник] AS Employee,
				                   MIN([Время загрузки]) AS LoadTime
		                    FROM [{table}]
		                    WHERE [Тип задачи системы] = 'Пополнение' AND [Тип задачи пользователя] IS NOT NULL {Resupply}
		                    GROUP BY [Складское подразделение], [Склад-получ#], [Тип задачи пользователя], [Работник], [Загруженный НЗ]) G
                    GROUP BY ZoneShipper, ZoneConsignee, UserTaskType, Employee, FORMAT(LoadTime, 'Short Date'), HOUR(LoadTime)"
        End Function


        Public Function GetManualResupplyScript(table As String) As String
            Dim ManualResupply = GetCompiledExpression(My.Settings.FileManualResupply)
            Return $"SELECT 4 AS SystemTaskType_id, ZoneShipper, NULL AS RowShipper, ZoneConsignee, UserTaskType, Employee, MIN(LoadTime) AS LoadTime, COUNT(*) AS QtyTasks
                    FROM (	SELECT [Складское подразделение] AS ZoneShipper,
				                   [Склад-получ#] AS ZoneConsignee,
				                   [Тип задачи пользователя] AS UserTaskType,
				                   [Работник] AS Employee,
				                   MIN([Время загрузки]) AS LoadTime
		                    FROM [{table}]
		                    WHERE [Тип задачи системы] = 'Перенос заказа на перемещение' AND [Тип задачи пользователя] IS NOT NULL {ManualResupply}
		                    GROUP BY [Складское подразделение], [Склад-получ#], [Тип задачи пользователя], [Работник], [Загруженный НЗ]) G
                    GROUP BY ZoneShipper, ZoneConsignee, UserTaskType, Employee, FORMAT(LoadTime, 'Short Date'), HOUR(LoadTime)"
        End Function


        Public Function GetMovementScript(table As String) As String
            Dim Movement = GetCompiledExpression(My.Settings.FileMovement)
            Return $"SELECT 5 AS SystemTaskType_id, ZoneShipper, NULL AS RowShipper, ZoneConsignee, UserTaskType, Employee, MIN(LoadTime) AS LoadTime, COUNT(*) AS QtyTasks
                    FROM (	SELECT [Складское подразделение] AS ZoneShipper,
				                   [Склад-получ#] AS ZoneConsignee,
				                   IIF([Тип задачи пользователя] IS NULL, 'M' & [Складское подразделение] & 'C' & [Склад-получ#], [Тип задачи пользователя]) AS UserTaskType,
				                   [Работник] AS Employee,
				                   MIN([Время загрузки]) AS LoadTime
		                    FROM [{table}]
		                    WHERE [Тип задачи системы] IN ('Перенос заказа на перемещение', 'Пополнение', 'Размещение') AND [Складское подразделение] IS NOT NULL {Movement}
		                    GROUP BY [Складское подразделение], [Склад-получ#], [Тип задачи пользователя], [Работник], [НЗ содержимого], [Номерной знак отправителя], [Загруженный НЗ]) G
                    GROUP BY ZoneShipper, ZoneConsignee, UserTaskType, Employee, FORMAT(LoadTime, 'Short Date'), HOUR(LoadTime)"
        End Function


        Public Function GetPickScript(table As String) As String
            Return $"SELECT 6 AS SystemTaskType_id, ZoneShipper, RowShipper, ZoneConsignee, UserTaskType, Employee, MIN(LoadTime) AS LoadTime, COUNT(*) AS QtyTasks
                    FROM (	SELECT [Складское подразделение] AS ZoneShipper,
				                   IIF([Складское подразделение] = 520, LEFT([Складское место], INSTR([Складское место], '.') - 1), NULL) AS RowShipper,
				                   [Склад-получ#] AS ZoneConsignee,
				                   [Тип задачи пользователя] AS UserTaskType,
				                   [Работник] AS Employee,
				                   MIN([Время загрузки]) AS LoadTime
		                    FROM [{table}]
		                    WHERE [Тип задачи системы] = 'Отбор' AND [План/задача] = 'Независимая задача' AND [Тип задачи пользователя] IS NOT NULL
		                    GROUP BY [Заголовок источника], [Номер строки], [Позиция], [Складское подразделение], [Складское место], [Склад-получ#], [СМ-получатель], [Тип задачи пользователя], [Работник], [Назначенное время]
		
		                    UNION ALL
		
		                    SELECT [Складское подразделение] AS ZoneShipper,
				                   IIF([Складское подразделение] = 520, LEFT([Складское место], INSTR([Складское место], '.') - 1), NULL) AS RowShipper,
				                   [Склад-получ#] AS ZoneConsignee,
				                   [Тип задачи пользователя] AS UserTaskType,
				                   [Работник] AS Employee,
				                   MIN([Время загрузки]) AS LoadTime
		                    FROM [{table}]
		                    WHERE [Тип задачи системы] = 'Отбор' AND [План/задача] = 'Дочерняя задача' AND [Тип задачи пользователя] IS NOT NULL
		                    GROUP BY [Позиция], [Складское подразделение], [Складское место], [Склад-получ#], [СМ-получатель], [Тип задачи пользователя], [Работник], [Назначенное время]) G
                    GROUP BY ZoneShipper, RowShipper, ZoneConsignee, UserTaskType, Employee, FORMAT(LoadTime, 'Short Date'), HOUR(LoadTime)"
        End Function


        Public Function GetLoadScript(table As String) As String
            Return $"SELECT 7 AS SystemTaskType_id,
		                    900 AS ZoneShipper,
                            NULL AS RowShipper,
		                    900 AS ZoneConsignee,
		                    'L900' AS UserTaskType,
		                    [Наименование сотрудника] AS Employee,
		                    [Дата] AS LoadTime,
		                    COUNT(*) AS QtyTasks
                    FROM [{table}]
                    GROUP BY [Наименование сотрудника], [Дата]"
        End Function


        Public Function GetControlScript(table As String) As String
            Return $"SELECT 8 AS SystemTaskType_id, ZoneShipper, NULL AS RowShipper, ZoneConsignee, UserTaskType, Employee, MIN(LoadTime) AS LoadTime, COUNT(*) AS QtyTasks
                    FROM (	SELECT Move.ZoneShipper,
		                           Move.ZoneConsignee,
		                           'C900' AS UserTaskType,
		                           Move.Employee,
		                           MIN(Move.LoadTime) AS LoadTime
		                    FROM (  SELECT [СМ-получатель] AS AddressConsignee, [Загруженный НЗ] AS LoadedLPN, [Выгруженный НЗ] AS UnloadedLPN
		                            FROM [{table}]
		                            WHERE [Тип задачи системы] = 'Отбор' AND [Тип задачи пользователя] IS NOT NULL) Pick,
		                         (  SELECT [Складское подразделение] AS ZoneShipper, [Складское место] AS AddressShipper, [Склад-получ#] AS ZoneConsignee, [Работник] AS Employee, [Назначенное время] AS LoadTime, [НЗ содержимого] AS ContentLPN
		                            FROM [{table}]
		                            WHERE [Тип задачи системы] = 'Перемещение для промежуточного хранения' AND [Складское место] <> [СМ-получатель] AND [НЗ содержимого] IS NOT NULL) Move
		                    WHERE Pick.UnloadedLPN = Move.ContentLPN AND Pick.AddressConsignee = Move.AddressShipper
		                    GROUP BY Pick.LoadedLPN, Move.ZoneShipper, Move.ZoneConsignee, Move.Employee) G
                    GROUP BY ZoneShipper, ZoneConsignee, UserTaskType, Employee, FORMAT(LoadTime, 'Short Date'), HOUR(LoadTime)"
        End Function


        Public Function GetUnionScript(table As String) As String
            Dim Placement = GetCompiledExpression(My.Settings.FilePlacement)
            Dim Resupply = GetCompiledExpression(My.Settings.FileResupply)
            Dim Movement = GetCompiledExpression(My.Settings.FileMovement)
            Dim ManualResupply = GetCompiledExpression(My.Settings.FileManualResupply)

            Return $"SELECT SystemTaskType_id, ZoneShipper, RowShipper, ZoneConsignee, UserTaskType, Employee, MIN(LoadTime) AS LoadTime, COUNT(*) AS QtyTasks
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
				                   [Тип задачи пользователя] AS UserTaskType,
				                   [Работник] AS Employee,
				                   MIN([Время загрузки]) AS LoadTime
		                    FROM [{table}]
		                    WHERE [Тип задачи системы] = 'Перенос заказа на перемещение' AND [Тип задачи пользователя] IS NOT NULL {ManualResupply}
		                    GROUP BY [Складское подразделение], [Склад-получ#], [Тип задачи пользователя], [Работник], [Загруженный НЗ]
		
		                    UNION ALL
		
		                    SELECT 5 AS SystemTaskType_id,
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
		
		                    SELECT 6 AS SystemTaskType_id,
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
		
		                    SELECT 6 AS SystemTaskType_id,
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
		
		                    SELECT 8 AS SystemTaskType_id,
		                           Move.ZoneShipper,
		                           NULL AS RowShipper,
		                           Move.ZoneConsignee,
		                           'C900' AS UserTaskType,
		                           Move.Employee,
		                           MIN(Move.LoadTime) AS LoadTime
		                    FROM (  SELECT [СМ-получатель] AS AddressConsignee, [Загруженный НЗ] AS LoadedLPN, [Выгруженный НЗ] AS UnloadedLPN
		                            FROM [{table}]
		                            WHERE [Тип задачи системы] = 'Отбор' AND [Тип задачи пользователя] IS NOT NULL) Pick,
		                         (  SELECT [Складское подразделение] AS ZoneShipper, [Складское место] AS AddressShipper, [Склад-получ#] AS ZoneConsignee, [Работник] AS Employee, [Назначенное время] AS LoadTime, [НЗ содержимого] AS ContentLPN
		                            FROM [{table}]
		                            WHERE [Тип задачи системы] = 'Перемещение для промежуточного хранения' AND [Складское место] <> [СМ-получатель] AND [НЗ содержимого] IS NOT NULL) Move
		                    WHERE Pick.UnloadedLPN = Move.ContentLPN AND Pick.AddressConsignee = Move.AddressShipper
		                    GROUP BY Pick.LoadedLPN, Move.ZoneShipper, Move.ZoneConsignee, Move.Employee) G
                    GROUP BY SystemTaskType_id, ZoneShipper, RowShipper, ZoneConsignee, UserTaskType, Employee, FORMAT(LoadTime, 'Short Date'), HOUR(LoadTime)"
        End Function


        Public Function GetExtraDataScript(table As String) As String
            Return $"SELECT	CDate(Pcs.XDate) AS XDate, Pcs.ZoneShipper, UnloadedLPN.QtyUnloadedLPN, Orders.QtyOrders, Pcs.AvgQtyPcs
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
                    WHERE UnloadedLPN.XDate = Orders.XDate AND UnloadedLPN.ZoneShipper = Orders.ZoneShipper AND UnloadedLPN.XDate = Pcs.XDate AND UnloadedLPN.ZoneShipper = Pcs.ZoneShipper"
        End Function


        Private Function GetCompiledExpression(fileName As String) As String
            If FileExists(fileName) Then
                Dim Result = Deserialize(Of SettingsExpressionTree)(fileName).CompiledExpression
                If Result Is Nothing Then Return ""
                Return $"AND {Result}"
            End If
            Return ""
        End Function

    End Module
End Namespace