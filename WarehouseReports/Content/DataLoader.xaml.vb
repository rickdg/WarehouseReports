Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Threading
Imports FirstFloor.ModernUI.Windows.Controls
Imports Microsoft.Win32

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
                Dialog.Buttons.First.IsEnabled = False
            Else
                Dialog.Title = "Отменено"
                ProgressRing.IsActive = False
            End If
        End Sub


        Public Sub LoadTasks(fileName As String)
            Try
                Dim ExcelTable As New DataTable

                Using Connection As New OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fileName};Extended Properties='Excel 12.0;HDR=YES';")
                    Connection.Open()
                    Dim Schema = (From Row In Connection.GetSchema("Columns")
                                  Group Row.Field(Of String)("COLUMN_NAME")
                                  By TableName = Row.Field(Of String)("TABLE_NAME").Trim("'"c) Into Columns = ToList
                                  Where TableName.EndsWith("$")).ToList

                    Dim Table = Schema.ElementAt(0).TableName
                    Dim SQL As String
                    Dim RecordCount As Integer
                    Dim Placement = GetCompiledExpression(SettingsPlacement.SerializeFileName)
                    Dim Resupply = GetCompiledExpression(SettingsResupply.SerializeFileName)
                    Dim Movement = GetCompiledExpression(SettingsMovement.SerializeFileName)

                    Select Case Schema.ElementAt(0).Columns.Count
                        Case 3 ' Загрузка в док
                            SQL = $"SELECT 6 AS SystemTaskType_id,
                                        900 AS ZoneShipper,
                                        900 AS ZoneConsignee,
                                        'L900' AS UserTaskType,
                                        [Наименование сотрудника] AS Employee,
                                        [Дата] AS LoadTime
                                FROM [{Table}]"
                        Case 4 ' Получение
                            SQL = $"SELECT 1 AS SystemTaskType_id,
                                        NULL AS ZoneShipper,
                                        0 AS ZoneConsignee,
                                        'A000' AS UserTaskType,
                                        [Получатель] AS Employee,
                                        MIN([Дата]) AS LoadTime
                                FROM [{Table}]
                                WHERE [Тип транзакции] = 'Получить' AND [Номерной знак переноса] IS NOT NULL
                                GROUP BY [Получатель], [Номерной знак переноса]"
                        Case Else
                            SQL = $"SELECT 2 AS SystemTaskType_id,
		                                IIF([Складское подразделение] IS NULL, 0, [Складское подразделение]) AS ZoneShipper,
		                                [Склад-получ#] AS ZoneConsignee,
		                                'W' & ZoneShipper & 'C' & [Склад-получ#] AS UserTaskType,
		                                [Работник] AS Employee,
		                                MIN([Время загрузки]) AS LoadTime
                                FROM [{Table}]
                                WHERE [Тип задачи системы] = 'Размещение' {Placement}
                                GROUP BY [Складское подразделение], [Склад-получ#], [Работник], [НЗ содержимого], [Номерной знак отправителя]

                                UNION ALL

                                SELECT 3 AS SystemTaskType_id,
		                                [Складское подразделение] AS ZoneShipper,
		                                [Склад-получ#] AS ZoneConsignee,
		                                [Тип задачи пользователя] AS UserTaskType,
		                                [Работник] AS Employee,
		                                MIN([Время загрузки]) AS LoadTime
                                FROM [{Table}]
                                WHERE [Тип задачи системы] = 'Пополнение' AND [Тип задачи пользователя] IS NOT NULL {Resupply}
                                GROUP BY [Складское подразделение], [Склад-получ#], [Тип задачи пользователя], [Работник], [Загруженный НЗ]

                                UNION ALL

                                SELECT 4 AS SystemTaskType_id,
		                                [Складское подразделение] AS ZoneShipper,
		                                [Склад-получ#] AS ZoneConsignee,
		                                IIF([Тип задачи пользователя] IS NULL, 'M' & [Складское подразделение] & 'C' & [Склад-получ#] , [Тип задачи пользователя]) AS UserTaskType,
		                                [Работник] AS Employee,
		                                MIN([Время загрузки]) AS LoadTime
                                FROM [{Table}]
                                WHERE [Тип задачи системы] IN ('Перенос заказа на перемещение', 'Пополнение', 'Размещение') AND [Складское подразделение] IS NOT NULL {Movement}
                                GROUP BY [Складское подразделение], [Склад-получ#], [Тип задачи пользователя], [Работник], [НЗ содержимого], [Номерной знак отправителя], [Загруженный НЗ]

                                UNION ALL

                                SELECT 5 AS SystemTaskType_id,
		                                [Складское подразделение] AS ZoneShipper,
		                                [Склад-получ#] AS ZoneConsignee,
		                                [Тип задачи пользователя] AS UserTaskType,
		                                [Работник] AS Employee,
		                                MIN([Время загрузки]) AS LoadTime
                                FROM [{Table}]
                                WHERE [Тип задачи системы] = 'Отбор' AND [План/задача] = 'Независимая задача'
                                GROUP BY [Заголовок источника], [Номер строки], [Позиция], [Складское подразделение], [Складское место], [Склад-получ#], [СМ-получатель], [Тип задачи пользователя], [Работник], [Назначенное время]

                                UNION ALL

                                SELECT 5 AS SystemTaskType_id,
		                                [Складское подразделение] AS ZoneShipper,
		                                [Склад-получ#] AS ZoneConsignee,
		                                [Тип задачи пользователя] AS UserTaskType,
		                                [Работник] AS Employee,
		                                MIN([Время загрузки]) AS LoadTime
                                FROM [{Table}]
                                WHERE [Тип задачи системы] = 'Отбор' AND [План/задача] = 'Дочерняя задача'
                                GROUP BY [Позиция], [Складское подразделение], [Складское место], [Склад-получ#], [СМ-получатель], [Тип задачи пользователя], [Работник], [Назначенное время]

                                UNION ALL

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
                                GROUP BY Pick.LoadedLPN, Move.ZoneShipper, Move.ZoneConsignee, Move.Employee"
                    End Select

                    Using Adapter As New OleDbDataAdapter(SQL, Connection)
                        RecordCount = Adapter.Fill(ExcelTable)
                    End Using

                    If RecordCount = 0 Then Throw New ArgumentException("Нет данных для загрузки")

                    Dispatcher.Invoke(Sub()
                                          Dialog.Title = "Загрузка"
                                          Message.Text = $"Количество задач {RecordCount.ToString}"
                                      End Sub)

                End Using

                Using Connection As New SqlConnection(My.Settings.WarehouseDataConnectionString)
                    Connection.Open()
                    Using Command = Connection.CreateCommand()
                        Command.CommandTimeout = 1800
                        Command.CommandText = "dbo.LoadTasks"
                        Command.CommandType = CommandType.StoredProcedure
                        Command.Parameters.Add("@ExcelTasks", SqlDbType.Structured).TypeName = "TaskExcelTable"
                        Command.Parameters("@ExcelTasks").Value = ExcelTable
                        Command.ExecuteReader()
                    End Using
                End Using

                Dispatcher.Invoke(Sub() Dialog.Title = "Завершено")

            Catch ex As Exception

                Dispatcher.Invoke(Sub()
                                      Dialog.Title = "Ошибка"
                                      Message.Text = ex.Message
                                  End Sub)

            Finally

                Dispatcher.Invoke(Sub()
                                      Dialog.Buttons.First.IsEnabled = True
                                      ProgressRing.IsActive = False
                                  End Sub)

            End Try
        End Sub


        Private Function GetCompiledExpression(fileName As String) As String
            If FileExists("", fileName) Then
                Dim Result = Deserialize(Of SettingsExpressionTree)("", fileName).CompiledExpression
                If IsNothing(Result) Then Return ""
                Return $"AND {Result}"
            End If
            Return ""
        End Function

    End Class
End Namespace