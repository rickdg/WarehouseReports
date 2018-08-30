Imports FirstFloor.ModernUI.Presentation
Imports FirstFloor.ModernUI.Windows.Controls
Imports WarehouseReports.Content

Namespace Pages
    Public Class DataBaseManagementVM

        Public ReadOnly Property CmdLoadTasks As ICommand = New RelayCommand(AddressOf LoadTasksExecute)
        Private Sub LoadTasksExecute(obj As Object)
            Dim Dlg As New ModernDialog
            Dlg.Buttons = {Dlg.OkButton}
            Dlg.Content = New DataLoader(Dlg)
            Dlg.ShowDialog()
        End Sub

    End Class
End Namespace