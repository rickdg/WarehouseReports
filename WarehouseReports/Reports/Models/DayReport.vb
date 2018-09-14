Imports OfficeOpenXml
Imports WarehouseReports.Enums

Public Class DayReport
    Inherits BaseReportVM

    Public Sub New()
        Name = "По дням.xlsm"
    End Sub


    Public Overrides Sub CreateReport()
        Using Package As New ExcelPackage(NewFile)
            Linq = New Linq
            Worksheets = Package.Workbook.Worksheets
            CountDataSheet = 0
            Package.Workbook.CreateVBAProject()

            AddPivotAllTasksByDay()
            AddPivotPickByDay()
            AddPivotAllTasksByHour()
            AddPivotPlacementByDay()
            AddPivotMoveByDay(SystemTaskType.Resupply)
            AddPivotMoveByDay(SystemTaskType.ManualResupply)
            AddPivotMoveByDay(SystemTaskType.Movement)

            Linq.Dispose()
            Package.Save()
        End Using
    End Sub

End Class