Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Text
Imports System.Threading
Imports FirstFloor.ModernUI.Windows.Controls
Imports Microsoft.Win32
Imports WarehouseReports.Enums
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
                    Dim TaskType As SystemTaskType
                    Select Case Table.Columns.Count
                        Case 3 ' Загрузка в док
                            SQL = GetLoadScript(Table.Name)
                            TaskType = SystemTaskType.Load
                        Case 4 ' Получение
                            SQL = GetReceiptScript(Table.Name)
                            TaskType = SystemTaskType.Receipt
                        Case Else
                            SQL = GetUnionScript(Table.Name)
                            SQL2 = GetExtraDataScript(Table.Name)
                            TaskType = SystemTaskType.Pick
                    End Select
                    Dim CheckResult = CheckColumns(TaskType, Table.Columns)
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

    End Class
End Namespace