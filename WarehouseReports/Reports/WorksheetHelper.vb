Imports OfficeOpenXml
Imports OfficeOpenXml.Drawing.Chart
Imports OfficeOpenXml.Table
Imports OfficeOpenXml.Table.PivotTable

Public Class WorksheetHelper

    Private _RowPosition As Integer
    Private _ColumnPosition As Integer


    Public Sub New()
        Row = 1
        Column = 1
    End Sub


    Public Property Sheet As ExcelWorksheet
    Public Property CurrentPivotTable As ExcelPivotTable
    Public Property Row As Integer
    Public Property Column As Integer
    Public Property RowPosition As Integer
        Get
            Return _RowPosition
        End Get
        Set
            If IsEndChartLine Then
                _RowPosition = Value
            End If
        End Set
    End Property
    Public Property ColumnPosition As Integer
        Get
            Return _ColumnPosition
        End Get
        Set
            If IsEndChartLine Then
                _ColumnPosition = 0
            Else
                _ColumnPosition = Value
            End If
        End Set
    End Property
    Public Property IsEndChartLine As Boolean
    Public ReadOnly Property CurrentAddress As String
        Get
            Return GetAddress(Row, Column)
        End Get
    End Property


#Region "Charts"
    Public Sub AddDoughnutChart(Of T)(collection As IEnumerable(Of T), chartTitle As String,
                                      Optional large As Boolean = False, Optional endChartLine As Boolean = False)
        IsEndChartLine = endChartLine
        Dim DataRange = LoadFromCollection(collection, True)
        Dim Chart = CType(Sheet.Drawings.AddChart(chartTitle, eChartType.Doughnut), ExcelDoughnutChart)
        Chart.Title.Text = chartTitle
        Chart.Title.Font.Size = 12
        Chart.Title.Font.Bold = True
        Chart.SetPosition(RowPosition, 0, ColumnPosition, 0)
        If large Then
            Chart.SetSize(384, 300)
            ColumnPosition += 6
            RowPosition += 15
        Else
            Chart.SetSize(256, 240)
            ColumnPosition += 4
            RowPosition += 12
        End If
        Dim NameAddress = GetAddress(DataRange.Start.Row + 1, DataRange.Start.Column, DataRange.End.Row, DataRange.Start.Column)
        Dim ValueAddress = GetAddress(DataRange.Start.Row + 1, DataRange.End.Column, DataRange.End.Row, DataRange.End.Column)
        Chart.Series.Add(ValueAddress, NameAddress)
        Chart.DataLabel.ShowPercent = True
        Chart.DataLabel.ShowValue = True
        Chart.Border.Fill.Transparancy = 100
        IsEndChartLine = False
    End Sub


    Public Sub AddPieChart(Of T)(collection As IEnumerable(Of T), chartTitle As String,
                                 Optional large As Boolean = False, Optional endChartLine As Boolean = False)
        IsEndChartLine = endChartLine
        Dim DataRange = LoadFromCollection(collection, True)
        Dim Chart = CType(Sheet.Drawings.AddChart(chartTitle, eChartType.Pie), ExcelPieChart)
        Chart.Title.Text = chartTitle
        Chart.Title.Font.Size = 12
        Chart.Title.Font.Bold = True
        Chart.SetPosition(RowPosition, 0, ColumnPosition, 0)
        If large Then
            Chart.SetSize(384, 300)
            ColumnPosition += 6
            RowPosition += 15
        Else
            Chart.SetSize(256, 240)
            ColumnPosition += 4
            RowPosition += 12
        End If
        Dim NameAddress = GetAddress(DataRange.Start.Row + 1, DataRange.Start.Column, DataRange.End.Row, DataRange.Start.Column)
        Dim ValueAddress = GetAddress(DataRange.Start.Row + 1, DataRange.End.Column, DataRange.End.Row, DataRange.End.Column)
        Chart.Series.Add(ValueAddress, NameAddress)
        Chart.DataLabel.ShowPercent = True
        Chart.DataLabel.ShowValue = True
        Chart.Border.Fill.Transparancy = 100
        IsEndChartLine = False
    End Sub


    Public Sub AddColumnClusteredChart(Of T)(collection As IEnumerable(Of T), chartTitle As String, legend As Boolean,
                                             Optional endChartLine As Boolean = False)
        IsEndChartLine = endChartLine
        Dim DataRange = LoadFromCollection(collection, True)
        Dim Chart = Sheet.Drawings.AddChart(chartTitle, eChartType.ColumnClustered)
        Chart.Title.Text = chartTitle
        Chart.SetPosition(RowPosition, 0, ColumnPosition, 0)
        Chart.SetSize(640, 300)
        ColumnPosition += 10
        RowPosition += 15
        Dim NameAddress = GetAddress(DataRange.Start.Row + 1, DataRange.Start.Column, DataRange.End.Row, DataRange.Start.Column)
        Dim ValueAddress = GetAddress(DataRange.Start.Row + 1, DataRange.End.Column, DataRange.End.Row, DataRange.End.Column)
        Chart.Series.Add(ValueAddress, NameAddress)
        If Not legend Then Chart.Legend.Remove()
        Chart.Border.Fill.Transparancy = 100
        IsEndChartLine = False
    End Sub


    Public Sub AddColumnClusteredChart(Of T)(collection As IEnumerable(Of T), dataAddress As String, chartTitle As String,
                                             rowPosition As Integer, columnPosition As Integer, legend As Boolean,
                                             Optional endChartLine As Boolean = False)
        IsEndChartLine = endChartLine
        Dim DataRange = Sheet.Cells(dataAddress).LoadFromCollection(collection, True)
        Dim Chart = Sheet.Drawings.AddChart(chartTitle, eChartType.ColumnClustered)
        Chart.Title.Text = chartTitle
        Chart.SetPosition(rowPosition, 0, columnPosition, 0)
        Chart.SetSize(640, 260)
        Dim NameAddress = GetAddress(DataRange.Start.Row + 1, DataRange.Start.Column, DataRange.End.Row, DataRange.Start.Column)
        Dim ValueAddress = GetAddress(DataRange.Start.Row + 1, DataRange.End.Column, DataRange.End.Row, DataRange.End.Column)
        Chart.Series.Add(ValueAddress, NameAddress)
        If Not legend Then Chart.Legend.Remove()
        Chart.Border.Fill.Transparancy = 100
        IsEndChartLine = False
    End Sub


    Public Sub AddSingleIndicatorChart(Of T)(collection As IEnumerable(Of T), chartTitle As String,
                                             Optional endChartLine As Boolean = False)
        IsEndChartLine = endChartLine
        Dim DataRange = LoadFromCollection(collection, False)
        DataRange.Style.Numberformat.Format = "0.0%"
        Dim Chart = CType(Sheet.Drawings.AddChart(chartTitle, eChartType.BarClustered), ExcelBarChart)
        Chart.Title.Text = chartTitle
        Chart.SetPosition(RowPosition, 0, ColumnPosition, 0)
        Chart.SetSize(256, 240)
        ColumnPosition += 4
        RowPosition += 12
        Dim ValueAddress = GetAddress(DataRange.End.Row, DataRange.Start.Column)
        Chart.Series.Add(ValueAddress, "EEE1").Header = "Показатель"
        Dim NormAddress = GetAddress(DataRange.End.Row, DataRange.End.Column)
        Chart.Series.Add(NormAddress, "EEE1").Header = "Норматив"
        Chart.Legend.Position = eLegendPosition.Bottom
        Chart.DataLabel.ShowValue = True
        Chart.Border.Fill.Transparancy = 100
        IsEndChartLine = False
    End Sub
#End Region


#Region "Pivot"
    Public Function AddPivotTable(row As Integer, column As Integer, pivotDataRange As ExcelRangeBase, pivotName As String, style As TableStyles) As ExcelPivotTable
        Dim Result = Sheet.PivotTables.Add(Sheet.Cells(row, column), pivotDataRange, pivotName)
        Result.TableStyle = style
        CurrentPivotTable = Result
        Return Result
    End Function


    Public Sub PivotAddRowField(name As String)
        CurrentPivotTable.RowFields.Add(CurrentPivotTable.Fields(name)).Sort = eSortType.Ascending
    End Sub


    Public Sub PivotAddColumnFields(name As String)
        CurrentPivotTable.ColumnFields.Add(CurrentPivotTable.Fields(name)).Sort = eSortType.Ascending
    End Sub


    Public Sub PivotAddDataField(name As String)
        CurrentPivotTable.DataFields.Add(CurrentPivotTable.Fields(name))
    End Sub
#End Region


    Public Function LoadFromCollection(Of T)(Collection As IEnumerable(Of T), headers As Boolean) As ExcelRangeBase
        Dim Result = Sheet.Cells(CurrentAddress).LoadFromCollection(Collection, headers)
        Column += GetType(T).GetProperties.Count
        Return Result
    End Function


    Public Sub LoadVBACode(fileName As String, sheetName As String)
        Sheet.CodeModule.Code = String.Format(ReadTextFile(GetDirectoryInfo("VBA-Code"), fileName), sheetName)
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