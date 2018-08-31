Imports System.IO
Imports FirstFloor.ModernUI.Presentation
Imports FirstFloor.ModernUI.Windows.Controls
Imports Microsoft.Win32
Imports OfficeOpenXml
Imports WarehouseReports.Content
Imports WarehouseReports.Pages

Public MustInherit Class BaseReportVM

    Public Property Name As String
    Public ReadOnly Property Lable As String
        Get
            Return Split(Name, ".")(0)
        End Get
    End Property
    Public Property NewFile As FileInfo
    Public Property Worksheets As ExcelWorksheets


    Public ReadOnly Property CmdOpenReport As ICommand = New RelayCommand(AddressOf OpenReportExecute)
    Public Overridable Sub OpenReportExecute(parameter As Object)
        NewFile = GetFileInfo(GetDirectoryInfo("Reports"), Name)
        If NewFile.Exists Then
            Try
                NewFile.Delete()
            Catch ex As Exception
                Dim Dlg As New ModernDialog With {.Title = "Ошибка", .Content = New ErrorMessage(ex)}
                Dlg.ShowDialog()
                Return
            End Try
        End If
        CreateReport()
        Process.Start(NewFile.FullName)
    End Sub
    Public ReadOnly Property CmdSaveReport As ICommand = New RelayCommand(AddressOf SaveReportExecute)
    Public Overridable Sub SaveReportExecute(parameter As Object)
        Dim Extension = Split(Name, ".")(1)
        Dim NamePart = $"{PageReports.Model.StartDate.Year} {MonthName(PageReports.Model.StartDate.Month)}"
        Dim SaveDlg As New SaveFileDialog With {
            .OverwritePrompt = False,
            .FileName = $"{NamePart} {Lable}",
            .Filter = $"{Extension} files (*.{Extension})|*.{Extension}"}
        If SaveDlg.ShowDialog Then
            NewFile = New FileInfo(SaveDlg.FileName)
            Try
                NewFile.Delete()
                CreateReport()
                Process.Start(NewFile.DirectoryName)
            Catch ex As Exception
                Dim Dlg As New ModernDialog With {.Title = "Ошибка", .Content = New ErrorMessage(ex)}
                Dlg.ShowDialog()
            End Try
        End If
    End Sub


    Public Function AddWorksheet(name As String) As WorksheetHelper
        Return New WorksheetHelper With {.Sheet = Worksheets.Add(name)}
    End Function


    Public Overridable Sub CreateReport()
    End Sub


    Public Function GetAddress(row As Integer, column As Integer) As String
        Return ExcelAddress.GetAddress(row, column)
    End Function


    Public Function GetAddress(fromRow As Integer, fromColumn As Integer, toRow As Integer, toColumn As Integer) As String
        Return ExcelAddress.GetAddress(fromRow, fromColumn, toRow, toColumn)
    End Function

End Class