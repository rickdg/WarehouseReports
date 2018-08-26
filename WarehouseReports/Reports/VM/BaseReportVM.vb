Imports System.IO
Imports FirstFloor.ModernUI.Presentation
Imports FirstFloor.ModernUI.Windows.Controls
Imports Microsoft.Win32
Imports OfficeOpenXml
Imports OfficeOpenXml.Drawing.Chart
Imports WarehouseReports.Content
Imports WarehouseReports.Pages

Public MustInherit Class BaseReportVM

    Public Property Name As String
    Public ReadOnly Property Lable As String
        Get
            Return Split(Name, ".")(0)
        End Get
    End Property
    Public Property NewFile As FileInfo
    Public Property CurrentWorksheet As ExcelWorksheet
    Public Property Worksheets As ExcelWorksheets


    Public ReadOnly Property CmdOpenReport As ICommand = New RelayCommand(AddressOf OpenReportExecute)
    Public Overridable Sub OpenReportExecute(obj As Object)
        NewFile = GetFileInfo(GetDirectoryInfo("Reports"), Name)
        If NewFile.Exists Then
            Try
                NewFile.Delete()
            Catch ex As Exception
                Dim Dlg As New ModernDialog With {.Title = "Ошибка", .Content = New ErrorMessage(ex)}
                Dlg.ShowDialog()
                Return
            End Try
        End If
        CreateReport()
        Process.Start(NewFile.FullName)
    End Sub
    Public ReadOnly Property CmdSaveReport As ICommand = New RelayCommand(AddressOf SaveReportExecute)
    Public Overridable Sub SaveReportExecute(obj As Object)
        Dim Extension = Split(Name, ".")(1)
        Dim NamePart = $"{PageReports.Model.StartDate.Year} {MonthName(PageReports.Model.StartDate.Month)}"
        Dim SaveDlg As New SaveFileDialog With {
            .OverwritePrompt = False,
            .FileName = $"{NamePart} {Lable}",
            .Filter = $"{Extension} files (*.{Extension})|*.{Extension}"}
        If SaveDlg.ShowDialog Then
            NewFile = New FileInfo(SaveDlg.FileName)
            Try
                NewFile.Delete()
                CreateReport()
                Process.Start(NewFile.DirectoryName)
            Catch ex As Exception
                Dim Dlg As New ModernDialog With {.Title = "Ошибка", .Content = New ErrorMessage(ex)}
                Dlg.ShowDialog()
            End Try
        End If
    End Sub


    Public Function AddWorksheet(name As String) As ExcelWorksheet
        CurrentWorksheet = Worksheets.Add(name)
        Return CurrentWorksheet
    End Function


    Public Function OverwriteWorksheet(name As String) As ExcelWorksheet
        If Worksheets.SingleOrDefault(Function(w) w.Name = name) IsNot Nothing Then
            Worksheets.Delete(name)
        End If
        CurrentWorksheet = Worksheets.Add(name)
        Return CurrentWorksheet
    End Function


    Public Overridable Sub CreateReport()
    End Sub


    Public Sub CreateDoughnutChart(Of T)(collection As IEnumerable(Of T), dataAddress As String, chartTitle As String,
                                     rowPosition As Integer, columnPosition As Integer, width As Integer, height As Integer)
        Dim DataRange = CurrentWorksheet.Cells(dataAddress).LoadFromCollection(collection, True)
        Dim Chart = CType(CurrentWorksheet.Drawings.AddChart(chartTitle, eChartType.Doughnut), ExcelDoughnutChart)
        Chart.Title.Text = chartTitle
        Chart.Title.Font.Size = 12
        Chart.Title.Font.Bold = True
        Chart.SetPosition(rowPosition, 0, columnPosition, 0)
        Chart.SetSize(width, height)
        Dim NameAddress = New ExcelAddress(DataRange.Start.Row + 1, DataRange.Start.Column, DataRange.End.Row, DataRange.Start.Column)
        Dim ValueAddress = New ExcelAddress(DataRange.Start.Row + 1, DataRange.End.Column, DataRange.End.Row, DataRange.End.Column)
        Chart.Series.Add(ValueAddress.Address, NameAddress.Address)
        Chart.DataLabel.ShowPercent = True
        Chart.DataLabel.ShowValue = True
    End Sub


    Public Sub CreateColumnClusteredChart(Of T)(collection As IEnumerable(Of T), dataAddress As String, chartTitle As String,
                                              rowPosition As Integer, columnPosition As Integer, width As Integer, height As Integer,
                                              legend As Boolean)
        Dim DataRange = CurrentWorksheet.Cells(dataAddress).LoadFromCollection(collection, True)
        Dim Chart = CurrentWorksheet.Drawings.AddChart(chartTitle, eChartType.ColumnClustered)
        Chart.Title.Text = chartTitle
        Chart.SetPosition(rowPosition, 0, columnPosition, 0)
        Chart.SetSize(width, height)
        Dim NameAddress = New ExcelAddress(DataRange.Start.Row + 1, DataRange.Start.Column, DataRange.End.Row, DataRange.Start.Column)
        Dim ValueAddress = New ExcelAddress(DataRange.Start.Row + 1, DataRange.End.Column, DataRange.End.Row, DataRange.End.Column)
        Chart.Series.Add(ValueAddress.Address, NameAddress.Address)
        If Not legend Then Chart.Legend.Remove()
    End Sub


    Public Sub CreateSingleIndicatorChart(Of T)(collection As IEnumerable(Of T), dataAddress As String, chartTitle As String,
                                                rowPosition As Integer, columnPosition As Integer, width As Integer, height As Integer)
        Dim DataRange = CurrentWorksheet.Cells(dataAddress).LoadFromCollection(collection, True)
        Dim Chart = CType(CurrentWorksheet.Drawings.AddChart(chartTitle, eChartType.BarClustered), ExcelBarChart)
        Chart.Title.Text = chartTitle
        Chart.SetPosition(rowPosition, 0, columnPosition, 0)
        Chart.SetSize(width, height)
        Dim NameValueAddress = New ExcelAddress(DataRange.Start.Row, DataRange.Start.Column, DataRange.Start.Row, DataRange.Start.Column)
        Dim ValueAddress = New ExcelAddress(DataRange.End.Row, DataRange.Start.Column, DataRange.End.Row, DataRange.Start.Column)
        Chart.Series.Add(ValueAddress.Address, NameValueAddress.Address).Header = "Показатель"
        Dim NameNormAddress = New ExcelAddress(DataRange.Start.Row, DataRange.End.Column, DataRange.Start.Row, DataRange.End.Column)
        Dim NormAddress = New ExcelAddress(DataRange.End.Row, DataRange.End.Column, DataRange.End.Row, DataRange.End.Column)
        Chart.Series.Add(NormAddress.Address, NameNormAddress.Address).Header = "Норматив"
        Chart.Legend.Position = eLegendPosition.Bottom
        Chart.DataLabel.ShowValue = True
    End Sub

End Class