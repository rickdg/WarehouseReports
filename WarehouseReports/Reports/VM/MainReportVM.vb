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
            Worksheet.CodeModule.Code = GetCodeModule(GetDirectoryInfo("VBA-Code"), "MainReportPivot.txt")
            Dim PivotTable = Worksheet.PivotTables.Add(Worksheet.Cells("A3"), DataRange, "Задачи по дням")
            PivotTable.RowFields.Add(PivotTable.Fields("Дата")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.RowFields.Add(PivotTable.Fields("Смена")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.ColumnFields.Add(PivotTable.Fields("Склад")).Sort = Table.PivotTable.eSortType.Ascending
            PivotTable.DataFields.Add(PivotTable.Fields("Задачи"))
#End Region

#Region "Charts"
            Worksheet = Package.Workbook.Worksheets.Add("Диаграммы")
            Worksheet.CodeModule.Code = GetCodeModule(GetDirectoryInfo("VBA-Code"), "MainReportCharts.txt")
            Worksheet.Cells("A1").LoadFromCollection(Linq.GetTasksByGroupAZonePickingNorm, True)
            Worksheet.Cells("D:D").Style.Numberformat.Format = "0%"

            CreateColumnStackedChart(Linq.GetAvgTasksByHour, "G1", "Среднее кол-во задач в час", 0, 6, 640, 300, False)
            CreateDoughnutChart(Linq.GetAvgTasksByWeekday, "I1", "Среднее кол-во задач по дням", 0, 16, 448, 300)
            CreateDoughnutChart(Linq.GetTasksByGroupA, "K1", "Отбор по группам", 0, 23, 448, 300)

            CreateDoughnutChart(Linq.GetTasksByZoneGroupA({500}), "U1", "Отбор с мезонина", 15, 6, 256, 240)
            CreateDoughnutChart(Linq.GetTasksByZoneGroupA({100}), "O1", "Отбор бухт", 15, 10, 256, 240)
            CreateDoughnutChart(Linq.GetTasksByZoneGroupA({300}), "M1", "Отбор барабанов", 15, 14, 256, 240)
            CreateDoughnutChart(Linq.GetTasksByZone(New Integer?() {203, 213}), "Q1", "Отбор железа", 15, 18, 256, 240)
            CreateDoughnutChart(Linq.GetTasksByUpDownGroupA({200}), "S1", "Отбор 200 верх/низ", 15, 22, 256, 240)
            CreateDoughnutChart(Linq.GetTasksByGroupA({200, 500}), "W1", "Отбор по группам 200-500", 15, 26, 256, 240)

            CreateColumnStackedChart(Linq.GetMechanization, "Y1", "КМ", 27, 6, 320, 240, False)
#End Region

            Linq.Dispose()
            Package.Save()
        End Using
    End Sub

End Class