Imports FirstFloor.ModernUI.Windows.Controls
Imports OfficeOpenXml
Imports WarehouseReports.Content

Public Class MainReportVM
    Inherits BaseReportVM

    Public Sub New()
        Name = "Основной отчет.xlsm"
    End Sub


    Public Overrides Sub CreateReport()
        Using Package As New ExcelPackage(NewFile)
            Worksheets = Package.Workbook.Worksheets

            If Package.Workbook.VbaProject Is Nothing Then
                Package.Workbook.CreateVBAProject()
            End If
            Dim Linq As New Linq

            Dim NamePart = $"{Linq.StartDate.Year} {MonthName(Linq.StartDate.Month, True)}"

#Region "Pivot"

            OverwriteWorksheet("Данные")
            Dim DataRange = Worksheet.Cells("A1").LoadFromCollection(Linq.GetTasksByDayGangGroupZone, True)
            Worksheet.Cells("F1").LoadFromCollection(Linq.GetTasksByDayMainGroup({500}), True)
            Worksheet.Cells("J1").LoadFromCollection(Linq.GetTasksByDayMainGroup({200}), True)
            Worksheet.Cells("N1").LoadFromCollection(Linq.GetTasksByDayMainGroupUpDown({200}, False), True)
            Worksheet.Cells("S1").LoadFromCollection(Linq.GetTasksByDayUpDown(True), True)

            OverwriteWorksheet($"{NamePart} Задачи")
            Worksheet.CodeModule.Code = ReadTextFile(GetDirectoryInfo("VBA-Code"), "MainReportPivot.txt")
            Dim PivotTable = Worksheet.PivotTables.Add(Worksheet.Cells("A3"), DataRange, "Задачи по дням")
            PivotTable.RowFields.Add(PivotTable.Fields("Дата")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.RowFields.Add(PivotTable.Fields("Смена")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.ColumnFields.Add(PivotTable.Fields("Группа")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.ColumnFields.Add(PivotTable.Fields("Склад")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.DataFields.Add(PivotTable.Fields("Задачи"))
            PivotTable.TableStyle = Table.TableStyles.Light8
#End Region

#Region "Charts"
            OverwriteWorksheet($"{NamePart} Диаграммы")
            Worksheet.CodeModule.Code = ReadTextFile(GetDirectoryInfo("VBA-Code"), "MainReportCharts.txt")
            Worksheet.Cells("A1").LoadFromCollection(Linq.GetTasksByGroupAZonePickingNorm, True)

            CreateColumnClusteredChart(Linq.GetAvgTasksByHour, "G1", "Среднее кол-во задач в час", 0, 6, 640, 300, False)
            CreateDoughnutChart(Linq.GetAvgTasksByWeekday, "I1", "Среднее кол-во задач по дням", 0, 16, 448, 300)
            CreateDoughnutChart(Linq.GetTasksByMainGroup, "K1", "Отбор по группам", 0, 23, 448, 300)

            CreateDoughnutChart(Linq.GetTasksByZone({500}), "U1", "Отбор с мезонина", 15, 6, 256, 240)
            CreateDoughnutChart(Linq.GetTasksByUpDown({200}), "S1", "Отбор 200 верх/низ", 15, 10, 256, 240)
            CreateDoughnutChart(Linq.GetTasksByMainGroup({200, 500}), "W1", "Отбор по группам 200-500", 15, 14, 256, 240)
            CreateDoughnutChart(Linq.GetTasksByZone(New Integer?() {203, 213}), "Q1", "Отбор железа", 15, 18, 256, 240)
            CreateDoughnutChart(Linq.GetTasksByZone({100}), "O1", "Отбор 100 группы", 15, 22, 256, 240)
            CreateDoughnutChart(Linq.GetTasksByZone({300}), "M1", "Отбор 300 группы", 15, 26, 256, 240)

            CreateSingleIndicatorChart(Linq.GetMechanization, "Y1", "КМ", 27, 6, 256, 240, True)

            CreateDoughnutChart(Linq.GetTasksByZone({100}, New Integer?() {101}), "AA1", "Отбор бухт", 27, 22, 256, 240)
            CreateDoughnutChart(Linq.GetTasksByZone({300}, New Integer?() {311}), "AC1", "Отбор барабанов", 27, 26, 256, 240)


            Dim List = Linq.GetTasksByDateHour()
            If List.Count > 0 Then
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
                    CreateColumnClusteredChart(DayList, Address, $"{FirstDate.ToShortDateString} Кол-во задач в час", Row - 1, 0, 640, 260, False)
                    Row += 13
                    Column = If(Column = 2, 4, 2)
                    FirstDate = FirstDate.AddDays(1)
                End While


                Dim List2 = Linq.GetAvgTasksByWeekNumHour()
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
                            WeekList.Add(New AvgTasksByWeekNumHour With {.Час = HourNum})
                        End If
                    Next
                    WeekList = WeekList.OrderBy(Function(i) i.Час).ToList

                    Dim Address = New ExcelAddress(Row, Column, Row, Column).Address
                    CreateColumnClusteredChart(WeekList, Address, $"{WeekNum} неделя среднее кол-во задач в час", Row - 1, 13, 640, 260, False)
                    Row += 13
                    Column = If(Column = 15, 17, 15)
                Next
            End If
#End Region

            Linq.Dispose()
            Try
                Package.Save()
            Catch ex As Exception
                Dim Dlg As New ModernDialog With {.Title = "Ошибка", .Content = New ErrorMessage(ex)}
                Dlg.ShowDialog()
            End Try
        End Using
    End Sub

End Class