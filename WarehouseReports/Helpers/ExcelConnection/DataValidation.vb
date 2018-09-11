Imports System.Data
Imports System.Data.OleDb
Imports FirstFloor.ModernUI.Windows.Controls
Imports Microsoft.Win32
Imports OfficeOpenXml
Imports OfficeOpenXml.Table
Imports WarehouseReports.Content
Imports WarehouseReports.Enums

Namespace ExcelConnection
    Public Module DataValidation

        Public Async Sub ViewDataAsync(loadType As LoadType)
            Try
                Await Task.Factory.StartNew(Sub() Execute(loadType))
            Catch ex As Exception
                Dim Dlg As New ModernDialog With {.Title = "Ошибка", .Content = New ErrorMessage(ex)}
                Dlg.ShowDialog()
            End Try
        End Sub


        Private Sub Execute(loadType As LoadType)
            Dim DialogWindow As New OpenFileDialog With {.Title = "Выбрать файл"}
            If Not DialogWindow.ShowDialog Then Exit Sub

            Using Connection As New OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={DialogWindow.FileName};Extended Properties='Excel 12.0;HDR=YES';")
                Connection.Open()
                Dim Table = (From Row In Connection.GetSchema("Columns")
                             Group New Column(Row.Field(Of String)("COLUMN_NAME"), Row.Field(Of Integer)("DATA_TYPE"))
                              By TableName = Row.Field(Of String)("TABLE_NAME").Trim("'"c) Into Columns = ToList
                             Where TableName.EndsWith("$")
                             Select New Table(TableName) With {.Columns = Columns}).First

                Dim SQL = GetPreviewScript(loadType, Table.Name)
                Dim CheckResult = CheckColumns(loadType, Table.Columns)
                If CheckResult <> "" Then Throw New Exception(CheckResult)

                Dim ExcelTable As New DataTable
                Using Adapter As New OleDbDataAdapter(SQL, Connection)
                    Dim RecordCount = Adapter.Fill(ExcelTable)
                    If RecordCount = 0 Then Throw New Exception("Запрос вернул пустые строки")
                End Using

                Dim NewFile = GetInBaseFileInfo(GetInBaseDirectoryInfo("Validation"), $"{loadType.ToString}.xlsx")
                Using Package As New ExcelPackage(NewFile)
                    Dim Sheet = Package.Workbook.Worksheets.Add(loadType.ToString)
                    Dim DataRange = Sheet.Cells("A1").LoadFromDataTable(ExcelTable, True, TableStyles.Light9)
                    'Sheet.Column(2 * DataRange.End.Column - 9).Style.Numberformat.Format = "DD.MM.YYYY - HHч"
                    Sheet.Cells.AutoFitColumns()
                    Package.Save()
                End Using
                Process.Start(NewFile.FullName)
            End Using
        End Sub

    End Module
End Namespace