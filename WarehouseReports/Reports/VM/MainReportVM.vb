Imports OfficeOpenXml

Public Class MainReportVM
    Inherits BaseReportVM

    Public Sub New()
        Name = "Основной отчет.xlsm"
    End Sub


    Public Overrides Sub CreateReport()
        Using Package As New ExcelPackage(NewFile)
            Linq = New Linq
            CountDataSheet = 0

            Worksheets = Package.Workbook.Worksheets
            Package.Workbook.CreateVBAProject()










            Linq.Dispose()
            Package.Save()
        End Using
    End Sub

End Class