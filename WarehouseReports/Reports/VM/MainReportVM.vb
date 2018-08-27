Imports OfficeOpenXml

Public Class MainReportVM
    Inherits BaseReportVM

    Public Sub New()
        Name = "Основной отчет.xlsm"
    End Sub


    Public Overrides Sub CreateReport()
        Using Package As New ExcelPackage(NewFile)
            Dim Linq As New Linq
            Dim NamePart = $"{Linq.StartDate.Month}-{Linq.StartDate.Year}"

            Worksheets = Package.Workbook.Worksheets
            Package.Workbook.CreateVBAProject()


#Region "Data"
            Dim DataSheet = AddWorksheet("Данные")
            Dim PivotDataRange = DataSheet.LoadFromCollection(Linq.GetTasksByDayGangGroupZone, True)
            DataSheet.LoadFromCollection(Linq.GetTasksByDayMainGroup({500}), True)
            DataSheet.LoadFromCollection(Linq.GetTasksByDayMainGroup({200}), True)
            DataSheet.LoadFromCollection(Linq.GetTasksByDayMainGroupUpDown({200}, False), True)
            DataSheet.LoadFromCollection(Linq.GetTasksByDayUpDown(True), True)
#End Region


#Region "Pivot"
            Dim PivotSheet = AddWorksheet($"{NamePart} Задачи")
            PivotSheet.LoadVBACode("MainReportPivot.txt")
            Dim PivotTable = PivotSheet.AddPivotTable(3, 1, PivotDataRange, "Задачи по дням")
            PivotTable.RowFields.Add(PivotTable.Fields("Дата")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.RowFields.Add(PivotTable.Fields("Смена")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.ColumnFields.Add(PivotTable.Fields("Группа")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.ColumnFields.Add(PivotTable.Fields("Склад")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.DataFields.Add(PivotTable.Fields("Задачи"))
            PivotTable.TableStyle = Table.TableStyles.Light8
#End Region


#Region "Charts"
            Dim ChartsSheet = AddWorksheet($"{NamePart} Диаграммы")
            ChartsSheet.LoadVBACode("MainReportCharts.txt")
            ChartsSheet.LoadFromCollection(Linq.GetTasksByMainGroupZonePickingNorm, True)

            ChartsSheet.Column += 2
            ChartsSheet.AddColumnClusteredChart(Linq.GetAvgTasksByHour, "Среднее кол-во задач в час", 0, 6, 640, 300, False)
            ChartsSheet.AddDoughnutChart(Linq.GetAvgTasksByWeekday, "Среднее кол-во задач по дням", 0, 16, 448, 300)
            ChartsSheet.AddDoughnutChart(Linq.GetTasksByMainGroup, "Отбор по группам", 0, 23, 448, 300)

            ChartsSheet.AddDoughnutChart(Linq.GetTasksByZone({500}), "Отбор с мезонина", 15, 6, 256, 240)
            ChartsSheet.AddDoughnutChart(Linq.GetTasksByUpDown({200}), "Отбор 200 верх/низ", 15, 10, 256, 240)
            ChartsSheet.AddDoughnutChart(Linq.GetTasksByMainGroup({200, 500}), "Отбор по группам 200-500", 15, 14, 256, 240)
            ChartsSheet.AddDoughnutChart(Linq.GetTasksByZone(New Integer?() {203, 213}), "Отбор железа", 15, 18, 256, 240)
            ChartsSheet.AddDoughnutChart(Linq.GetTasksByZone({100}), "Отбор 100 группы", 15, 22, 256, 240)
            ChartsSheet.AddDoughnutChart(Linq.GetTasksByZone({300}), "Отбор 300 группы", 15, 26, 256, 240)

            ChartsSheet.AddSingleIndicatorChart(Linq.GetMechanization, "КМ", 27, 6, 256, 240)

            ChartsSheet.AddDoughnutChart(Linq.GetTasksByZone({100}, New Integer?() {101}), "Отбор бухт", 27, 22, 256, 240)
            ChartsSheet.AddDoughnutChart(Linq.GetTasksByZone({300}, New Integer?() {311}), "Отбор барабанов", 27, 26, 256, 240)

            Dim List = Linq.GetTasksByDateHour
            If List.Count = 0 Then
                Linq.Dispose()
                Package.Save()
                Return
            End If

            Dim FirstDate = List.First.Дата
            Dim LastDate = List.Last.Дата
            Dim Row = 40
            Dim Column = 2
            While FirstDate <= LastDate
                Dim DayList = List.Where(Function(t) t.Дата = FirstDate).ToList
                If DayList.Sum(Function(i) i.Задачи) = 0 Then
                    FirstDate = FirstDate.AddDays(1)
                    Continue While
                End If

                For h = 0 To 23
                    Dim HourNum = h
                    If DayList.SingleOrDefault(Function(i) i.Час = HourNum) Is Nothing Then
                        DayList.Add(New TasksByDateHour With {.Час = HourNum})
                    End If
                Next
                DayList = DayList.OrderBy(Function(i) i.Час).ToList

                Dim Address = New ExcelAddress(Row, Column, Row, Column).Address
                ChartsSheet.AddColumnClusteredChart(DayList, Address, $"{FirstDate.ToShortDateString} Кол-во задач в час", Row - 1, 0, 640, 260, False)
                Row += 13
                Column = If(Column = 2, 4, 2)
                FirstDate = FirstDate.AddDays(1)
            End While


            Dim List2 = Linq.GetAvgTasksByWeekHour
            Row = 40
            Column = 15
            For n = List2.First.WeekNum To List2.Last.WeekNum
                Dim WeekNum = n
                Dim WeekList = List2.Where(Function(t) t.WeekNum = WeekNum).ToList
                If WeekList.Sum(Function(i) i.СреднееКолвоЗадач) = 0 Then
                    Continue For
                End If

                For h = 0 To 23
                    Dim HourNum = h
                    If WeekList.SingleOrDefault(Function(i) i.Час = HourNum) Is Nothing Then
                        WeekList.Add(New AvgTasksByWeekHour With {.Час = HourNum})
                    End If
                Next
                WeekList = WeekList.OrderBy(Function(i) i.Час).ToList

                Dim Address = New ExcelAddress(Row, Column, Row, Column).Address
                ChartsSheet.AddColumnClusteredChart(WeekList, Address, $"{WeekNum} неделя среднее кол-во задач в час", Row - 1, 13, 640, 260, False)
                Row += 13
                Column = If(Column = 15, 17, 15)
            Next
#End Region


#Region "Pipeline"
            Dim PipelineSheet = AddWorksheet($"{NamePart} Мониторинг")
            PipelineSheet.LoadVBACode("MainPipeline.txt")
            PipelineSheet.Row += 1
            PipelineSheet.LoadFromCollection(Linq.GetPipelineMonitoring, True)
#End Region


#Region "Pick520"
            Dim Pick520Sheet = AddWorksheet($"{NamePart} Почасовой отбор 520")
            Pick520Sheet.LoadVBACode("MainPick520.txt")
            Dim List3 = Linq.GetTasksByDateEmployeeHour(New Integer?() {520})
            FirstDate = List3.First.Дата
            LastDate = List3.Last.Дата
            Row = 1
            While FirstDate <= LastDate
                Dim DayList = List3.Where(Function(t) t.Дата = FirstDate).ToList
                If DayList.Sum(Function(i) i.Задачи) = 0 Then
                    FirstDate = FirstDate.AddDays(1)
                    Continue While
                End If

                Pick520Sheet.Sheet.Cells(Row, 27).Value = "Максимальное"
                Pick520Sheet.Sheet.Cells(Row, 28).Value = DayList.Max(Function(t) t.Задачи)
                Pick520Sheet.Sheet.Cells(Row + 1, 27).Value = "Среднее"
                Pick520Sheet.Sheet.Cells(Row + 1, 28).Value = CInt(DayList.Average(Function(t) t.Задачи))

                Dim Employee = DayList.First.Работник
                For h = 0 To 23
                    Dim HourNum = h
                    If DayList.Where(Function(i) i.Работник = Employee AndAlso i.Час = HourNum).FirstOrDefault Is Nothing Then
                        DayList.Add(New TasksByDateEmployeeHour With {.Дата = FirstDate, .Работник = Employee, .Час = HourNum})
                    End If
                Next

                PivotDataRange = DataSheet.LoadFromCollection(DayList, True)

                Dim PivotName = $"{FirstDate.ToShortDateString} - {WeekdayName(FirstDate.DayOfWeek, True)}"
                PivotTable = Pick520Sheet.AddPivotTable(Row, 1, PivotDataRange, PivotName)
                PivotTable.RowFields.Add(PivotTable.Fields("Работник")).Sort = Table.PivotTable.eSortType.Ascending
                PivotTable.ColumnFields.Add(PivotTable.Fields("Час")).Sort = Table.PivotTable.eSortType.Ascending
                PivotTable.DataFields.Add(PivotTable.Fields("Задачи"))
                PivotTable.TableStyle = Table.TableStyles.Medium8

                Row += DayList.Select(Function(i) i.Работник).Distinct.Count + 4
                FirstDate = FirstDate.AddDays(1)
            End While
#End Region


            Linq.Dispose()
            Package.Save()
        End Using
    End Sub

End Class