﻿Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Text
Imports System.Threading
Imports FirstFloor.ModernUI.Windows.Controls
Imports Microsoft.Win32
Imports WarehouseReports.ExcelConnection

Namespace Content
    Partial Public Class DataLoader
        Inherits UserControl

        Private Dialog As ModernDialog


        Public Sub New(dlg As ModernDialog)
            InitializeComponent()
            Dialog = dlg

            Dim DialogWindow As New OpenFileDialog With {.Title = "Выбрать файл"}
            If DialogWindow.ShowDialog Then
                Dim LoadThread As New Thread(Sub() LoadTasks(DialogWindow.FileName)) With {.Priority = ThreadPriority.Highest}
                LoadThread.SetApartmentState(ApartmentState.STA)
                LoadThread.Start()

                Dialog.Title = "Запрос"
                Dialog.Buttons.First.Visibility = Visibility.Collapsed
            Else
                Dialog.Title = "Отменено"
                ProgressRing.IsActive = False
            End If
        End Sub


        Public Sub LoadTasks(fileName As String)
            Try
                Dim ExcelTable As New DataTable
                Dim ExcelTable2 As New DataTable

                Using Connection As New OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fileName};Extended Properties='Excel 12.0;HDR=YES';")
                    Connection.Open()
                    Dim Table = (From Row In Connection.GetSchema("Columns")
                                 Group New Column(Row.Field(Of String)("COLUMN_NAME"), Row.Field(Of Integer)("DATA_TYPE"))
                                      By TableName = Row.Field(Of String)("TABLE_NAME").Trim("'"c) Into Columns = ToList
                                 Where TableName.EndsWith("$")
                                 Select New Table(TableName) With {.Columns = Columns}).First

                    Dim SQL As String
                    Dim SQL2 As String = ""
                    Dim OriginalColumns As IEnumerable(Of Column)
                    Select Case Table.Columns.Count
                        Case 3 ' Загрузка в док
                            SQL = GetLoadScript(Table.Name)
                            OriginalColumns = {
                                New Column("Дата", AdoEnums.adDate),
                                New Column("Наименование сотрудника", AdoEnums.adWChar),
                                New Column("LPN", AdoEnums.adWChar)}
                        Case 4 ' Получение
                            SQL = GetReceptionScript(Table.Name)
                            OriginalColumns = {
                                New Column("Тип транзакции", AdoEnums.adWChar),
                                New Column("Получатель", AdoEnums.adWChar),
                                New Column("Номерной знак переноса", AdoEnums.adWChar),
                                New Column("Дата", AdoEnums.adDate)}
                        Case Else
                            SQL = GetUniunScript(Table.Name)
                            SQL2 = GetExtraDataScript(Table.Name)
                            OriginalColumns = {
                                New Column("План/задача", AdoEnums.adWChar),
                                New Column("Тип задачи системы", AdoEnums.adWChar),
                                New Column("Заголовок источника", AdoEnums.adDouble),
                                New Column("Номер строки", AdoEnums.adWChar),
                                New Column("Позиция", AdoEnums.adDouble),
                                New Column("Складское подразделение", AdoEnums.adDouble),
                                New Column("Складское место", AdoEnums.adWChar),
                                New Column("Склад-получ#", AdoEnums.adDouble),
                                New Column("СМ-получатель", AdoEnums.adWChar),
                                New Column("Количество", AdoEnums.adDouble),
                                New Column("Тип задачи пользователя", AdoEnums.adWChar),
                                New Column("Работник", AdoEnums.adWChar),
                                New Column("Назначенное время", AdoEnums.adDate),
                                New Column("Время загрузки", AdoEnums.adDate),
                                New Column("НЗ содержимого", AdoEnums.adWChar),
                                New Column("Номерной знак отправителя", AdoEnums.adWChar),
                                New Column("Загруженный НЗ", AdoEnums.adWChar),
                                New Column("Выгруженный НЗ", AdoEnums.adWChar)}
                    End Select
                    Dim CheckResult = CheckColumns(OriginalColumns, Table.Columns)
                    If CheckResult <> "" Then Throw New ArgumentException(CheckResult)

                    Using Adapter As New OleDbDataAdapter(SQL, Connection)
                        Dim RecordCount = Adapter.Fill(ExcelTable)
                        If RecordCount = 0 Then Throw New ArgumentException("Нет данных для загрузки")

                        If SQL2 <> "" Then
                            Adapter.SelectCommand.CommandText = SQL2
                            Adapter.Fill(ExcelTable2)
                        End If

                        Dim QtyTasks = ExcelTable.Select.Sum(Function(r) r.Field(Of Integer)("QtyTasks"))
                        Dispatcher.Invoke(Sub()
                                              Dialog.Title = "Загрузка"
                                              Message.BBCode = $"Количество задач {QtyTasks}"
                                          End Sub)
                    End Using
                End Using

                ExecuteStoredProcedure("dbo.LoadTasks", "@ExcelTasks", "TypeTasksExcelTable", ExcelTable)

                If ExcelTable2.Rows.Count > 0 Then
                    ExecuteStoredProcedure("dbo.LoadExtraData", "@ExcelExtraData", "TypeExtraDataExcelTable", ExcelTable2)
                End If

                Dispatcher.Invoke(Sub()
                                      Dialog.Title = "Завершено"
                                      Complete.Visibility = Visibility.Visible
                                  End Sub)
            Catch ex As Exception
                Dispatcher.Invoke(Sub()
                                      Dialog.Title = "Ошибка"
                                      Message.BBCode = GetInnerException(ex)
                                      Warning.Visibility = Visibility.Visible
                                  End Sub)
            Finally
                Dispatcher.Invoke(Sub()
                                      Dialog.Buttons.First.Visibility = Visibility.Visible
                                      ProgressRing.IsActive = False
                                      ProgressRing.Visibility = Visibility.Collapsed
                                  End Sub)
            End Try
        End Sub


        Private Sub ExecuteStoredProcedure(commandText As String, parameterName As String, typeName As String, parameterValue As DataTable)
            Using Connection As New SqlConnection(My.Settings.WarehouseDataConnectionString)
                Connection.Open()
                Using Command = Connection.CreateCommand()
                    Command.CommandTimeout = 1800
                    Command.CommandText = commandText
                    Command.CommandType = CommandType.StoredProcedure
                    Command.Parameters.Add(parameterName, SqlDbType.Structured).TypeName = typeName
                    Command.Parameters(parameterName).Value = parameterValue
                    Command.ExecuteReader()
                End Using
            End Using
        End Sub


        Private Function GetInnerException(ex As Exception) As String
            Dim Result As New StringBuilder
            Result.Append(ex.Message & vbCrLf & vbCrLf)
            If ex.InnerException IsNot Nothing Then
                Result.Append(GetInnerException(ex.InnerException))
            End If
            Return Result.ToString
        End Function


        Private Function GetCompiledExpression(fileName As String) As String
            If FileExists(fileName) Then
                Dim Result = Deserialize(Of SettingsExpressionTree)(fileName).CompiledExpression
                If Result Is Nothing Then Return ""
                Return $"AND {Result}"
            End If
            Return ""
        End Function


        Private Function CheckColumns(original As IEnumerable(Of Column), verifiable As IEnumerable(Of Column)) As String
            Dim Result As New StringBuilder
            For Each Source In original
                Dim Target = verifiable.SingleOrDefault(Function(c) c.Name = Source.Name)
                If Target Is Nothing Then
                    Result.AppendLine($"Отсутствует столбец - [b]{Source}[/b]")
                Else
                    If Target.DataType <> Source.DataType Then
                        Result.AppendLine($"Столбец - [b]{Target}[/b] ([url=https://docs.microsoft.com/ru-ru/sql/ado/reference/ado-api/datatypeenum?view=sql-server-2017][b]{Target.DataType}[/b][/url]) несоответствует типу данных [url=https://docs.microsoft.com/ru-ru/sql/ado/reference/ado-api/datatypeenum?view=sql-server-2017][b]{Source.DataType}[/b][/url]")
                    End If
                End If
            Next
            Return Result.ToString
        End Function


        Private Function GetReceptionScript(table As String) As String
            Return $"SELECT SystemTaskType_id, ZoneShipper, ZoneConsignee, UserTaskType, Employee, MIN(LoadTime) AS LoadTime, COUNT(*) AS QtyTasks
                    FROM (	SELECT 1 AS SystemTaskType_id,
				                   NULL AS ZoneShipper,
				                   0 AS ZoneConsignee,
				                   'A000' AS UserTaskType,
				                   [Получатель] AS Employee,
				                   MIN([Дата]) AS LoadTime
		                    FROM [{table}]
		                    WHERE [Тип транзакции] = 'Получить' AND [Номерной знак переноса] IS NOT NULL
		                    GROUP BY [Получатель], [Номерной знак переноса]) G
                    GROUP BY SystemTaskType_id, ZoneShipper, ZoneConsignee, UserTaskType, Employee, FORMAT(LoadTime, 'Short Date'), HOUR(LoadTime)"
        End Function


        Private Function GetLoadScript(table As String) As String
            Return $"SELECT 6 AS SystemTaskType_id,
		                    900 AS ZoneShipper,
		                    900 AS ZoneConsignee,
		                    'L900' AS UserTaskType,
		                    [Наименование сотрудника] AS Employee,
		                    [Дата] AS LoadTime,
		                    COUNT(*) AS QtyTasks
                    FROM [{table}]
                    GROUP BY [Наименование сотрудника], [Дата]"
        End Function


        Private Function GetUniunScript(table As String) As String
            Dim Placement = GetCompiledExpression(My.Settings.FilePlacement)
            Dim Resupply = GetCompiledExpression(My.Settings.FileResupply)
            Dim Movement = GetCompiledExpression(My.Settings.FileMovement)

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
		                    GROUP BY Pick.LoadedLPN, Move.ZoneShipper, Move.ZoneConsignee, Move.Employee) G
                    GROUP BY SystemTaskType_id, ZoneShipper, RowShipper, ZoneConsignee, UserTaskType, Employee, FORMAT(LoadTime, 'Short Date'), HOUR(LoadTime)"
        End Function


        Private Function GetExtraDataScript(table As String) As String
            Return $"SELECT	CDate(Pcs.xDate) AS xDate, Pcs.ZoneShipper, UnloadedLPN.QtyUnloadedLPN, Orders.QtyOrders, Pcs.AvgQtyPcs
                    FROM (	SELECT xDate, ZoneShipper, COUNT(*) AS QtyUnloadedLPN
		                    FROM (	SELECT FORMAT([Время загрузки], 'Short Date') AS xDate, [Складское подразделение] AS ZoneShipper
				                    FROM [{table}]
				                    WHERE [Тип задачи системы] = 'Отбор'
				                    GROUP BY FORMAT([Время загрузки], 'Short Date'), [Складское подразделение], [Выгруженный НЗ]) G
		                    GROUP BY xDate, ZoneShipper) UnloadedLPN,

	                    (	SELECT xDate, ZoneShipper, COUNT(*) AS QtyOrders
		                    FROM (	SELECT FORMAT([Время загрузки], 'Short Date') AS xDate, [Складское подразделение] AS ZoneShipper
				                    FROM [{table}]
				                    WHERE [Тип задачи системы] = 'Отбор'
				                    GROUP BY FORMAT([Время загрузки], 'Short Date'), [Складское подразделение], [Заголовок источника]) G
		                    GROUP BY xDate, ZoneShipper) Orders,

	                    (	SELECT xDate, ZoneShipper, ROUND(AVG(QtyPcs), 0) AS AvgQtyPcs
		                    FROM (	SELECT FORMAT([Время загрузки], 'Short Date') AS xDate, [Складское подразделение] AS ZoneShipper, SUM(Количество) AS QtyPcs
				                    FROM [{table}]
				                    WHERE [Тип задачи системы] = 'Отбор'
				                    GROUP BY FORMAT([Время загрузки], 'Short Date'), [Складское подразделение], [Заголовок источника], [Номер строки]) G
		                    GROUP BY xDate, ZoneShipper) Pcs
                    WHERE UnloadedLPN.xDate = Orders.xDate AND UnloadedLPN.ZoneShipper = Orders.ZoneShipper AND UnloadedLPN.xDate = Pcs.xDate AND UnloadedLPN.ZoneShipper = Pcs.ZoneShipper"
        End Function

    End Class
End Namespace