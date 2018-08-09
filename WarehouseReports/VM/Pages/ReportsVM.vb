Imports System.Data
Imports FirstFloor.ModernUI.Presentation
Imports WarehouseReports.DAL

Namespace Pages
    Public Class ReportsVM
        Inherits NotifyPropertyChanged

        Public Property DataView As IEnumerable


#Region "Commands"
        Public ReadOnly Property CmdGetReport As ICommand = New RelayCommand(AddressOf GetReportExecute)
        Private Sub GetReportExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                DataView = (From t In Context.TaskDatas Select t).ToList
                OnPropertyChanged("DataView")
            End Using
        End Sub


        Public ReadOnly Property CmdCreateExcelTable As ICommand = New RelayCommand(AddressOf Reports.ReportTasksByDayExecute)
#End Region

    End Class
End Namespace