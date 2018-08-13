Imports OfficeOpenXml

Public Class MainReportVM
    Inherits BaseReportVM

    Public Sub New()
        Name = "Основной отчет.xlsm"
    End Sub


    Public Overrides Sub CreateReport()
        Using Package As New ExcelPackage(NewFile)
            Package.Workbook.CreateVBAProject()
            Dim Linq As New Linq

#Region "Pivot"
            Worksheet = Package.Workbook.Worksheets.Add("Данные")
            Dim DataRange = Worksheet.Cells("A1").LoadFromCollection(Linq.GetTasksByDayGangZone, True)
            Worksheet.Cells("F1").LoadFromCollection(Linq.GetTasksByDayGroupA({500}), True)
            Worksheet.Cells("J1").LoadFromCollection(Linq.GetTasksByDayGroupA({200}), True)
            Worksheet.Cells("N1").LoadFromCollection(Linq.GetTasksByDayGroupAUpDown({200}, False), True)
            Worksheet.Cells("S1").LoadFromCollection(Linq.GetTasksByDayUpDown(False), True)

            Worksheet = Package.Workbook.Worksheets.Add("Задачи по дням")
            Worksheet.CodeModule.Code = GetCodeModule(GetDirectoryInfo("VBA-Code"), "TasksByDay.txt")
            Dim PivotTable = Worksheet.PivotTables.Add(Worksheet.Cells("A3"), DataRange, "Задачи по дням")
            PivotTable.RowFields.Add(PivotTable.Fields("Дата")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.RowFields.Add(PivotTable.Fields("Смена")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.ColumnFields.Add(PivotTable.Fields("Склад")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.DataFields.Add(PivotTable.Fields("Задачи"))
#End Region

#Region "Charts"
            Worksheet = Package.Workbook.Worksheets.Add("Диаграммы")
            DataRange = Worksheet.Cells("A1").LoadFromCollection(Linq.GetTasksByGroupAZone, True)

            CreateDoughnutChart(Linq.GetAvgTasksByWeekday, "E1", "Среднее кол-во задач по дням", 0, 6, 320, 240)
            CreateDoughnutChart(Linq.GetTasksByGroupA, "L1", "Отбор по группам", 0, 13, 320, 240)
            CreateDoughnutChart(Linq.GetTasksByZoneGroupA({300}), "S1", "Отбор барабанов", 0, 20, 320, 240)

            CreateDoughnutChart(Linq.GetTasksByZoneGroupA({100}), "E13", "Отбор бухт", 12, 6, 320, 240)
            CreateDoughnutChart(Linq.GetTasksByZone(New Integer?() {203, 213}), "L13", "Отбор железа", 12, 13, 320, 240)
            CreateDoughnutChart(Linq.GetTasksByUpDownGroupA({200}), "S13", "Отбор 200 верх/низ", 12, 20, 320, 240)

            CreateDoughnutChart(Linq.GetTasksByZoneGroupA({500}), "E25", "Отбор с мезонина", 24, 6, 320, 240)
            CreateDoughnutChart(Linq.GetTasksByGroupA({200, 500}), "L25", "Отбор по группам 200-500", 24, 13, 320, 240)
            CreateColumnStackedChart(Linq.GetMechanization, "S25", "КМ", 24, 20, 320, 240, False)

            CreateColumnStackedChart(Linq.GetAvgTasksByHour, "A37", "Среднее кол-во задач в час", 36, 2, 640, 320, False)
#End Region

            Linq.Dispose()
            Package.Save()
        End Using
    End Sub

End Class