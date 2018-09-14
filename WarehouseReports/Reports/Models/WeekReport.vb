Imports OfficeOpenXml

Public Class WeekReport
    Inherits BaseReportVM

    Public Sub New()
        Name = "По неделям.xlsm"
    End Sub


    Public Overrides Sub CreateReport()
        Using Package As New ExcelPackage(NewFile)
            Linq = New Linq
            Worksheets = Package.Workbook.Worksheets
            CountDataSheet = 0
            Package.Workbook.CreateVBAProject()

            AddPivotAllTasksByWeek()
            AddPivotPickByWeek()

            Linq.Dispose()
            Package.Save()
        End Using
    End Sub

End Class