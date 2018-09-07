﻿Imports OfficeOpenXml

Public Class MainReportVM
    Inherits BaseReportVM

    Public Sub New()
        Name = "Основной.xlsm"
    End Sub


    Public Overrides Sub CreateReport()
        Using Package As New ExcelPackage(NewFile)
            Linq = New Linq
            Worksheets = Package.Workbook.Worksheets
            CountDataSheet = 0
            Package.Workbook.CreateVBAProject()

            AddPivotPickByDay()
            AddPivotPickByWeek()
            AddPickCharts()
            AddPickPerHour(New Integer?() {520})
            AddPipeline()

            Linq.Dispose()
            Package.Save()
        End Using
    End Sub

End Class