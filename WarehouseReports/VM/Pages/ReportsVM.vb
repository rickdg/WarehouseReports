Imports System.Data
Imports FirstFloor.ModernUI.Presentation
Imports WarehouseReports.DAL

Namespace Pages
    Public Class ReportsVM
        Inherits NotifyPropertyChanged

        Public Property DataView As IList


#Region "Commands"
        Public ReadOnly Property CmdGetReport As ICommand = New RelayCommand(AddressOf GetReportExecute)
        Private Sub GetReportExecute(obj As Object)
            Using Context As New WarehouseDataEntities
                DataView = (From t In Context.TaskDatas Select t).ToList
                OnPropertyChanged("DataView")
            End Using
        End Sub
#End Region

    End Class
End Namespace