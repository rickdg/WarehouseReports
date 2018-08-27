Imports OfficeOpenXml
Imports OfficeOpenXml.Drawing.Chart
Imports OfficeOpenXml.Table.PivotTable

Public Class WorksheetHelper

    Public Sub New()
        Row = 1
        Column = 1
    End Sub


    Public Property Sheet As ExcelWorksheet
    Public Property Row As Integer
    Public Property Column As Integer
    Public ReadOnly Property Address As String
        Get
            Return GetAddress(Row, Column)
        End Get
    End Property


    Public Function LoadFromCollection(Of T)(Collection As IEnumerable(Of T), headers As Boolean) As ExcelRangeBase
        Dim Result = Sheet.Cells(Address).LoadFromCollection(Collection, headers)
        Column += GetType(T).GetProperties.Count
        Return Result
    End Function


    Public Function AddPivotTable(row As Integer, column As Integer, pivotDataRange As ExcelRangeBase, pivotName As String) As ExcelPivotTable
        Return Sheet.PivotTables.Add(Sheet.Cells(row, column), pivotDataRange, pivotName)
    End Function


    Public Sub AddDoughnutChart(Of T)(collection As IEnumerable(Of T), chartTitle As String,
                                         rowPosition As Integer, columnPosition As Integer, width As Integer, height As Integer)
        Dim DataRange = LoadFromCollection(collection, True)
        Dim Chart = CType(Sheet.Drawings.AddChart(chartTitle, eChartType.Doughnut), ExcelDoughnutChart)
        Chart.Title.Text = chartTitle
        Chart.Title.Font.Size = 12
        Chart.Title.Font.Bold = True
        Chart.SetPosition(rowPosition, 0, columnPosition, 0)
        Chart.SetSize(width, height)
        Dim NameAddress = GetAddress(DataRange.Start.Row + 1, DataRange.Start.Column, DataRange.End.Row, DataRange.Start.Column)
        Dim ValueAddress = GetAddress(DataRange.Start.Row + 1, DataRange.End.Column, DataRange.End.Row, DataRange.End.Column)
        Chart.Series.Add(ValueAddress, NameAddress)
        Chart.DataLabel.ShowPercent = True
        Chart.DataLabel.ShowValue = True
    End Sub


    Public Sub AddColumnClusteredChart(Of T)(collection As IEnumerable(Of T), chartTitle As String,
                                                rowPosition As Integer, columnPosition As Integer, width As Integer, height As Integer,
                                                legend As Boolean)
        Dim DataRange = LoadFromCollection(collection, True)
        Dim Chart = Sheet.Drawings.AddChart(chartTitle, eChartType.ColumnClustered)
        Chart.Title.Text = chartTitle
        Chart.SetPosition(rowPosition, 0, columnPosition, 0)
        Chart.SetSize(width, height)
        Dim NameAddress = GetAddress(DataRange.Start.Row + 1, DataRange.Start.Column, DataRange.End.Row, DataRange.Start.Column)
        Dim ValueAddress = GetAddress(DataRange.Start.Row + 1, DataRange.End.Column, DataRange.End.Row, DataRange.End.Column)
        Chart.Series.Add(ValueAddress, NameAddress)
        If Not legend Then Chart.Legend.Remove()
    End Sub


    Public Sub AddColumnClusteredChart(Of T)(collection As IEnumerable(Of T), dataAddress As String, chartTitle As String,
                                          rowPosition As Integer, columnPosition As Integer, width As Integer, height As Integer,
                                          legend As Boolean)
        Dim DataRange = Sheet.Cells(dataAddress).LoadFromCollection(collection, True)
        Dim Chart = Sheet.Drawings.AddChart(chartTitle, eChartType.ColumnClustered)
        Chart.Title.Text = chartTitle
        Chart.SetPosition(rowPosition, 0, columnPosition, 0)
        Chart.SetSize(width, height)
        Dim NameAddress = GetAddress(DataRange.Start.Row + 1, DataRange.Start.Column, DataRange.End.Row, DataRange.Start.Column)
        Dim ValueAddress = GetAddress(DataRange.Start.Row + 1, DataRange.End.Column, DataRange.End.Row, DataRange.End.Column)
        Chart.Series.Add(ValueAddress, NameAddress)
        If Not legend Then Chart.Legend.Remove()
    End Sub


    Public Sub AddSingleIndicatorChart(Of T)(collection As IEnumerable(Of T), chartTitle As String,
                                                rowPosition As Integer, columnPosition As Integer, width As Integer, height As Integer)
        Dim DataRange = LoadFromCollection(collection, False)
        Dim Chart = CType(Sheet.Drawings.AddChart(chartTitle, eChartType.BarClustered), ExcelBarChart)
        Chart.Title.Text = chartTitle
        Chart.SetPosition(rowPosition, 0, columnPosition, 0)
        Chart.SetSize(width, height)
        Dim ValueAddress = GetAddress(DataRange.End.Row, DataRange.Start.Column, DataRange.End.Row, DataRange.Start.Column)
        Chart.Series.Add(ValueAddress, "EEE1").Header = "Показатель"
        Dim NormAddress = GetAddress(DataRange.End.Row, DataRange.End.Column, DataRange.End.Row, DataRange.End.Column)
        Chart.Series.Add(NormAddress, "EEE1").Header = "Норматив"
        Chart.Legend.Position = eLegendPosition.Bottom
        Chart.DataLabel.ShowValue = True
    End Sub


    Public Sub LoadVBACode(fileName As String)
        Sheet.CodeModule.Code = ReadTextFile(GetDirectoryInfo("VBA-Code"), fileName)
    End Sub


    Public Function GetAddress(row As Integer, column As Integer) As String
        Return ExcelAddress.GetAddress(row, column)
    End Function


    Public Function GetAddress(fromRow As Integer, fromColumn As Integer, toRow As Integer, toColumn As Integer) As String
        Return ExcelAddress.GetAddress(fromRow, fromColumn, toRow, toColumn)
    End Function

End Class