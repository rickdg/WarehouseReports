Imports OfficeOpenXml
Imports OfficeOpenXml.Table
Imports OfficeOpenXml.Table.PivotTable

Public Class MainReportVM
    Inherits BaseReportVM

    Public Sub New()
        Name = "Основной отчет.xlsm"
    End Sub


    Public Overrides Sub CreateReport()
        Using Package As New ExcelPackage(NewFile)
            Dim Linq As New Linq

            Worksheets = Package.Workbook.Worksheets
            Package.Workbook.CreateVBAProject()

#Region "Pivot Tasks By Day"
            Dim SheetDataTasksByDay = AddWorksheet("Data1")
            Dim PivotData1 = SheetDataTasksByDay.LoadFromCollection(Linq.GetBy_Day_Gang_Group_Zone, True)

            If PivotData1.Rows = 2 Then
                Linq.Dispose()
                Package.Save()
                Return
            End If

            SheetDataTasksByDay.LoadFromCollection(Linq.GetBy_Day_Main_Group({500}), True)
            SheetDataTasksByDay.LoadFromCollection(Linq.GetBy_Day_Main_Group({200}), True)
            SheetDataTasksByDay.LoadFromCollection(Linq.GetBy_Day_Main_Group_UpDown({200}, False), True)
            SheetDataTasksByDay.LoadFromCollection(Linq.GetBy_Day_UpDown(True), True)

            Dim SheetTasksByDay = AddWorksheet("Задачи по дням")
            SheetTasksByDay.LoadVBACode("Pivot.txt", SheetDataTasksByDay.Sheet.Name)
            SheetTasksByDay.AddPivotTable(3, 1, PivotData1, "Задачи по дням", TableStyles.Light8)
            SheetTasksByDay.PivotAddRowField("XDate", eSortType.Ascending)
            SheetTasksByDay.PivotAddRowField("Gang", eSortType.Ascending)
            SheetTasksByDay.PivotAddColumnFields("Group", eSortType.Ascending)
            SheetTasksByDay.PivotAddColumnFields("Zone", eSortType.Ascending)
            SheetTasksByDay.PivotAddDataField("Qty")
#End Region


#Region "Pivot Tasks By Week"
            Dim SheetDataTasksByWeek = AddWorksheet("Data2")
            Dim PivotData2 = SheetDataTasksByWeek.LoadFromCollection(Linq.GetBy_Week_Gang_Group_Zone, True)
            SheetDataTasksByWeek.LoadFromCollection(Linq.GetBy_Week_Main_Group({500}), True)
            SheetDataTasksByWeek.LoadFromCollection(Linq.GetBy_Week_Main_Group({200}), True)
            SheetDataTasksByWeek.LoadFromCollection(Linq.GetBy_Week_MainGroup_UpDown({200}, False), True)
            SheetDataTasksByWeek.LoadFromCollection(Linq.GetBy_Week_UpDown(True), True)

            Dim SheetTasksByWeek = AddWorksheet("Задачи по неделям")
            SheetTasksByWeek.LoadVBACode("Pivot.txt", SheetDataTasksByWeek.Sheet.Name)
            SheetTasksByWeek.AddPivotTable(3, 1, PivotData2, "Задачи по неделям", TableStyles.Light8)
            SheetTasksByWeek.PivotAddRowField("Week", eSortType.Ascending)
            SheetTasksByWeek.PivotAddRowField("Gang", eSortType.Ascending)
            SheetTasksByWeek.PivotAddColumnFields("Group", eSortType.Ascending)
            SheetTasksByWeek.PivotAddColumnFields("Zone", eSortType.Ascending)
            SheetTasksByWeek.PivotAddDataField("Qty")
#End Region


#Region "Pivot Tasks By Month"
            Dim SheetDataTasksByMonth = AddWorksheet("Data3")
            Dim PivotData3 = SheetDataTasksByMonth.LoadFromCollection(Linq.GetBy_Month_Gang_Group_Zone, True)
            SheetDataTasksByMonth.LoadFromCollection(Linq.GetBy_Month_Main_Group({500}), True)
            SheetDataTasksByMonth.LoadFromCollection(Linq.GetBy_Month_Main_Group({200}), True)
            SheetDataTasksByMonth.LoadFromCollection(Linq.GetBy_Month_Main_Group_UpDown({200}, False), True)
            SheetDataTasksByMonth.LoadFromCollection(Linq.GetBy_Month_UpDown(True), True)

            Dim SheetTasksByMonth = AddWorksheet("Задачи по месяцам")
            SheetTasksByMonth.LoadVBACode("Pivot.txt", SheetDataTasksByMonth.Sheet.Name)
            SheetTasksByMonth.AddPivotTable(3, 1, PivotData3, "Задачи по месяцам", TableStyles.Light8)
            SheetTasksByMonth.PivotAddRowField("Month", eSortType.Ascending)
            SheetTasksByMonth.PivotAddRowField("Gang", eSortType.Ascending)
            SheetTasksByMonth.PivotAddColumnFields("Group", eSortType.Ascending)
            SheetTasksByMonth.PivotAddColumnFields("Zone", eSortType.Ascending)
            SheetTasksByMonth.PivotAddDataField("Qty")
#End Region


#Region "Charts"
            Dim SheetCharts = AddWorksheet("Диаграммы")
            SheetCharts.LoadVBACode("MainReportCharts.txt")

            SheetCharts.AddColumnClusteredChart(Linq.GetAvgBy_Hour, "Среднее кол-во задач в час", False)
            SheetCharts.AddDoughnutChart(Linq.GetAvgBy_Weekday, "Среднее кол-во задач по дням", True)
            SheetCharts.AddDoughnutChart(Linq.GetBy_MainGroup, "Отбор по группам", True)
            SheetCharts.AddDoughnutChart(Linq.GetBy_Zone({500}), "Отбор с мезонина", True, endChartLine:=True)

            SheetCharts.AddDoughnutChart(Linq.GetBy_UpDown({200}), "Отбор 200 верх/низ")
            SheetCharts.AddDoughnutChart(Linq.GetBy_MainGroup({200, 500}), "Отбор по группам 200-500")
            SheetCharts.AddDoughnutChart(Linq.GetBy_Zone(New Integer?() {203, 213}), "Отбор железа")
            SheetCharts.AddDoughnutChart(Linq.GetBy_Zone({100}), "Отбор 100 группы")
            SheetCharts.AddDoughnutChart(Linq.GetBy_Zone({300}), "Отбор 300 группы")
            SheetCharts.AddDoughnutChart(Linq.GetBy_Zone({100}, New Integer?() {101}), "Отбор бухт")
            SheetCharts.AddDoughnutChart(Linq.GetBy_Zone({300}, New Integer?() {311}), "Отбор барабанов", endChartLine:=True)

            SheetCharts.AddSingleIndicatorChart(Linq.GetMechanization, "КМ")

            Dim List = Linq.GetBy_Date_Hour
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

                Dim Address = GetAddress(Row, Column, Row, Column)
                SheetCharts.AddColumnClusteredChart(DayList, Address, $"{FirstDate.ToShortDateString} Кол-во задач в час", Row - 1, 0, False)
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

                Dim Address = GetAddress(Row, Column, Row, Column)
                SheetCharts.AddColumnClusteredChart(WeekList, Address, $"{WeekNum} неделя среднее кол-во задач в час", Row - 1, 13, False)
                Row += 13
                Column = If(Column = 15, 17, 15)
            Next
#End Region


#Region "Pipeline"
            Dim SheetPipeline = AddWorksheet("Мониторинг")
            SheetPipeline.LoadVBACode("Pipeline.txt")
            SheetPipeline.Row += 1
            SheetPipeline.LoadFromCollection(Linq.GetPipelineMonitoring, True)
#End Region


#Region "Pick per hour 520"
            Dim SheetDataPick520 = AddWorksheet("Data4")
            Dim SheetPick520 = AddWorksheet("Почасовой отбор 520")
            SheetPick520.LoadVBACode("PickPerHour.txt", "Data4")
            Dim List3 = Linq.GetBy_Date_Employee_Hour(New Integer?() {520})
            FirstDate = List3.First.XDate
            LastDate = List3.Last.XDate
            Row = 1
            While FirstDate <= LastDate
                Dim DayList = List3.Where(Function(t) t.XDate = FirstDate).ToList
                If DayList.Sum(Function(i) i.Qty) = 0 Then
                    FirstDate = FirstDate.AddDays(1)
                    Continue While
                End If

                SheetPick520.Sheet.Cells(Row, 27).Value = "Максимальное"
                SheetPick520.Sheet.Cells(Row, 28).Value = DayList.Max(Function(t) t.Qty)
                SheetPick520.Sheet.Cells(Row + 1, 27).Value = "Среднее"
                SheetPick520.Sheet.Cells(Row + 1, 28).Value = CInt(DayList.Average(Function(t) t.Qty))

                Dim Employee = DayList.First.Employee
                For h = 0 To 23
                    Dim HourNum = h
                    If DayList.Where(Function(i) i.Employee = Employee AndAlso i.HourNum = HourNum).FirstOrDefault Is Nothing Then
                        DayList.Add(New Date_Employee_Hour With {.XDate = FirstDate, .Employee = Employee, .HourNum = HourNum})
                    End If
                Next

                Dim PivotData4 = SheetDataPick520.LoadFromCollection(DayList, True)

                Dim PivotName = $"{FirstDate.ToShortDateString} - {WeekdayName(FirstDate.DayOfWeek, True)}"
                SheetPick520.AddPivotTable(Row, 1, PivotData4, PivotName, TableStyles.Medium8)
                SheetPick520.PivotAddRowField("Employee", eSortType.Ascending)
                SheetPick520.PivotAddColumnFields("HourNum", eSortType.Ascending)
                SheetPick520.PivotAddDataField("Qty")

                Row += DayList.Select(Function(i) i.Employee).Distinct.Count + 4
                FirstDate = FirstDate.AddDays(1)
            End While
#End Region


            Linq.Dispose()
            Package.Save()
        End Using
    End Sub

End Class