Imports System.Data.OleDb
Imports System.Data.SqlClient

Public Class DataLoader

    Public Shared Function LoadTasks(fileName As String) As Boolean
        Try
            Dim ExcelTable As New DataTable

            Using Connection As New OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fileName};Extended Properties='Excel 12.0;HDR=NO';")
                Connection.Open()
                Dim Schema = (From Row In Connection.GetSchema("Columns")
                              Group Row.Field(Of String)("COLUMN_NAME")
                                  By TableName = Row.Field(Of String)("TABLE_NAME").Trim("'"c) Into Columns = ToList
                              Where TableName.EndsWith("$")).ToList

                Dim Table = Schema.ElementAt(0).TableName
                Dim SQL As String = ""
                Dim RecordCount As Integer

                Select Case Schema.ElementAt(0).Columns.Count
                    Case 3 ' Загрузка в док
                        SQL = $"SELECT 6 AS SystemTaskType_id,
                                       900 AS ZoneShipper,
                                       900 AS ZoneConsignee,
                                       'L900' AS UserTaskType,
                                       F1 AS Employee,
                                       F3 AS LoadTime
                                FROM [{Table}]"
                    Case 4 ' Получение
                        SQL = $"SELECT 1 AS SystemTaskType_id,
                                       NULL AS ZoneShipper,
                                       0 AS ZoneConsignee,
                                       'A000' AS UserTaskType,
                                       F2 AS Employee,
                                       MIN(F4) AS LoadTime
                                FROM [{Table}]
                                WHERE F1 = 'Получить' AND F3 IS NOT NULL
                                GROUP BY F2, F3"
                    Case 17
                        SQL = $"SELECT 5 AS SystemTaskType_id,
                                       F6 AS ZoneShipper,
                                       F8 AS ZoneConsignee,
                                       F10 AS UserTaskType,
                                       F11 AS Employee,
                                       MIN(F13) AS LoadTime
                                FROM [{Table}]
                                WHERE F2 = 'Отбор' AND F1 = 'Независимая задача'
                                GROUP BY F3, F4, F5, F6, F7, F8, F9, F10, F11, F12

                                UNION ALL

                                SELECT 5 AS SystemTaskType_id,
                                       F6 AS ZoneShipper,
                                       F8 AS ZoneConsignee,
                                       F10 AS UserTaskType,
                                       F11 AS Employee,
                                       MIN(F13) AS LoadTime
                                FROM [{Table}]
                                WHERE F2 = 'Отбор' AND F1 = 'Дочерняя задача'
                                GROUP BY F5, F6, F7, F8, F9, F10, F11, F12

                                UNION ALL

                                SELECT 2 AS SystemTaskType_id,
                                       IIF(F6 IS NULL, 0, F6) AS ZoneShipper,
                                       F8 AS ZoneConsignee,
                                       'W'&ZoneShipper&'C'&F8 AS UserTaskType,
                                       F11 AS Employee,
                                       MIN(F13) AS LoadTime
                                FROM [{Table}]
                                WHERE F2 = 'Размещение' AND F11 IS NOT NULL
                                GROUP BY F6, F8, F11, F14, F15
                                
                                UNION ALL
                                
                                SELECT 7 AS SystemTaskType_id,
                                       Move.ZoneShipper,
                                       Move.ZoneConsignee,
                                       'C900' AS UserTaskType,
                                       Move.Employee,
                                       MIN(Move.LoadTime)
                                FROM (  SELECT F9 AS AddressConsignee, F16 AS LoadedLPN, F17 AS UnloadedLPN
                                        FROM [{Table}]) Pick,
                                     (  SELECT F6 AS ZoneShipper, F7 AS AddressShipper, F8 AS ZoneConsignee, F11 AS Employee, F12 AS LoadTime, F14 AS ContentLPN
                                        FROM [{Table}]
                                        WHERE F2 = 'Перемещение для промежуточного хранения' AND F6 = F8 AND F7 <> F9 AND F14 IS NOT NULL) Move
                                WHERE Pick.UnloadedLPN = Move.ContentLPN AND Pick.AddressConsignee = Move.AddressShipper
                                GROUP BY Pick.LoadedLPN, Move.ZoneShipper, Move.ZoneConsignee, Move.Employee"
                End Select

                Using Adapter As New OleDbDataAdapter(SQL, Connection)
                    RecordCount = Adapter.Fill(ExcelTable)
                End Using

                If RecordCount = 0 Then Return False
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

            MsgBox("Выполнено", MsgBoxStyle.Information)
            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try
    End Function

End Class