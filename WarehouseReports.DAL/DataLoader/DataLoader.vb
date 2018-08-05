Imports System.Data.SqlClient

Public Class DataLoader

    Public Shared Function LoadTasks(fileName As String) As Boolean
        Dim Excel = New ExcelTable(fileName)
        Dim Sql = "SELECT MIN(F10), F8, F5
                   FROM [{0}]
                   WHERE F1 = 'Независимая задача' AND F10 IS NOT NULL
                   GROUP BY F2, F3, F4, F5, F6, F7, F8, F9
                   UNION ALL
                   SELECT MIN(F10), F8, F5
                   FROM [{0}]
                   WHERE F1 = 'Дочерняя задача' AND F10 IS NOT NULL
                   GROUP BY F4, F5, F6, F7, F8, F9"

        If Not Excel.Fill(0, Sql) Then
            Return False
        End If

        Try
            Using Connection As New SqlConnection(My.Settings.WarehouseDataConnectionString)
                Connection.Open()
                Using Command = Connection.CreateCommand()
                    Dim CommandText = "dbo.LoadTasks"
                    Dim Parameter = "@ExcelTasks"
                    Dim TypeName = "TaskExcelTable"
                    Command.CommandTimeout = 1800
                    Command.CommandText = CommandText
                    Command.CommandType = CommandType.StoredProcedure
                    Command.Parameters.Add(Parameter, SqlDbType.Structured).TypeName = TypeName
                    Command.Parameters(Parameter).Value = Excel
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
