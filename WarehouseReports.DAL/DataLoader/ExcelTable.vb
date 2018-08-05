Imports System.Data.OleDb

Public Class ExcelTable
    Inherits DataTable

    Private Connection As OleDbConnection

    ''' <summary>
    ''' Инициализирует новый экземпляр ExcelTable
    ''' </summary>
    ''' <param name="excelFile">Путь к файлу</param>
    Public Sub New(excelFile As String)
        MyBase.New
        Connection = New OleDbConnection With {
        .ConnectionString = New OleDbConnectionStringBuilder(";Extended Properties=""Excel 12.0 Xml;HDR=NO"";") With {
        .Provider = "Microsoft.ACE.OLEDB.12.0",
        .DataSource = excelFile}.ConnectionString}
    End Sub

    ''' <summary>
    ''' Выполняет запрос к указанному листу Excel и заполняет таблицу результатом запроса
    ''' </summary>
    ''' <param name="sheetIndex">Индекс листа Excel к которому будет выполняться запрос</param>
    ''' <param name="query">SQL запрос</param>
    Public Function Fill(sheetIndex As Integer, query As String) As Boolean
        Try
            Connection.Open()
            Dim SheetName = Replace(Connection.GetSchema("Tables").Rows(sheetIndex)("TABLE_NAME").ToString, "'", "")
            query = String.Format(query, SheetName)
            Dim RowCount = New OleDbDataAdapter(query, Connection).Fill(Me)
            Connection.Close()
            Return RowCount > 0
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try
    End Function
End Class