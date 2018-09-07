Imports OfficeOpenXml

Public Class MonthReport
    Inherits BaseReportVM

    Public Sub New()
        Name = "По месяцам.xlsm"
    End Sub


    Public Overrides Sub CreateReport()
        Using Package As New ExcelPackage(NewFile)
            Linq = New Linq
            Worksheets = Package.Workbook.Worksheets
            CountDataSheet = 0
            Package.Workbook.CreateVBAProject()

            AddPivotAllTasksByMonth()
            AddPivotPickByMonth()

            Linq.Dispose()
            Package.Save()
        End Using
    End Sub

End Class