Imports System.IO
Imports FirstFloor.ModernUI.Presentation
Imports FirstFloor.ModernUI.Windows.Controls
Imports OfficeOpenXml
Imports OfficeOpenXml.Table
Imports WarehouseReports.Content
Imports WarehouseReports.Enums

Public MustInherit Class BaseReportVM

    Public Property Name As String
    Public ReadOnly Property Lable As String
        Get
            Return Split(Name, ".")(0)
        End Get
    End Property
    Public Property NewFile As FileInfo
    Public Property Worksheets As ExcelWorksheets
    Public Property Linq As Linq
    Public Property CountDataSheet As Integer


    Public ReadOnly Property CmdOpenReport As ICommand = New RelayCommand(AddressOf OpenReportExecuteAsync)
    Public Async Sub OpenReportExecuteAsync(parameter As Object)
        Try
            Await Task.Factory.StartNew(Sub()
                                            NewFile = GetInBaseFileInfo(GetInBaseDirectoryInfo("Reports"), Name)
                                            CreateReport()
                                            Process.Start(NewFile.FullName)
                                        End Sub)
        Catch ex As Exception
            Dim Dlg As New ModernDialog With {.Title = "Ошибка", .Content = New ErrorMessage(ex)}
            Dlg.ShowDialog()
            Return
        End Try
    End Sub


#Region "Pivot"

#Region "All"
    Public Sub AddPivotAllTasksByHour()
        Dim DataSheetName = GetDataSheetName()
        Dim WorksheetHelper = AddWorksheet(DataSheetName)
        Dim PivotData = WorksheetHelper.LoadFromCollection(Linq.GetBy_Date_Employee_Hour, True)

        WorksheetHelper.Sheet.Column(PivotData.Start.Column).Style.Numberformat.Format = "DD.MM.YYYY"

        With AddWorksheet("Все задачи по часам")
            .LoadVBACode("PivotAllTasksByHour.txt", DataSheetName)
            .AddPivotTable(3, 1, PivotData, "Все задачи по часам", TableStyles.Dark4)
            .PivotAddRowField("XDate")
            .PivotAddRowField("Employee")
            .PivotAddColumnFields("HourNum")
            .PivotAddDataField("Qty")
        End With
    End Sub


    Public Sub AddPivotAllTasksByDay()
        Dim DataSheetName = GetDataSheetName()
        Dim PivotData = AddWorksheet(DataSheetName).LoadFromCollection(Linq.GetBy_Day_Gang_TaskType, True)

        With AddWorksheet("Все задачи по дням")
            .LoadVBACode("PivotAll.txt", DataSheetName)
            .AddPivotTable(3, 1, PivotData, "Все задачи по дням", TableStyles.Light8)
            .PivotAddRowField("XDate")
            .PivotAddRowField("Gang")
            .PivotAddColumnFields("SystemTaskType")
            .PivotAddDataField("Qty")
        End With
    End Sub


    Public Sub AddPivotAllTasksByWeek()
        Dim DataSheetName = GetDataSheetName()
        Dim PivotData = AddWorksheet(DataSheetName).LoadFromCollection(Linq.GetBy_Week_Gang_TaskType, True)

        With AddWorksheet("Все задачи по неделям")
            .LoadVBACode("PivotAll.txt", DataSheetName)
            .AddPivotTable(3, 1, PivotData, "Все задачи по неделям", TableStyles.Light8)
            .PivotAddRowField("Week")
            .PivotAddRowField("Gang")
            .PivotAddColumnFields("SystemTaskType")
            .PivotAddDataField("Qty")
        End With
    End Sub


    Public Sub AddPivotAllTasksByMonth()
        Dim DataSheetName = GetDataSheetName()
        Dim PivotData = AddWorksheet(DataSheetName).LoadFromCollection(Linq.GetBy_Month_Gang_TaskType, True)

        With AddWorksheet("Все задачи по месяцам")
            .LoadVBACode("PivotAll.txt", DataSheetName)
            .AddPivotTable(3, 1, PivotData, "Все задачи по месяцам", TableStyles.Light8)
            .PivotAddRowField("Month")
            .PivotAddRowField("Gang")
            .PivotAddColumnFields("SystemTaskType")
            .PivotAddDataField("Qty")
        End With
    End Sub
#End Region


#Region "Placement"
    Public Sub AddPivotPlacementByDay()
        Dim DataSheetName = GetDataSheetName()
        Dim PivotData = AddWorksheet(DataSheetName).LoadFromCollection(Linq.GetBy_Day_Gang_Zone, True)

        With AddWorksheet("Размещение по дням")
            '.LoadVBACode("AllPivot.txt", DataSheetName)
            .AddPivotTable(3, 1, PivotData, "Размещение по дням", TableStyles.Light8)
            .PivotAddRowField("XDate")
            .PivotAddRowField("Gang")
            .PivotAddColumnFields("Zone")
            .PivotAddDataField("Qty")
        End With
    End Sub
#End Region


#Region "Resupply & Movement"
    Public Sub AddPivotMoveByDay(taskType As SystemTaskType)
        Dim SheetName As String
        Select Case taskType
            Case SystemTaskType.Resupply
                SheetName = "Пополнение по дням"
            Case SystemTaskType.ManualResupply
                SheetName = "Ручное пополнение по дням"
            Case SystemTaskType.Movement
                SheetName = "Перемещение по дням"
            Case Else
                Return
        End Select

        Dim DataSheetName = GetDataSheetName()
        Dim PivotData = AddWorksheet(DataSheetName).LoadFromCollection(Linq.GetBy_Day_Gang_Direction(taskType), True)

        With AddWorksheet(SheetName)
            '.LoadVBACode("AllPivot.txt", DataSheetName)
            .AddPivotTable(3, 1, PivotData, SheetName, TableStyles.Light8)
            .PivotAddRowField("XDate")
            .PivotAddRowField("Gang")
            .PivotAddColumnFields("Direction")
            .PivotAddDataField("Qty")
        End With
    End Sub
#End Region


#Region "Pick"
    Public Sub AddPivotPickByDay()
        Dim DataSheetName = GetDataSheetName()
        Dim DataSheet = AddWorksheet(DataSheetName)
        Dim PivotData = DataSheet.LoadFromCollection(Linq.GetBy_Day_Gang_Group_Zone, True)

        With DataSheet
            .LoadFromCollection(Linq.GetBy_Day_Main_Group({500}), True)
            .LoadFromCollection(Linq.GetBy_Day_Main_Group({200}), True)
            .LoadFromCollection(Linq.GetBy_Day_Main_Group_UpDown({200}, False), True)
            .LoadFromCollection(Linq.GetBy_Day_UpDown(True), True)
        End With

        With AddWorksheet("Отбор по дням")
            .LoadVBACode("PivotPick.txt", DataSheet.Sheet.Name)
            .AddPivotTable(3, 1, PivotData, "Отбор по дням", TableStyles.Light8)
            .PivotAddRowField("XDate")
            .PivotAddRowField("Gang")
            .PivotAddColumnFields("Group")
            .PivotAddColumnFields("Zone")
            .PivotAddDataField("Qty")
        End With
    End Sub


    Public Sub AddPivotPickByWeek()
        Dim DataSheetName = GetDataSheetName()
        Dim DataSheet = AddWorksheet(DataSheetName)
        Dim PivotData = DataSheet.LoadFromCollection(Linq.GetBy_Week_Gang_Group_Zone, True)

        With DataSheet
            .LoadFromCollection(Linq.GetBy_Week_Main_Group({500}), True)
            .LoadFromCollection(Linq.GetBy_Week_Main_Group({200}), True)
            .LoadFromCollection(Linq.GetBy_Week_MainGroup_UpDown({200}, False), True)
            .LoadFromCollection(Linq.GetBy_Week_UpDown(True), True)
        End With

        With AddWorksheet("Отбор по неделям")
            .LoadVBACode("PivotPick.txt", DataSheetName)
            .AddPivotTable(3, 1, PivotData, "Отбор по неделям", TableStyles.Light8)
            .PivotAddRowField("Week")
            .PivotAddRowField("Gang")
            .PivotAddColumnFields("Group")
            .PivotAddColumnFields("Zone")
            .PivotAddDataField("Qty")
        End With
    End Sub


    Public Sub AddPivotPickByMonth()
        Dim DataSheetName = GetDataSheetName()
        Dim DataSheet = AddWorksheet(DataSheetName)
        Dim PivotData = DataSheet.LoadFromCollection(Linq.GetBy_Month_Gang_Group_Zone, True)

        With DataSheet
            .LoadFromCollection(Linq.GetBy_Month_Main_Group({500}), True)
            .LoadFromCollection(Linq.GetBy_Month_Main_Group({200}), True)
            .LoadFromCollection(Linq.GetBy_Month_Main_Group_UpDown({200}, False), True)
            .LoadFromCollection(Linq.GetBy_Month_UpDown(True), True)
        End With

        With AddWorksheet("Отбор по месяцам")
            .LoadVBACode("PivotPick.txt", DataSheetName)
            .AddPivotTable(3, 1, PivotData, "Отбор по месяцам", TableStyles.Light8)
            .PivotAddRowField("Month")
            .PivotAddRowField("Gang")
            .PivotAddColumnFields("Group")
            .PivotAddColumnFields("Zone")
            .PivotAddDataField("Qty")
        End With
    End Sub


    Public Sub AddPickPerHour(containsZone As Integer?())
        Dim DataSheetName = GetDataSheetName()
        Dim DataSheet = AddWorksheet(DataSheetName)

        Dim Worksheet = AddWorksheet("Почасовой отбор")
        Worksheet.LoadVBACode("PickPerHour.txt", DataSheetName)

        Dim List = Linq.GetBy_Date_Employee_Hour(containsZone)

        If List.Count = 0 Then Return

        Dim FirstDate = List.First.XDate
        Dim LastDate = List.Last.XDate
        Dim Row = 1

        While FirstDate <= LastDate
            Dim DayList = List.Where(Function(t) t.XDate = FirstDate).ToList
            If DayList.Sum(Function(i) i.Qty) = 0 Then
                FirstDate = FirstDate.AddDays(1)
                Continue While
            End If

            With Worksheet
                .Sheet.Cells(Row, 27).Value = "Максимальное"
                .Sheet.Cells(Row, 28).Value = DayList.Max(Function(t) t.Qty)
                .Sheet.Cells(Row + 1, 27).Value = "Среднее"
                .Sheet.Cells(Row + 1, 28).Value = CInt(DayList.Average(Function(t) t.Qty))
            End With

            Dim Employee = DayList.First.Employee
            For h = 0 To 23
                Dim HourNum = h
                If DayList.Where(Function(i) i.Employee = Employee AndAlso i.HourNum = HourNum).FirstOrDefault Is Nothing Then
                    DayList.Add(New Date_Employee_Hour With {.XDate = FirstDate, .Employee = Employee, .HourNum = HourNum})
                End If
            Next

            Dim PivotData = DataSheet.LoadFromCollection(DayList, True)

            Dim PivotName = $"{FirstDate.ToShortDateString} - {WeekdayName(FirstDate.DayOfWeek, True)}"
            With Worksheet
                .AddPivotTable(Row, 1, PivotData, PivotName, TableStyles.Medium8)
                .PivotAddRowField("Employee")
                .PivotAddColumnFields("HourNum")
                .PivotAddDataField("Qty")
            End With

            Row += DayList.Select(Function(i) i.Employee).Distinct.Count + 4
            FirstDate = FirstDate.AddDays(1)
        End While
    End Sub
#End Region

#End Region


#Region "Pipeline"
    Public Sub AddPipeline()
        With AddWorksheet("Мониторинг")
            .LoadVBACode("Pipeline.txt")
            .Row += 1
            .LoadFromCollection(Linq.GetPipelineMonitoring, True)
        End With
    End Sub
#End Region


#Region "Charts"

#Region "Pick"
    Public Sub AddPickCharts()
        Dim Worksheet = AddWorksheet("Диаграммы")
        With Worksheet
            .LoadVBACode("MainReportCharts.txt")

            .AddColumnClusteredChart(Linq.GetAvgBy_Hour, "Среднее кол-во задач в час", False)
            .AddDoughnutChart(Linq.GetAvgBy_Weekday, "Среднее кол-во задач по дням", True)
            .AddDoughnutChart(Linq.GetBy_MainGroup, "Отбор по группам", True)
            .AddDoughnutChart(Linq.GetBy_Zone({500}), "Отбор с мезонина", True, endChartLine:=True)

            .AddDoughnutChart(Linq.GetBy_UpDown({200}), "Отбор 200 верх/низ")
            .AddDoughnutChart(Linq.GetBy_MainGroup({200, 500}), "Отбор по группам 200-500")
            .AddDoughnutChart(Linq.GetBy_Zone(New Integer?() {203, 213}), "Отбор железа")
            .AddDoughnutChart(Linq.GetBy_Zone({100}), "Отбор 100 группы")
            .AddDoughnutChart(Linq.GetBy_Zone({300}), "Отбор 300 группы")
            .AddDoughnutChart(Linq.GetBy_Zone({100}, New Integer?() {101}), "Отбор бухт")
            .AddDoughnutChart(Linq.GetBy_Zone({300}, New Integer?() {311}), "Отбор барабанов", endChartLine:=True)

            .AddSingleIndicatorChart(Linq.GetMechanization, "КМ")
            .AddPieChart(Linq.GetBy_TaskType, "Задачи по типам")
        End With

        Dim List = Linq.GetBy_Date_Hour

        If List.Count = 0 Then Return

        Dim FirstDate = List.First.XDate
        Dim LastDate = List.Last.XDate
        Dim Row = 42
        Dim Column = 2

        While FirstDate <= LastDate
            Dim DayList = List.Where(Function(t) t.XDate = FirstDate).ToList
            If DayList.Sum(Function(i) i.Qty) = 0 Then
                FirstDate = FirstDate.AddDays(1)
                Continue While
            End If

            For h = 0 To 23
                Dim HourNum = h
                If DayList.SingleOrDefault(Function(i) i.HourNum = HourNum) Is Nothing Then
                    DayList.Add(New Date_Hour With {.HourNum = HourNum})
                End If
            Next
            DayList = DayList.OrderBy(Function(i) i.HourNum).ToList

            Dim Address = GetAddress(Row, Column)
            Worksheet.AddColumnClusteredChart(DayList, Address, $"{FirstDate.ToShortDateString} Кол-во задач в час", Row - 1, 0, False)
            Row += 13
            Column = If(Column = 2, 4, 2)
            FirstDate = FirstDate.AddDays(1)
        End While


        Dim List2 = Linq.GetAvgBy_Week_Hour
        Row = 42
        Column = 15
        For n = List2.First.WeekNum To List2.Last.WeekNum
            Dim WeekNum = n
            Dim WeekList = List2.Where(Function(t) t.WeekNum = WeekNum).ToList
            If WeekList.Sum(Function(i) i.Avg) = 0 Then
                Continue For
            End If

            For h = 0 To 23
                Dim HourNum = h
                If WeekList.SingleOrDefault(Function(i) i.HourNum = HourNum) Is Nothing Then
                    WeekList.Add(New AvgByWeek_Hour With {.HourNum = HourNum})
                End If
            Next
            WeekList = WeekList.OrderBy(Function(i) i.HourNum).ToList

            Dim Address = GetAddress(Row, Column)
            Worksheet.AddColumnClusteredChart(WeekList, Address, $"{WeekNum} неделя среднее кол-во задач в час", Row - 1, 13, False)
            Row += 13
            Column = If(Column = 15, 17, 15)
        Next
    End Sub
#End Region

#End Region


    Private Function GetDataSheetName() As String
        CountDataSheet += 1
        Return $"Data{CountDataSheet}"
    End Function


    Public Function AddWorksheet(name As String) As WorksheetHelper
        Return New WorksheetHelper With {.Sheet = Worksheets.Add(name)}
    End Function


    Public MustOverride Sub CreateReport()


    Public Function GetAddress(row As Integer, column As Integer) As String
        Return ExcelAddress.GetAddress(row, column)
    End Function


    Public Function GetAddress(fromRow As Integer, fromColumn As Integer, toRow As Integer, toColumn As Integer) As String
        Return ExcelAddress.GetAddress(fromRow, fromColumn, toRow, toColumn)
    End Function

End Class