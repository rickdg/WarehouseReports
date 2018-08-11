Imports System.IO
Imports Microsoft.Win32
Imports OfficeOpenXml
Imports OfficeOpenXml.Drawing.Chart

Public MustInherit Class BaseReport

    Public Property Name As String
    Public ReadOnly Property Lable As String
        Get
            Return Split(Name, ".")(0)
        End Get
    End Property
    Public Property NewFile As FileInfo
    Public Property Worksheet As ExcelWorksheet


    Public ReadOnly Property CmdOpenReport As ICommand = New RelayCommand(AddressOf OpenReportExecute)
    Public Overridable Sub OpenReportExecute(obj As Object)
        NewFile = GetFileInfo(GetDirectoryInfo("Reports"), Name)
        CreateReport()
        Process.Start(NewFile.FullName)
    End Sub
    Public ReadOnly Property CmdSaveReport As ICommand = New RelayCommand(AddressOf SaveReportExecute)
    Public Overridable Sub SaveReportExecute(obj As Object)
        Dim SplitName = Split(Name, ".")
        Dim SaveDlg As New SaveFileDialog With {
            .FileName = SplitName(0),
            .Filter = $"{SplitName(1)} files (*.{SplitName(1)})|*.{SplitName(1)}"}
        If SaveDlg.ShowDialog Then
            NewFile = New FileInfo(SaveDlg.FileName)
            If NewFile.Exists Then NewFile.Delete()
            CreateReport()
        End If
    End Sub


    Public Overridable Sub CreateReport()
    End Sub


    Public Sub CreateDoughnutChart(Of T)(collection As IEnumerable(Of T), dataAddress As String, chartTitle As String,
                                     rowPosition As Integer, columnPosition As Integer, width As Integer, height As Integer)
        Dim DataRange = Worksheet.Cells(dataAddress).LoadFromCollection(collection, True)
        Dim Chart = CType(Worksheet.Drawings.AddChart(chartTitle, eChartType.Doughnut), ExcelDoughnutChart)
        Chart.Title.Text = chartTitle
        Chart.Title.Font.Size = 12
        Chart.Title.Font.Bold = True
        Chart.SetPosition(rowPosition, 0, columnPosition, 0)
        Chart.SetSize(width, height)
        Dim ValueAddress = New ExcelAddress(DataRange.Start.Row + 1, DataRange.End.Column, DataRange.End.Row, DataRange.End.Column)
        Dim NameAddress = New ExcelAddress(DataRange.Start.Row + 1, DataRange.Start.Column, DataRange.End.Row, DataRange.Start.Column)
        Chart.Series.Add(ValueAddress.Address, NameAddress.Address)
        Chart.DataLabel.ShowPercent = True
        Chart.DataLabel.ShowValue = True
        Chart.DataLabel.ShowLeaderLines = True
    End Sub


    Public Sub CreateColumnStackedChart(Of T)(collection As IEnumerable(Of T), dataAddress As String, chartTitle As String,
                                               rowPosition As Integer, columnPosition As Integer, width As Integer, height As Integer)
        Dim DataRange = Worksheet.Cells(dataAddress).LoadFromCollection(collection, True)
        Dim Chart = Worksheet.Drawings.AddChart(chartTitle, eChartType.ColumnStacked)
        Chart.Title.Text = chartTitle
        Chart.SetPosition(rowPosition, 0, columnPosition, 0)
        Chart.SetSize(width, height)
        Dim ValueAddress = New ExcelAddress(DataRange.Start.Row + 1, DataRange.End.Column, DataRange.End.Row, DataRange.End.Column)
        Dim NameAddress = New ExcelAddress(DataRange.Start.Row + 1, DataRange.Start.Column, DataRange.End.Row, DataRange.Start.Column)
        Chart.Series.Add(ValueAddress.Address, NameAddress.Address)
    End Sub

End Class