Imports OfficeOpenXml
Imports OfficeOpenXml.Table

Public Class DataReport
    Inherits BaseReportVM

    Public Sub New()
        Name = "Данные.xlsx"
    End Sub


    Public Overrides Sub CreateReport()
        Using Package As New ExcelPackage(NewFile)
            Linq = New Linq
            Worksheets = Package.Workbook.Worksheets
            CountDataSheet = 0

            AddWorksheet("Данные").LoadFromCollection(Linq.GetData, True, TableStyles.Light9)

            Linq.Dispose()
            Package.Save()
        End Using
    End Sub

End Class